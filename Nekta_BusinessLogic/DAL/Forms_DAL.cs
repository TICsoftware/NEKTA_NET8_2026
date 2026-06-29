
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace Nekta_BusinessLogic
{
    public class Forms_DAL : DBHelper
    {
        public Forms_DAL(IConfiguration configuration)
        : base(configuration) //  call base class constructor 
        {
        }

        protected void Contact_Forms_Insert_DAL(Form_Fields objforms)
        {
            SqlParameter[] Sqlparam = new SqlParameter[8];
            try
            {
                Sqlparam = [
                    new ("@fullname",string.IsNullOrWhiteSpace( objforms.FullName) ? string.Empty : objforms.FullName.Trim()),
                    new ("@email_Id", string.IsNullOrWhiteSpace(objforms.Email)? string.Empty : objforms.Email.Trim()),
                    new ("@contact_number", string.IsNullOrWhiteSpace( objforms.Contact) ? string.Empty : objforms.Contact.Trim()) ,
                    new ("@City", string.IsNullOrWhiteSpace(objforms.City) ? string.Empty : objforms.City.Trim()),
                    new ("@Labname", string.IsNullOrWhiteSpace(objforms.Labname) ? string.Empty : objforms.Labname.Trim()),
                    new ("@Message",string.IsNullOrWhiteSpace( objforms.Message) ? string.Empty : objforms.Message.Trim()),
                    new ("@attachment", string.IsNullOrWhiteSpace(objforms.attachment_path) ? string.Empty : objforms.attachment_path.Trim()),
                    new ("@form_type", string.IsNullOrWhiteSpace(objforms.form_type) ? string.Empty : objforms.form_type.Trim()),
                    new ("@enquiry_type", string.IsNullOrWhiteSpace(objforms.Enquiry_Type) ? string.Empty : objforms.Enquiry_Type.Trim())
                ];
                SQLInsert_Update_Delete_Data("Contact_forms_Insert", Sqlparam);
            }
            catch
            {
                throw;
            }
        }

    }
}