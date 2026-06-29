
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nekta_BusinessLogic;
using Nekta_MVC.Classes;


namespace Nekta_MVC.Controllers
{
    public class CareersController : Controller
    {
        private readonly IConfiguration objconfig;
        private readonly IWebHostEnvironment _env;
        public CareersController(IConfiguration configuration, IWebHostEnvironment env)
        {
            objconfig = configuration;
            _env = env;
        }

        public IActionResult DisplayJobs(string? pageno = "1")
        {
            if (TempData["Career_title"] == null)
                TempData["Career_title"] = "";
            if (TempData["Career_Intro"] == null)
                TempData["Career_Intro"] = "";
            if (TempData["Career_image"] == null)
                TempData["Career_image"] = "";
            if (TempData["alt_image"] == null)
                TempData["alt_image"] = "";
            return ViewComponent("Career", new { pageno = pageno });
        }

        [HttpGet]
        public IActionResult ApplyJob(string Id)
        {
            ViewBag.captchapublickey = objconfig["CaptchaKeys:PublicKey"];
            Job_application Model = new();
            Model.Job_id = Id;
            return PartialView(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyJob(Job_application Model)
        {
            using CareerJobs_BAL objBal = new(objconfig);
            string Mail_Content = "", job_role = "";
            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string tempPath = Path.Combine(_env.WebRootPath, "uploads", "Careers", "temp");
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
                        Application_Details obj = new();
                        obj.Name = Model.Name;
                        obj.Age = Model.Age;
                        obj.Gender = Model.Gender;
                        obj.Email = Model.Email;
                        obj.Contact = Model.Contact;
                        obj.Current_Salary = Model.Current_Salary;
                        obj.Notice_Period = Model.Notice_Period;
                        obj.Location = Model.Location;
                        obj.Relocate_job = Model.Relocate_job;
                        obj.Message = Model.Message;
                        obj.Job_id = Convert.ToInt32(CryptoEngine.Decrypt(Model.Job_id));
                        if (Model.attachment != null && Model.attachment.Length > 0)
                        {
                            string cleanName = Classes.Helper.CleanFileName(Path.GetFileNameWithoutExtension(Model.attachment.FileName));
                            string extension = Path.GetExtension(Model.attachment.FileName);
                            string FinalFileName = $"{cleanName}-{timeStamp}{extension}";

                            var filePath = Path.Combine(_env.WebRootPath, "uploads", "Careers", FinalFileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await Model.attachment.CopyToAsync(stream);
                            }
                            obj.attachment = filePath.ToString();
                        }
                        else if (!string.IsNullOrEmpty(Model.TempFileName))
                        {
                            string FinalFileName = Path.GetFileNameWithoutExtension(Model.TempFileName) + "-" + timeStamp + Path.GetExtension(Model.TempFileName);
                            string finalPath = Path.Combine(_env.WebRootPath, "uploads", "Careers", FinalFileName);
                            var tempFile = Path.Combine(tempPath, Model.TempFileName);

                            if (System.IO.File.Exists(tempFile))
                            {
                                System.IO.File.Move(tempFile, finalPath);
                            }
                            obj.attachment = finalPath.ToString();
                        }
                        job_role = objBal.Job_Application_BAL(obj);
                        ViewBag.SuccessMessage = "Thank you for applying to the Centre for Oncopathology.<br />We have received your application and appreciate your interest in joining our team. Our team will review your details and get in touch with you if your profile is shortlisted for the next stage.";

                        Mail_Content = "Dear Applicant,<br /><br/>"
                            + "Thank you for applying to the Centre for Oncopathology, a unit of Tata Cancer Care Foundation.<br /><br />"
                            + "We have received your application and our team will review the details shared by you. In case your profile is shortlisted for the role, a member of our team will get in touch with you regarding the next steps.<br /><br />"
                            + "Regards,<br/>Talent Acquisition<br/>Centre for Oncopathology<br/>A unit of Tata Cancer Care Foundation";
                        Helper.SendAMail(Model.Name, Model.Email.Trim(), "", "", "Your application has been received", Mail_Content.ToString(), true, objconfig);
                        //Helper.SendAMail(Model.Name, "vrushali.thakur@ticworks.com", "niyati.darekar@ticworks.com", "sonu.verma@ticworks.com", "Your application has been received", Mail_Content.ToString(), true, objconfig);
                        Mail_Content = string.Empty;
                        Mail_Content = "Dear Team,<br/><br/>"
                            + "Role : " + job_role + "<br />"
                            + "Full Name : " + Model.Name.Trim() + "<br />"
                            + "Age : " + Model.Age + "<br />"
                            + "Gender : " + Model.Gender + "<br />"
                            + "Email Address : " + Model.Email + "<br />"
                            + "Mobile Number : " + Model.Contact + "<br />"
                            + "Current Salary : " + Model.Current_Salary + "<br />"
                            + "Notice Period : " + Model.Notice_Period + "<br />"
                            + "Relocate job : " + Model.Relocate_job + "<br />";
                        if (!string.IsNullOrWhiteSpace(Model.Message))
                        {
                            Mail_Content = Mail_Content + "Message : " + Model.Message + "<br />";
                        }
                        Helper.SendAMail("infocop@tatacancercare.org", "infocop@tatacancercare.org", "", "", "New job application received", Mail_Content.ToString(), true, objconfig, string.IsNullOrWhiteSpace(obj.attachment) ? "" : obj.attachment);
                        //Helper.SendAMail("infocop@tatacancercare.org", "vrushali.thakur@ticworks.com", "niyati.darekar@ticworks.com", "sonu.verma@ticworks.com", "New job application received", Mail_Content.ToString(), true, objconfig, string.IsNullOrWhiteSpace(obj.attachment) ? "" : obj.attachment);
                        ModelState.Clear();
                        return PartialView(new Job_application());

                    }
                    else
                    {
                        ModelState.AddModelError("", "Captcha validation failed. Please try again");
                    }
                }

            }
            catch (Exception ex)
            {
                FileLogger.LogError("/Careers/ApplyJob : Post :", ex);
                ModelState.AddModelError("", "Something went wrong. Please try again");
                //  ViewBag.SuccessMessage = "";
            }
            finally
            {
                objBal.Dispose();
            }
            return PartialView(Model);
        }
    }
}