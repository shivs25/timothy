using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using System.Drawing;


namespace timothy.Controllers {
  [HandleError]
  public class HomeController : Controller {
    //public ActionResult Index() {
    //  this.setupForBackgroundImage(ViewData);
    //  this.setupLinks(ViewData);
      

    //  return View();
    //}

    public ActionResult Index() {
      bool isRetina;
      bool isDouble;
      List<int> openFolderIndexes;
      this.getQueryValues(Request, out isRetina, out isDouble, out openFolderIndexes);
      this.setupLinks(ViewData, openFolderIndexes);
      return View("IndexNew");
    }

    public ActionResult Portfolio() {
      bool isRetina;
      bool isDouble;
      List<int> openFolderIndexes;
      this.getQueryValues(Request, out isRetina, out isDouble, out openFolderIndexes);

      ViewData["FOLDER_ID"] = -1;
      ViewData["GALLERY_ID"] = -1;
      ViewData["FOLDER_NAME"] = "Portfolio";
      ViewData["IS_RETINA"] = isRetina;
      ViewData["IS_DOUBLE"] = false;
      this.setupLinks(ViewData, openFolderIndexes);
      return View("GalleryNew");
    }

    public ActionResult Personal() {
      bool isRetina;
      bool isDouble;
      List<int> openFolderIndexes;
      this.getQueryValues(Request, out isRetina, out isDouble, out openFolderIndexes);

      ViewData["FOLDER_ID"] = -1;
      ViewData["GALLERY_ID"] = -1;
      ViewData["FOLDER_NAME"] = "Personal";
      ViewData["IS_RETINA"] = isRetina;
      ViewData["IS_DOUBLE"] = false;
      this.setupLinks(ViewData, openFolderIndexes);
      return View("GalleryNew");
    }

    public ActionResult About() {
      bool isRetina;
      bool isDouble;
      List<int> openFolderIndexes;
      this.getQueryValues(Request, out isRetina, out isDouble, out openFolderIndexes);

      this.setupLinks(ViewData, openFolderIndexes);

      string aboutText = (Server.MapPath("~").TrimEnd("/\\".ToCharArray()) + "\\" + ConfigurationManager.AppSettings["DATA_SUBDIRECTORY"]).TrimEnd("/\\".ToCharArray()) + "\\data\\about.txt";

      timothy.Models.About returnValue = new Models.About();

      if (System.IO.File.Exists(aboutText)) {
        StreamReader reader = new StreamReader(aboutText);

        try {
          string text = reader.ReadToEnd();
          returnValue.AboutHtml = text;
        }
        catch { }
        finally {
          reader.Close();
          reader.Dispose();
        }
      }

      return View(returnValue);
    }

