using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nekta_MVC.Models;

namespace Nekta_MVC.Controllers;

public class CompanyinformationController : Controller
{
    private readonly ILogger<CompanyinformationController> _logger;

    public CompanyinformationController(ILogger<CompanyinformationController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }


}
