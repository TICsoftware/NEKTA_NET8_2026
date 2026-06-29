using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nekta_MVC.Models;
using Nekta_MVC.Classes;
using Core_project_BusinessLogic;
using Nekta_BusinessLogic.BAL;
using Nekta_BusinessLogic.Entity;

namespace Nekta_MVC.Controllers
{
    public class SearchController : Controller
    {
        private readonly ILogger<SearchController> _logger;
        private readonly Search_BAL _bal;

        public SearchController(ILogger<SearchController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _bal = new Search_BAL(configuration);
        }

        public IActionResult Index(string Id)
        {
            Search model = new Search();

            if (!string.IsNullOrWhiteSpace(Id))
            {
                try
                {
                    model.txttitle = Id.ToString();
                    model = _bal.search_display(Id.Replace("of ", ""), 1);
                }
                catch (Exception ex)
                {
                    FileLogger.LogError("/Search/Index :", ex);
                    return View(new Search());
                }
            }
            return View(model);

        }


        public ActionResult LoadMoreSearch(string keyword, int page)
        {
            try
            {
                Search model = _bal.search_display(keyword, page);

                if (model == null ||
                    model.objsearchdisplay == null ||
                    model.objsearchdisplay.Count == 0)
                {
                    return Content("");
                }
                
                return PartialView("_LoadMoreSearch", model.objsearchdisplay);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;

                return Content("Error : " + ex.Message);
            }
        }


    }
}