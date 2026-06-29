using Microsoft.Extensions.Configuration;

namespace Nekta_BusinessLogic
{
    public class Forms : Forms_DAL
    {
        public Forms(IConfiguration configuration)
        : base(configuration) //  call base class constructor 
        {
        }

        public void Contact_Forms_Insert(Form_Fields objforms)
        {
            Contact_Forms_Insert_DAL(objforms);
        }

    }
}