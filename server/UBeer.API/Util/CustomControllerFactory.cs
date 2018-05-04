using PagedList;

using System;
using System.Linq;
//using System.Collections.Generic;
//using System.Configuration;
using System.IO;

using System.Web;

using System.Reflection;
using System.Web.Routing;
using System.Web.SessionState;

using System.Web.Mvc;


// *** This class's purpose is to enable us to disable session in method level 
// the session will slow down the web, as MVC will block concurrent requests from the same session, 
// to speed things up we need to disable the session.

// Normally, we can disable the session in the class level but doing so affects the whole site
// To narrow down the session restriction to only some methods, 
// we have to create our own MVC controller class with attribute to enable/disable session for each method
// And to introduce our controller to MVC, we have to create our own controller factory as well.

namespace UBeer.API
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class ActionSessionStateAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public SessionStateBehavior Behavior { get; private set; } 
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="behavior"></param>
        public ActionSessionStateAttribute(SessionStateBehavior behavior)
        {
            this.Behavior = behavior;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class CustomControllerFactory : DefaultControllerFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        protected override SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return SessionStateBehavior.Default;
            }

            try
            {
                var actionName = requestContext.RouteData.Values["action"].ToString();
                MethodInfo actionMethodInfo = controllerType.GetMethod(actionName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (actionMethodInfo != null)
                {
                    var actionSessionStateAttr = actionMethodInfo.GetCustomAttributes(typeof(ActionSessionStateAttribute), false)
                                        .OfType<ActionSessionStateAttribute>()
                                        .FirstOrDefault();

                    if (actionSessionStateAttr != null)
                    {
                        return actionSessionStateAttr.Behavior;
                    }
                }
            }
            catch (AmbiguousMatchException ex)
            {
                // debugging purpose
                string str = ex.Message;
            }


            return base.GetControllerSessionBehavior(requestContext, controllerType);
        }
    }

}