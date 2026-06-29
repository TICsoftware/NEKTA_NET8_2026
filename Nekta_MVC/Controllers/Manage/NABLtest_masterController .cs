using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Core_project_BusinessLogic.BAL;
using Core_project_BusinessLogic.Entity;
using System.Linq;
using Nekta_MVC.Helpers;
using Nekta_MVC.Filters;
using Microsoft.AspNetCore.Authorization;

namespace Nekta_MVC.Controllers.Manage
{
    [Authorize]
[SessionAuthorize]
    public class NABLtest_masterController : Controller
    {
        private readonly NABLtest_Master_BAL _bal;


         public NABLtest_masterController(IConfiguration config)
        {
            _bal = new NABLtest_Master_BAL(config);
        }


    public IActionResult Index(
    string type = "Specimen",
    string search = "",
    int page = 1)
{
    MasterEntity entity = new()
    {
        MasterType = type,
        SearchText = search,
        PageNumber = page,
        PageSize = 10
    };

    var result = _bal.GetPaged(entity);

    ViewBag.List = result.Item1;

    entity.TotalRecords = result.Item2;

    return View(entity);
}
            
    [HttpPost]
public IActionResult AddAjax([FromBody] MasterEntity model)
{
    if (model == null)
        return BadRequest(new { message = "Invalid request." });

    if (string.IsNullOrWhiteSpace(model.MasterType))
        ModelState.AddModelError(nameof(model.MasterType), "Master type is required.");

    if (!ModelState.IsValid)
        return BadRequest(new { message = GetValidationMessage() });

    _bal.Save(model);
    return Ok();
}

[HttpPost]
public IActionResult UpdateAjax([FromBody] MasterEntity model)
{
    if (model == null)
        return BadRequest(new { message = "Invalid request." });

    if (model.ID <= 0)
        ModelState.AddModelError(nameof(model.ID), "Invalid record id.");

    if (string.IsNullOrWhiteSpace(model.MasterType))
        ModelState.AddModelError(nameof(model.MasterType), "Master type is required.");

    if (!ModelState.IsValid)
        return BadRequest(new { message = GetValidationMessage() });

    _bal.Save(model);
    return Ok();
}

private string GetValidationMessage()
{
    return string.Join(" ",
        ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .Where(m => !string.IsNullOrWhiteSpace(m)));
}

// [HttpPost]
// public IActionResult Deactivate([FromBody] dynamic data)
// {
//     int id = data.id;
//     string type = data.type;

//     _bal.Deactivate(id, type);
//     return Ok();
// }


[HttpPost]
public IActionResult Deactivate([FromBody] DeactivateModel model)
{
    if (model == null || model.Id == 0 || string.IsNullOrEmpty(model.Type))
        return BadRequest("Invalid data");

    _bal.Deactivate(model.Id, model.Type);
    return Ok();
}

[HttpPost]
public IActionResult ChangeStatus([FromBody] NABLStatusModel model)
{
    if (model == null || model.Id == 0 || string.IsNullOrEmpty(model.Type))
        return BadRequest("Invalid data");

    _bal.ChangeStatus(model.Id, model.Type, model.Status);
    return Ok();
}

[HttpPost]
public IActionResult UpdateSequence([FromBody] List<MasterEntity> list)
{
    var type = list.FirstOrDefault()?.MasterType;
    _bal.UpdateSequence(list, type);
    return Ok();
}
    
    }




}