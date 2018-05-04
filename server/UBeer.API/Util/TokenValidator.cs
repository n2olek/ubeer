using System;
using System.Linq;
using System.Net.Http;

namespace UBeer.API
{
    /// <summary>
    /// Image Manager
    /// </summary>
    public class TokenValidator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string ExtractToken(HttpRequestMessage msg)
        {
            string token = "";
            
            // read from the HTTP header                
            try
            {
                token = msg.Headers.GetValues("AuthorizationToken").FirstOrDefault();
            }
            catch { }

            // read from the Query string                
            if (token == "")
            {
                try
                {
                    var queryStrings = msg.GetQueryNameValuePairs();
                    if (queryStrings == null)
                        return null;

                    var match = queryStrings.FirstOrDefault(kv => string.Compare(kv.Key, "AuthorizationToken", true) == 0);
                    if (!string.IsNullOrEmpty(match.Value))
                        token = match.Value;

                }
                catch { }
            }

            return String.IsNullOrEmpty(token) ? "" : token;
        }


        /// <summary>
        /// validate token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsValid(string token)
        {         
            return (token == WebContext.Token);
        }
    }
}
