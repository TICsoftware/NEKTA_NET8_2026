using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nekta_BusinessLogic;
using Nekta_MVC.Classes;

namespace Nekta_MVC.Controllers
{
    public class ConnectController : Controller
    {
        private readonly IConfiguration objconfig;
        private readonly IWebHostEnvironment _env;

        public ConnectController(IConfiguration configuration, IWebHostEnvironment env)
        {
            // HttpContext.Session.SetString("userid", "1");
            objconfig = configuration;
            _env = env;
        }
        [HttpGet]
        public IActionResult Enquiry()
        {
            ViewBag.captchapublickey = objconfig["CaptchaKeys:PublicKey"];
            ContactUs Model = new();
            return PartialView(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enquiry(ContactUs Model)
        {
            using Forms objBal = new(objconfig);
            string Mail_Content = "";
            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string tempPath = Path.Combine(_env.WebRootPath, "uploads", "formdocuments", "temp");
            try
            {
                ViewBag.captchapublickey = objconfig["CaptchaKeys:PublicKey"];
                var recaptchaResponse = Request.Form["g-recaptcha-response"];
                string secret = objconfig["CaptchaKeys:PrivateKey"]; // FIXED ":" instead of "."

                if (string.IsNullOrEmpty(recaptchaResponse))
                {
                    ModelState.AddModelError("", "Captcha validation failed. Please try again");
                    return PartialView(Model);
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

                ModelState.Remove("AcceptTerms");

                if (ModelState.IsValid)
                {
                    if (captchaResponse != null && Convert.ToBoolean(captchaResponse.Success) == true)
                    {
                        Form_Fields obj = new();
                        obj.FullName = Model.FullName;
                        obj.Email = Model.Email;
                        obj.Contact = Model.Contact;
                        obj.City = Model.City;
                        obj.Labname = Model.Labname;
                        obj.Message = Model.Message;
                        obj.form_type = "Submit your enquiry";
                        obj.Enquiry_Type = Model.Enquiry_Type;
                        if (Model.attachment != null && Model.attachment.Length > 0)
                        {
                            string cleanName = Classes.Helper.CleanFileName(Path.GetFileNameWithoutExtension(Model.attachment.FileName));
                            string extension = Path.GetExtension(Model.attachment.FileName);
                            string FinalFileName = $"{cleanName}-{timeStamp}{extension}";

                            var filePath = Path.Combine(_env.WebRootPath, "uploads", "formdocuments", FinalFileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await Model.attachment.CopyToAsync(stream);
                            }
                            obj.attachment_path = filePath.ToString();
                        }
                        else if (!string.IsNullOrEmpty(Model.TempFileName))
                        {
                            string FinalFileName = Path.GetFileNameWithoutExtension(Model.TempFileName) + "-" + timeStamp + Path.GetExtension(Model.TempFileName);
                            string finalPath = Path.Combine(_env.WebRootPath, "uploads", "formdocuments", FinalFileName);
                            var tempFile = Path.Combine(tempPath, Model.TempFileName);

                            if (System.IO.File.Exists(tempFile))
                            {
                                System.IO.File.Move(tempFile, finalPath);
                            }
                            obj.attachment_path = finalPath.ToString();
                        }
                        objBal.Contact_Forms_Insert(obj);
                        ViewBag.SuccessMessage = "Thank you for your data submission.<br />Our team has received your details and will get back to you shortly.";
                        Mail_Content = "Dear User,<br /><br/>"
                            + "Thank you for reaching out to the Centre for Oncopathology, a unit of Tata Cancer Care Foundations .<br /><br />"
                            + "We have received your enquiry and our team will get in touch with you shortly.<br /><br />"
                            + "Regards,<br/>Team COP";
                        Helper.SendAMail(Model.FullName, Model.Email.Trim(), "", "", "New Enquiry Received", Mail_Content.ToString(), true, objconfig);
                        //Helper.SendAMail(Model.FullName, "vrushali.thakur@ticworks.com", "", "", "New Enquiry Received", Mail_Content.ToString(), true, objconfig);
                        Mail_Content = string.Empty;
                        Mail_Content = "Dear Team,<br/><br/>"
                            + "Full Name : " + Model.FullName.Trim() + "<br />"
                            + "Email Address : " + obj.Email + "<br />"
                            + "Mobile Number : " + obj.Contact + "<br />"
                            + "City : " + obj.City + "<br />"
                            + "Organisation/Hospital/Lab : " + obj.Labname + "<br />"
                            + "Enquiry Type : " + obj.Enquiry_Type + "<br />"
                            + "Message : " + obj.Message + "<br />";
                        Helper.SendAMail("infocop@tatacancercare.org", "infocop@tatacancercare.org", "", "", "New Enquiry Received", Mail_Content.ToString(), true, objconfig, string.IsNullOrWhiteSpace(obj.attachment_path) ? "" : obj.attachment_path);
                        //Helper.SendAMail("infocop@tatacancercare.org", "vrushali.thakur@ticworks.com", "", "", "New Enquiry Received", Mail_Content.ToString(), true, objconfig, string.IsNullOrWhiteSpace(obj.attachment_path) ? "" : obj.attachment_path);
                        ModelState.Clear();
                        return PartialView(new ContactUs());

                    }
                    else
                    {
                        ModelState.AddModelError("", "Captcha validation failed. Please try again");
                    }
                }

            }
            catch (Exception ex)
            {
                FileLogger.LogError("/Connect/Enquiry : Post :", ex);
                ModelState.AddModelError("", "Something went wrong. Please try again");
                //  ViewBag.SuccessMessage = "";
            }
            finally
            {
                objBal.Dispose();
            }
            return PartialView(Model);
        }

        public IActionResult MedicalForm(string Id)
        {
            ViewBag.captchapublickey = objconfig["CaptchaKeys:PublicKey"];
            return ViewComponent("Medicalenquiry", new { Id = Id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Medicalenquiry(ContactUs Model)
        {
            using Forms objBal = new(objconfig);
            string tempPath = Path.Combine(_env.WebRootPath, "uploads", "formdocuments", "temp");
            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string form_type;
            string Mail_Content = "";
            try
            {
                ViewBag.captchapublickey = objconfig["CaptchaKeys:PublicKey"];
                var recaptchaResponse = Request.Form["g-recaptcha-response"];
                string secret = objconfig["CaptchaKeys:PrivateKey"]; // FIXED ":" instead of "."

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

                ModelState.Remove("Enquiry_Type");
                ModelState.Remove("AcceptTerms");
                ModelState.Remove("City");

                if (ModelState.IsValid)
                {
                    if (captchaResponse != null && Convert.ToBoolean(captchaResponse.Success) == true)
                    {
                        Form_Fields obj = new();
                        obj.FullName = Model.FullName;
                        obj.Email = Model.Email;
                        obj.Contact = Model.Contact;
                        obj.City = string.Empty;
                        obj.Labname = string.Empty;
                        obj.Message = Model.Message;
                        if (Model.form_type == "submitcase")
                            obj.form_type = "Submit a case";
                        else
                            obj.form_type = "Second opinion";

                        if (Model.attachment != null && Model.attachment.Length > 0)
                        {
                            string cleanName = Classes.Helper.CleanFileName(Path.GetFileNameWithoutExtension(Model.attachment.FileName));
                            string extension = Path.GetExtension(Model.attachment.FileName);
                            string FinalFileName = $"{cleanName}-{timeStamp}{extension}";

                            var filePath = Path.Combine(_env.WebRootPath, "uploads", "formdocuments", FinalFileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await Model.attachment.CopyToAsync(stream);
                            }
                            obj.attachment_path = filePath.ToString();
                        }
                        else if (!string.IsNullOrEmpty(Model.TempFileName))
                        {
                            string FinalFileName = Path.GetFileNameWithoutExtension(Model.TempFileName) + "-" + timeStamp + Path.GetExtension(Model.TempFileName);
                            string finalPath = Path.Combine(_env.WebRootPath, "uploads", "formdocuments", FinalFileName);
                            var tempFile = Path.Combine(tempPath, Model.TempFileName);

                            if (System.IO.File.Exists(tempFile))
                            {
                                System.IO.File.Move(tempFile, finalPath);
                            }
                            obj.attachment_path = finalPath.ToString();
                        }
                        else
                        {
                            obj.attachment_path = null;
                        }
                        objBal.Contact_Forms_Insert(obj);

                        Classes.Helper.Remove_temporary_files("wwwroot/uploads/formdocuments/temp");
                        ViewBag.SuccessMessage = "Thank you for your submission.<br/>Our team has received your details and will get back to you shortly.";
                        form_type = Model.form_type ?? "secondopinion";
                        ModelState.Clear();
                        if (form_type == "secondopinion")
                        {
                            Mail_Content = "Dear User,<br/><br/>"
                               + "Thank you for submitting your second opinion request to the Centre for Oncopathology, a unit of Tata Cancer Care Foundation.<br/><br/>"
                               + "We have received the details shared by you and our team will review your request carefully. A member of our team will get in touch with you shortly regarding the next steps or in case any additional information is required.<br/><br/>"
                               + "Regards,<br/>Team CoP";
                            Helper.SendAMail(Model.FullName, Model.Email.Trim(), "", "", "Second Opinion Request", Mail_Content.ToString(), true, objconfig);
                        }
                        else
                        {
                            Mail_Content = "Dear User,<br/><br/>"
                           + "Thank you for submitting your case to the Centre for Oncopathology, a unit of Tata Cancer Care Foundation.<br/><br/>"
                           + "We have received your submission and our team will review the details shared. A member of our team will get in touch with you shortly regarding the next steps or in case any additional information is required.<br/><br/>"
                           + "Regards,<br/>Team CoP";
                            Helper.SendAMail(Model.FullName, Model.Email.Trim(), "", "", "New Case Submission", Mail_Content.ToString(), true, objconfig);
                        }

                        Mail_Content = string.Empty;

                        Mail_Content = "Dear Team,<br/><br/>"
                            + "Full Name : " + Model.FullName.Trim() + "<br/>"
                            + "Email Address : " + obj.Email + "<br/>"
                            + "Mobile Number : " + obj.Contact + "<br/>"
                            + "Message : " + obj.Message + "<br/>";
                        Helper.SendAMail("infocop@tatacancercare.org", "infocop@tatacancercare.org", "", "", form_type == "secondopinion" ? "Second Opinion Request" : "New Case Submission", Mail_Content.ToString(), true, objconfig, string.IsNullOrWhiteSpace(obj.attachment_path) ? "" : obj.attachment_path);
                        //Helper.SendAMail("infocop@tatacancercare.org", "vrushali.thakur@ticworks.com", "", "cop@ticworks.com", form_type == "secondopinion" ? "Second Opinion Request" : "New Case Submission", Mail_Content.ToString(), true, objconfig, string.IsNullOrWhiteSpace(obj.attachment_path) ? "" : obj.attachment_path);
                        return ViewComponent("Medicalenquiry", new { Id = form_type });
                        //return PartialView(new ContactUs() { form_status = "1" });
                        // return Json(new { success = true, message = "Thank you! Your enquiry has been submitted successfully." });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Captcha validation failed. Please try again");
                    }
                }
                else
                {
                    bool isAttachmentValid = true;
                    if (ModelState.TryGetValue("attachment", out var entry))
                    {
                        isAttachmentValid = entry.Errors.Count == 0;
                    }
                    if (isAttachmentValid && Model.attachment != null && Model.attachment.Length > 0)
                    {
                        var tempFileName = Guid.NewGuid() + Classes.Helper.CleanFileName(Model.attachment.FileName);
                        var fullPath = Path.Combine(tempPath, tempFileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await Model.attachment.CopyToAsync(stream);
                        }
                        Model.TempFileName = tempFileName;

                    }

                }
            }
            catch (Exception ex)
            {
                FileLogger.LogError("/Connect/Medicalenquiry : Post :", ex);
                ModelState.AddModelError("", "Something went wrong. Please try again");
            }
            finally
            {
                objBal.Dispose();
            }
            return ViewComponent("Medicalenquiry", Model);
            //  return PartialView(Model);
        }
    }
}