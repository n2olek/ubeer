using System.Web;
using System.Web.Http;
using UBeer.Models;

namespace UBeer.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api")]
    public class LoginController : BaseAPIController
    {
        /// <summary>
        /// Login
        /// </summary>
        /// <returns>status 0,1</returns>
        /// <returns>message</returns>
        [Route("Login")]
        [HttpPost]
        public IHttpActionResult Login()
        {
            var httpRequest = HttpContext.Current.Request;
            string username = httpRequest.Form["user"].ToString();
            string password = httpRequest.Form["pass"].ToString();

            JSONTemplate result = new JSONTemplate();
            if (IsLoginSuccess(username, password))
            {
                result.status = 1;
                result.message = WebContext.Token;
            }
            else
            {
                result.status = 0;
                result.message = "Invalid username or password";
            }
            return JsonResp(result);
        }

        private bool IsLoginSuccess(string username, string password)
        {
            string correctUsername = WebContext.Username;
            string correctPassword = WebContext.Password;

            return (
                        username == correctUsername &&
                        password == correctPassword
                    );
        }
    }
}
