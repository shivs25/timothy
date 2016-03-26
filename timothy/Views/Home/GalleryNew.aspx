<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNew.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%: ViewData["FOLDER_NAME"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<script>
    (function (i, s, o, g, r, a, m) {
        i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
            (i[r].q = i[r].q || []).push(arguments)
        }, i[r].l = 1 * new Date(); a = s.createElement(o),
          m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
    })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

    ga('create', 'UA-44825484-1', 'timothyshivers.com');
    ga('send', 'pageview');

</script>

<script type="text/javascript">
    var _index = 0;
    var _gallery = null;
    var _captionVisible = false;

    var preloadImages = function () {
        for (var i = 0; i < _gallery.Images.length; i++) {
            for (var j = 0; j < _gallery.Images[i].length; j++) {
                var img = new Image();
                img.src = _gallery.Images[i][j].URL;
            }
        }
    };

    var loadImage = function (index) {
        if (index == 0) {
            $("#divBackButton").hide();

        }
        else {
            $("#divBackButton").show();
        }

        if (index == _gallery.Images.length - 1) {
            $("#divNextButton").hide();
        }
        else {
            $("#divNextButton").show();
        }

        if ("1" == $("#hidIsDouble").val()) {
            var html = _gallery.Images[index][0].Html;
            var html2 = _gallery.Images[index][1].Html;

            //Left side.

            if (null != html && html.length > 0) {
                $("#divDoubleBackgroundAStory").html(html);
                $("#divDoubleBackgroundCaptionA").hide();
                $("#divDoubleBackgroundImageA").hide();
                $("#divDoubleBackgroundAStory").show();

            }
            else {

                $("#divDoubleBackgroundImageA").css("background-image", "url('" + _gallery.Images[index][0].URL + "')");
                var caption = _gallery.Images[index][0].Caption;

                $("#divDoubleBackgroundCaptionA").html(_gallery.Images[index][0].Caption);
                $("#divDoubleBackgroundCaptionA").show();
                $("#divDoubleBackgroundImageA").show();
                $("#divDoubleBackgroundAStory").hide();
            }

            if (null != html2 && html2.length > 0) {
                $("#divDoubleBackgroundBStory").html(html2);
                $("#divDoubleBackgroundCaptionB").hide();
                $("#divDoubleBackgroundImageB").hide();
                $("#divDoubleBackgroundBStory").show();
            }
            else {

                $("#divDoubleBackgroundImageB").css("background-image", "url('" + _gallery.Images[index][1].URL + "')");
                var caption2 = _gallery.Images[index][1].Caption;

                $("#divDoubleBackgroundCaptionB").html(_gallery.Images[index][1].Caption);
                $("#divDoubleBackgroundCaptionB").show();
                $("#divDoubleBackgroundImageB").show();
                $("#divDoubleBackgroundBStory").hide();
            }

            if ((null != caption && caption.length > 0) || (null != caption2 && caption2.length > 0)) {
                $("#divDoubleBackgroundInfoButton").show();
            }
            else {
                $("#divDoubleBackgroundInfoButton").hide();
            }
        }
        else {
            $("#divSingleBackgroundImage").css("background-image", "url('" + _gallery.Images[index][0].URL + "')");

            var caption = _gallery.Images[index][0].Caption;

            $("#divSingleBackgroundCaption").html(_gallery.Images[index][0].Caption);
            if (null != caption && caption.length > 0) {
                $("#divSingleBackgroundInfoButton").show();
            }
            else {
                $("#divSingleBackgroundInfoButton").hide();
            }

        }
    };

    var onDoubleClick = function () {
        if (_captionVisible) {
            _captionVisible = false;
            $("#divDoubleBackgroundCaptionA").animate({
                opacity: 0
            }, 250);
            $("#divDoubleBackgroundCaptionB").animate({
                opacity: 0
            }, 250);

            $("#divDoubleBackgroundInfoButton").removeClass("on");

        }
        else {
            var caption = _gallery.Images[_index][0].Caption;
            if (null != caption && caption.length > 0) {
                _captionVisible = true;
                $("#divDoubleBackgroundCaptionA").animate({
                    opacity:1
                }, 250);
                $("#divDoubleBackgroundCaptionB").animate({
                    opacity: 1
                }, 250);

                $("#divDoubleBackgroundInfoButton").addClass("on");
            }

        }
    };

    $(function () {


        if ("1" == $("#hidIsDouble").val()) {
            $("#divDouble").show();
        }
        else {
            $("#divSingle").show();
            $("#divGalleryContainer").addClass("singleImageContainer");
        }


        $(".navigation").click(function () {
            $(this).find("div").css({ opacity: 0.75 });
            $(this).find("div").show();
            $(this).find("div").animate({
                opacity: 0
            }, 250, function () {
                $(this).find("div").hide();
            });
        });

        $("#divDoubleBackgroundImageA").click(function () {
            onDoubleClick();
        });

        $("#divDoubleBackgroundInfoButton").click(function () {
            onDoubleClick();
        });


        $("#divSingleBackgroundInfoButton").click(function () {
            if (_captionVisible) {
                _captionVisible = false;
                $("#divSingleBackgroundCaption").animate({
                    opacity: 0
                }, 250);
                $("#divSingleBackgroundInfoButton").removeClass("on");

                var caption = _gallery.Images[_index][0].Caption;
                if (!(null != caption && caption.length > 0)) {
                    $("#divSingleBackgroundImage").css("cursor", "default");
                }

            }
            else {
                var caption = _gallery.Images[_index][0].Caption;
                if (null != caption && caption.length > 0) {
                    _captionVisible = true;
                    $("#divSingleBackgroundCaption").animate({
                        opacity: 1
                    }, 250);
                    $("#divSingleBackgroundInfoButton").addClass("on");

                }

            }
        });

        $("#divBackButton").click(function () {
            if (_index > 0) {
                _index--;
            }
            else {
                _index = _gallery.Images.length - 1;
            }
            loadImage(_index);
        });

        $("#divNextButton").click(function () {
            if (_index < _gallery.Images.length - 1) {
                _index++;
            }
            else {
                _index = 0;
            }
            loadImage(_index);
        });

        $.ajax({
            type: "POST",
            traditional: true,
            url: '/Home/GalleryImages?folderId=' + $("#hidFolderId").val() + "&galleryId=" + $("#hidGalleryId").val() + "&isRetina=" + $("#hidRetina").val() + "&isDouble=" + ($("#hidIsDouble").val() == "1") + "&folderName=" + $("#hidFolderName").val(),
            async: true,
            timeout: 15000,
            success: function (data, textStatus) {
                if (null != data) {
                    _gallery = data;
                    preloadImages();
                    _index = 0;
                    loadImage(_index);
                }
            },
            error: function (XMLHttpRequest, errorThrown) {
            }
        });
    });
