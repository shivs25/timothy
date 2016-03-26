using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace timothy.Models {
  public class Link {

    public string Text {
      get;
      set;
    }

    public string URL {
      get;
      set;
    }

    public string ToolTip {
      get;
      set;
    }

    public List<Link> Links {
      get;
      set;
    }


    public bool IsDouble { get; set; }

    public bool IsOpen { get; set; }
  }
}