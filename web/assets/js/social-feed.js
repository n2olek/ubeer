(function ($) {
    "use strict"; // Start of use strict

    var SocialFeed = function (timerInterval) {

        var timerInterval = timerInterval;

        var popupGallery = null;

        var getFeed = function (objPopupGallery) {
            // store object
            popupGallery = objPopupGallery;

            // API
            var api = new API(appConfig.API_URL, "");
            api.getPosts(appConfig.PAGE_SIZE, render);
        };

        var render = function (resp) {
            //console.log(resp);
            var result = " ";
            jQuery.each(resp, function (postIndex, value) {
                var comment = resp[postIndex].CommentCount == null ? 0 : resp[postIndex].CommentCount;
                var message = resp[postIndex].Message == null ? ' ' : '<p>' + resp[postIndex].Message; + '</p>'
                var socialURL = resp[postIndex].SocialType == 'Instagram' ? '<i class="fa fa-instagram" aria-hidden="true"></i> View on Instagram'
                    : resp[postIndex].SocialType == 'Twitter' ? '<i class="fa fa-twitter-square" aria-hidden="true"></i> View on Twitter'
                        : resp[postIndex].SocialType == 'Facebook' ? '<i class="fa fa-facebook-square" aria-hidden="true"></i> View on Facebook'
                            : resp[postIndex].SocialType == 'UploadFile' ? ' ' : null;
                var postURL = resp[postIndex].PostURL == null ? ' ' : '<a href="' + resp[postIndex].PostURL + '" class="view-link" target="_blank">' + socialURL + '</a>';
                var thumbnailImage = resp[postIndex].ContentType == 'Photo' ? resp[postIndex].OriginURL : 'assets/images/placehold_video.jpg';
                var playerBox = resp[postIndex].ContentType == 'Photo' ? '<img src="' + resp[postIndex].OriginURL + '" class="img-responsive">' :
                    '<video controls><source src="' + resp[postIndex].OriginURL + '" type="video/mp4"></video>';

                result += '<div class="col-md-15 col-sm-4 col-xsm-6 col-xs-12">'
                    + '<a href="#post-popup-' + [postIndex] + '" class="popup">'
                    + '<div class="post-hover">'
                    + '<div class="icon-favorite">'
                    + '<span><i class="fa fa-heart" aria-hidden="true"></i> ' + resp[postIndex].LikeCount + '</span>'
                    + '<span><i class="fa fa-comment" aria-hidden="true"></i> ' + comment + '</span>'
                    + '</div>'
                    + '</div>'
                    + '<img src="' + thumbnailImage + '" class="img-responsive">'
                    + '</a>'
                    + '<div id="post-popup-' + [postIndex] + '" class="post-popup mfp-hide white-popup">'
                    + '<div class="figure-feed">'
                    + playerBox
                    + '</div>'
                    + '<div class="caption-feed">'
                    + '<div class="view-favorite">'
                    + '<span><i class="fa fa-heart" aria-hidden="true"></i> ' + resp[postIndex].LikeCount + '</span>'
                    + '<span><i class="fa fa-comment" aria-hidden="true"></i> ' + comment + '</span>' + postURL
                    + '</div>' + message
                    + '</div>'
                    + '</div>'
                    + '</div>';
            }); // jQuery.each

            $(popupGallery).empty().append(result);

            $('.popup').magnificPopup({
                type: 'inline',
                midClick: true,
                gallery: {
                    enabled: true
                }
            });
        }

        // public method
        this.load = function (objPopupGallery) {
            // load once
            getFeed(objPopupGallery);

            // the timer to load periodically
            window.setInterval(getFeed, timerInterval);
        };
    };

    // --- execute ---
    var socialFeed = new SocialFeed(appConfig.TIMER_INTERVAL);
    socialFeed.load($('.popup-gallery'));

})(jQuery); // End of use strict
