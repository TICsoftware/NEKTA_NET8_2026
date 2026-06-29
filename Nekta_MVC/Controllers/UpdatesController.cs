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
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Nekta_MVC.Controllers;

public class UpdatesController : Controller
{
    private readonly ILogger<UpdatesController> _logger;
    private readonly IRazorViewEngine _viewEngine;
    private readonly Updates_BAL _bal;

    public UpdatesController(ILogger<UpdatesController> logger, IConfiguration configuration, IRazorViewEngine viewEngine)
    {
        _logger = logger;
        _bal = new Updates_BAL(configuration);
        _viewEngine = viewEngine;
    }



    public IActionResult Index(string title)
    {
        try
        {
            string pageName = title ?? string.Empty;
            var data = _bal.GetContent_BAL(pageName, 1, 1);
            return View(data);
        }
        catch (Exception ex)
        {
            FileLogger.LogError("/Updates/Index :", ex);
            return View(new UpdatesModel());
        }
        finally
        {
            _bal.Dispose();
        }
    }

    public IActionResult MediaInside(string id)
    {
        try
        {
            string pageName = id ?? string.Empty;
            var data = _bal.GetMediaInside_BAL(pageName, 1, 1);
            return View(data);
        }
        catch (Exception ex)
        {
            FileLogger.LogError("/Updates/MediaInside :", ex);
            return View(new UpdatesModel());
        }
        finally
        {
            _bal.Dispose();
        }
    }


    public ActionResult LoadMoreAnnualArticles(int Cont_id, int PageNumber = 1, int PageSize = 10)
    {
        var data = _bal.Updates_Articles_Load_BAL(Cont_id, PageNumber, PageSize);

        return PartialView("_annual_loadmore", data);
    }


    public ActionResult LoadMoreMediaArticles(int Cont_id, int PageNumber = 1, int PageSize = 10)
    {
        var data = _bal.Updates_Articles_Load_BAL(Cont_id, PageNumber, PageSize);

        return PartialView("_media_loadmore", data);
    }


    // public ActionResult LoadMoreAnnualArticles(int Cont_id, int FinancialYear = 0, int PageNumber = 1, int PageSize = 10)
    // {
    //     var data = _bal.Get_AnnualReport_ByYear_BAL(Cont_id, FinancialYear, PageNumber, PageSize);

    //     return PartialView("_annual_bysearch", data.Annual_Report_List);
    // }

    public async Task<JsonResult> Get_AnnualReport_ByYear(int Cont_id, int FinancialYear = 0, int PageNumber = 1, int PageSize = 10)
    {
        var data = _bal.Get_AnnualReport_ByYear_BAL(Cont_id, FinancialYear, PageNumber, PageSize);

        var html = await RenderPartialViewToStringAsync("_annual_bysearch", data.Annual_Report_List);

        return Json(new
        {
            html,
            totalCount = data.AnnualtotalCount
        });
    }


    private async Task<string> RenderPartialViewToStringAsync(
        string viewName,
        object model)
    {
        ViewData.Model = model;

        using var sw = new StringWriter();

        var viewResult = _viewEngine.FindView(
            ControllerContext,
            viewName,
            false);

        if (viewResult.View == null)
        {
            throw new ArgumentNullException(
                $"{viewName} view was not found");
        }

        var viewContext = new ViewContext(
            ControllerContext,
            viewResult.View,
            ViewData,
            TempData,
            sw,
            new HtmlHelperOptions()
        );

        await viewResult.View.RenderAsync(viewContext);

        return sw.ToString();
    }


}