</script>


<div id="divGalleryContainer" class="fill">
    <div id="divGalleryMainContainer">
        <div id="divSingle" class="fill" style="display:none;">
            <div id="divSingleBackgroundImage"></div>
            
            <div id="divSingleBackgroundCaption" style="opacity:0;" class="caption"></div>
            <div id="divSingleBackgroundInfoButton" style="display:none;"></div>
        </div>
        <div id="divDouble" class="fill" style="display:none;">
            
            <div id="divDoubleContainerA"><div id="divDoubleBackgroundAStory" class="fill story" style="display:none;"></div><div id="divDoubleBackgroundImageA" class="fill"></div><div id="divDoubleBackgroundCaptionA" style="opacity:0;" class="caption"></div></div>
            <div id="divDoubleContainerB"><div id="divDoubleBackgroundBStory" class="fill story" style="display:none;"></div><div id="divDoubleBackgroundImageB" class="fill"></div><div id="divDoubleBackgroundCaptionB" style="opacity:0;" class="caption"></div></div>
            <div id="divDoubleBackgroundInfoButton" style="display:none;"></div>
        </div>
        <div id="divStory" class="fill" style="display:none;">
        
        </div>
    </div>
    <div id="divThumbnails"></div>
    <div id="divBackButton" class="navigation"><div class="fill" style="display:none;"></div></div>
    <div id="divNextButton" class="navigation"><div class="fill" style="display:none;"></div></div>
</div>


<%: Html.Hidden("hidFolderId", ViewData["FOLDER_ID"]) %>
<%: Html.Hidden("hidGalleryId", ViewData["GALLERY_ID"]) %>
<%: Html.Hidden("hidIsDouble", ViewData["IS_DOUBLE"].ToString().ToLower() == "true" ? "1" : "0") %>
<%: Html.Hidden("hidFolderName", ViewData["FOLDER_NAME"]) %>


</asp:Content>

