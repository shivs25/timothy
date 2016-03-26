<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNew.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<timothy.Models.Video>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Videos
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
        $(function () {
            var videoWidth = parseInt($("#hidVideoWidth").val());
            var numVideos = $("#hidNumVideos").val();
            var padding = parseInt($("#hidVideoPadding").val());

            $("#divVideoContents").css({
                width: ((numVideos * (videoWidth + padding))) + "px"
            });

            $(".videoContainer").css({
                width: (videoWidth + padding) + "px"
            });
        });
    </script>
    
    <div id="divVideoViewPort">
        <div id="divVideoContents">
            <% foreach(timothy.Models.Video v in this.Model) { %>
                <div class="videoContainer"><% Response.Write(v.EmbedScript); %></div>
               
            <% }%>
        </div>
    </div>

    <%: Html.Hidden("hidNumVideos", this.Model.Count().ToString()) %>
    <%: Html.Hidden("hidVideoWidth", ViewData["VIDEO_WIDTH"].ToString()) %>
    <%: Html.Hidden("hidVideoPadding", ViewData["VIDEO_PADDING"].ToString()) %>
</asp:Content>

