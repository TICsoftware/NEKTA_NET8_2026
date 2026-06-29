using Microsoft.AspNetCore.Mvc;
using Core_project_BusinessLogic;
using Core_project_BusinessLogic.BAL;
using Core_project_BusinessLogic.Entity.Manage;
using Nekta_MVC.Filters;
using Microsoft.AspNetCore.Authorization;
namespace Nekta_MVC.Controllers.Manage
{
      [Authorize]
[SessionAuthorize]
    public class MainSpotTemplateReferenceController : Controller
    {
        private readonly MainSpotTemplateReference_BAL bal;

        public MainSpotTemplateReferenceController(IConfiguration config)
        {
            bal = new MainSpotTemplateReference_BAL(config);
        }

        public IActionResult Index()
        {
            return View(bal.GetAll());
        }

        public IActionResult Add()
        {
            ViewBag.GetTemplates = bal.GetAllTemplates();
            ViewBag.MainStopTemplate = bal.GetAllMainStopTemplate();
            ViewBag.Languages = bal.GetLanguages();
            return View();
        }

        [HttpPost]
        public IActionResult Add(MainSpotTemplateReference model)
        {
            if (ModelState.IsValid)
            {
                model.Created_UserID = 1;
                bal.Add(model);
                TempData["msg"] = "Reference Added Successfully!";                       
                return RedirectToAction("Index");
            }
            ViewBag.GetTemplates = bal.GetAllTemplates();
            ViewBag.MainStopTemplate = bal.GetAllMainStopTemplate();
            ViewBag.Languages = bal.GetLanguages();
            return View(model);
        }

        public IActionResult Edit(string Id)
        {
            int realId = Convert.ToInt32(CryptoEngine.Decrypt(Id));
            var model = bal.GetById(realId);

            ViewBag.GetTemplates = bal.GetAllTemplates();
            ViewBag.MainStopTemplate = bal.GetAllMainStopTemplate();
            ViewBag.Languages = bal.GetLanguages();
            ModelState.Remove("ID");
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(MainSpotTemplateReference model)
        {
            if (ModelState.IsValid)
            {
                model.Updated_UserID = 1;
                bal.Update(model);
                TempData["msg"] = "Template Updated Successfully!";
                return RedirectToAction("Index");
            }

            ViewBag.GetTemplates = bal.GetAllTemplates();
            ViewBag.MainStopTemplate = bal.GetAllMainStopTemplate();
            ViewBag.Languages = bal.GetLanguages();
            return View(model);
        }

        public IActionResult Deactivate(string Id)
        {
            int realId = Convert.ToInt32(CryptoEngine.Decrypt(Id));
            bal.Deactivate(realId, 1);
            TempData["msg"] = "Template Deactivated!";
            return RedirectToAction("Index");
        }
    }

}
