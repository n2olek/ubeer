// *** To use log, make sure there's "Log" folder with the write permission in the target folder
#define USE_LOG

using System;
//using System.Collections.Generic;
using System.Text;
using System.Web;

// Logger - NLog (must have NLog.confg file in the app project, need to copy to bin folder as well
using NLog;


namespace S9.Utility
{
    public class LogUtil
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();
        //public static Logger Logger;

        /*/
        public static void init()
        { 
            Logger = LogManager.GetCurrentClassLogger();
        }
        /*/


        public static void WriteErrorToLog(Exception ex)
        {
            if (null == ex) return;

            var exm = ex.Message;
            var exim = "n/a";
            var exim_inner = "n/a";

            if (null != ex.InnerException)
            {
                exim = ex.InnerException.Message;

                if (null != ex.InnerException.InnerException)
                    exim_inner = ex.InnerException.InnerException.Message;
            }

#if USE_LOG
            Logger.Error("[Exception] {0} [InnerException] {1} [InnerException2] {2}\r\n [StackTrace] {3}\r\n", exm, exim, exim_inner, ex.ToString());
#endif            
        }

        public static void Info(string format, params object[] args)
        {
            string logtext = "";
            logtext += string.Format(format, args);

#if USE_LOG
            Logger.Info(logtext);
#endif
        }

        public static void InfoWithIP(string format, params object[] args)
        {
            string logtext = "";
            logtext += "IP:" + getIPAddress(HttpContext.Current.Request) + " ";
            logtext += string.Format(format, args);

#if USE_LOG
            Logger.Info(logtext);
#endif
        }


        
        public static void InfoInternal(string format, params object[] args)
        {
            var logger = LogManager.GetLogger("internal");
            string logtext = "";
            logtext += "IP:" + getIPAddress(HttpContext.Current.Request) + " ";
            logtext += string.Format(format, args);

#if USE_LOG
            logger.Info(logtext);
#endif
        }
                
        public static string getIPAddress(HttpRequest request)
        {
            string szRemoteAddr = request.UserHostAddress;
            string szXForwardedFor = request.ServerVariables["X_FORWARDED_FOR"];
            string szIP = "";

            if (szXForwardedFor == null)
            {
                szIP = szRemoteAddr;
            }
            else
            {
                szIP = szXForwardedFor;
                if (szIP.IndexOf(",") > 0)
                {
                    string[] arIPs = szIP.Split(',');

                    foreach (string item in arIPs)
                    {
                        if (!isIPLocal(item))
                        {
                            return item;
                        }
                    }
                }
            }
            return szIP;
        }

        private static bool isIPLocal(string ipaddress)
        {
            String[] straryIPAddress = ipaddress.Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            int[] iaryIPAddress = new int[] { int.Parse(straryIPAddress[0]), int.Parse(straryIPAddress[1]), int.Parse(straryIPAddress[2]), int.Parse(straryIPAddress[3]) };
            if (iaryIPAddress[0] == 10 || (iaryIPAddress[0] == 192 && iaryIPAddress[1] == 168) || (iaryIPAddress[0] == 172 && (iaryIPAddress[1] >= 16 && iaryIPAddress[1] <= 31)))
            {
                return true;
            }
            else
            {
                // IP Address is "probably" public. This doesn't catch some VPN ranges like OpenVPN and Hamachi.
                return false;
            }
        }
    }
}
