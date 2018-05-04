using System.Collections.Generic;
using System.Web.Mvc;
using UBeer.Models;
using UBeer.Services;

namespace UBeer.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class TestController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            List<Models.Post> posts = new List<Models.Post>();
            using (Entities db = new Entities())
            {
                // get the first record
                PostService postService = new PostService(db);
                posts = postService.GetPosts(20);
            }
            ViewBag.count = posts.Count;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult TestUpload()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult TestExport()
        {
            return View();
        }

    }

}
