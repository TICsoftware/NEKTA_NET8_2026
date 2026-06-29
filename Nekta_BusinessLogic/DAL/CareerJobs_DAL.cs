
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml;

namespace Nekta_BusinessLogic
{
    public class CareerJobs_DAL(IConfiguration configuration) : DBHelper(configuration)
    {
        protected DataSet Get_Career_Jobs_DAL(int pageno, int pagesize, string? searchkeywords = "")
        {
            if (string.IsNullOrWhiteSpace(searchkeywords))
            {
                return GetDataSet("Job_careers_List", "@pageno", pageno.ToString(), "@pagesize", pagesize.ToString());
            }
            else
            {
                return GetDataSet("Job_careers_List", "@pageno", pageno.ToString(), "@pagesize", pagesize.ToString(), "@searchkeywords", searchkeywords);
            }
        }


        protected DataTable Job_Application_DAL(Application_Details obj)
        {

            SqlParameter[] Sqlparam = new SqlParameter[12];
            try
            {
                Sqlparam[0] = new SqlParameter("@Job_Id", obj.Job_id);
                Sqlparam[1] = new SqlParameter("@applicant_name", obj.Name ?? (object)DBNull.Value);
                Sqlparam[2] = new SqlParameter("@Age", obj.Age ?? (object)DBNull.Value);
                Sqlparam[3] = new SqlParameter("@Gender", obj.Gender ?? (object)DBNull.Value);
                Sqlparam[4] = new SqlParameter("@Email", obj.Email ?? (object)DBNull.Value);
                Sqlparam[5] = new SqlParameter("@Contact", obj.Contact ?? (object)DBNull.Value);
                Sqlparam[6] = new SqlParameter("@Current_Salary", obj.Current_Salary ?? (object)DBNull.Value);
                Sqlparam[7] = new SqlParameter("@Notice_Period", obj.Notice_Period ?? (object)DBNull.Value);
                Sqlparam[8] = new SqlParameter("@Location", obj.Location ?? (object)DBNull.Value);
                Sqlparam[9] = new SqlParameter("@Relocate_job", obj.Relocate_job ?? (object)DBNull.Value);
                Sqlparam[10] = new SqlParameter("@Applicant_Message", obj.Message ?? (object)DBNull.Value);
                Sqlparam[11] = new SqlParameter("@Attachment", obj.attachment ?? (object)DBNull.Value);

                return GetDataSet("Job_application_insert", Sqlparam).Tables[0];
            }
            catch
            {
                throw;
            }
        }

    }


}