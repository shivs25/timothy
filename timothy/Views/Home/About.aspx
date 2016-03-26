<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteNew.Master" Inherits="System.Web.Mvc.ViewPage<timothy.Models.About>" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
    About Timothy Shivers
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
          (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
          (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
          m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
          })(window,document,'script','//www.google-analytics.com/analytics.js','ga');

          ga('create', 'UA-44825484-1', 'timothyshivers.com');
          ga('send', 'pageview');

    </script>
    <script type="text/javascript" language="javascript">
    </script>

    <div id="divAboutContainer" class="contentContainer">
        <div id="divAboutImage">
        </div>
        <div id="divAboutTextContainer" style="float:left;">
            <%: MvcHtmlString.Create(Model.AboutHtml) %>
        </div>
    </div>
</asp:Content>
