using System.Web.Http;
using System.Net.Http;
using System.Text;

using S9.Utility;

namespace UBeer.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseAPIController : ApiController
    {
        /// <summary>
        /// function that return object as json
        /// </summary>
        /// <param name="result"></param>
        /// <returns>IHttpActionResult</returns>
        public IHttpActionResult JsonResp(object result)
        {
            //init HttpResponseMessage 
            var resp = new HttpResponseMessage()
            {
                //Serialize object to json format and encoding as UTF8
                Content = new StringContent(JSON<string>.Serialize(result), Encoding.UTF8, "application/json")
            };
            return ResponseMessage(resp);
        }
    }
}
