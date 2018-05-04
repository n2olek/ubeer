#define TEST

using System.Web.Http;
using WebActivatorEx;
using UBeer.API;
using Swashbuckle.Application;


#if TEST
[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace UBeer.API
{
    /// <summary>
    /// 
    /// </summary>
    public class SwaggerConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public static void Register()
        {
            var thisAssembly = typeof (SwaggerConfig).Assembly;
        }
    }
}
#endif