    public ActionResult GalleryImages(int folderId, int galleryId, string folderName) {
      timothy.Models.Gallery returnVal = new timothy.Models.Gallery();
      //returnVal.FolderName = folderName;

      bool isRetina;
      bool isDouble;
      List<int> openFolderIndexes;
      this.getQueryValues(Request, out isRetina, out isDouble, out openFolderIndexes);
      
      this.setupLinks(ViewData, openFolderIndexes);

      string directory = (Server.MapPath("~").TrimEnd("/\\".ToCharArray()) + "\\" + ConfigurationManager.AppSettings["DATA_SUBDIRECTORY"]).TrimEnd("/\\".ToCharArray()) + "\\data\\";
      string subDirectory;
      if (folderId >= 0) {
        directory += folderId + "\\" + galleryId;
        subDirectory = "/data/" + folderId + "/" + galleryId;
      }
      else {
        directory += folderName;
        subDirectory = "/data/" + folderName;
      }
      
      

      if (Directory.Exists(directory)) {

        if (isDouble) {
          Dictionary<string, string[]> captions = new Dictionary<string, string[]>();
          Dictionary<string, int> links = new Dictionary<string, int>();

          string captionFile = directory + "\\captions.txt";
          if (System.IO.File.Exists(captionFile)) {
            StreamReader reader = new StreamReader(captionFile);
            try {
              string name, caption, caption2, link;
              int linkFolderId;
              string line;
              string[] split;
              while (!reader.EndOfStream) {
                line = reader.ReadLine();
                split = line.Split(new char[] { '|' });

                switch (split.Length) {
                  case 3:
                    name = split[0];
                    caption = split[1];
                    caption2 = split[2];

                    if (!captions.ContainsKey(name)) {
                      captions.Add(name, new string[] { caption, caption2 });
                    }
                    break;
                  case 4:
                    name = split[0];
                    caption = split[1];
                    caption2 = split[2];
                    link = split[3];
                    if (!captions.ContainsKey(name)) {
                      captions.Add(name, new string[] { caption, caption2 });
                    }
                    if (!links.ContainsKey(name) && int.TryParse(link, out linkFolderId)) {
                      links.Add(name, linkFolderId);
                    }
                    break;
                }
              }
            }
            catch { }
            finally {
              reader.Close();
              reader.Dispose();
            }
          }

          List<string> fileNames = new List<string>();
          List<int> widths = new List<int>();
          Size imageSize, imageSize2;
          int realWidth;
          int width = 0;
          int height = 580;

          string f1, f2;
          string t1, t2;
          foreach (string dir in Directory.GetDirectories(directory)) {
            f1 = dir + "\\1.jpg";
            f2 = dir + "\\2.jpg";
            t1 = dir + "\\1.txt";
            t2 = dir + "\\2.txt";

            if (System.IO.File.Exists(f1) && System.IO.File.Exists(f2)) {
              imageSize = ImageDimensions.ImageHelper.GetDimensions(f1);
              imageSize2 = ImageDimensions.ImageHelper.GetDimensions(f2);

              if (imageSize.Width > 0 &&
                  imageSize.Height > 0 &&
                  imageSize2.Width > 0 &&
                  imageSize2.Height > 0) {

                realWidth = (int)((double)height * (double)imageSize.Width / (double)imageSize.Height);
                width += realWidth;
                widths.Add(realWidth);
                fileNames.Add(dir);
              }
            }
            else if (System.IO.File.Exists(f1) && System.IO.File.Exists(t2)) {
              imageSize = ImageDimensions.ImageHelper.GetDimensions(f1);
              if (imageSize.Width > 0 &&
                  imageSize.Height > 0) {
                realWidth = (int)((double)height * (double)imageSize.Width / (double)imageSize.Height);
                width += realWidth;
                widths.Add(realWidth);
                fileNames.Add(dir);
              }
            }
            else if (System.IO.File.Exists(t1) && System.IO.File.Exists(f2)) {
              imageSize2 = ImageDimensions.ImageHelper.GetDimensions(f2);
              if (imageSize2.Width > 0 &&
                  imageSize2.Height > 0) {
                    realWidth = (int)((double)height * (double)imageSize2.Width / (double)imageSize2.Height);
                width += realWidth;
                widths.Add(realWidth);
                fileNames.Add(dir);
              }
            }

          }

          returnVal.GalleryWidth = width;

          //Sort alphabetically
          List<List<timothy.Models.PagePanel>> images = new List<List<timothy.Models.PagePanel>>();
          fileNames.Sort(delegate(string a, string b) {

            return Convert.ToInt32(new FileInfo(a).Name).CompareTo(Convert.ToInt32(new FileInfo(b).Name));
          });
          string f;
          string fName;
          for (int i = 0; i < fileNames.Count; i++) {
            f = fileNames[i];
            fName = new FileInfo(f).Name;

            List<timothy.Models.PagePanel> pairImages = new List<Models.PagePanel>();

            if (System.IO.File.Exists(f + "\\1.txt")) {
              StreamReader reader = new StreamReader(f + "\\1.txt");

              try {
                string text = reader.ReadToEnd();
                pairImages.Add(new Models.PagePanel() {
                  Html = text
                });
              }
              catch { }
              finally {
                reader.Close();
                reader.Dispose();
              }
            }
            else {

              if (isRetina && System.IO.File.Exists(f + "\\1@2x.jpg")) {
                pairImages.Add(new timothy.Models.PagePanel() {
                  URL = subDirectory + "/" + new DirectoryInfo(f).Name + "/1@2x.jpg",
                  Caption = captions.ContainsKey(fName) ? captions[fName][0] : "",
                  Width = widths[i],
                  LinkFolderId = -1,// links.ContainsKey(f) ? links[f] : -1,
                });
              }
              else {
                pairImages.Add(new timothy.Models.PagePanel() {
                  URL = subDirectory + "/" + new DirectoryInfo(f).Name + "/1.jpg",
                  Caption = captions.ContainsKey(fName) ? captions[fName][0] : "",
                  Width = widths[i],
                  LinkFolderId = -1,//links.ContainsKey(f) ? links[f] : -1,
                });
              }
            }


            if (System.IO.File.Exists(f + "\\2.txt")) {
              StreamReader reader = new StreamReader(f + "\\2.txt");

              try {
                string text = reader.ReadToEnd();
                pairImages.Add(new Models.PagePanel() {
                  Html = text
                });
              }
              catch { }
              finally {
                reader.Close();
                reader.Dispose();
              }
            }
            else {
              if (isRetina && System.IO.File.Exists(f + "\\2@2x.jpg")) {
                pairImages.Add(new timothy.Models.PagePanel() {
                  URL = subDirectory + "/" + new DirectoryInfo(f).Name + "/2@2x.jpg",
                  Caption = captions.ContainsKey(fName) ? captions[fName][1] : "",
                  Width = widths[i],
                  LinkFolderId = -1,// links.ContainsKey(f) ? links[f] : -1,
                });
              }
              else {
                pairImages.Add(new timothy.Models.PagePanel() {
                  URL = subDirectory + "/" + new DirectoryInfo(f).Name + "/2.jpg",
                  Caption = captions.ContainsKey(fName) ? captions[fName][1] : "",
                  Width = widths[i],
                  LinkFolderId = -1,//links.ContainsKey(f) ? links[f] : -1,
                });
              }
            }
            

            images.Add(pairImages);
          }

          returnVal.Images = images;
        }
        else {
          string descriptionFile = directory + "\\message.txt";
          if (System.IO.File.Exists(descriptionFile)) {
            StreamReader reader = new StreamReader(descriptionFile);
            try {
              returnVal.Description = reader.ReadToEnd();
            }
            catch { }
            finally {
              reader.Close();
              reader.Dispose();
            }
          }

          Dictionary<string, string> captions = new Dictionary<string, string>();
          Dictionary<string, int> links = new Dictionary<string, int>();

          string captionFile = directory + "\\captions.txt";
          if (System.IO.File.Exists(captionFile)) {
            StreamReader reader = new StreamReader(captionFile);
            try {
              string name, caption, link;
              int linkFolderId;
              string line;
              string[] split;
              while (!reader.EndOfStream) {
                line = reader.ReadLine();
                split = line.Split(new char[] { '|' });

                switch (split.Length) {
                  case 2:
                    name = split[0];
                    caption = split[1];

                    if (!captions.ContainsKey(name)) {
                      captions.Add(name, caption);
                    }
                    break;
                  case 3:
                    name = split[0];
                    caption = split[1];
                    link = split[2];
                    if (!captions.ContainsKey(name)) {
                      captions.Add(name, caption);
                    }
                    if (!links.ContainsKey(name) && int.TryParse(link, out linkFolderId)) {
                      links.Add(name, linkFolderId);
                    }
                    break;
                }
              }
            }
            catch { }
            finally {
              reader.Close();
              reader.Dispose();
            }
          }


          List<string> fileNames = new List<string>();
          List<int> widths = new List<int>();
          string ext;
          Size imageSize;
          int realWidth;
          int width = 0;
          int height = 580;

          string fName;
          foreach (string file in Directory.GetFiles(directory)) {
            ext = Path.GetExtension(file);
            if (ext.ToLower() == ".jpg") {
              fName = Path.GetFileNameWithoutExtension(file);

              if (!fName.Contains("@2x")) {
                if (!fileNames.Contains(Path.GetFileNameWithoutExtension(file))) {
                  imageSize = ImageDimensions.ImageHelper.GetDimensions(file);

                  if (imageSize.Width > 0 &&
                      imageSize.Height > 0) {
                    realWidth = (int)((double)height * (double)imageSize.Width / (double)imageSize.Height);
                    width += realWidth;
                    widths.Add(realWidth);
                    fileNames.Add(Path.GetFileNameWithoutExtension(file));
                  }
                }
              }
            }
          }

          returnVal.GalleryWidth = width;

          //Sort alphabetically
          List<List<timothy.Models.PagePanel>> images = new List<List<timothy.Models.PagePanel>>();
          fileNames.Sort(delegate(string a, string b) {

            return Convert.ToInt32(a).CompareTo(Convert.ToInt32(b));
          });
         
          string f;
          for (int i = 0; i < fileNames.Count; i++) {
            f = fileNames[i];

            if (isRetina && System.IO.File.Exists(directory.TrimEnd("/\\".ToCharArray()) + "\\" + f + "@2x.jpg")) {
              images.Add(new List<timothy.Models.PagePanel>() { new timothy.Models.PagePanel() {
                URL = subDirectory + "/" + f + "@2x.jpg",
                Caption = captions.ContainsKey(f) ? captions[f] : "",
                Width = widths[i],
                LinkFolderId = -1,// links.ContainsKey(f) ? links[f] : -1,
              } });
            }
            else {
              images.Add(new List<timothy.Models.PagePanel>() { new timothy.Models.PagePanel() {
                URL = subDirectory + "/" + f + ".jpg",
                Caption = captions.ContainsKey(f) ? captions[f] : "",
                Width = widths[i],
                LinkFolderId = -1,//links.ContainsKey(f) ? links[f] : -1,
              }});
            }

          }

          returnVal.Images = images;
        }

      }

      return Json(returnVal);
    }

