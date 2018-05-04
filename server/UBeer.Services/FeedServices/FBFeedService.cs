using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;

using UBeer.Models;

// need to select the version matched to one in S9.Utilitym
using Newtonsoft.Json;
using S9.Utility;

// FB
using Facebook;


// FB Graph API tool: https://developers.facebook.com/tools/explorer
// Examples
//      [FanPageID]/posts?fields=id,created_time,updated_time,likes,message,from,status_type,comments,object_id&since=14-07-01
//      [FanPageID]/feed?fields=id,created_time,updated_time,likes,message,from,status_type,comments,object_id&limit=100
namespace UBeer.Services
{
    public class FBFeedService : ITaggedMediaFeedService
    {
        private const string FB_API_VERSION = "v2.6";
        private const int PAGE_SIZE = 25;
        private const int MESSAGE_LIMIT = 100;

        private FacebookClient _FBClient = null;
        private long _FBFanPageID = 0;

        // approach 1: get an access token from authentication, the result might not have enough permission to get LIKE info.
        public void Init(string FBAppID, string FBSecretKey, long FBFanPageID)
        {
            _FBClient = new FacebookClient();

            // get the token
            dynamic result = _FBClient.Get("oauth/access_token", new
            {
                client_id = FBAppID,
                client_secret = FBSecretKey,
                grant_type = "client_credentials",
            });
            _FBClient.AccessToken = result.access_token;

            _FBFanPageID = FBFanPageID;
        }

        // approacch 2: already got an access token from FB Graph API explorer site, this helps to get a permanent token which has permission to access LIKE info
        public void Init(string FBToken, long FBFanPageID)
        {
            _FBClient = new FacebookClient();

            _FBClient.AccessToken = FBToken;
            _FBFanPageID = FBFanPageID;
        }


        public List<MediaPost> GetRecentTaggedMediaPosts(string[] tags)
        {
            FeedResponse feedResponse = Query(_FBFanPageID);

            // construct filter
            var conditions = PredicateBuilder.False<Post>();
            tags.ToList().ForEach(
                                    o =>
                                    conditions = conditions.Or(post => post.message.ToLower().Contains(o.Trim().ToLower()))
                                 );

            List<MediaPost> feedData = feedResponse.data.AsQueryable().Where(
                                                                                o => o.message != null &&
                                                                                o.object_id != null
                                                                            )
                                                                      .Where(
                                                                                conditions
                                                                            )
                                                                      .Select(
                                                                                ConvertToMediaPost
                                                                             ).ToList();
            return feedData;
        }

        public List<Post> GetRawFeed()
        {
            FeedResponse feedResponse = Query(_FBFanPageID);
            return feedResponse.data;
        }

        public int GetLikeCount(Post post)
        {
            if (post.likes == null)
                return 0;

            return (int)post.likes.data.Count;
        }

        public int GetCommmentCount(Post post)
        {
            if (post.comments == null)
                return 0;

            return (int)post.comments.data.Count;
        }

        // get all FBIDs that like the given post
        public List<long> GetLikes(Post post)
        {
            List<long> result = new List<long>();

            if (post.likes == null)
                return result;

            string postID = post.id.ToString();

            Likes likes = post.likes;
            List<long> FBIDs = likes.data.Select(o => long.Parse(o.id)).ToList();
            result.AddRange(FBIDs);

            // check if there's still a next page            
            while (HasNextPage(likes.paging))
            {
                string query = postID.ToString() + "/likes?limit=" + PAGE_SIZE.ToString() + "&after=" + HttpUtility.UrlEncode(likes.paging.cursors.after);
                JsonObject response = _FBClient.Get(query) as JsonObject;
                likes = JSON<Likes>.Deserialize(response.ToString());

                FBIDs = likes.data.Select(o => long.Parse(o.id)).ToList();
                result.AddRange(FBIDs);
            }

            return result;
        }

        public List<Datum_Comment> GetComments(Post post)
        {
            List<Datum_Comment> result = new List<Datum_Comment>();

            if (post.comments == null)
                return result;

            string postID = post.id.ToString();

            Comments comments = post.comments;
            result = comments.data;

            // check if there's a next page
            while (HasNextPage(comments.paging))
            {
                string query = postID + "/comments?limit=" + PAGE_SIZE.ToString() + "&after=" + HttpUtility.UrlEncode(comments.paging.cursors.after);
                JsonObject response = _FBClient.Get(query) as JsonObject;
                comments = JSON<Comments>.Deserialize(response.ToString());

                result.AddRange(comments.data);
            }

            return result;
        }

