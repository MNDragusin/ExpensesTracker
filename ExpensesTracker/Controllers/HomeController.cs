using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ExpensesTracker.Models;
using ExpensesTracker.Services;

namespace ExpensesTracker.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWalletServices _walletServices;

    public HomeController(ILogger<HomeController> logger, IWalletServices walletServices)
    {
        _logger = logger;
        _walletServices = walletServices;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
