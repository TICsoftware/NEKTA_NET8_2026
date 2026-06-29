using Microsoft.AspNetCore.Mvc;
using Nekta_BusinessLogic;
namespace Nekta_MVC.ViewComponents
{
    public class CareersViewComponent : ViewComponent
    {
        private readonly IConfiguration objconfig;

        private int pagesize = 20;
        public CareersViewComponent(IConfiguration configuration)
        {
            objconfig = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync(string? pageno = null, string? searchkeywords = null)
        {
            CMS_job_list Model = new();
            using CareerJobs_BAL objBal = new(objconfig);
            JobModel obj = new();
            try
            {
                if (TempData["Career_title"] != null)
                {
                    Model.DisplayTitle = TempData["Career_title"].ToString();
                }
                if (TempData["Career_Intro"] != null)
                {
                    Model.Intro = TempData["Career_Intro"].ToString();
                }
                if (TempData["Career_image"] != null)
                {
                    Model.Masthead_image = TempData["Career_image"].ToString();
                }
                if (TempData["alt_image"] != null)
                {
                    Model.Image_alt_text = TempData["alt_image"].ToString();
                }
                if (string.IsNullOrWhiteSpace(pageno))
                {
                    pageno = "1";
                }
                obj = objBal.Career_Jobs_BAL(Convert.ToInt32(pageno), pagesize, searchkeywords);
                if (obj.Jobs != null && obj.Jobs.Count > 0)
                {
                    Model.joblists = [];
                    foreach (var item in obj.Jobs)
                    {
                        Model.joblists.Add(new Career
                        {
                            Encrypt_job_Id = CryptoEngine.Encrypt(item.Job_Id.ToString()),
                            Role = item.Role,
                            Education = item.Education,
                            Experience = item.Experience,
                            Job_Description = item.Job_Description,
                            Location = item.Location,
                            Salary_range = item.Salary_range,
                            About_the_Role = item.About_the_Role,
                            Workmode = item.Workmode,
                            Expiry_date = item.Expiry_date,
                        });
                        Model.objpaging = new()
                        {
                            PageNumber = 1,
                            PageSize = pagesize,
                            TotalRecords = obj.TotalRecords
                        };
                    }
                }
                else
                {
                    Model.joblists = null;
                    Model.objpaging = null;
                }
            }
            catch (Exception ex)
            {
                FileLogger.LogError("/ViewComponents/CareerComponent : Post :", ex);
                ModelState.AddModelError("", "Something went wrong. Please try again");
            }
            finally
            {
                objBal.Dispose();
            }
            return View("ViewJobs", Model);
        }
    }
}