using System;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace MvcController.Extensions
{
  public class TranslatedXmlResult : ActionResult
  {
    private XslCompiledTransform _xslt;

    public XDocument Document { get; private set; }

    public TranslatedXmlResult(XDocument doc)
    {
      this.Document = doc;
    }

    public override void ExecuteResult(ControllerContext context)
    {
      var controller = context.RouteData.Values["controller"];
      var action = context.RouteData.Values["action"];
      var path = String.Format("~/Views/{0}/{1}.xslt", controller, action);
      _xslt = new XslCompiledTransform();
      _xslt.Load(context.HttpContext.Server.MapPath(path));
      _xslt.Transform(Document.CreateReader(), null,
          context.HttpContext.Response.OutputStream);
    }
  }
}