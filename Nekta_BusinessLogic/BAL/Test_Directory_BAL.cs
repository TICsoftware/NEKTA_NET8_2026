using System.Data;
using Microsoft.Extensions.Configuration;
using Nekta_BusinessLogic.Entity;

namespace Nekta_BusinessLogic
{
    public class Test_Directory_BAL : Test_Directory_DAL
    {
        private readonly IConfiguration _configuration;
        public Test_Directory_BAL(IConfiguration configuration)  : base(configuration) 
        {
            _configuration = configuration;
        }


        public TestDirectory Get_Test_Directory_BAL(Test_Detail Detailobj, string pageName, int languageId, int geographyId)
        {
            var obj = new TestDirectory();

            try
            {
                // 🔹 STEP 1: FETCH MAIN DATA
                using (var ds = GetTestDirectoryContent_DAL(Detailobj, pageName, languageId, geographyId))
                {
                    obj.Tests = new List<Test>();
                    string baseurl = _configuration["AppSettings:BaseUrl"]?.TrimEnd('/') ?? "";

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];

                        var Image = row.Field<string>("masthead_image_path") ?? "";
                        var canUrl = row.Field<string>("pageurl") ?? "";

                        obj.Content = new ContentViewModel
                        {
                            ContId = Convert.ToInt32(row["cont_id"]),
                            ContTitle = Convert.ToString(row["cont_title"]),
                            Cont_intro = Convert.ToString(row["cont_intro"]),
                            Cont_hmpg_intro = Convert.ToString(row["cont_hmpg_intro"]),
                            Content = Convert.ToString(row["content"]),
                            PageName = Convert.ToString(row["cont_pagename"]),
                            MastheadImage = Convert.ToString(row["masthead_image_path"]),
                            MobileMastheadImage = Convert.ToString(row["mobile_masthead_image_path"]),
                            Template_Master_ID = Convert.ToInt32(row["Template_Master_ID"]),
                            BreadcrumPath = Convert.ToString(row["BreadcrumPath"]) ?? string.Empty,
                            cont_window_title = row.Field<string>("cont_window_title") ?? "",
                            cont_metadesc = row.Field<string>("cont_metadesc") ?? "",
                            cont_metatag = row.Field<string>("cont_metatag") ?? "",
                            Hmpg_thumbnail = row.Field<string>("Hmpg_thumbnail") ?? "",
                            Hmpg_thumbnail_alt_text = row.Field<string>("Hmpg_thumbnail_alt_text") ?? "",
                            CanonicalUrl = row.Field<string>("pageurl") ?? ""
                        };
                    }

                    // ✅ TABLE 0 → DATA
                    if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    {
                        obj.Tests = ds.Tables[1].AsEnumerable().Select(row => new Test
                        {
                            Title = row["TestName"]?.ToString(),
                            Specimen = row["SpecimenName"]?.ToString(),
                            TypeTest = row["TestTypeName"]?.ToString(),
                            Organ = row["OrganName"]?.ToString(),
                            Department = row["DepartmentName"]?.ToString(),
                            covered_under_NABL = ToInt(row["NABL_Option"]) // 🔥 FIXED
                        }).ToList();
                    }

                    // ✅ TABLE 1 → COUNT
                    if (ds != null && ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[2].Rows)
                        {
                            int option = ToInt(row["NABL_Option"]); // 🔥 FIXED
                            int count = ToInt(row["cnt"]);          // 🔥 FIXED

                            if (option == 1)
                                obj.NABL_option_1_count = count;
                            else if (option == 2)
                                obj.NABL_option_2_count = count;
                        }
                    }
                }

                // 🔹 STEP 2: LOAD DROPDOWNS (ONLY PAGE LOAD)

