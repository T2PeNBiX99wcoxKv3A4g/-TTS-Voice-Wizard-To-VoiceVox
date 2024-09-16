using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TTSToVOICEVOX.Models;

namespace TTSToVOICEVOX.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IResult Docs()
    {
        return Results.Redirect("/swagger");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}