        public List<Post> GetShares(Post post)
        {
            List<Post> result = new List<Post>();

            if (post.object_id == null)
                return result;

            string objID = post.object_id.ToString();

            JsonObject response = _FBClient.Get(objID + "/sharedposts?fields=from") as JsonObject;
            FeedResponse feedResponse = JSON<FeedResponse>.Deserialize(response.ToString());
            result = feedResponse.data;

            // check if there's a next page
            while (HasNextPage(feedResponse.paging))
            {
                string query = objID + "/sharedposts?limit=" + PAGE_SIZE.ToString() + "&after=" + HttpUtility.UrlEncode(feedResponse.paging.cursors.after);
                response = _FBClient.Get(query) as JsonObject;
                feedResponse = JSON<FeedResponse>.Deserialize(response.ToString());

                result.AddRange(feedResponse.data);
            }

            return result;
        }

        private bool HasNextPage(Paging paging)
        {
            if (paging == null)
                return false;

            return (paging.next != null);
        }

        private FeedResponse Query(long FBFanPageID)
        {
            // retrieve the feed
            JsonObject result = _FBClient.Get("/" + FB_API_VERSION + "/" + FBFanPageID + "/feed?fields=id,created_time,updated_time,likes,message,from,status_type,comments,object_id,link,type&limit=" + MESSAGE_LIMIT.ToString()) as JsonObject;
            return JSON<FeedResponse>.Deserialize(result.ToString());
        }

        private MediaURLs GetMediaURLs(string mediaID)
        {
            // the object_id (media id) doesn't support sub-fields, cannot include sub fields in the query in the first place, thus uneed to query each post
            JsonObject result = _FBClient.Get("/" + FB_API_VERSION + "/" + mediaID + "/?fields=picture,source") as JsonObject;
            return JSON<MediaURLs>.Deserialize(result.ToString());
        }

        private MediaPost ConvertToMediaPost(Post node)
        {
            MediaPost entry = new MediaPost();

            entry.ID = node.id;
            entry.Source = UBeerEnum.SOURCE.Facebook.ToString();
            entry.Message = node.message;
            entry.CreateDate = DateTime.Parse(node.created_time);
            entry.LikeCount = GetLikeCount(node);
            entry.CommentCount = GetCommmentCount(node);
            entry.PostURL = node.link;
            entry.ContentType = GetContentType(node.type);

            if (node.object_id != null)
            {
                // has image, VDO
                MediaURLs urls = GetMediaURLs(node.object_id);

                entry.ImageURL = urls.picture;
                entry.OriginURL = urls.source;
            }

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
    }




    public class From
    {
        public string category { get; set; }
        public string name { get; set; }
        public string id { get; set; }
    }

    public class Datum_Like
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Cursors
    {
        public string after { get; set; }
        public string before { get; set; }
    }

    public class Paging
    {
        public Cursors cursors { get; set; }
        public string next { get; set; }
    }

    public class Likes
    {
        public List<Datum_Like> data { get; set; }
        public Paging paging { get; set; }
    }

    public class From_Comment
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Datum_Comment
    {
        public string id { get; set; }
        [JsonProperty("from")]
        public From_Comment From { get; set; }
        public string message { get; set; }
        public bool can_remove { get; set; }
        public string created_time { get; set; }
        public int like_count { get; set; }
        public bool user_likes { get; set; }
    }

    public class Comments
    {
        public List<Datum_Comment> data { get; set; }
        public Paging paging { get; set; }
    }

    public class Post
    {
        public string id { get; set; }
        public string created_time { get; set; }
        public string updated_time { get; set; }
        public string message { get; set; }
        [JsonProperty("from")]
        public From From { get; set; }
        public string status_type { get; set; }
        public string object_id { get; set; }
        public Likes likes { get; set; }
        public Comments comments { get; set; }
        public string link;
        public string type;
    }

    public class FeedResponse
    {
        public List<Post> data { get; set; }
        public Paging paging { get; set; }
    }

    public class MediaURLs
    {
        public string id;
        public string picture;
        public string source;
    }

}
