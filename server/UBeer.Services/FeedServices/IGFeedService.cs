using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;

using UBeer.Models;

using S9.Utility;

namespace UBeer.Services
{
    public class IGFeedService : ITaggedMediaFeedService
    {
        public const string EXPLORE_URL = "https://www.instagram.com/p/";

        public List<MediaPost> GetRecentTaggedMediaPosts(string[] tags)
        {
            // get IG feed content
            IGPageExtractor igExplorer = new IGPageExtractor();
            string IGFeedContent = igExplorer.GetFeedJSON(tags[0]);

            // extract info
            RootObject taggedMediaJson = JSON<RootObject>.Deserialize(IGFeedContent);
            List<MediaPost> posts = new List<MediaPost>();

            if (taggedMediaJson.entry_data.TagPage[0].tag == null)
            {
                posts = taggedMediaJson.entry_data.TagPage[0].graphql.hashtag.edge_hashtag_to_media.edges
                                                    .Select(    // convert into universal data model
                                                                ConvertToMediaPost
                                                    ).ToList();
            }
            else
            {
                posts = taggedMediaJson.entry_data.TagPage[0].tag.media.nodes
                                                    .Select(    // convert into universal data model
                                                                ConvertToMediaPost
                                                    ).ToList();
            }

            return posts;
        }

        private MediaPost ConvertToMediaPost(Node node)
        {
            string postUrl = EXPLORE_URL + node.code;
            string contentType = GetContentType(node.is_video);

            MediaPost entry = new MediaPost();
            entry.ID = node.id;
            entry.Source = UBeerEnum.SOURCE.Instagram.ToString();
            entry.ImageURL = node.thumbnail_src;
            entry.OriginURL = GetOriginURL(contentType, node.display_src, postUrl);
            entry.CreateDate = DateUtil.UnixTimeStampToDateTime(node.date);
            entry.LikeCount = node.likes.count;
            entry.CommentCount = node.comments.count;
            entry.PostURL = postUrl;
            entry.ContentType = contentType;
            entry.Message = node.caption;
            return entry;
        }

        private MediaPost ConvertToMediaPost(Edge edge)
        {
            string postUrl = EXPLORE_URL + edge.node.shortcode;
            string contentType = GetContentType(edge.node.is_video);

            MediaPost entry = new MediaPost();
            entry.ID = edge.node.id;
            entry.Source = UBeerEnum.SOURCE.Instagram.ToString();
            entry.ImageURL = edge.node.thumbnail_src;
            entry.OriginURL = GetOriginURL(contentType, edge.node.display_url, postUrl);
            entry.CreateDate = DateUtil.UnixTimeStampToDateTime(edge.node.taken_at_timestamp);
            entry.LikeCount = edge.node.edge_liked_by.count;
            entry.CommentCount = edge.node.edge_media_to_comment.count;
            entry.PostURL = postUrl;
            entry.ContentType = contentType;
            entry.Message = edge.node.edge_media_to_caption.edges[0].node.text;
            return entry;
        }

        private string GetOriginURL(string contentType, string display_url, string postUrl)
        {
            if (contentType == UBeerEnum.CONTENT_TYPE.Photo.ToString())
            {
                return display_url;
            }
            else if (contentType == UBeerEnum.CONTENT_TYPE.Video.ToString())
            {
                IGPageExtractor igExplorer = new IGPageExtractor();
                string videoPostContent = igExplorer.GetVideoPostJSON(postUrl);
                PostVideoIGModel taggedMediaJson = JSON<PostVideoIGModel>.Deserialize(videoPostContent);
                return taggedMediaJson.entry_data.PostPage[0].graphql.shortcode_media.video_url;
            }
            return "";
        }

        private string GetContentType(bool isVideo)
        {
            return isVideo ? UBeerEnum.CONTENT_TYPE.Video.ToString() : UBeerEnum.CONTENT_TYPE.Photo.ToString();
        }
    }

    // this is the class to deal with the feed from instagram.com site, in case of the HTML layout change, fix here
    public class IGPageExtractor
    {
        private const string IG_URL_PREFIX = "https://www.instagram.com/explore/tags/";
        private const string START_TAG = "window._sharedData = ";
        private const string END_TAG = ";</script>";

        public string GetFeedJSON(string tag)
        {
            tag = SanitizeTag(tag);

            // get the IG content
            HTMLPageExtractor igExplorer = new HTMLPageExtractor(IG_URL_PREFIX + tag, START_TAG, END_TAG);
            return igExplorer.GetContent();
        }

        private string SanitizeTag(string tag)
        {
            tag = tag.Trim();
            if (tag.StartsWith("#"))
                tag = tag.Substring(1);

            return tag;
        }

