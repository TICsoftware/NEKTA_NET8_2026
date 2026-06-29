
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using Nekta_BusinessLogic.Entity;

namespace Nekta_BusinessLogic
{
    public class Test_Directory_DAL : DBHelper
    {
        public Test_Directory_DAL(IConfiguration configuration) : base(configuration) 
        {
        }

        protected DataSet Fetch_Tests_Details_DAL()
        {
            return GetDataSet("Fetch_Tests_Details");
        }

        protected DataSet GetTestDirectoryContent_DAL(Test_Detail obj, string pageName, int LanguageID, int GeographyID)
        {
            SqlParameter[] Sqlparam = new SqlParameter[5];
            try
            {
                Sqlparam = [
                    new ("@pagename", string.IsNullOrWhiteSpace( pageName) ? null : pageName.Trim()),
                    new ("@LanguageID", LanguageID >0 ? LanguageID : null),
                    new ("@GeographyID", GeographyID >0 ? GeographyID : null),
                    new ("@searchtext",string.IsNullOrWhiteSpace( obj.Title) ? null : obj.Title.Trim()),
                    new ("@Specimen_Id",  obj.Specimen_Id > 0 ? obj.Specimen_Id :null  ),
                    new ("@TypeText_Id", obj.TypeTestId > 0 ?obj.TypeTestId:null ) ,
                    new ("@Organ_Id", obj.OrganId >0 ? obj.OrganId : null),
                    new ("@Department_Id", obj.DepartmentId > 0 ? obj.DepartmentId : null),
                ];
                return GetDataSet("GetTestDirectoryContent", Sqlparam);
            }
            catch
            {
                throw;
            }
        }

        protected DataSet Search_Test_Directory_DAL(Test_Detail obj, int pageno = 1, int pagesize = 10)
        {
            SqlParameter[] Sqlparam = new SqlParameter[5];
            try
            {
                Sqlparam = [
                    new ("@searchtext",string.IsNullOrWhiteSpace( obj.Title) ? null : obj.Title.Trim()),
                    new ("@Specimen_Id",  obj.Specimen_Id > 0 ? obj.Specimen_Id :null  ),
                    new ("@TypeText_Id", obj.TypeTestId > 0 ?obj.TypeTestId:null ) ,
                    new ("@Organ_Id", obj.OrganId >0 ? obj.OrganId : null),
                    new ("@Department_Id", obj.DepartmentId > 0 ? obj.DepartmentId : null),
                ];
                return GetDataSet("Search_MedicalTests", Sqlparam);
            }
            catch
            {
                throw;
            }
        }

        protected DataSet Search_Test_Directory_ByNABL_DAL(Test_Detail obj, int pageno, int pagesize, int NABL_Option)
        {
            SqlParameter[] Sqlparam = new SqlParameter[8];
            try
            {
                Sqlparam = [
                    new ("@searchtext",string.IsNullOrWhiteSpace( obj.Title) ? null : obj.Title.Trim()),
                    new ("@Specimen_Id",  obj.Specimen_Id > 0 ? obj.Specimen_Id :null  ),
                    new ("@TypeText_Id", obj.TypeTestId > 0 ?obj.TypeTestId:null ) ,
                    new ("@Organ_Id", obj.OrganId >0 ? obj.OrganId : null),
                    new ("@Department_Id", obj.DepartmentId > 0 ? obj.DepartmentId : null),
                    new ("@pageno",pageno),
                    new ("@pagesize", pagesize),
                    new ("@NABL_Option", NABL_Option),
                ];
                return GetDataSet("Search_MedicalTests_ByNABL", Sqlparam);
            }
            catch
            {
                throw;
            }
        }



    }

}