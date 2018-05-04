using AutoMapper;
using S9.Utility;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using UBeer.Models;
using UBeer.Services;

namespace UBeer.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api")]
    public class ExportController : BaseAPIController
    {
        IMapper mapper;
        /// <summary>
        /// ExportExcel
        /// </summary>
        /// <returns>export url to excel</returns>
        [Route("ExportExcel")]
        [TokenValidatorAttribute]
        [HttpGet]
        public HttpResponseMessage ExportExcel()
        {
            string fileName = WebContext.ExportFileName;
            string sheetName = WebContext.ExportSheetName;

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Models.Post, PostModel>();
            });
            mapper = config.CreateMapper();

            using (Entities db = new Entities())
            {
                PostService postService = new PostService(db);
                List<Models.Post> posts = postService.GetPosts();

                List<PostModel> postModels = mapper.Map<List<Models.PostModel>>(posts);

                ExcelGenerator<PostModel> excel = new ExcelGenerator<PostModel>(fileName, sheetName, postModels);

                System.IO.MemoryStream s = excel.GetStream();

                // processing the stream.
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(s.ToArray())
                };
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                return result;
            }

        }

    }
}
