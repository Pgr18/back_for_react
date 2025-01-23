using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace My_WebApp
{
    public class AuthOptions
    {
        public const string ISSUER = "WebApp_Server";
        public const string AUDIENCE = "WebApp_Client";
        public const string KEY = "020AC56B-5832-42ED-A8F1-2FFDFB1F013C";
        public const int LIFETIME = 1;


        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}

