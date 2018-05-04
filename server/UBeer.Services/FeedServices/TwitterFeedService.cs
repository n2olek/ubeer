using System;
using System.Linq;
using System.Collections.Generic;

using UBeer.Models;

using S9.Utility;

using Tweetinvi;
using Tweetinvi.Parameters;
using Tweetinvi.Models;

namespace UBeer.Services
{
    public class TwitterFeedService : ITaggedMediaFeedService
    {
        public void Init(string consumerKey, string consumerSecret, string token, string tokenSecret)
        {
            Auth.SetUserCredentials(consumerKey, consumerSecret, token, tokenSecret);
        }

        public List<MediaPost> GetRecentTaggedMediaPosts(string[] tags)
        {
            // build query string
            string query = BuildQueryString(tags);

            var searchParameter = new SearchTweetsParameters(query)
            {
                SearchType = SearchResultType.Recent,
                MaximumNumberOfResults = 100,
                TweetSearchType = TweetSearchType.OriginalTweetsOnly
            };

            if (Search.SearchTweets(searchParameter) == null) return new List<MediaPost>();

            List<MediaPost> feedData = Search.SearchTweets(searchParameter).AsQueryable()
                                        .Where(     // filter only one with image/VDO
                                                    o => o.Media.Count > 0
                                              )
                                        .Select(    // convert into global model
                                                    ConvertToMediaPost
                                                ).ToList();

            return feedData;
        }

        private MediaPost ConvertToMediaPost(ITweet node)
        {
            MediaPost entry = new MediaPost();

            try
            {
                entry.ID = node.Id.ToString();
                entry.Source = UBeerEnum.SOURCE.Twitter.ToString();
                entry.Message = node.Text;
                entry.CreateDate = node.TweetLocalCreationDate;
                entry.LikeCount = node.FavoriteCount;
                //entry.CommentCount = node.RetweetCount;
                entry.PostURL = node.Url;

                if (node.Media.Count > 0)
                {
                    // has image, VDO
                    entry.ImageURL = node.Media[0].MediaURL;
                    entry.OriginURL = node.Media[0].MediaURL;
                    entry.ContentType = GetContentType(node.Media[0].MediaType);
                }
            }
            catch { }

            return entry;
        }

        private string GetContentType(string contentType)
        {
            if (contentType == "video")
                return UBeerEnum.CONTENT_TYPE.Video.ToString();
            else if (contentType == "photo")
                return UBeerEnum.CONTENT_TYPE.Photo.ToString();
            else
                return "";
        }

        private string BuildQueryString(string[] tags)
        {
            string query = String.Join(" OR ", tags);
            return query;
        }
    }
}
