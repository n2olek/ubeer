using System;
using System.Collections.Generic;
using System.Web.Http;

using UBeer.Services;
using UBeer.Models;

namespace UBeer.API.Controllers
{

    /// <summary>
    /// About UBeer info. APIs
    /// </summary>
    [RoutePrefix("api")]
    public class PostController : BaseAPIController
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public PostController()
        {
        }

        /// <summary>
        /// Get Posts
        /// </summary>
        /// <param name="pageSize">
        /// </param>
        /// <returns>List of post entries</returns>
        [Route("GetPosts")]
        [HttpGet]
        public IHttpActionResult GetPosts(int? pageSize = null)
        {
            List<Models.Post> posts = new List<Models.Post>();
            using (Entities db = new Entities())
            {
                PostService postService = new PostService(db);
                if (pageSize == null)
                    posts = postService.GetPosts();
                else
                    posts = postService.GetPosts((int)pageSize);
            }
            return JsonResp(posts);
        }

        /// <summary>
        /// Delete Post
        /// </summary>
        /// <returns>Weather delete success or not</returns>
        [Route("DeletePost")]
        [HttpPost]
        [TokenValidatorAttribute]
        public IHttpActionResult DeletePost(string postID)
        {
            JSONTemplate result = new JSONTemplate();
            result.status = 0;

            using (Entities db = new Entities())
            {
                PostService postService = new PostService(db);
                result.status = postService.DeletePost(postID, UBeerEnum.SOURCE.UploadFile.ToString()) ? 1 : 0;
            }
            return JsonResp(result);
        }
    }
}