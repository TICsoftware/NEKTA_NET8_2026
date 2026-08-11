using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nekta_MVC.Models;

namespace Nekta_MVC.Controllers;

public class FoodprogramController : Controller
{
    private readonly ILogger<FoodprogramController> _logger;

    public FoodprogramController(ILogger<FoodprogramController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

   

   
}
