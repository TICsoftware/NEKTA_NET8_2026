using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nekta_MVC.Models;

namespace Nekta_MVC.Controllers;

public class HealthcareController : Controller
{
    private readonly ILogger<HealthcareController> _logger;

    public HealthcareController(ILogger<HealthcareController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

   

   
}
