using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nekta_MVC.Models;

namespace Nekta_MVC.Controllers;

public class CulinaryexcellenceController : Controller
{
    private readonly ILogger<CulinaryexcellenceController> _logger;

    public CulinaryexcellenceController(ILogger<CulinaryexcellenceController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

   

   
}
