using System;
using System.Configuration;
using System.Net.Configuration;
using System.Text;
using System.Web;

namespace S9.Utility
{
    public class UserAgent
    {
        private string UserAgentText = "";
        
        // constructor
        public UserAgent(string userAgentText)
        {
            if(string.IsNullOrEmpty(userAgentText))
                UserAgentText = "";

            UserAgentText = userAgentText;
        }

        public bool IsiOS
        {
            get
            {
                if (!IsWindowsPhone)
                {
                    return UserAgentText.ToUpper().Contains("IPHONE") || UserAgentText.ToUpper().Contains("IPAD");
                }
                return false;
            }
        }

        public bool IsAndroid
        {
            get{            
                if (!IsWindowsPhone)
                {
                    return UserAgentText.ToUpper().Contains("ANDROID");
                }
                return false;
            }
        }

        public bool IsWindowsPhone
        {
            get
            {            
                if (UserAgentText.ToUpper().Contains("WINDOWS PHONE"))
                {
                    return true;
                }
                return false;
            }
        }
    }
}