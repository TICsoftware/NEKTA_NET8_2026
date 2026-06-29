using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nekta_MVC.Models;
using Nekta_MVC.Classes;
using Core_project_BusinessLogic;
using Nekta_BusinessLogic.BAL;
using Nekta_BusinessLogic.Entity;

namespace Nekta_MVC.Controllers;

public class OurteamController : Controller
{
    private readonly ILogger<OurteamController> _logger;
    private readonly Page_Manage_BAL _bal;

    public OurteamController(ILogger<OurteamController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _bal = new Page_Manage_BAL(configuration);
    }

    public IActionResult Index(string title)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                var data = _bal.GetPageData_BAL(title, 1, 1);
                return View(data);
            }
            else
            {
                return View(new PageViewModel());
            }
        }
        catch (Exception ex)
        {
            FileLogger.LogError("/Ourteam/Index :", ex);
            return View(new PageViewModel());
        }
        finally
        {
            _bal.Dispose();
        }

    }

    public IActionResult GetTeamDetails(int contentId, string groupId)
    {
        var model = _bal.GetContentComponentById_BAL(contentId, groupId);

        var group = model.Components.FirstOrDefault();

        if (group == null)
            return Json(null);

        var dict = group.Fields
            .GroupBy(x => x.FieldName)
            .ToDictionary(g => g.Key, g => g.First());

        string GetValue(string key)
        {
            return dict.ContainsKey(key) ? dict[key].FieldValue : "";
        }

        var image = group.Fields.FirstOrDefault(x => !string.IsNullOrEmpty(x.ImagePath))?.ImagePath;

        return Json(new
        {
            title = GetValue("Title"),
            intro = GetValue("Intro"),
            content = GetValue("content"),
            popupIntro = GetValue("popup intro"),
            popupContent = GetValue("popup content"),
            thumbnailAlt = GetValue("thumbnail image alt"),
            thumbnailImage = image
        });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}
