using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomsoft.BusinessLogic.Configuration
{
    public class LuceedConfig
    {
        public const string ConfigName = "Luceed";

        public string? BaseUrl { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

        public string GetValueForBAsicAuthHeader()
        {
            if(string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                throw new ArgumentNullException("Username or password is null");
            }

            var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Username}:{Password}"));
            return base64EncodedAuthenticationString;
        }
    }
}