        public string GetVideoPostJSON(string url)
        {
            // get the IG content
            HTMLPageExtractor igExplorer = new HTMLPageExtractor(url, START_TAG, END_TAG);
            return igExplorer.GetContent();
        }
    }





    public class ActivityCounts
    {
        public int comment_likes { get; set; }
        public int comments { get; set; }
        public int likes { get; set; }
        public int relationships { get; set; }
        public int usertags { get; set; }
    }

    public class Viewer
    {
        public string biography { get; set; }
        public object external_url { get; set; }
        public string full_name { get; set; }
        public bool has_profile_pic { get; set; }
        public string id { get; set; }
        public string profile_pic_url { get; set; }
        public string profile_pic_url_hd { get; set; }
        public string username { get; set; }
    }

    public class Config
    {
        public string csrf_token { get; set; }
        public Viewer viewer { get; set; }
    }

    public class Dimensions
    {
        public int height { get; set; }
        public int width { get; set; }
    }

    public class Owner
    {
        public string id { get; set; }
    }

    public class ThumbnailResource
    {
        public string src { get; set; }
        public int config_width { get; set; }
        public int config_height { get; set; }
    }

    public class IGComments
    {
        public int count { get; set; }
    }

    public class IGLikes
    {
        public int count { get; set; }
    }

    public class Node
    {
        public bool comments_disabled { get; set; }
        public string id { get; set; }
        public Dimensions dimensions { get; set; }
        public Owner owner { get; set; }
        public string thumbnail_src { get; set; }
        public List<ThumbnailResource> thumbnail_resources { get; set; }
        public bool is_video { get; set; }
        public string code { get; set; }
        public int date { get; set; }
        public string display_src { get; set; }
        public string caption { get; set; }
        public IGComments comments { get; set; }
        public IGLikes likes { get; set; }
    }

    public class PageInfo
    {
        public bool has_next_page { get; set; }
        public object end_cursor { get; set; }
    }

    public class Media
    {
        public List<Node> nodes { get; set; }
        public int count { get; set; }
        public PageInfo page_info { get; set; }
    }

    public class TopPosts
    {
        public List<object> nodes { get; set; }
    }

    public class Tag
    {
        public string name { get; set; }
        public bool is_top_media_only { get; set; }
        public object content_advisory { get; set; }
        public Media media { get; set; }
        public TopPosts top_posts { get; set; }
    }

    public class PageInfo2
    {
        public bool has_next_page { get; set; }
        public object end_cursor { get; set; }
    }

    public class Node3
    {
        public string text { get; set; }
    }

    public class Edge2
    {
        public Node3 node { get; set; }
    }

    public class EdgeMediaToCaption
    {
        public List<Edge2> edges { get; set; }
    }

    public class EdgeMediaToComment
    {
        public int count { get; set; }
    }

    public class Dimensions2
    {
        public int height { get; set; }
        public int width { get; set; }
    }

    public class EdgeLikedBy
    {
        public int count { get; set; }
    }

    public class Owner2
    {
        public string id { get; set; }
    }

    public class ThumbnailResource2
    {
        public string src { get; set; }
        public int config_width { get; set; }
        public int config_height { get; set; }
    }

    public class Node2
    {
        public bool comments_disabled { get; set; }
        public string id { get; set; }
        public EdgeMediaToCaption edge_media_to_caption { get; set; }
        public string shortcode { get; set; }
        public EdgeMediaToComment edge_media_to_comment { get; set; }
        public int taken_at_timestamp { get; set; }
        public Dimensions2 dimensions { get; set; }
        public string display_url { get; set; }
        public EdgeLikedBy edge_liked_by { get; set; }
        public Owner2 owner { get; set; }
        public string thumbnail_src { get; set; }
        public List<ThumbnailResource2> thumbnail_resources { get; set; }
        public bool is_video { get; set; }
    }

    public class Edge
    {
        public Node2 node { get; set; }
    }

    public class EdgeHashtagToMedia
    {
        public int count { get; set; }
        public PageInfo2 page_info { get; set; }
        public List<Edge> edges { get; set; }
    }

    public class EdgeHashtagToTopPosts
    {
        public List<object> edges { get; set; }
    }

    public class EdgeHashtagToContentAdvisory
    {
        public int count { get; set; }
        public List<object> edges { get; set; }
    }

    public class Hashtag
    {
        public string name { get; set; }
        public bool is_top_media_only { get; set; }
        public EdgeHashtagToMedia edge_hashtag_to_media { get; set; }
        public EdgeHashtagToTopPosts edge_hashtag_to_top_posts { get; set; }
        public EdgeHashtagToContentAdvisory edge_hashtag_to_content_advisory { get; set; }
    }

