using Microsoft.AspNetCore.Mvc;
namespace Nekta_MVC.ViewComponents
{
    public class MedicalenquiryViewComponent : ViewComponent
    {

        private readonly IConfiguration objconfig;
        public MedicalenquiryViewComponent(IConfiguration configuration)
        {
            // HttpContext.Session.SetString("userid", "1");
            objconfig = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync(string? Id = null)
        {
            ViewBag.captchapublickey = objconfig["CaptchaKeys:PublicKey"];
            var model = new ContactUs
            {
                form_type = string.IsNullOrWhiteSpace(Id) ? "secondopinion" : Id
            };
            return View("form", model);
        }
    }

    public class ExploreTestsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string? Id = null)
        {
            var model = new ContactUs
            {
                form_type = string.IsNullOrWhiteSpace(Id) ? "secondopinion" : Id
            };
            return View("form", model);
        }
    }

}