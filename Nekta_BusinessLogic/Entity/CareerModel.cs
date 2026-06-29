namespace Nekta_BusinessLogic
{
    public class Job
    {
        public int Job_Id { get; set; }
        public string? Encrypt_job_Id { get; set; }
        public string? Role { get; set; }
        public string? Education { get; set; }
        public string? Experience { get; set; }
        public string? Job_Description { get; set; }
        public string? Location { get; set; }
        public string? Salary_range { get; set; }
        public string? About_the_Role { get; set; }
        public string? Workmode { get; set; }
        public DateTime? Expiry_date { get; set; }
        public DateTime? Created_date { get; set; }
    }

    public class JobModel
    {
        public List<Job> Jobs { get; set; }
        public int TotalRecords { get; set; }
    }

    public class Application_Details
    {
        public string? Name { get; set; }
        public string? Age { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public string? Contact { get; set; }
        public string? Current_Salary { get; set; }
        public string? Notice_Period { get; set; }
        public string? Location { get; set; }
        public string? Relocate_job { get; set; }
        public string? Message { get; set; }
        public string? attachment { get; set; }
        public int Job_id { get; set; }
    }
}