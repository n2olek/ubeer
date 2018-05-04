using S9.Utility;

namespace UBeer.Housekeeper
{
    /// <summary>
    /// 
    /// </summary>
    public class AppContext
    {
        /// <summary>
        /// 
        /// </summary>
        public static int LimitPostCheckDeleted => ConfigService.GetValueAsInt("LimitPostCheckDeleted");

        /// <summary>
        /// 
        /// </summary>
        public static string PathLogHousekeeper => ConfigService.GetValue("PathLogHousekeeper");

        /// <summary>
        /// 
        /// </summary>
        public static string PathLogAPI => ConfigService.GetValue("PathLogAPI");

        /// <summary>
        /// 
        /// </summary>
        public static string PathLogFeedMonitoring => ConfigService.GetValue("PathLogFeedMonitoring");

        /// <summary>
        /// 
        /// </summary>
        public static string PrefixFileNameLog => ConfigService.GetValue("PrefixFileNameLog");

        /// <summary>
        /// 
        /// </summary>
        public static int DeleteFileDurationInMonth => ConfigService.GetValueAsInt("DeleteFileDurationInMonth");
    }
}

