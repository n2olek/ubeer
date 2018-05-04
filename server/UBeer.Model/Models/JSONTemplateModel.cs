using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

namespace UBeer.Models
{
    public static class API_RESULT
    {
        public static string OK = "OK";
        public static string FAIL = "FAIL";
    }

    public class JSONTemplate
    {
        public int status { get; set; }
        public string message { get; set; }
        public string version
        {
            get
            {
                //return ConfigurationManager.AppSettings["APP_Version"];
                return "1.0";
            }
        }
        public object data { get; set; }
    }


    public class ViewModelTemplate
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Param { get; set; }
        /*/
        public string Version
        {
            get
            {
                return ConfigurationManager.AppSettings["APP_Version"];
            }
        }
        public object data { get; set; }
        /*/
    }

}
