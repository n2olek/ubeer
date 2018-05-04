using S9.Utility;

namespace UBeer.FeedMonitor
{
    /// <summary>
    /// 
    /// </summary>
    public class AppContext
    {                
        /// <summary>
        /// 
        /// </summary>
        public static string FacebookToken => ConfigService.GetValue("FacebookToken");

        /// <summary>
        /// 
        /// </summary>
        public static long FacebookFanpageID => ConfigService.GetValueAsLong("FacebookFanpageID");

        /// <summary>
        /// 
        /// </summary>
        public static string TwitterConsumerKey => ConfigService.GetValue("TwitterConsumerKey");

        /// <summary>
        /// 
        /// </summary>
        public static string TwitterConsumerSecret => ConfigService.GetValue("TwitterConsumerSecret");

        /// <summary>
        /// 
        /// </summary>
        public static string TwitterAccessToken => ConfigService.GetValue("TwitterAccessToken");

        /// <summary>
        /// 
        /// </summary>
        public static string TwitterAccessTokenSecret => ConfigService.GetValue("TwitterAccessTokenSecret");


        /// <summary>
        /// 
        /// </summary>
        //public static string HashTag => ConfigService.GetValue("HashTag");

        /// <summary>
        /// 
        /// </summary>
        public static int LimitPostCheckDeleted => ConfigService.GetValueAsInt("LimitPostCheckDeleted");

        /// <summary>
        /// 
        /// </summary>
        public static string EmailForSendError => ConfigService.GetValue("EmailForSendError");

        /// <summary>
        /// 
        /// </summary>
        public static string SubjectEmailError => ConfigService.GetValue("SubjectEmailError");

    }
}

