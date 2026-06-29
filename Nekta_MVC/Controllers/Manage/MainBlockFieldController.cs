using Microsoft.AspNetCore.Mvc;
using Core_project_BusinessLogic;

using Core_project_BusinessLogic.Entity;
using Core_project_BusinessLogic.BAL;
using Nekta_MVC.Filters;
using Microsoft.AspNetCore.Authorization;
namespace Nekta_MVC.Controllers.Manage
{
    [Authorize]
[SessionAuthorize]
    public class MainBlockFieldController : Controller
    {
        private readonly MainBlockField_BAL bal;
        private readonly maincomponentmasterFields_BAL _bal;
        public MainBlockFieldController(IConfiguration config)
        {
            bal = new MainBlockField_BAL(config);
        }

        public IActionResult Index()
        {
            return View(bal.GetAll());
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(BlockMasterField model)
        {
            if (ModelState.IsValid)
            {
                model.Created_UserID = 1; // Example
                bal.Insert(model);
                TempData["msg"] = "Field created successfully";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Edit(string id)
        {
            int realId = Convert.ToInt32(CryptoEngine.Decrypt(id));
            var model = bal.GetById(realId);
            ModelState.Remove("id");
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(BlockMasterField model)
        {
            if (ModelState.IsValid)
            {
                model.Updated_UserID = 1;
                bal.Update(model);
                TempData["msg"] = "Field updated";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Deactivate(string id)
        {
            int realId = Convert.ToInt32(CryptoEngine.Decrypt(id));
            bal.Deactivate(realId, 1);
            TempData["AlertMessage"] = "Context field deactivated";
            return RedirectToAction("Index");
        }

    }
}
