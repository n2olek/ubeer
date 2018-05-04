using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBeer.Models
{
    public class PostVideoIGModel
    {
        public object activity_counts { get; set; }
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

    public class Config
    {
        public string csrf_token { get; set; }
        public object viewer { get; set; }
    }

    public class Dimensions
    {
        public int height { get; set; }
        public int width { get; set; }
    }

    public class DisplayResource
    {
        public string src { get; set; }
        public int config_width { get; set; }
        public int config_height { get; set; }
    }

    public class DashInfo
    {
        public bool is_dash_eligible { get; set; }
        public object video_dash_manifest { get; set; }
        public int number_of_qualities { get; set; }
    }

    public class EdgeMediaToTaggedUser
    {
        public List<object> edges { get; set; }
    }

    public class Node
    {
        public string text { get; set; }
    }

    public class Edge
    {
        public Node node { get; set; }
    }

    public class EdgeMediaToCaption
    {
        public List<Edge> edges { get; set; }
    }

    public class PageInfo
    {
        public bool has_next_page { get; set; }
        public object end_cursor { get; set; }
    }

    public class EdgeMediaToComment
    {
        public int count { get; set; }
        public PageInfo page_info { get; set; }
        public List<object> edges { get; set; }
    }

    public class EdgeMediaPreviewLike
    {
        public int count { get; set; }
        public List<object> edges { get; set; }
    }

    public class EdgeMediaToSponsorUser
    {
        public List<object> edges { get; set; }
    }

    public class Owner
    {
        public string id { get; set; }
        public string profile_pic_url { get; set; }
        public string username { get; set; }
        public bool blocked_by_viewer { get; set; }
        public bool followed_by_viewer { get; set; }
        public string full_name { get; set; }
        public bool has_blocked_viewer { get; set; }
        public bool is_private { get; set; }
        public bool is_unpublished { get; set; }
        public bool is_verified { get; set; }
        public bool requested_by_viewer { get; set; }
    }

    public class EdgeWebMediaToRelatedMedia
    {
        public List<object> edges { get; set; }
    }

    public class ShortcodeMedia
    {
        public string __typename { get; set; }
        public string id { get; set; }
        public string shortcode { get; set; }
        public Dimensions dimensions { get; set; }
        public object gating_info { get; set; }
        public string media_preview { get; set; }
        public string display_url { get; set; }
        public List<DisplayResource> display_resources { get; set; }
        public DashInfo dash_info { get; set; }
        public string video_url { get; set; }
        public int video_view_count { get; set; }
        public bool is_video { get; set; }
        public bool should_log_client_event { get; set; }
        public string tracking_token { get; set; }
        public EdgeMediaToTaggedUser edge_media_to_tagged_user { get; set; }
        public EdgeMediaToCaption edge_media_to_caption { get; set; }
        public bool caption_is_edited { get; set; }
        public EdgeMediaToComment edge_media_to_comment { get; set; }
        public bool comments_disabled { get; set; }
        public int taken_at_timestamp { get; set; }
        public EdgeMediaPreviewLike edge_media_preview_like { get; set; }
        public EdgeMediaToSponsorUser edge_media_to_sponsor_user { get; set; }
        public object location { get; set; }
        public bool viewer_has_liked { get; set; }
        public bool viewer_has_saved { get; set; }
        public bool viewer_has_saved_to_collection { get; set; }
        public Owner owner { get; set; }
        public bool is_ad { get; set; }
        public EdgeWebMediaToRelatedMedia edge_web_media_to_related_media { get; set; }
    }

    public class Graphql
    {
        public ShortcodeMedia shortcode_media { get; set; }
    }

    public class PostPage
    {
        public Graphql graphql { get; set; }
    }

    public class EntryData
    {
        public List<PostPage> PostPage { get; set; }
    }

    public class Gatekeepers
    {
        public bool ld { get; set; }
    }

    public class P
    {
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
    }

    public class RegVp
    {
        public string g { get; set; }
        public P20 p { get; set; }
    }

    public class P21
    {
    }

    public class ProfPicUpsell
    {
        public string g { get; set; }
        public P21 p { get; set; }
    }

    public class P22
    {
    }

    public class ProfPicCreation
    {
        public string g { get; set; }
        public P22 p { get; set; }
    }

    public class P23
    {
    }

    public class Onetaplogin
    {
        public string g { get; set; }
        public P23 p { get; set; }
    }

    public class P24
    {
    }

    public class FeedVp
    {
        public string g { get; set; }
        public P24 p { get; set; }
    }

    public class P25
    {
    }

    public class PushNotifications
    {
        public string g { get; set; }
        public P25 p { get; set; }
    }

    public class P26
    {
    }

    public class LoginPoe
    {
        public string g { get; set; }
        public P26 p { get; set; }
    }

    public class P27
    {
    }

    public class Prefetch
    {
        public string g { get; set; }
        public P27 p { get; set; }
    }

    public class P28
    {
    }

    public class ReportHaf
    {
        public string g { get; set; }
        public P28 p { get; set; }
    }

    public class P29
    {
    }

    public class Reporting
    {
        public string g { get; set; }
        public P29 p { get; set; }
    }

    public class P30
    {
    }

    public class A2hs
    {
        public string g { get; set; }
        public P30 p { get; set; }
    }

    public class P31
    {
    }

    public class BgSync
    {
        public string g { get; set; }
        public P31 p { get; set; }
    }

    public class P32
    {
    }

    public class DiscPpl
    {
        public string g { get; set; }
        public P32 p { get; set; }
    }

    public class P33
    {
    }

    public class EbdsimLi
    {
        public string g { get; set; }
        public P33 p { get; set; }
    }

    public class P34
    {
    }

    public class EbdsimLo
    {
        public string g { get; set; }
        public P34 p { get; set; }
    }

    public class P35
    {
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

    public class P39
    {
    }

    public class Fs
    {
        public string g { get; set; }
        public P39 p { get; set; }
    }

    public class P40
    {
    }

    public class __invalid_type__404AsReact
    {
        public string g { get; set; }
        public P40 p { get; set; }
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
        public ProfPicUpsell prof_pic_upsell { get; set; }
        public ProfPicCreation prof_pic_creation { get; set; }
        public Onetaplogin onetaplogin { get; set; }
        public FeedVp feed_vp { get; set; }
        public PushNotifications push_notifications { get; set; }
        public LoginPoe login_poe { get; set; }
        public Prefetch prefetch { get; set; }
        public ReportHaf report_haf { get; set; }
        public Reporting reporting { get; set; }
        public A2hs a2hs { get; set; }
        public BgSync bg_sync { get; set; }
        public DiscPpl disc_ppl { get; set; }
        public EbdsimLi ebdsim_li { get; set; }
        public EbdsimLo ebdsim_lo { get; set; }
        public Embeds embeds { get; set; }
        public PrvcyTggl prvcy_tggl { get; set; }
        public VGrid v_grid { get; set; }
        public TpPblshr tp_pblshr { get; set; }
        public Fs fs { get; set; }
        public __invalid_type__404AsReact __invalid_name__404_as_react { get; set; }
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
        public object activity_counts { get; set; }
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
