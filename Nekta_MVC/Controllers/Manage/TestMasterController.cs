using Microsoft.AspNetCore.Mvc;
using Core_project_BusinessLogic.BAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Core_project_BusinessLogic.BAL;
using Core_project_BusinessLogic;
using Core_project_BusinessLogic.Entity;
using System.Net;
using Nekta_MVC.Helpers;
using Nekta_MVC.Filters;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Serialization;

namespace Nekta_MVC.Controllers.Manage
{
    [Authorize]
[SessionAuthorize]
public class TestMasterController : Controller
{
    private readonly TestMaster_BAL _bal;

    public TestMasterController(IConfiguration config)
    {
        _bal = new TestMaster_BAL(config);
    }
public IActionResult Index(
    string search = "",
    int page = 1)
{
    int pageSize = 10;

    var result = _bal.GetPaged(
        search,
        page,
        pageSize
    );

    ViewBag.Specimen = _bal.GetSpecimen();
    ViewBag.TestType = _bal.GetTestType();
    ViewBag.Organ = _bal.GetOrgan();
    ViewBag.Department = _bal.GetDepartment();

    ViewBag.NABL_Option =
        new List<MasterEntity>
    {
        new MasterEntity
        {
            ID = 1,
            Name =
            "Tests covered under NABL accrediation"
        },

        new MasterEntity
        {
            ID = 2,
            Name =
            "Tests not covered under NABL accrediation"
        }
    };

    ViewBag.Search = search;

    ViewBag.Page = page;

    ViewBag.PageSize = pageSize;

    ViewBag.TotalRecords = result.Item2;

    return View(result.Item1);
}
[HttpGet]
public JsonResult GetById(int id)
{
    var data = _bal.GetById(id);

    return Json(data);
}
    [HttpPost]
    public IActionResult Save(TestMasterEntity model)
    {
        if (model == null)
            return BadRequest();

        if (string.IsNullOrWhiteSpace(model.TestName))
        {
            TempData["ValidationError"] = "Test Name is required.";
            return RedirectToAction("Index");
        }

        if (!model.NABL_Option.HasValue || model.NABL_Option.Value < 1)
        {
            TempData["ValidationError"] = "NABL Option is required.";
            return RedirectToAction("Index");
        }

        bool hasMasterType =
            (model.SpecimenId.HasValue && model.SpecimenId.Value > 0) ||
            (model.TestTypeId.HasValue && model.TestTypeId.Value > 0) ||
            (model.OrganId.HasValue && model.OrganId.Value > 0) ||
            (model.DepartmentId.HasValue && model.DepartmentId.Value > 0);

        if (!hasMasterType)
        {
            TempData["ValidationError"] =
                "Select at least one: Specimen, Test Type, Organ, or Department.";
            return RedirectToAction("Index");
        }

        model.TestName = model.TestName.Trim();
        model.Created_UserID = Convert.ToInt32(User.GetUserId());

        _bal.Save(model);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Deactivate(int id)
    {
        int userId = Convert.ToInt32(User.GetUserId()); // logged user
        _bal.Deactivate(id, userId);
        return RedirectToAction("Index");
    }

    [HttpPost]
public IActionResult ChangeStatus([FromBody] StatusModel model)
{
    _bal.ChangeStatus(model.id, model.status);

    return Ok();
}
}
}