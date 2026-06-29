using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nekta_MVC.Models;
using Nekta_MVC.Classes;
using Core_project_BusinessLogic;
using Nekta_BusinessLogic.BAL;
using Nekta_BusinessLogic;
using Nekta_BusinessLogic.Entity;

namespace Nekta_MVC.Controllers;

public class TestsServicesController : Controller
{
    private readonly ILogger<TestsServicesController> _logger;
    private readonly TestsServices_BAL _bal;
    private readonly Test_Directory_BAL _Testbal;

    public TestsServicesController(ILogger<TestsServicesController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _bal = new TestsServices_BAL(configuration);
        _Testbal = new Test_Directory_BAL(configuration);
    }

    public IActionResult Index()
    {
        try
        {
            string pageName = HttpContext?.Request?.Path.Value?.Trim('/') ?? string.Empty;
            ViewBag.TestFinder = _Testbal.Fetch_Tests_Details_BAL();
            var data = _bal.GetTestServices_BAL(pageName, 1, 1);
            return View(data);
        }
        catch (Exception ex)
        {
            FileLogger.LogError("/TestsServices/Index :", ex);
            return View(new TestsServicesModel());
        }
        finally
        {
            _bal.Dispose();
        }

    }






}
