using MvcController.Models;
using System;
using System.Web.Mvc;

namespace MvcController.Extensions
{
  public class LoggingAttribute : ActionFilterAttribute
  {
    private MvcControllerContext db = new MvcControllerContext();

    public override void OnResultExecuted(ResultExecutedContext filterContext)
    {
      if (filterContext == null)
      {
        throw new ArgumentNullException("filterContext");
      }
      var req = filterContext.HttpContext.Request;
      var log = new AccessLog()
      {
        Url = req.Url.AbsoluteUri,
        UserAgent = req.UserAgent,
        Accessed = DateTime.Now
      };
      db.AccessLogs.Add(log);
      db.SaveChanges();
    }
  }
}
