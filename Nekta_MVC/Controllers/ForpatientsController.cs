using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nekta_MVC.Models;
using Nekta_MVC.Classes;
using Core_project_BusinessLogic;
using Nekta_BusinessLogic.BAL;
using Nekta_BusinessLogic.Entity;

namespace Nekta_MVC.Controllers;

public class ForPatientsController : Controller
{
    private readonly ILogger<ForPatientsController> _logger;
    private readonly TestsServices_BAL _bal;

    public ForPatientsController(ILogger<ForPatientsController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _bal = new TestsServices_BAL(configuration);
    }

    public IActionResult Index()
    {
        try
        {
            string pageName = HttpContext?.Request?.Path.Value?.Trim('/') ?? string.Empty;
            var data = _bal.GetForPAtients_BAL(pageName, 1, 1);
            return View(data);
        }
        catch (Exception ex)
        {
            FileLogger.LogError("/ForPatients/Index :", ex);
            return View(new TestsServicesModel());
        }
        finally
        {
            _bal.Dispose();
        }

    }






}
