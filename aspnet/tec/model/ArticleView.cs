using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcModel.Models
{
  public class ArticleView
  {
    public string Title { get; set; }
    public int Viewcount { get; set; }
    public string Released { get; set; }
  }
}