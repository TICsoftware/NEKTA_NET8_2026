using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Core_project_BusinessLogic.BAL;
using Core_project_BusinessLogic;
using Core_project_BusinessLogic.Entity;
using Nekta_MVC.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Nekta_MVC.Helpers;

namespace Nekta_MVC.Controllers.Manage
{
    [Authorize]
[SessionAuthorize]
   public class EventMasterController : Controller
    {
        private readonly EventMaster_BAL _bal;

        public EventMasterController(IConfiguration config)
        {
            _bal = new EventMaster_BAL(config);
        }

        // =========================================
        // 🔹 INDEX (LIST + SEARCH + PAGING)
        // =========================================
    public IActionResult Index(string search = "", int page = 1)
{
    EventMaster entity = new()
    {
        SearchText = search,
        PageNumber = page,
        PageSize = 10
    };

    var result = _bal.GetPaged(entity);

    EventMasterVM vm = new()
    {
        List = result.Item1,
        TotalRecords = result.Item2,
        PageNumber = page,
        PageSize = 10,
        SearchText = search,

        Current = new EventMaster()   // 🔥 FIX (IMPORTANT)
    };

    ViewBag.EventType = GetEventType();
    ViewBag.EventMode = GetEventMode();

    return View(vm);
}

public IActionResult Edit(int id)
{
    // 🔥 Get selected record
    var data = _bal.GetById(id);

    // 🔥 Paging (IMPORTANT to avoid SQL error)
    EventMaster entity = new()
    {
        PageNumber = 1,
        PageSize = 10
    };

    var result = _bal.GetPaged(entity);

    EventMasterVM vm = new()
    {
        List = result.Item1,
        TotalRecords = result.Item2,
        PageNumber = 1,
        PageSize = 10,

        // 🔥 THIS BINDS YOUR FORM
        Current = data
    };

    ViewBag.EventType = GetEventType();
    ViewBag.EventMode = GetEventMode();

    return View("Index", vm); // return same page
}

        // =========================================
        // 🔹 GET BY ID (EDIT)
        // =========================================
        [HttpGet]
        public JsonResult GetById(int id)
        {
            var data = _bal.GetById(id);
            return Json(data);
        }

        // =========================================
        // 🔹 SAVE (INSERT + UPDATE)
        // =========================================
        [HttpPost]
public IActionResult Save(EventMaster model)
{
    if (model == null)
        return BadRequest();

    if (!ModelState.IsValid)
    {
        EventMaster entity = new()
        {
            PageNumber = 1,
            PageSize = 10
        };

        var result = _bal.GetPaged(entity);

        EventMasterVM vm = new()
        {
            List = result.Item1,
            TotalRecords = result.Item2,
            PageNumber = 1,
            PageSize = 10,
            Current = model
        };

        ViewBag.EventType = GetEventType();
        ViewBag.EventMode = GetEventMode();

        return View("Index", vm);
    }

    model.Created_UserID = Convert.ToInt32(User.GetUserId());
    model.Updated_UserID = Convert.ToInt32(User.GetUserId());
    _bal.Save(model);

    return RedirectToAction("Index");
} 
[HttpPost]

public IActionResult UploadFile(IFormFile file)
{
    if (file == null || file.Length == 0)
        return BadRequest();

    string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/Events");

    if (!Directory.Exists(folder))
        Directory.CreateDirectory(folder);

    string fileName = Path.GetFileName(file.FileName);
    string path = Path.Combine(folder, fileName);

    using (var stream = new FileStream(path, FileMode.Create))
    {
        file.CopyTo(stream);
    }

    return Ok("/uploads/Events/" + fileName);
}

        // =========================================
        // 🔹 ACTIVATE / DEACTIVATE
        // =========================================
        [HttpPost]
        public IActionResult ChangeStatus([FromBody] StatusModel m)
        {
            _bal.ChangeStatus(m.id, m.status);
            return Ok();
        }

        // =========================================
        // 🔹 DROPDOWN HELPERS (TEMP STATIC)
        // =========================================
        private List<MasterEntity> GetEventType()
        {
            return new List<MasterEntity>
            {
                new MasterEntity { ID = 1, Name = "CME Symposium" },
                new MasterEntity { ID = 2, Name = "Workshop" },
                new MasterEntity { ID = 3, Name = "Webinar" }
            };
        }

        private List<MasterEntity> GetEventMode()
        {
            return new List<MasterEntity>
            {
                new MasterEntity { ID = 1, Name = "Hybrid" },
                new MasterEntity { ID = 2, Name = "Online" },
                new MasterEntity { ID = 3, Name = "In-person" }
            };
        }
    }
}