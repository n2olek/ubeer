using System;
using System.Web;
using S9.Utility;

namespace UBeer.API
{
    /// <summary>
    /// 
    /// </summary>
    public class WebContext
    {
        /// <summary>
        /// 
        /// </summary>
        public static string TempRelativeURL => ConfigService.GetValue("TempRelativeURL");
        
        /// <summary>
        /// 
        /// </summary>
        public static string TempFilePath => ConfigService.GetValue("TempFilePath");
        
        /// <summary>
        /// 
        /// </summary>
        public static string WebURL => ConfigService.GetValue("WebURL");
        
        /// <summary>
        /// 
        /// </summary>
        public static string WebFilePath => ConfigService.GetValue("WebFilePath");
        
        /// <summary>
        /// 
        /// </summary>
        public static string AppVersion => ConfigService.GetValue("APP_Version");
        
        /// <summary>
        /// 
        /// </summary>
        public static int PageSize => ConfigService.GetValueAsInt("PageSize");
        
        /// <summary>
        /// 
        /// </summary>
        public static int ImageWidth => ConfigService.GetValueAsInt("ImageWidth");
        
        /// <summary>
        /// 
        /// </summary>
        public static int ImageHeight => ConfigService.GetValueAsInt("ImageHeight");

        /// <summary>
        /// 
        /// </summary>
        public static int ImageWidthThumbnail => ConfigService.GetValueAsInt("ImageWidthThumbnail");

        /// <summary>
        /// 
        /// </summary>
        public static int ImageHeightThumbnail => ConfigService.GetValueAsInt("ImageHeightThumbnail");

        /// <summary>
        /// 
        /// </summary>
        public static int ImageSize => ConfigService.GetValueAsInt("ImageSize");

        /// <summary>
        /// 
        /// </summary>
        public static string[] ImageExtension => ConfigService.GetValue("ImageExtension").ToString().Split(',');

        /// <summary>
        /// 
        /// </summary>
        public static int VideoSize => ConfigService.GetValueAsInt("VideoSize");

        /// <summary>
        /// 
        /// </summary>
        public static string[] VideoExtension => ConfigService.GetValue("VideoExtension").ToString().Split(',');

        /// <summary>
        /// 
        /// </summary>
        public static int JPEGCompressRate => ConfigService.GetValueAsInt("JPEGCompressRate");        
        
        /// <summary>
        /// Convert the provided app-relative path into an full physical path 
        /// </summary>
        /// <param name="relativePath">App-Relative path</param>
        /// <returns>Provided relativeUrl parameter as fully qualified Url</returns>
        /// <example>~\path\to\foo to c:\web\path\to\foo</example>
        public static string ToAbsolutePath(string relativePath)
        {
            if (HttpContext.Current == null)
                return relativePath;

            string filepath = HttpContext.Current.Request.PhysicalApplicationPath;        
            relativePath = relativePath.Replace("~", "");

            return filepath + relativePath;
        }

        /// <summary>
        /// Convert the provided app-relative path into an absolute Url containing the 
        /// full host name
        /// </summary>
        /// <param name="relativeUrl">App-Relative path</param>
        /// <returns>Provided relativeUrl parameter as fully qualified Url</returns>
        /// <example>~/path/to/foo to http://www.web.com/path/to/foo</example>
        private static string ToAbsoluteUrl(string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (HttpContext.Current == null)
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            var url = HttpContext.Current.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            return String.Format("{0}://{1}{2}{3}",
                url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }

        /// <summary>
        /// 
        /// </summary>
        public static string ExportFileName => ConfigService.GetValue("ExportFileName");

        /// <summary>
        /// 
        /// </summary>
        public static string ExportSheetName => ConfigService.GetValue("ExportSheetName");

        /// <summary>
        /// 
        /// </summary>
        public static string Username => ConfigService.GetValue("Username");

        /// <summary>
        /// 
        /// </summary>
        public static string Password => ConfigService.GetValue("Password");

        /// <summary>
        /// 
        /// </summary>
        public static string Token => ConfigService.GetValue("Token");
    }
}