    public class Graphql
    {
        public Hashtag hashtag { get; set; }
    }

    public class TagPage
    {
        public Tag tag { get; set; }
        public Graphql graphql { get; set; }
    }

    public class EntryData
    {
        public List<TagPage> TagPage { get; set; }
    }

    public class Gatekeepers
    {
        public bool ld { get; set; }
        public bool nr { get; set; }
        public bool pl { get; set; }
        public bool sms { get; set; }
        public bool vl { get; set; }
        public bool cds { get; set; }
    }

    public class P
    {
        public string enforce_gl { get; set; }
        public string is_enabled { get; set; }
        public string is_feed_organic_enabled { get; set; }
        public string variant { get; set; }
    }

    public class DashForVod
    {
        public string g { get; set; }
        public P p { get; set; }
    }

    public class P2
    {
    }

    public class Bc3l
    {
        public string g { get; set; }
        public P2 p { get; set; }
    }

    public class P3
    {
    }

    public class Aysf
    {
        public string g { get; set; }
        public P3 p { get; set; }
    }

    public class P4
    {
    }

    public class Notif
    {
        public string g { get; set; }
        public P4 p { get; set; }
    }

    public class P5
    {
        public string is_inline { get; set; }
    }

    public class FollowButton
    {
        public string g { get; set; }
        public P5 p { get; set; }
    }

    public class P6
    {
        public string is_enabled { get; set; }
    }

    public class LoginViaSignupPage
    {
        public string g { get; set; }
        public P6 p { get; set; }
    }

    public class P7
    {
        public string new_cta { get; set; }
        public string remove_upsell_banner { get; set; }
        public string update_nav { get; set; }
    }

    public class Loggedout
    {
        public string g { get; set; }
        public P7 p { get; set; }
    }

    public class P8
    {
        public string mobile_auto_advance { get; set; }
    }

    public class Stories
    {
        public string g { get; set; }
        public P8 p { get; set; }
    }

    public class P9
    {
    }

    public class ExitStoryCreation
    {
        public string g { get; set; }
        public P9 p { get; set; }
    }

    public class P10
    {
        public string has_msisdn_prefill { get; set; }
    }

    public class SuUniverse
    {
        public string g { get; set; }
        public P10 p { get; set; }
    }

    public class P11
    {
    }

    public class UsLi
    {
        public string g { get; set; }
        public P11 p { get; set; }
    }

    public class P12
    {
    }

    public class Sidecar
    {
        public string g { get; set; }
        public P12 p { get; set; }
    }

    public class P13
    {
    }

    public class Video
    {
        public string g { get; set; }
        public P13 p { get; set; }
    }

    public class P14
    {
    }

    public class Filters
    {
        public string g { get; set; }
        public P14 p { get; set; }
    }

    public class P15
    {
        public string no_pill { get; set; }
    }

    public class Appsell
    {
        public string g { get; set; }
        public P15 p { get; set; }
    }

    public class P16
    {
    }

    public class Collections
    {
        public string g { get; set; }
        public P16 p { get; set; }
    }

    public class P17
    {
        public string is_enabled { get; set; }
    }

    public class Save
    {
        public string g { get; set; }
        public P17 p { get; set; }
    }

    public class P18
    {
    }

    public class Stale
    {
        public string g { get; set; }
        public P18 p { get; set; }
    }

    public class P19
    {
    }

    public class Reg
    {
        public string g { get; set; }
        public P19 p { get; set; }
    }

    public class P20
    {
        public string hide_value_prop { get; set; }
    }

    public class RegVp
    {
        public string g { get; set; }
        public P20 p { get; set; }
    }

    public class P21
    {
    }

    public class Nux
    {
        public string g { get; set; }
        public P21 p { get; set; }
    }

    public class P22
    {
    }

    public class ProfPicUpsell
    {
        public string g { get; set; }
        public P22 p { get; set; }
    }

    public class P23
    {
    }

    public class ProfPicCreation
    {
        public string g { get; set; }
        public P23 p { get; set; }
    }

    public class P24
    {
    }

    public class Onetaplogin
    {
        public string g { get; set; }
        public P24 p { get; set; }
    }

    public class P25
    {
        public string is_hidden { get; set; }
    }

    public class FeedVp
    {
        public string g { get; set; }
        public P25 p { get; set; }
    }

    public class P26
    {
    }

    public class PushNotifications
    {
        public string g { get; set; }
        public P26 p { get; set; }
    }

    public class P27
    {
    }

