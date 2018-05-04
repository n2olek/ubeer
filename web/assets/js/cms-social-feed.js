(function ($) {
    "use strict"; // Start of use strict

    var cookieValue = $.cookie("token");
    if (cookieValue == undefined)//token invalid
        location.href = "index.html";

    var CMSSocialFeed = function () {

        var table = null;

        var getFeed = function (objTable) {
            table = objTable;
            var api = new API(appConfig.API_URL, "");
            api.getPosts("", render);
        };

        var render = function (resp) {

            $(table).DataTable({
                "data": resp,
                "columns": [
                    {
                        "data": "Message",
                        "title": "Message"
                    },
                    {
                        "data": "OriginURL", render: function (data, type, row) {

                            var result = " ";
                            var thumbnailImage = row.ContentType == 'Photo' ? row.OriginURL : '../assets/images/placehold_video.jpg';
                            var popupView = row.ContentType == 'Photo' ? '<img src="' + row.OriginURL + '" class="img-responsive">' :
                                '<video controls><source src="' + row.OriginURL + '" type="video/mp4"></video>';

                            result += '<a href="#popup-table-' + row.ID + '" class="popup-table figure-block">'
                                + '<img src="' + thumbnailImage + '" class="img-responsive">'
                                + '</a>'
                                + '<div id="popup-table-' + row.ID + '" class="mfp-hide white-popup">'
                                + '<div class="figure-feed">' + popupView + '</div>'
                                + '</div>';

                            return result
                        },
                        "title": "Origin URL"
                    },
                    {
                        "data": "PostURL", render: function (data, type, row) {
                            if (row.SocialType == 'UploadFile')
                                return '';
                            else
                                return '<a href="' + row.PostURL + '" target="_blank">' + row.PostURL + '</a>';

                        },
                        "title": "Post URL"
                    },
                    {
                        "data": "PostDate",
                        "title": "Post Date"
                    },
                    {
                        "data": "MobilePhone",
                        "title": "Mobile Phone"
                    },
                    {
                        "data": "MissionID",
                        "title": "Mission ID"
                    },
                    {
                        "data": "ID", render: function (data, type, row) {
                            if (row.SocialType == 'UploadFile')
                                return '<a href="javascript:deletePost(\'' + row.ID + '\');">Delete</a>';
                            else
                                return '';
                        },
                        "title": "Delete"
                    },

                ],
                "iDisplayLength": 10,
                "bLengthChange": false,
                "bFilter": false,
                "bSort": false,
                "scrollX": true,
                "drawCallback": function () {
                    $('.popup-table').magnificPopup({
                        type: 'inline',
                        midClick: true,
                        gallery: {
                            enabled: true
                        }
                    });
                }
            });
        }

        // public method
        this.load = function (objTable, objPopup) {
            // load once
            getFeed(objTable, objPopup);
        };
    };

    // --- execute ---
    var cmsSocialFeed = new CMSSocialFeed();
    cmsSocialFeed.load($('#dashboard'));

    function exportPosts() {
        var cookieValue = $.cookie("token");

        var api = new API(appConfig.API_URL, cookieValue);
        api.exportPosts();
    };

    $('#btn-export').click(function (event) {
        event.preventDefault();
        exportPosts();
    });

})(jQuery); // End of use strict

function deletePost(postID) {
    var cookieValue = $.cookie("token");
    if (cookieValue == undefined)
        cookieValue = "";

    var api = new API(appConfig.API_URL, cookieValue);
    api.deletePost(postID, deleteCompleted);
}

var deleteCompleted = function () {
    alert('Delete success');
    location.reload();
}

function logout() {
    $.removeCookie('token', { path: '/' });
    location.href = "index.html";
}
