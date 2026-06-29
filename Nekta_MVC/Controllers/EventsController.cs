using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nekta_MVC.Models;
using Nekta_MVC.Classes;
using Core_project_BusinessLogic;
using Nekta_BusinessLogic.BAL;
using Nekta_BusinessLogic.Entity;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Nekta_MVC.Controllers;

public class EventsController : Controller
{
    private readonly IConfiguration objconfig;
    private readonly ILogger<EventsController> _logger;
    private readonly Events_BAL _bal;

    public EventsController(ILogger<EventsController> logger, IConfiguration configuration)
    {
        objconfig = configuration;
        _logger = logger;
        _bal = new Events_BAL(configuration);
    }




    public IActionResult Index(string title)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                var data = _bal.GetEventsContent_BAL(title, 1, 1);
                return View(data);
            }
            else
            {
                return View(new EventsModel());
            }
        }
        catch (Exception ex)
        {
            FileLogger.LogError("/Events/EventsCME :", ex);
            return View(new EventsModel());
        }
        finally
        {
            _bal.Dispose();
        }
    }



    public ActionResult GetEventsBySearch(int EventTypeId = 0, int EventModeId = 0, string DateSearch = "")
    {
        DateTime? searchDate = null;

        if (!string.IsNullOrWhiteSpace(DateSearch))
        {
            DateTime parsedDate;

            if (DateTime.TryParse(DateSearch, out parsedDate))
            {
                searchDate = parsedDate;
            }
        }

        var data = _bal.Search_Past_Events_BAL(
            EventTypeId,
            EventModeId,
            searchDate
        );

        return PartialView("_past_events_loadmore", data);
    }


    public IActionResult GetEventsExpand(int EventId)
    {
        try
        {
            if (EventId <= 0)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Event Id."
                });
            }

            var model = _bal.GetEventsById_BAL(EventId);

            if (model == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Event not found."
                });
            }

            return Json(new
            {
                success = true,
                intro = model.Intro ?? "",
                content = model.Content ?? ""
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Something went wrong.",
                error = ex.Message
            });
        }
    }

    // public IActionResult GetEventsExpand(int EventId)
    // {
    //     var model = _bal.GetEventsById_BAL(EventId);


    //     return Json(new
    //     {
    //         intro = model.Intro,
    //         content = model.Content,
    //     });
    // }


    [HttpGet]
    public IActionResult Registration(string Id)
    {
        Event_Register model = new();
        ViewBag.captchapublickey = objconfig["CaptchaKeys:PublicKey"];
        model.Event_Id = string.IsNullOrWhiteSpace(Id) ? "" : Id;
        return PartialView(model);
    }

    [HttpPost]
    public async Task<IActionResult> Registration(Event_Register Model)
    {
        int Idevent=0;
        try
        {
            Idevent = Convert.ToInt32(CryptoEngine.Decrypt(Model.Event_Id));
            ViewBag.captchapublickey = objconfig["CaptchaKeys:PublicKey"];
            var recaptchaResponse = Request.Form["g-recaptcha-response"];
            string secret = objconfig["CaptchaKeys:PrivateKey"]; // FIXED ":" instead of "."
            string Mail_Content = "", event_name;
            if (string.IsNullOrEmpty(recaptchaResponse))
            {
                ModelState.AddModelError("", "Captcha validation failed. Please try again");
                return ViewComponent("Medicalenquiry", Model);
            }
            using var client = new HttpClient();
            var values = new Dictionary<string, string>
                    {
                        { "secret", secret },
                        { "response", recaptchaResponse }
                    };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(
                "https://www.google.com/recaptcha/api/siteverify",
                content);

            var json = await response.Content.ReadAsStringAsync();
            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(json);

            if (ModelState.IsValid)
            {
                if (captchaResponse != null && Convert.ToBoolean(captchaResponse.Success) == true)
                {
                    Register_Event obj = new();
                    obj.FullName = Model.Full_name;
                    obj.Email_Id = Model.Email_Id;
                    obj.Designation = Model.Designation;
                    obj.Contact_number = Model.Contact;
                    obj.EventId = Idevent;
                   event_name = _bal.Events_Registration_Insert_BAL(obj);
                    ViewBag.SuccessMessage = "Thank you for registering with the Centre for Oncopathology.<br /> We have received your details, and our team will keep you updated on the upcoming event or CME programme.";

                    //email functionality
                    Mail_Content = "Dear Applicant,<br /><br/>"
                          + "Thank you for registering for event updates from the Centre for Oncopathology, a unit of Tata Cancer Care Foundation.<br /><br />"
                          + "We have received your details and will keep you informed about upcoming events, academic programmes, workshops and other relevant updates.<br /><br />"
                          + "Regards,<br/>Team COP<br/>Centre for Oncopathology<br/>A unit of Tata Cancer Care Foundation";
                    Helper.SendAMail(Model.Full_name, Model.Email_Id.Trim(), "", "", "Event Registration", Mail_Content.ToString(), true, objconfig);
                     //Helper.SendAMail(Model.Full_name, "vrushali.thakur@ticworks.com", "niyati.darekar@ticworks.com", "sonu.verma@ticworks.com", "Event Registration", Mail_Content.ToString(), true, objconfig);
                   
                    Mail_Content = string.Empty;
                    Mail_Content = "Dear Team,<br/><br/>" 
                        + "Event : " + event_name + "<br />"
                        + "Full Name : " + Model.Full_name.Trim() + "<br />"
                        + "Email Address : " + Model.Email_Id + "<br />"
                        + "Contact Number : " + Model.Contact + "<br />"
                        + "Designation : " + Model.Designation + "<br />";
                    Helper.SendAMail("infocop@tatacancercare.org", "infocop@tatacancercare.org", "", "", "Event Registration", Mail_Content.ToString(), true, objconfig);
                    //Helper.SendAMail("infocop@tatacancercare.org", "vrushali.thakur@ticworks.com", "niyati.darekar@ticworks.com", "sonu.verma@ticworks.com", "Event Registration", Mail_Content.ToString(), true, objconfig);
                    ModelState.Clear();
                    return PartialView(new Event_Register());
                }
                else
                {
                    ModelState.AddModelError("", "Captcha validation failed. Please try again");
                }
            }
        }
        catch (Exception ex)
        {
            FileLogger.LogError("/Events/Registration : Post :", ex);
            ModelState.AddModelError("", "Something went wrong. Please try again");
        }
        finally
        {

        }
        return PartialView(Model);
    }





}