    public ActionResult Gallery(int folderId, int galleryId, string folderName) {
      bool isRetina;
      bool isDouble;
      List<int> openFolderIndexes;
      this.getQueryValues(Request, out isRetina, out isDouble, out openFolderIndexes);

      ViewData["FOLDER_ID"] = folderId;
      ViewData["GALLERY_ID"] = galleryId;
      ViewData["FOLDER_NAME"] = folderName;
      ViewData["IS_RETINA"] = isRetina;
      ViewData["IS_DOUBLE"] = isDouble;
      this.setupLinks(ViewData, openFolderIndexes);
      return View("GalleryNew");
    }



    public ActionResult Videos() {
      bool isRetina;
      bool isDouble;
      List<int> openFolderIndexes;
      this.getQueryValues(Request, out isRetina, out isDouble, out openFolderIndexes);

      List<timothy.Models.Video> returnValue = new List<Models.Video>();
      string directory = (Server.MapPath("~").TrimEnd("/\\".ToCharArray()) + "\\" + ConfigurationManager.AppSettings["DATA_SUBDIRECTORY"]).TrimEnd("/\\".ToCharArray()) + "\\data\\";

      string videosFile = directory + "\\videos.txt";

      if (System.IO.File.Exists(videosFile)) {
        StreamReader reader = new StreamReader(videosFile);
        try {
          string embedScript, url, caption;
          string line;
          string[] split;
          while (!reader.EndOfStream) {
            line = reader.ReadLine();
            split = line.Split(new char[] { '|' });

            switch (split.Length) {
              case 3:
                url = split[0];
                embedScript = split[1];
                caption = split[2];
                returnValue.Add(new Models.Video() {
                  Url = url,
                  EmbedScript = embedScript,
                  Caption = caption
                });
                break;
            }
          }
        }
        catch { }
        finally {
          reader.Close();
          reader.Dispose();
        }
      }

      ViewData["VIDEO_WIDTH"] = "750";
      ViewData["VIDEO_PADDING"] = "10";
      ViewData["FOLDER_NAME"] = "Videos";
      ViewData["IS_RETINA"] = isRetina;
      this.setupLinks(ViewData, openFolderIndexes);
      return View(returnValue);
    }

