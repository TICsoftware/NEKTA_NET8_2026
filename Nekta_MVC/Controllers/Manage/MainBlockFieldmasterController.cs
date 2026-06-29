

using Microsoft.AspNetCore.Mvc;
using Core_project_BusinessLogic;
using Core_project_BusinessLogic.BAL;
using Core_project_BusinessLogic.Entity;
using Nekta_MVC.Filters;
using Microsoft.AspNetCore.Authorization;
namespace Nekta_MVC.Controllers.Manage
{
    [Authorize]
[SessionAuthorize]
    public class MainBlockFieldmasterController : Controller
    {

        private readonly mainBlockmasterFields_BAL _bal;
        public MainBlockFieldmasterController(IConfiguration config)
        {
            _bal = new mainBlockmasterFields_BAL(config);
        }

        [HttpGet]
        public IActionResult Assign(string id)  // id = context_master_id
        {
            int realId = Convert.ToInt32(CryptoEngine.Decrypt(id));
            var model = _bal.Load(realId);
            ModelState.Remove("block_master_id");
            return View(model);
        }

        [HttpPost]
        public IActionResult Assign(BlockMasterFieldAssign model)
        {
            _bal.Save(model.block_master_id, model.SelectedFieldIds, 1);

            TempData["msg"] = "Fields updated!";
            return RedirectToAction("Index", "templateType");
        }
    }
}