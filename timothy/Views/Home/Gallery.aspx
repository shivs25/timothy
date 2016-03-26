<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNew.Master" Inherits="System.Web.Mvc.ViewPage<timothy.Models.Gallery>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Model.FolderName %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script>
          (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
          (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
          m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
          })(window,document,'script','//www.google-analytics.com/analytics.js','ga');

          ga('create', 'UA-44825484-1', 'timothyshivers.com');
          ga('send', 'pageview');

    </script>
    <script type="text/javascript" language ="javascript">
        
        $(function() {
            var galleryWidth = parseInt($("#hidGalleryWidth").val());
            var numImages = parseInt($("#hidNumImages").val());

            //200 for the left container.
            //300 for the description.

            var hasDescription = parseInt($("#hidHasDescription").val());


            var totalWidth = galleryWidth + ((numImages) * 20) + 10;
            if (hasDescription == 1) {
                totalWidth += 315;
            }

            $("#divGallery").css("width", totalWidth + "px");
        });
    </script>
    <div id="divGalleryContainer">
        <div id="divGallery">
        <% if (!string.IsNullOrEmpty(Model.Description)) {
               %>
               <div id="divDescriptionContainer">
                    <%: MvcHtmlString.Create(Model.Description) %>
                </div>
           
           <% }
             %>

         <%
             foreach (timothy.Models.Image img in Model.Images) {
                %>
                <div class="imageContainer">
                    <% if (-1 != img.LinkFolderId) { %>
                            <div><a href="#" onclick="loadDetails(<%: img.LinkFolderId %>);"><img alt="" src="<%=Url.Content(img.URL) %>" width="<%: img.Width %>" height="675" /></a></div>
                            <div class="imageCaption"><span><a href="#" onclick="loadDetails(<%: img.LinkFolderId %>);"><%: img.Caption %></a></span></div>
                       <% }
                       else { %>
                            <div><img alt="" src="<%=Url.Content(img.URL) %>" width="<%: img.Width %>" height="580" /></div>
                            <div class="imageCaption"><span><%: img.Caption %></span></div>
                       <% }
                  %>
                
                </div>
             <% }
              %>
   

        <%: Html.Hidden("hidGalleryWidth", Model.GalleryWidth) %>
        <%: Html.Hidden("hidNumImages", Model.Images.Count.ToString()) %>
        <%: Html.Hidden("hidHasDescription", string.IsNullOrEmpty(Model.Description) ? 0 : 1) %>
    </div>
    </div>
    
</asp:Content>