    private void getQueryValues(HttpRequestBase request, out bool isRetina, out bool isDouble, out List<int> openFolderIndexes) {
      isRetina = false;
      string p = request.QueryString["isRetina"];
      if (!string.IsNullOrEmpty(p)) {
        bool.TryParse(p, out isRetina);
      }
      isDouble = false;
      p = request.QueryString["isDouble"];
      if (!string.IsNullOrEmpty(p)) {
        bool.TryParse(p, out isDouble);
      }
      openFolderIndexes = new List<int>();
      p = request.QueryString["openMenus"];
      if (!string.IsNullOrEmpty(p)) {
        string[] menus = p.Split("|".ToCharArray());
        int mnu;
        foreach (string m in menus) {
          if (int.TryParse(m, out mnu)) {
            openFolderIndexes.Add(mnu);
          }
        }
      }
    }

    private void setupLinks(ViewDataDictionary viewData, List<int> openFolderIndexes) {

      string file = (Server.MapPath("~").TrimEnd("/\\".ToCharArray()) + "\\" + ConfigurationManager.AppSettings["DATA_SUBDIRECTORY"]).TrimEnd("/\\".ToCharArray()) + "\\data\\links.txt";

      if (System.IO.File.Exists(file)) {
        StreamReader r = new StreamReader(file);

        try {
          List<timothy.Models.Link> links = new List<timothy.Models.Link>();
          string line;
          string[] split;

          int folderIndex = 0;
          while (!r.EndOfStream) {
            line = r.ReadLine();

            split = line.Split(new char[] { '|' });


            if (3 == split.Length) {
              if (Directory.Exists((Server.MapPath("~").TrimEnd("/\\".ToCharArray()) + "\\" + ConfigurationManager.AppSettings["DATA_SUBDIRECTORY"]).TrimEnd("/\\".ToCharArray()) + "\\data\\" + split[0])) {
                if (split[0] != "0") {

                  string file2 = (Server.MapPath("~").TrimEnd("/\\".ToCharArray()) + "\\" + ConfigurationManager.AppSettings["DATA_SUBDIRECTORY"]).TrimEnd("/\\".ToCharArray()) + "\\data\\" + split[0] + "\\links.txt";

                  List<timothy.Models.Link> links2 = new List<timothy.Models.Link>();

                  if (System.IO.File.Exists(file2)) {
                    StreamReader r2 = new StreamReader(file2);

                    try {

                      string line2;
                      string[] split2;

                      while (!r2.EndOfStream) {
                        line2 = r2.ReadLine();

                        split2 = line2.Split(new char[] { '|' });


                        if (4 == split2.Length) {
                          if (Directory.Exists((Server.MapPath("~").TrimEnd("/\\".ToCharArray()) + "\\" + ConfigurationManager.AppSettings["DATA_SUBDIRECTORY"]).TrimEnd("/\\".ToCharArray()) + "\\data\\" + split[0] + "\\" + split2[0])) {
                            if (split2[0] != "0") {


                              bool isDouble = false;
                              bool.TryParse(split2[3], out isDouble);

                              links2.Add(new timothy.Models.Link() {
                                Text = split2[1],
                                URL = split2[0],
                                ToolTip = split2[2] != null ? split2[2] : "",
                                IsDouble = isDouble
                              });
                            }

                          }

                        }
                      }
                    }
                    finally {
                      r2.Close();
                      r2.Dispose();
                    }
                  }

                  links.Add(new timothy.Models.Link() {
                    Text = split[1],
                    URL = split[0],
                    ToolTip = split[2] != null ? split[2] : "",
                    Links = links2,
                    IsOpen = openFolderIndexes.Contains(folderIndex)
                  });

                  folderIndex++;
                }

              }

            }
          }

          viewData["Links"] = links;
        }
        finally {
          r.Close();
          r.Dispose();
        }
      }
      else {
        viewData["Links"] = new List<timothy.Models.Link>();
      }
    }

    private void setupForBackgroundImage(ViewDataDictionary viewData) {
      viewData["ShowBackground"] = true;
      viewData["SideBarTheme"] = "imageTheme";
      viewData["SideBarLinksTheme"] = "lightLinks";
      viewData["FooterTheme"] = "lightFooter";
    }

    private void setupForNoBackgroundImage(ViewDataDictionary viewData) {
      viewData["ShowBackground"] = false;
      viewData["SideBarTheme"] = "noImageTheme";
      viewData["SideBarLinksTheme"] = "darkLinks";
      viewData["FooterTheme"] = "darkFooter";
    }

  }
}