                using (var dsDrop = Fetch_Tests_Details_DAL())
                {
                    obj.Specimen = MapDropdown(dsDrop, 0, "SpecimenId", "SpecimenName", "Select Specimen");
                    obj.TypeTest = MapDropdown(dsDrop, 1, "TestTypeId", "TestTypeName", "Select type of test");
                    obj.Organ = MapDropdown(dsDrop, 2, "OrganId", "OrganName", "Select Organ");
                    obj.Department = MapDropdown(dsDrop, 3, "DepartmentId", "DepartmentName", "Select Department/ Section");

                    obj.NABL_option = new List<Options_List>();

                    if (dsDrop != null && dsDrop.Tables.Count > 4 && dsDrop.Tables[4].Rows.Count > 0)
                    {
                        obj.NABL_option = dsDrop.Tables[4].AsEnumerable().Select(row => new Options_List
                        {
                            id = ToInt(row["NABL_OptionId"]), // 🔥 FIXED
                            title = row["nabl_OptionName"]?.ToString()
                        }).ToList();
                    }
                }


                return obj;
            }
            catch
            {
                throw;
            }
        }

        public TestDirectory Search_Test_Directory_BAL(Test_Detail Detailobj)
        {
            var obj = new TestDirectory();

            try
            {
                // 🔹 STEP 1: FETCH MAIN DATA
                using (var ds = Search_Test_Directory_DAL(Detailobj))
                {
                    obj.Tests = new List<Test>();

                    // ✅ TABLE 0 → DATA
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        obj.Tests = ds.Tables[0].AsEnumerable().Select(row => new Test
                        {
                            Title = row["TestName"]?.ToString(),
                            Specimen = row["SpecimenName"]?.ToString(),
                            TypeTest = row["TestTypeName"]?.ToString(),
                            Organ = row["OrganName"]?.ToString(),
                            Department = row["DepartmentName"]?.ToString(),
                            covered_under_NABL = ToInt(row["NABL_Option"]) // 🔥 FIXED
                        }).ToList();
                    }

                    // ✅ TABLE 1 → COUNT
                    if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[1].Rows)
                        {
                            int option = ToInt(row["NABL_Option"]); // 🔥 FIXED
                            int count = ToInt(row["cnt"]);          // 🔥 FIXED

                            if (option == 1)
                                obj.NABL_option_1_count = count;
                            else if (option == 2)
                                obj.NABL_option_2_count = count;
                        }
                    }
                }

                // 🔹 STEP 2: LOAD DROPDOWNS (ONLY PAGE LOAD)

                using (var dsDrop = Fetch_Tests_Details_DAL())
                {
                    obj.Specimen = MapDropdown(dsDrop, 0, "SpecimenId", "SpecimenName", "Select Specimen");
                    obj.TypeTest = MapDropdown(dsDrop, 1, "TestTypeId", "TestTypeName", "Select type of test");
                    obj.Organ = MapDropdown(dsDrop, 2, "OrganId", "OrganName", "Select Organ");
                    obj.Department = MapDropdown(dsDrop, 3, "DepartmentId", "DepartmentName", "Select Department/ Section");

                    obj.NABL_option = new List<Options_List>();

                    if (dsDrop != null && dsDrop.Tables.Count > 4 && dsDrop.Tables[4].Rows.Count > 0)
                    {
                        obj.NABL_option = dsDrop.Tables[4].AsEnumerable().Select(row => new Options_List
                        {
                            id = ToInt(row["NABL_OptionId"]), // 🔥 FIXED
                            title = row["nabl_OptionName"]?.ToString()
                        }).ToList();
                    }
                }


                return obj;
            }
            catch
            {
                throw;
            }
        }

        private List<Options_List> MapDropdown(DataSet ds, int tableIndex, string idCol, string textCol, string defaultTitle = "Select")
        {
            var list = new List<Options_List>
            {
                new Options_List { id = 0, title = defaultTitle }   // ✅ dynamic
            };

            if (ds != null && ds.Tables.Count > tableIndex && ds.Tables[tableIndex].Rows.Count > 0)
            {
                list.AddRange(ds.Tables[tableIndex].AsEnumerable().Select(row => new Options_List
                {
                    id = ToInt(row[idCol]),
                    title = row[textCol]?.ToString()
                }));
            }

            return list;
        }

        private int ToInt(object value)
        {
            if (value == null || value == DBNull.Value)
                return 0;

            int result;
            return int.TryParse(value.ToString(), out result) ? result : 0;
        }



        public TestDirectory Search_Test_Directory_ByNABL_BAL(Test_Detail detail, int pageNo, int pageSize, int nablOption)
        {
            var obj = new TestDirectory();

            var ds = Search_Test_Directory_ByNABL_DAL(detail, pageNo, pageSize, nablOption);

            obj.Tests = new List<Test>();

            // 🔹 TABLE 0 → DATA
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    obj.Tests.Add(new Test()
                    {
                        Title = row["TestName"]?.ToString(),
                        Specimen = row["SpecimenName"]?.ToString(),
                        TypeTest = row["TestTypeName"]?.ToString(),
                        Organ = row["OrganName"]?.ToString(),
                        Department = row["DepartmentName"]?.ToString(),
                        covered_under_NABL = Convert.ToInt32(row["NABL_Option"])
                    });
                }
            }

            // 🔹 TABLE 1 → TOTAL COUNT
            if (ds.Tables[1].Rows.Count > 0)
            {
                obj.TotalCount = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]);
            }

            return obj;
        }



        public TestDirectory Fetch_Tests_Details_BAL()
        {
            TestDirectory obj = new();
            try
            {
                obj = Test_Directory_pageload();
                return obj;
            }
            catch { throw; }

        }

        private TestDirectory Test_Directory_pageload()
        {
            TestDirectory obj = new();
            DataSet? ds = new();
            try
            {
                ds = Fetch_Tests_Details_DAL();
                obj.Specimen = [new Options_List { id = 0, title = "Select Specimen" }];
                obj.TypeTest = [new Options_List { id = 0, title = "Select type of test" }];
                obj.Organ = [new Options_List { id = 0, title = "Select Organ" }];
                obj.Department = [new Options_List { id = 0, title = "Select Department/ Section" }];
                obj.NABL_option = new();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        obj.Specimen.Add(new Options_List { id = Convert.ToInt32(row["SpecimenId"].ToString()), title = row["SpecimenName"].ToString() });
                    }
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        obj.TypeTest.Add(new Options_List { id = Convert.ToInt32(row["TestTypeId"].ToString()), title = row["TestTypeName"].ToString() });
                    }
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[2].Rows)
                    {
                        obj.Organ.Add(new Options_List { id = Convert.ToInt32(row["OrganId"].ToString()), title = row["OrganName"].ToString() });
                    }
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[3].Rows)
                    {
                        obj.Department.Add(new Options_List { id = Convert.ToInt32(row["DepartmentId"].ToString()), title = row["DepartmentName"].ToString() });
                    }
                }
                if (ds.Tables[4].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[4].Rows)
                    {
                        obj.NABL_option.Add(new Options_List { id = Convert.ToInt32(row["NABL_OptionId"].ToString()), title = row["nabl_OptionName"].ToString() });
                    }
                }
                return obj;
            }
            catch { throw; }
            finally
            {
                ds = null;
            }
        }

        protected TestDirectory Search_Test_Directory_BAL(Test_Detail Detailobj, int pageno = 1, int pagesize = 4)
        {

            TestDirectory obj = new();
            DataSet? ds = new();
            try
            {
                ds = Search_Test_Directory_DAL(Detailobj, pageno, pagesize);

                obj.Tests = new();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        obj.Tests.Add(new Test()
                        {
                            Title = row["TestName"].ToString(),
                            Specimen = row["SpecimenName"].ToString(),
                            TypeTest = row["TestTypeName"].ToString(),
                            Organ = row["OrganName"].ToString(),
                            Department = row["DepartmentName"].ToString(),
                            covered_under_NABL = Convert.ToInt32(row["NABL_Option"].ToString())
                        });
                    }
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    obj.NABL_option_1_count = Convert.ToInt32(ds.Tables[1].Rows[0]["cnt"].ToString());
                    obj.NABL_option_2_count = Convert.ToInt32(ds.Tables[1].Rows[1]["cnt"].ToString());
                }
                ds = Fetch_Tests_Details_DAL();
                TestDirectory obj1 = Test_Directory_pageload();
                obj.Specimen = obj1.Specimen;
                obj.TypeTest = obj1.TypeTest;
                obj.Department = obj1.Department;
                obj.Organ = obj1.Organ;
                obj.NABL_option = obj1.NABL_option;
                return obj;
            }
            catch { throw; }
            finally
            {
                ds = null;
            }
        }

    }
}