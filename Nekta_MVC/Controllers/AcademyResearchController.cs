using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nekta_MVC.Models;
using Nekta_MVC.Classes;
using Core_project_BusinessLogic;
using Nekta_BusinessLogic.BAL;
using Nekta_BusinessLogic.Entity;

namespace Nekta_MVC.Controllers;

public class AcademyResearchController : Controller
{
    private readonly ILogger<AcademyResearchController> _logger;
    private readonly AcademicResearch_BAL _bal;

    public AcademyResearchController(ILogger<AcademyResearchController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _bal = new AcademicResearch_BAL(configuration);
    }

    public IActionResult Index()
    {
        //var data = _bal.GetAcademicResearch_BAL(2, 1, 1);
        return View();
    }

    public IActionResult AnitaBorges(string title)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                var data = _bal.GetAcademicResearch_BAL(title, 1, 1);
                return View(data);
            }
            else
            {
                return View(new AcademyResearchModel());
            }
        }
        catch (Exception ex)
        {
            FileLogger.LogError("/AcademyResearch/AnitaBorges :", ex);
            return View(new AcademyResearchModel());
        }
        finally
        {
            _bal.Dispose();
        }
    }



    public IActionResult FellowshipsTraining(string title)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                var data = _bal.GetFellowshipsTraining_BAL(title, 1, 1);
                return View(data);
            }
            else
            {
                return View(new AcademyResearchModel());
            }
        }
        catch (Exception ex)
        {
            FileLogger.LogError("/AcademyResearch/AnitaBorges :", ex);
            return View(new AcademyResearchModel());
        }
        finally
        {
            _bal.Dispose();
        }
    }


    public IActionResult PublicationsResources(string title)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                var data = _bal.GetPublicationsResources_BAL(title, 1, 1);
                return View(data);
            }
            else
            {
                return View(new AcademyResearchModel());
            }
        }
        catch (Exception ex)
        {
            FileLogger.LogError("/AcademyResearch/PublicationsResources :", ex);
            return View(new AcademyResearchModel());
        }
        finally
        {
            _bal.Dispose();
        }
    }

    public ActionResult LoadMorePublicationsResources(int Cont_id, int PageNumber = 1, int PageSize = 10)
    {
        var data = _bal.GetPublicationsResourcesPaging_BAL(Cont_id, PageNumber, PageSize);

        return PartialView("_partners_loadmore", data);
    }





    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}
