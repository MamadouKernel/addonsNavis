using EscaleReport.Web.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Infrastructure.Identity;

public class UserDirectoryService(UserManager<ApplicationUser> userManager) : IUserDirectoryService
{
    public async Task<IReadOnlyDictionary<string, string?>> GetEquipesByUserNameAsync(CancellationToken cancellationToken)
    {
        var users = await userManager.Users.AsNoTracking()
            .Select(u => new { u.UserName, u.Equipe })
            .ToListAsync(cancellationToken);

        return users
            .Where(u => u.UserName is not null)
            .ToDictionary(u => u.UserName!, u => u.Equipe);
    }

    public async Task<IReadOnlyList<string>> GetEquipesDisponiblesAsync(CancellationToken cancellationToken) =>
        await userManager.Users.AsNoTracking()
            .Where(u => u.Equipe != null && u.Equipe != "")
            .Select(u => u.Equipe!)
            .Distinct()
            .OrderBy(e => e)
            .ToListAsync(cancellationToken);
}
