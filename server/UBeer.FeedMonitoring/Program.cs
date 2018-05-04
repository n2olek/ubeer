using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using S9.Utility;

using UBeer.Models;
using UBeer.Services;

namespace UBeer.FeedMonitor
{
    class Program
    {
        static void Main(string[] args)
        {

            // get social posts
            List<MediaPost> posts = GetPosts();

            // save posts into DB
            SavePosts(posts);

            // debugging purpose
            DisplayPosts(posts);

            // do the heartbeat
            StampHeartbeat();
        }

        private static List<MediaPost> GetPosts()
        {
            LogUtil.Info(MethodBase.GetCurrentMethod().DeclaringType.Name + ":" + MethodBase.GetCurrentMethod().Name + "()");
            List<MediaPost> posts = null;

            try
            {
                string[] hashtags = GetTodayHashtags();
                if (null == hashtags || hashtags.Length <= 0)
                    return null;

                hashtags.ToList().ForEach(o => LogUtil.Info("# is " + o));

                SocialFeedService feedService = InitFeedService();

                posts = feedService.GetRecentTaggedMediaPosts(hashtags);

                FillInMissionID(ref posts);
            }
            catch (Exception ex)
            {
                LogUtil.WriteErrorToLog(ex);
                SendErrorNotification(ex.TargetSite.DeclaringType.Name, ex.Message.ToString());
            }

            // fill in appropriate mission ID

            return posts;
        }

        private static string[] GetTodayHashtags()
        {
            LogUtil.Info(MethodBase.GetCurrentMethod().Name + "()");

            string[] hashtags;
            using (Entities db = new Entities())
            {
                MissionService missionService = new MissionService(db);
                hashtags = missionService.GetMissionHashtags(new S9.Utility.Environment().Now());
            }

            return hashtags;
        }

        private static void FillInMissionID(ref List<MediaPost> posts)
        {
            LogUtil.Info(MethodBase.GetCurrentMethod().Name + "()");

            using (Entities db = new Entities())
            {
                MissionService missionService = new MissionService(db);
                posts.ForEach(
                                o => { o.MissionID = missionService.FindMissionFromHashtagsInMessage(o.Message); }
                            );
            }
        }

        private static SocialFeedService InitFeedService()
        {
            LogUtil.Info(MethodBase.GetCurrentMethod().Name + "()");

            SocialFeedService feedService = new SocialFeedService();

            // FB
            LogUtil.Info("-- Initialize FB service --");
            try
            {
                feedService.SetupFB(
                                        AppContext.FacebookToken,
                                        AppContext.FacebookFanpageID
                                    );
            }
            catch (Exception ex)
            {
                LogUtil.WriteErrorToLog(ex);
                SendErrorNotification(ex.TargetSite.DeclaringType.Name, ex.Message.ToString());
            }

            // Twitter
            LogUtil.Info("-- Initialize Twitter service --");
            try
            {
                feedService.SetupTwitter(
                                        AppContext.TwitterConsumerKey,
                                        AppContext.TwitterConsumerSecret,
                                        AppContext.TwitterAccessToken,
                                        AppContext.TwitterAccessTokenSecret
                                    );
            }
            catch (Exception ex)
            {
                LogUtil.WriteErrorToLog(ex);
                SendErrorNotification(ex.TargetSite.DeclaringType.Name, ex.Message.ToString());
            }

            // IG
            LogUtil.Info("-- Initialize IG service --");
            try
            {
                feedService.SetupIG();
            }
            catch (Exception ex)
            {
                LogUtil.WriteErrorToLog(ex);
                SendErrorNotification(ex.TargetSite.DeclaringType.Name, ex.Message.ToString());
            }

            return feedService;
        }

        private static void SavePosts(List<MediaPost> posts)
        {
            LogUtil.Info(MethodBase.GetCurrentMethod().DeclaringType.Name + ":" + MethodBase.GetCurrentMethod().Name + "()");

            if (posts == null)
                return;

            using (Entities db = new Entities())
            {
                FillInMissionID(ref posts);

                PostService postService = new PostService(db);
                postService.SavePosts(posts);
            }
        }

        private static void DisplayPosts(List<MediaPost> posts)
        {
            LogUtil.Info(MethodBase.GetCurrentMethod().DeclaringType.Name + ":" + MethodBase.GetCurrentMethod().Name + "()");

            if (posts == null)
                return;

            posts.ForEach(
                            o =>
                            {
                                LogUtil.Info("Result: " + o.Source + ":" + o.ID + ":" + o.Message);
                                LogUtil.Info("  Created: " + o.CreateDate.ToString("dd-MMM-yyyy hh:mm:ss"));
                                LogUtil.Info("  Image URL: " + o.ImageURL);
                                LogUtil.Info("  Source URL: " + o.OriginURL);
                                LogUtil.Info("  Like count: " + o.LikeCount.ToString());
                                LogUtil.Info("  Comment count: " + o.CommentCount.ToString());
                                LogUtil.Info("  Mission ID: " + o.MissionID.ToString());
                            }
                        );
        }

        private static void StampHeartbeat()
        {
            LogUtil.Info(MethodBase.GetCurrentMethod().DeclaringType.Name + ":" + MethodBase.GetCurrentMethod().Name + "()");

            try
            {
                using (Entities db = new Entities())
                {
                    HeartbeatService heartbeatService = new HeartbeatService(db);
                    heartbeatService.SaveHeartbeat(UBeerEnum.SERVICE.FeedMonitoring.ToDescription());
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteErrorToLog(ex);
                SendErrorNotification(ex.TargetSite.DeclaringType.Name, ex.Message.ToString());
            }
        }

        private static void SendErrorNotification(string className, string errorMessage)
        {
            SendEmailService sendEmailService = new SendEmailService();
            sendEmailService.SendErrorNotification(AppContext.EmailForSendError, AppContext.SubjectEmailError + className, errorMessage);
        }
    }

}
