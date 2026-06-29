using System.Data;
using Microsoft.Extensions.Configuration;
using Nekta_BusinessLogic.Entity;

namespace Nekta_BusinessLogic
{
    public class CareerJobs_BAL : CareerJobs_DAL
    {
        public CareerJobs_BAL(IConfiguration configuration)
        : base(configuration) //  call base class constructor 
        {
        }

        public JobModel Career_Jobs_BAL(int pageno, int pagesize, string? searchkeywords)
        {
            DataSet ds = new();
            JobModel obj = new();
            try
            {
                ds = Get_Career_Jobs_DAL(pageno, pagesize, searchkeywords);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    obj.Jobs = [];
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        obj.Jobs.Add(new Job
                        {
                            Encrypt_job_Id = CryptoEngine.Encrypt(dr["Job_id"].ToString()),
                            Job_Id = Convert.ToInt32(dr["Job_Id"].ToString()),
                            Role = dr["Role"].ToString(),
                            Education = dr["Education"].ToString(),
                            Experience = dr["Experience"].ToString(),
                            Job_Description = dr["Job_Description"].ToString(),
                            Location = dr["Location"].ToString(),
                            Salary_range = dr["Salary_range"].ToString(),
                            About_the_Role = dr["About_the_Role"].ToString(),
                            Workmode = dr["Workmode"].ToString(),
                            Expiry_date = dr["Expiry_date"] == DBNull.Value ? null : Convert.ToDateTime(dr["Expiry_date"].ToString()),
                        });
                    }
                    obj.TotalRecords = Convert.ToInt32(ds.Tables[1].Rows[0]["total_count"].ToString());
                }
                else
                    obj.TotalRecords = 0;
                return obj;
            }
            catch
            {
                throw;
            }
        }


        public string Job_Application_BAL(Application_Details obj)
        {
            DataTable dt = new();
            try
            {
                dt = Job_Application_DAL(obj);
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["Role"].ToString();
                }
                else
                { return ""; }
            }
            catch
            {
                throw;
            }
        }


    }
}