using S9.Utility;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using UBeer.Models;
using UBeer.Services;

namespace UBeer.API.Controllers
{
    /// <summary>
    /// For the URL UBeer.com/app, redirect to the google play/app store
    /// </summary>
    [RoutePrefix("api")]
    public class UploadController : BaseAPIController
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UploadController()
        {

        }

        /// <summary>
        /// UploadImage
        /// </summary>
        /// <returns>to upload file and save data into database</returns>
        [Route("UploadImage")]
        [HttpPost]
        public IHttpActionResult UploadImage()
        {
            const string THUMBNAIL_FILENAME_SUFFIX = "_t";

            var httpRequest = HttpContext.Current.Request;
            string mobilePhone = httpRequest.Form["mobilePhone"].ToString();
            if (httpRequest.Files.Count < 1)
                return JsonResp(Request.CreateResponse(HttpStatusCode.BadRequest));

            var file = httpRequest.Files[0];

            // check there's file passed to this method
            if (file == null || file.ContentLength <= 0)
                return JsonResp(Request.CreateResponse(HttpStatusCode.BadRequest));

            // Check file type, allow only one of these image formats
            string extension = FileUtil.GetFileExtension(file.FileName).ToLower();
            if (string.IsNullOrEmpty(extension))
                return JsonResp(Request.CreateResponse(HttpStatusCode.BadRequest));

            if (!WebContext.ImageExtension.Contains(extension) && !WebContext.VideoExtension.Contains(extension))
                return JsonResp(Request.CreateResponse(HttpStatusCode.BadRequest));

            IStorage storage = new Storage(WebContext.WebURL, WebContext.WebFilePath);

            // Set the file names
            string tempFilePath = HttpContext.Current.Server.MapPath(WebContext.TempFilePath);
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory + WebContext.WebFilePath;
            LogUtil.Info(filePath);
            string fileName = UniqueGenerator.GenerateGUID();
            string contentType = "";

            if (WebContext.ImageExtension.Contains(extension))
            {
                if (file.ContentLength > WebContext.ImageSize)
                {
                    ViewModelTemplate resultResp = new ViewModelTemplate()
                    {
                        Status = API_RESULT.FAIL,
                        Message = "Your image is over " + (WebContext.ImageSize / 1000000) + " MB"
                    };
                    return JsonResp(resultResp);
                }
                LogUtil.InfoWithIP("The image file name: " + fileName + extension);

                // original version
                if (!SaveAndResizeImage(file, storage, tempFilePath, filePath, fileName, "", extension, WebContext.ImageWidth, WebContext.ImageHeight))
                    LogUtil.Info(" -- Error:: Save image file failed");

                // thumbnail version           
                if (!SaveAndResizeImage(file, storage, tempFilePath, filePath, fileName, THUMBNAIL_FILENAME_SUFFIX, extension, WebContext.ImageWidthThumbnail, WebContext.ImageHeightThumbnail))
                    LogUtil.Info(" -- Error:: Save thumbnail file failed");

                contentType = UBeerEnum.CONTENT_TYPE.Photo.ToString();
            }
            else if (WebContext.VideoExtension.Contains(extension))
            {
                if (file.ContentLength > WebContext.VideoSize)
                {
                    ViewModelTemplate resultResp = new ViewModelTemplate()
                    {
                        Status = API_RESULT.FAIL,
                        Message = "Your VDO is over " + (WebContext.VideoSize / 1000000) + " MB"
                    };
                    return JsonResp(resultResp);
                }

                if (!SaveVideo(file, storage, filePath, fileName + extension))
                    LogUtil.Info(" -- Error:: Save image file failed");

                contentType = UBeerEnum.CONTENT_TYPE.Video.ToString();
            }


            using (Entities db = new Entities())
            {
                PostService postService = new PostService(db);
                MissionService missionService = new MissionService(db);

                MediaPost post = new MediaPost()
                {
                    ID = fileName,
                    Source = UBeerEnum.SOURCE.UploadFile.ToString(),
                    Message = "",
                    CreateDate = (new S9.Utility.Environment()).Now(),
                    LikeCount = 0,
                    CommentCount = 0,
                    MobilePhone = mobilePhone,
                    ContentType = contentType,
                    ImageURL = storage.GetFullURL(fileName + THUMBNAIL_FILENAME_SUFFIX + extension),
                    OriginURL = storage.GetFullURL(fileName + extension),
                    MissionID = missionService.GetMissionID(new S9.Utility.Environment().Now())
                };
                postService.SavePost(post);
            }

            ViewModelTemplate result = new ViewModelTemplate()
            {
                Status = API_RESULT.OK,
                Message = fileName,
                Param = storage.GetFullURL(fileName + extension)
            };

            return JsonResp(result);
        }

        private bool SaveAndResizeImage(HttpPostedFile file, IStorage storage, string sourceImageFilePath, string destImageFilePath, string fileName, string fileNameSuffix, string fileExtension, int width, int height)
        {
            string fullFileName = fileName + fileNameSuffix + fileExtension;
            string sourceFullFilePath = sourceImageFilePath + fullFileName;
            string destFullFilePath = destImageFilePath + fullFileName;

            // prepare folders
            if (!storage.MakeFolderExist(sourceImageFilePath))
                return false;

            if (!storage.MakeFolderExist(destImageFilePath))
                return false;

            // save the file to source folder
            file.SaveAs(sourceFullFilePath);

            // resize
            ImageUtil imageUtil = new ImageUtil();
            imageUtil.ResizeImage(sourceFullFilePath, destFullFilePath, width, height, false, WebContext.JPEGCompressRate);

            // delete the source file
            storage.DeleteFile(sourceFullFilePath);

            return true;
        }

        private bool SaveVideo(HttpPostedFile file, IStorage storage, string destImageFilePath, string fullFileName)
        {
            string destFullFilePath = destImageFilePath + fullFileName;

            // prepare folders
            if (!storage.MakeFolderExist(destImageFilePath))
                return false;

            // save the file
            file.SaveAs(destFullFilePath);

            return true;
        }
    }
}
