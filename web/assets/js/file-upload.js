(function ($) {
    "use strict"; // Start of use strict

    $('input[type=file]').on('change', prepareUpload);

    // Grab the files and set them to our variable
    var files;
    function prepareUpload(event) {
        files = event.target.files;
    }

    function isImageFile(fileExtension) {
        return (-1 < appConfig.ALLOWED_IMAGE_EXTENSIONS.indexOf(fileExtension));
    }

    function isVDOFile(fileExtension) {
        return (-1 < appConfig.ALLOWED_VDO_EXTENSIONS.indexOf(fileExtension));
    }

    function isFileTypeAllowed(fileExtension) {
        return (isImageFile(fileExtension) || isVDOFile(fileExtension));
    }

    $("#btnSubmit").click(function (event) {
        // stop submit the form, we will post it manually.
        event.preventDefault();

        if (!appConfig.PHONE_PATTERN.test($('#mobilePhone').val())) {
            alert('Invalid phone number');
            return false;
        }

        if (!files || (0 == files.length)) {
            alert('Please upload file');
            return false;
        }

        // Create a formdata object and add the files
        var file = files[0]; // only the first file

        // check file type
        var fileExtension = file.type.split('/')[1];

        if (!isFileTypeAllowed(fileExtension)) {
            alert("File type not allowed");
            return false;
        }

        if (isImageFile(fileExtension)) {
            // image, check file size.
            if (file.size > appConfig.MAX_IMAGE_SIZE) {
                alert("Your image is over " + ((appConfig.MAX_IMAGE_SIZE / 1000000).toFixed(0)) + "MB");
                return false;
            }
        }
        else {
            // VDO, check file size.
            if (file.size > appConfig.MAX_VDO_SIZE) {
                alert("Your VDO is over " + ((appConfig.MAX_VDO_SIZE / 1000000).toFixed(0)) + "MB");
                return false;
            }
        }

        // show in progress
        $('#divProgress').css('visibility', 'visible');

        // everything is OK, disable the submit button
        $("#btnSubmit").prop("disabled", true);


        setTimeout(function () {
            // hide in progress
            $('#divProgress').css('visibility', 'hidden');

            // make an AJAX request
            var api = new API(appConfig.API_URL, "");
            api.uploadImage(file, $('#mobilePhone').val(), uploadCompleted);

        }, appConfig.TIMER_PROGRESS);
    });

    var uploadCompleted = function () {
        alert('Upload success');
        location.reload();
    }

})(jQuery); // End of use strict
