using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using Microsoft.Data.SqlClient;

namespace Nekta_BusinessLogic.DAL
{
    public class Updates_DAL : DBHelper
    {
        public Updates_DAL(IConfiguration configuration) : base(configuration) //  call base class constructor 
        {
        }

        public DataSet GetContent_DAL(string pagename, int LanguageID, int GeographyID)
        {
            SqlParameter[] sqlParams =
            {
                new ("@pagename", string.IsNullOrWhiteSpace( pagename) ? null : pagename.Trim()),
                new ("@LanguageID", LanguageID >0 ? LanguageID : null),
                new ("@GeographyID", GeographyID >0 ? GeographyID : null),
            };

            return GetDataSet("GetContent", sqlParams);
        }

        public DataSet Get_UpdatesArticles_load_DAL(int cont_id, int PageNumber = 1, int PageSize = 10)
        {
            try
            {
                SqlParameter[] sqlParam =
                {
                    new("@cont_id", cont_id),
                    new("@PageNumber", PageNumber),
                    new("@PageSize", PageSize)
                };

                return GetDataSet("Get_UpdatesArticles_load", sqlParam);
            }
            catch
            {
                throw;
            }
        }


        public DataSet Get_AnnualReport_ByYear_DAL(int cont_id, int FinancialYear = 0, int PageNumber = 1, int PageSize = 10)
        {
            try
            {
                SqlParameter[] sqlParam =
                {
                    new("@cont_id", cont_id),
                    new("@FinancialYear", FinancialYear > 0 ? FinancialYear: DBNull.Value),
                    new("@PageNumber", PageNumber),
                    new("@PageSize", PageSize)
                };

                return GetDataSet("Get_AnnualReport_ByYear", sqlParam);
            }
            catch
            {
                throw;
            }
        }


    }
}