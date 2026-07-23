using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EscaleReport.Web.Models;

namespace EscaleReport.Web.Controllers;

public class HomeController : Controller
{
    // Reste accessible anonymement : référencé par app.UseExceptionHandler("/Home/Error")
    // dans Program.cs, y compris pour une exception levée avant authentification.
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
