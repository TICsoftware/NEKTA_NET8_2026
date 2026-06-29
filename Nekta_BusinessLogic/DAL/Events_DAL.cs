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
    public class Events_DAL : DBHelper
    {
        public Events_DAL(IConfiguration configuration) : base(configuration) //  call base class constructor 
        {
        }

        public DataSet GetEventsContent_DAL(string pagename, int LanguageID, int GeographyID)
        {
            SqlParameter[] sqlParams =
            {
                new ("@pagename", string.IsNullOrWhiteSpace( pagename) ? null : pagename.Trim()),
                new ("@LanguageID", LanguageID >0 ? LanguageID : null),
                new ("@GeographyID", GeographyID >0 ? GeographyID : null),
            };

            return GetDataSet("GetEventsContent", sqlParams);
        }


        public DataSet Search_Past_Events_DAL(int EventTypeId, int EventModeId, DateTime? DateSearch)
        {
            SqlParameter[] Sqlparam = new SqlParameter[5];
            try
            {
                Sqlparam = [
                    new ("@EventTypeId",  EventTypeId > 0 ? EventTypeId : null  ),
                    new ("@EventModeId", EventModeId > 0 ? EventModeId : null ) ,
                    new ("@DateSearch", DateSearch != DateTime.MinValue ? (object)DateSearch : DBNull.Value),
                ];
                return GetDataSet("Get_Past_Events_Search", Sqlparam);
            }
            catch
            {
                throw;
            }
        }


        public DataSet GetEventsById_DAL(int EventId)
        {
            SqlParameter[] Sqlparam = new SqlParameter[5];
            try
            {
                Sqlparam = [
                    new ("@EventId",  EventId > 0 ? EventId : null  ),
                ];
                return GetDataSet("GetEventsById", Sqlparam);
            }
            catch
            {
                throw;
            }
        }

        protected DataTable Events_Registration_Insert_DAL(Entity.Register_Event obj)
        {
            DataTable dt=new();
            SqlParameter[] Sqlparam = new SqlParameter[5];
            try
            {
                Sqlparam[0] = new SqlParameter("@Event_Id", obj.EventId);
                Sqlparam[1] = new SqlParameter("@full_name", obj.FullName ?? (object)DBNull.Value);  
                Sqlparam[2] = new SqlParameter("@Email", obj.Email_Id ?? (object)DBNull.Value);
                Sqlparam[3] = new SqlParameter("@Contact", obj.Contact_number ?? (object)DBNull.Value); 
                Sqlparam[4] = new SqlParameter("@Designation", obj.Designation ?? (object)DBNull.Value); 

               dt= GetDataSet("Events_Registration_Insert", Sqlparam).Tables[0];
               return dt;
            }
            catch
            {
                throw;
            }
        }




    }
}