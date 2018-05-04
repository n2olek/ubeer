using System;
using System.Collections.Generic;
using System.Reflection;
using System.Net;
using System.IO;

using S9.Utility;

using UBeer.Models;
using UBeer.Services;

namespace UBeer.Housekeeper
{
    class Program
    {
        static void Main(string[] args)
        {
            DeleteDeadPosts();

            ClearLog(AppContext.PathLogHousekeeper);
            ClearLog(AppContext.PathLogAPI);
            ClearLog(AppContext.PathLogFeedMonitoring);
        }

        private static void DeleteDeadPosts()
        {
            LogUtil.Info(MethodBase.GetCurrentMethod().DeclaringType.Name + ":" + MethodBase.GetCurrentMethod().Name + "()");

            List<Models.Post> posts = new List<Models.Post>();
            using (Entities db = new Entities())
            {
                PostService postService = new PostService(db);
                posts = postService.GetSocialPosts(AppContext.LimitPostCheckDeleted);

                foreach (Models.Post post in posts)
                {
                    LogUtil.Info("PostURL: " + post.PostURL);
                    if (IsBrokenLink(post.PostURL))
                    {
                        LogUtil.Info("Delete " + post.ID);
                        postService.DeletePost(post.ID, post.SocialType);

                        // no need to check further, move to next post
                        continue;
                    }

                    LogUtil.Info("OriginURL: " + post.OriginURL);
                    if (IsBrokenLink(post.OriginURL))
                    {
                        LogUtil.Info("Delete " + post.ID);
                        postService.DeletePost(post.ID, post.SocialType);
                    }
                }
            }
        }

        private static bool IsBrokenLink(string URL)
        {
            LogUtil.Info(MethodBase.GetCurrentMethod().Name + "()");

            try
            {
                var request = HttpWebRequest.Create(URL);
                request.Method = "HEAD";
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response != null)
                        response.Close();
                }

                // no error, the link is still alive
                return false;
            }
            catch (Exception ex)
            {
                LogUtil.WriteErrorToLog(ex);

                return true;
            }
        }

        private static void ClearLog(string filePath)
        {
            LogUtil.Info(MethodBase.GetCurrentMethod().Name + "()");

            try
            {
                List<FileInfo> files = FileUtil.GetFiles(filePath);

                DateTime keepLogDate = new S9.Utility.Environment().Now().AddMonths(-AppContext.DeleteFileDurationInMonth);
                foreach (FileInfo file in files)
                {
                    if (file.LastWriteTime < keepLogDate)
                    {
                        LogUtil.Info("Delete file: " + file.Name + " " + file.LastWriteTime);
                        FileUtil.DeleteFile(file.FullName);

                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteErrorToLog(ex);
            }
        }
    }

}
