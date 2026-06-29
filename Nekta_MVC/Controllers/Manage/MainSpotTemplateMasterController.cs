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
    public class MainSpotTemplateMasterController : Controller
    {
        private readonly MainSpotTemplateMaster_BAL bal;


        private readonly TemplateMaster_BAL _tempBal;
        public MainSpotTemplateMasterController(IConfiguration config)
        {
            bal = new MainSpotTemplateMaster_BAL(config);
            _tempBal = new TemplateMaster_BAL(config);

        }

        public IActionResult Index(string search, int page = 1)
        {
            int size = 10;
            var result = bal.GetPaged(search, page, size);

            ViewBag.Search = search;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(result.total / (double)size);

            return View(result.data);
        }
        public IActionResult Add()
        {
            //  ViewBag.TemplateTypes = bal.GetTemplateTypes();
            ViewBag.Languages = bal.GetLanguages();
            ViewBag.TemplateMasters = _tempBal.GetAllTemplates();
            return View();
        }



        [HttpPost]
        public IActionResult Add(MainSpotTemplateMaster model, List<int> TemplateIds)
        {
            if (ModelState.IsValid)
            {
                model.Created_UserID = 1;
                int mainSpotId = bal.Add(model);


                bal.SaveMappings(mainSpotId, TemplateIds);

                TempData["msg"] = "Components Layout Added Successfully!";
                return RedirectToAction("Index");
            }

            ViewBag.Languages = bal.GetLanguages();
            ViewBag.TemplateMasters = _tempBal.GetAllTemplates();
            return View(model);
        }



        [HttpGet]
        public IActionResult Edit(string Id)
        {
            int realId = Convert.ToInt32(CryptoEngine.Decrypt(Id));
            var model = bal.GetById(realId);
            // ViewBag.TemplateTypes = bal.GetTemplateTypes();

            ViewBag.Languages = bal.GetLanguages();
            ViewBag.TemplateMasters = _tempBal.GetAllTemplates();
            // load mappings
            ViewBag.SelectedTemplates = bal.GetTemplateIds(realId);

            ViewBag.EncId = Id;   //  keep encrypted id for post
            ModelState.Remove("ID");
            return View(model);
        }


        [HttpPost]
        public IActionResult Edit(MainSpotTemplateMaster model, List<int> TemplateIds)
        {
            if (ModelState.IsValid)
            {
                model.Updated_UserID = 1;
                bal.Update(model);


                bal.SaveMappings(model.ID, TemplateIds);

                TempData["msg"] = "Components Layout Updated Successfully!";
                return RedirectToAction("Index");
            }
            
            ViewBag.TemplateMasters = _tempBal.GetAllTemplates();
            ViewBag.SelectedTemplates = TemplateIds;

            ViewBag.Languages = bal.GetLanguages();
            return View(model);
        }




        public IActionResult Deactivate(string Id)
        {
            int realId = Convert.ToInt32(CryptoEngine.Decrypt(Id));
            bal.Deactivate(realId, 1);
            TempData["msg"] = "Components Layout Deactivated!";
            return RedirectToAction("Index");
        }
    }

}
