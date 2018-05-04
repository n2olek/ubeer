var API = function (baseURL, token) {
    var url = baseURL;
    var tokenValue = token;    

    $.ajaxSetup({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
    });

    this.getPosts = function (pageSize, callbackFunc) {
        var pageSizeParameter = "";
        if (pageSize != "")
            pageSizeParameter = '?pageSize=' + pageSize;

        var getPostsURL = url + 'GetPosts' + pageSizeParameter;

        $.ajax({
            url: getPostsURL,
            type: 'GET',
            success: function (response) {
                callbackFunc(response);
            }, error: function (e) {

            }
        });
    };

    this.deletePost = function (postID, callbackFunc) {
        var deletePostURL = url + 'DeletePost?postID=' + postID;

        $.ajax({
            url: deletePostURL,
            type: 'POST',
            headers: {
                'AuthorizationToken': tokenValue,
            },
            success: function (response) {
                if (response.status == 1) {
                    callbackFunc();
                }
                else {
                    alert(response.message);
                }
            }, error: function (e) {
                alert(e);
            }
        });
    };

    this.uploadImage = function (file, mobilePhone, callbackFunc) {
        var uploadImageURL = url + 'UploadImage';

        var formData = new FormData();
        formData.append('0', file);
        formData.append("mobilePhone", mobilePhone);

        $.ajax({
            url: uploadImageURL,    
            type: "POST",
            enctype: 'multipart/form-data',
            contentType: false,
            processData: false,
            cache: false,
            data: formData,
            success: function (response) {
                if (response.Status == "FAIL")
                    alert(response.message);
                else
                    callbackFunc();
            },
            error: function (err) {
                alert(err);
            }
        });
    };

    this.exportPosts = function () {
        var exportPostsURL = url + 'ExportExcel?AuthorizationToken=' + tokenValue;

        window.location.href = exportPostsURL;
    };

    this.login = function (user, password, callbackFunc) {
        var loginURL = url + 'login';

        var formData = new FormData();
        formData.append("user", user);
        formData.append("pass", password);

        $.ajax({
            url: loginURL,
            type: "POST",
            enctype: 'multipart/form-data',
            contentType: false,
            processData: false,
            cache: false,
            data: formData, // serializes the form's elements
            success: function (response) {
                if (response.status == 0)
                    alert(response.message);
                else
                    callbackFunc(response);
            },
            error: function (err) {
                alert(err);
            }
        });
    };
}