using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nekta_BusinessLogic;
using Nekta_BusinessLogic.Entity;

namespace Nekta_MVC.Controllers
{
    public class TestDirectoryController : Controller
    {
        private readonly ILogger<TestDirectoryController> _logger;
        private readonly Test_Directory_BAL _bal;
        public TestDirectoryController(ILogger<TestDirectoryController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _bal = new Test_Directory_BAL(configuration);
        }

        public IActionResult Index()
        {
            try
            {
                string pageName = HttpContext?.Request?.Path.Value?.Trim('/') ?? string.Empty;

                // 🔹 Always initialize (avoid null completely)
                Test_Detail filters = new Test_Detail();

                //var sessionData = HttpContext.Session.GetString("TestFilters");
                string sessionData = HttpContext?.Session?.GetString("TestFilters") ?? string.Empty;

                if (!string.IsNullOrEmpty(sessionData))
                {
                    filters = JsonConvert.DeserializeObject<Test_Detail>(sessionData) ?? new Test_Detail();
                    ViewBag.Filters = filters;
                    HttpContext.Session.Remove("TestFilters");
                }

                TestDirectory data;

                if (HasFilters(filters))
                {
                    data = _bal.Get_Test_Directory_BAL(filters, pageName, 1, 1);
                }
                else
                {
                    data = _bal.Get_Test_Directory_BAL(filters, pageName, 1, 1);
                }

                return View(data);
            }
            catch (Exception ex)
            {
                FileLogger.LogError("/TestDirectory/Index :", ex);
                return View(new TestDirectory());
            }
            finally
            {
                _bal.Dispose();
            }
        }

        // public IActionResult Index()
        // {
        //    string pageName = HttpContext?.Request?.Path.Value?.Trim('/') ?? string.Empty;

        //     // 🔹 Always initialize (avoid null completely)
        //     Test_Detail filters = new Test_Detail();

        //     var sessionData = HttpContext.Session.GetString("TestFilters");

        //     if (!string.IsNullOrEmpty(sessionData))
        //     {
        //         filters = JsonConvert.DeserializeObject<Test_Detail>(sessionData) ?? new Test_Detail();
        //         ViewBag.Filters = filters;
        //         HttpContext.Session.Remove("TestFilters");
        //     }

        //     TestDirectory data;

        //     if (HasFilters(filters))
        //     {
        //         data = _bal.Get_Test_Directory_BAL(filters, pageName, 1, 1);
        //     }
        //     else
        //     {
        //         data = _bal.Get_Test_Directory_BAL(filters, pageName, 1, 1);
        //     }

        //     return View(data);
        // }

        private bool HasFilters(Test_Detail f)
        {
            if (f == null) return false;

            return (f.Specimen_Id.HasValue && f.Specimen_Id > 0) ||
                   (f.TypeTestId.HasValue && f.TypeTestId > 0) ||
                   (f.OrganId.HasValue && f.OrganId > 0) ||
                   (f.DepartmentId.HasValue && f.DepartmentId > 0) ||
                   !string.IsNullOrWhiteSpace(f.Title);
        }


        [HttpPost]
        public IActionResult LoadTests(Test_Detail detail)
        {
            var data = _bal.Search_Test_Directory_BAL(detail);

            return Json(new
            {
                tests = data.Tests,
                nabl1Count = data.NABL_option_1_count,
                nabl2Count = data.NABL_option_2_count
            });
        }

        [HttpPost]
        [Route("TestDirectory/LoadMoreTests")]
        public IActionResult loadmoretests(Test_Detail detail, int pageno, int pagesize, int nabloption)
        {
            try
            {
                var data = _bal.Search_Test_Directory_ByNABL_BAL(detail, pageno, pagesize, nabloption);

                return Json(new
                {
                    success = true,
                    tests = data.Tests,
                    total = data.TotalCount
                });
            }
            catch (Exception ex)
            {
                FileLogger.LogError("/TestDirectory/loadmoretests : Post :", ex);

                return Json(new
                {
                    success = false,
                    message = "Something went wrong. Please try again" + ex
                });
            }
            finally
            {
                _bal.Dispose();
            }
        }


        // [HttpPost]
        // public IActionResult LoadMoreTests(Test_Detail detail, int pageNo, int pageSize, int nablOption)
        // {
        //     var data;
        //     try
        //     {
        //         data = _bal.Search_Test_Directory_ByNABL_BAL(detail, pageNo, pageSize, nablOption);

        //         return Json(new
        //         {
        //             tests = data.Tests,
        //             total = data.TotalCount
        //         });
        //     }
        //     catch (Exception ex)
        //     {
        //         FileLogger.LogError("/TestDirectory/LoadMoreTests : Post :", ex);
        //         ModelState.AddModelError("", "Something went wrong. Please try again");
        //     }
        //     finally
        //     {
        //         _bal.Dispose();
        //     }
        // }



        [HttpPost]
        public IActionResult StoreFilters(Test_Detail model)
        {
            HttpContext.Session.SetString("TestFilters",
                JsonConvert.SerializeObject(model));

            return Ok();
        }


        public IActionResult TestFinder()
        {
            var data = _bal.Fetch_Tests_Details_BAL();
            return View(data);
        }





    }
}
