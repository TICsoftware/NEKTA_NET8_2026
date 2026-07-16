using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nekta_MVC.Models;

namespace Nekta_MVC.Controllers;

public class OutdooreventsController : Controller
{
    private readonly ILogger<OutdooreventsController> _logger;

    public OutdooreventsController(ILogger<OutdooreventsController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

   

   
}
