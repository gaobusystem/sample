using System;
using System.Web.Mvc;

namespace MvcController.Extensions
{
  public class ReferrerAttribute : ActionMethodSelectorAttribute
  {

    public bool CanNull { get; private set; }

    public ReferrerAttribute(bool canNull)
    {
      this.CanNull = canNull;
    }

    public override bool IsValidForRequest(ControllerContext controllerContext, System.Reflection.MethodInfo methodInfo)
    {
      var req = controllerContext.HttpContext.Request;
      var referrer = req.UrlReferrer;
      if (referrer == null) { return this.CanNull; }
      var refHost = referrer.Host;
      var currentHost = req.Url.Host;
      return refHost.Equals(currentHost, StringComparison.InvariantCultureIgnoreCase);
    }
  }
}
