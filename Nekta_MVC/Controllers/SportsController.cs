using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nekta_MVC.Models;

namespace Nekta_MVC.Controllers;

public class EducationController : Controller
{
    private readonly ILogger<EducationController> _logger;

    public EducationController(ILogger<EducationController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

   

   
}
