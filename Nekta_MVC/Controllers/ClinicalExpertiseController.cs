using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nekta_BusinessLogic.BAL;
using Nekta_BusinessLogic.Entity;

namespace Nekta_MVC.Controllers
{
    public class ClinicalExpertiseController : Controller
    {
        private readonly ILogger<ClinicalExpertiseController> _logger;
        private readonly ClinicalExpertise_BAL _bal;


        public ClinicalExpertiseController(ILogger<ClinicalExpertiseController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _bal = new ClinicalExpertise_BAL(configuration);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CaseConsults(string title)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(title))
                {
                    var data = _bal.GetCaseConsults_BAL(title, 1, 1);
                    return View(data);
                }
                else
                {
                    return View(new PageViewModel());
                }
            }
            catch (Exception ex)
            {
                FileLogger.LogError("/ClinicalExpertise/CaseConsults :", ex);
                return View(new PageViewModel());
            }
            finally
            {
                _bal.Dispose();
            }
        }


    }
}