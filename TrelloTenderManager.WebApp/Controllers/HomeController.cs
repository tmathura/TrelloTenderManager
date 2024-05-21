using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TrelloTenderManager.WebApp.Models;

namespace TrelloTenderManager.WebApp.Controllers;

/// <summary>
/// Represents the controller for the home page.
/// </summary>
public class HomeController : Controller
{
    /// <summary>
    /// Action method for the home page.
    /// </summary>
    /// <returns>The action result.</returns>
    public IActionResult Index()
    {
        return RedirectToAction("ProcessCsv", "Card");
    }

    /// <summary>
    /// Action method for handling errors.
    /// </summary>
    /// <returns>The action result.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
