using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace timothy.Models {
  public class PagePanel {
    public string Html {
      get;
      set;
    }

    public string URL {
      get;
      set;
    }

    public string Caption {
      get;
      set;
    }

    public int Width {
      get;
      set;
    }

    public int LinkFolderId {
      get;
      set;
    }
  }
}