using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nekta_MVC.Models;
using Nekta_MVC.Classes;
using Core_project_BusinessLogic;
using Nekta_BusinessLogic.BAL;
using Nekta_BusinessLogic.Entity;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nekta_MVC.Controllers;

public class AboutController : Controller
{
    private readonly ILogger<AboutController> _logger;
    private readonly About_BAL _bal;

    public AboutController(ILogger<AboutController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _bal = new About_BAL(configuration);
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About_Cop()
    {
        try
        {
            string pageName = HttpContext?.Request?.Path.Value?.Trim('/') ?? string.Empty;
            var data = _bal.GetAboutCop_BAL(pageName, 1, 1);
            return View(data);
        }
        catch (Exception ex)
        {
            FileLogger.LogError("/About_Cop :", ex);
            return View(new AboutModel());
        }
        finally
        {
            _bal.Dispose();
        }
    }

    public IActionResult Careers(string title)
    {
        try
        {
             if (!string.IsNullOrWhiteSpace(title))
        {
            string pageName = title.ToString();
            var data = _bal.GetCareers_BAL(pageName, 1, 1);
            return View(data);
          }
          return View(new AboutModel());
        }
        catch (Exception ex)
        {
            FileLogger.LogError("/About/Careers :", ex);
            return View(new AboutModel());
        }
        finally
        {
            _bal.Dispose();
        }
    }


    public IActionResult PartnersCollaborators(string title)
    {
        try
        {
            string pageName = title ?? string.Empty;
            var data = _bal.GetPartnersCollaborators_BAL(pageName, 1, 1);
            return View(data);
        }
        catch (Exception ex)
        {
            FileLogger.LogError("/About/PartnersCollaborators :", ex);
            return View(new AboutModel());
        }
        finally
        {
            _bal.Dispose();
        }
    }

    public ActionResult LoadMorePartnersCollaborators(int Cont_id, int PageNumber = 1, int PageSize = 10)
    {
        var data = _bal.GetPartnerscollaborators_BAL(Cont_id, PageNumber, PageSize);

        return PartialView("_partners_loadmore", data);
    }

   





}
