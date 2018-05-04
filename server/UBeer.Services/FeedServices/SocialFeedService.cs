using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;

using UBeer.Models;

namespace UBeer.Services
{
    public class SocialFeedService
    {
        private List<ITaggedMediaFeedService> _feedServices = new List<ITaggedMediaFeedService>();

        public void SetupFB(string token, long fanpageID)
        {
            ITaggedMediaFeedService fbService = FeedServiceFactory.CreateFBFeedService(
                                                                            token,
                                                                            fanpageID
                                                                        );
            _feedServices.Add(fbService);
        }

        public void SetupTwitter(string consumerKey, string consumerSecret, string token, string tokenSecret)
        {
            ITaggedMediaFeedService twitterService = FeedServiceFactory.CreateTwitterFeedService(
                                                                            consumerKey,
                                                                            consumerSecret,
                                                                            token,
                                                                            tokenSecret
                                                                        );
            _feedServices.Add(twitterService);
        }

        public void SetupIG()
        {
            ITaggedMediaFeedService igService = FeedServiceFactory.CreateIGFeedService();
            _feedServices.Add(igService);
        }

        public List<MediaPost> GetRecentTaggedMediaPosts(string[] tags)
        {
            List<MediaPost> posts = new List<MediaPost>();

            // get posts from each source and accumulate them
            _feedServices.ForEach(
                                    o =>
                                    {
                                        List<MediaPost> bufferPosts = o.GetRecentTaggedMediaPosts(tags);
                                        if (bufferPosts != null)
                                            posts.AddRange(bufferPosts);
                                    }
            );

            return posts;
        }
    }
}