    public class LoginPoe
    {
        public string g { get; set; }
        public P27 p { get; set; }
    }

    public class P28
    {
        public string use_feed_media_prefetch { get; set; }
        public string use_feed_prefetch { get; set; }
    }

    public class Prefetch
    {
        public string g { get; set; }
        public P28 p { get; set; }
    }

    public class P29
    {
        public string show_media_haf_flow { get; set; }
    }

    public class ReportHaf
    {
        public string g { get; set; }
        public P29 p { get; set; }
    }

    public class P30
    {
        public string is_enabled { get; set; }
    }

    public class ReportCategoryReorder
    {
        public string g { get; set; }
        public P30 p { get; set; }
    }

    public class P31
    {
        public string is_enabled { get; set; }
    }

    public class A2hs
    {
        public string g { get; set; }
        public P31 p { get; set; }
    }

    public class P32
    {
    }

    public class BgSync
    {
        public string g { get; set; }
        public P32 p { get; set; }
    }

    public class P33
    {
        public string is_enabled { get; set; }
    }

    public class DiscPpl
    {
        public string g { get; set; }
        public P33 p { get; set; }
    }

    public class P34
    {
        public string is_enabled { get; set; }
    }

    public class EbdsimLi
    {
        public string g { get; set; }
        public P34 p { get; set; }
    }

    public class P35
    {
        public string stateless { get; set; }
    }

    public class Embeds
    {
        public string g { get; set; }
        public P35 p { get; set; }
    }

    public class P36
    {
    }

    public class PrvcyTggl
    {
        public string g { get; set; }
        public P36 p { get; set; }
    }

    public class P37
    {
    }

    public class VGrid
    {
        public string g { get; set; }
        public P37 p { get; set; }
    }

    public class P38
    {
    }

    public class TpPblshr
    {
        public string g { get; set; }
        public P38 p { get; set; }
    }

    public class Qe
    {
        public DashForVod dash_for_vod { get; set; }
        public Bc3l bc3l { get; set; }
        public Aysf aysf { get; set; }
        public Notif notif { get; set; }
        public FollowButton follow_button { get; set; }
        public LoginViaSignupPage login_via_signup_page { get; set; }
        public Loggedout loggedout { get; set; }
        public Stories stories { get; set; }
        public ExitStoryCreation exit_story_creation { get; set; }
        public SuUniverse su_universe { get; set; }
        public UsLi us_li { get; set; }
        public Sidecar sidecar { get; set; }
        public Video video { get; set; }
        public Filters filters { get; set; }
        public Appsell appsell { get; set; }
        public Collections collections { get; set; }
        public Save save { get; set; }
        public Stale stale { get; set; }
        public Reg reg { get; set; }
        public RegVp reg_vp { get; set; }
        public Nux nux { get; set; }
        public ProfPicUpsell prof_pic_upsell { get; set; }
        public ProfPicCreation prof_pic_creation { get; set; }
        public Onetaplogin onetaplogin { get; set; }
        public FeedVp feed_vp { get; set; }
        public PushNotifications push_notifications { get; set; }
        public LoginPoe login_poe { get; set; }
        public Prefetch prefetch { get; set; }
        public ReportHaf report_haf { get; set; }
        public ReportCategoryReorder report_category_reorder { get; set; }
        public A2hs a2hs { get; set; }
        public BgSync bg_sync { get; set; }
        public DiscPpl disc_ppl { get; set; }
        public EbdsimLi ebdsim_li { get; set; }
        public Embeds embeds { get; set; }
        public PrvcyTggl prvcy_tggl { get; set; }
        public VGrid v_grid { get; set; }
        public TpPblshr tp_pblshr { get; set; }
    }

    public class DisplayPropertiesServerGuess
    {
        public double pixel_ratio { get; set; }
        public int viewport_width { get; set; }
        public int viewport_height { get; set; }
        public string orientation { get; set; }
    }

    public class ZeroData
    {
    }

    public class RootObject
    {
        public ActivityCounts activity_counts { get; set; }
        public Config config { get; set; }
        public string country_code { get; set; }
        public string language_code { get; set; }
        public string locale { get; set; }
        public EntryData entry_data { get; set; }
        public Gatekeepers gatekeepers { get; set; }
        public Qe qe { get; set; }
        public string hostname { get; set; }
        public DisplayPropertiesServerGuess display_properties_server_guess { get; set; }
        public bool environment_switcher_visible_server_guess { get; set; }
        public string platform { get; set; }
        public string nonce { get; set; }
        public ZeroData zero_data { get; set; }
        public string rollout_hash { get; set; }
        public bool probably_has_app { get; set; }
        public bool show_app_install { get; set; }
    }

}
