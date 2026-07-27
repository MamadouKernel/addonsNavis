using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Audit;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Controllers;
[Authorize(Roles = Roles.Administrateur)]
public sealed class RecycleBinController(IApplicationDbContext dbContext, ICurrentUserService currentUser) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var escales = await dbContext.Escales.IgnoreQueryFilters().AsNoTracking()
            .Where(e => e.IsDeleted).OrderByDescending(e => e.DeletedAtUtc)
            .Select(e => new DeletedEscaleItem(e.Id, e.Navire, e.Voyage, e.DeletedAtUtc, e.DeletedBy))
            .ToListAsync(cancellationToken);
        return View(new RecycleBinViewModel { Escales = escales });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> RestoreEscale(Guid id, CancellationToken cancellationToken)
    {
        var escale = await dbContext.Escales.IgnoreQueryFilters().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (escale is null || !escale.IsDeleted) return NotFound();
        escale.IsDeleted = false;
        escale.DeletedAtUtc = null;
        escale.DeletedBy = null;
        dbContext.AuditLogEntries.Add(new AuditLogEntry { Id = Guid.NewGuid(), DateUtc = DateTime.UtcNow, UserId = currentUser.UserId, UserName = currentUser.UserName, Action = "RestoreEscaleCommand", Cible = id.ToString() });
        await dbContext.SaveChangesAsync(cancellationToken);
        TempData["Success"] = $"L’escale {escale.Navire} a été restaurée.";
        return RedirectToAction(nameof(Index));
    }
}
