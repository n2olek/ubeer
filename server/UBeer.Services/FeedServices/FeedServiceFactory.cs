using System;
using System.Web;

namespace UBeer.Services
{
    public class FeedServiceFactory
    {
        public static ITaggedMediaFeedService CreateFBFeedService(string token, long fanpageID)
        {
            FBFeedService fbService = new FBFeedService();

            fbService.Init(token, fanpageID);
            return fbService;
        }

        public static ITaggedMediaFeedService CreateTwitterFeedService(string consumerKey, string consumerSecret, string token, string tokenSecret)
        {
            TwitterFeedService twitterService = new TwitterFeedService();

            twitterService.Init(consumerKey, consumerSecret, token, tokenSecret);
            return twitterService;
        }

        public static ITaggedMediaFeedService CreateIGFeedService()
        {
            IGFeedService igService = new IGFeedService();
            return igService;
        }

    }
}
