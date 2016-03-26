using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace timothy.Models {
  public class Gallery {
    public string FolderName {
      get;
      set;
    }

    public string Description {
      get;
      set;
    }

    public List<List<PagePanel>> Images {
      get;
      set;
    }

    public int GalleryWidth {
      get;
      set;
    }
  }
}