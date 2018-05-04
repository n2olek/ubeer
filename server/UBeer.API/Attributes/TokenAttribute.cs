using S9.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using UBeer.Models;

namespace UBeer.API
{
    /// <summary>
    /// TokenValidator
    /// </summary>
    public class TokenValidatorAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public TokenValidatorAttribute()
        {
        }
        /// <summary>
        /// OnActionExecuting
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {

            string token = TokenValidator.ExtractToken(actionContext.Request);

            bool isVerified = TokenValidator.IsValid(token);
            if (isVerified)
                return;

            JSONTemplate result = new JSONTemplate();
            result.status = 0;
                result.message = "Invalid token";

            actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(JSON<string>.Serialize(result), Encoding.UTF8, "application/json")
            };
            return;
        }
    }
}