using MvcController.Models;
using System;
using System.Web.Mvc;

namespace MvcController.Extensions
{
  public class LoggingErrorAttribute : FilterAttribute, IExceptionFilter
  {
    private MvcControllerContext db = new MvcControllerContext();

    public void OnException(ExceptionContext filterContext)
    {
      if (filterContext == null)
      {
        throw new ArgumentNullException("filterContext");
      }
      var route = filterContext.RouteData;
      var exp = filterContext.Exception;

      var err = new ErrorLog
      {
        Controller = route.Values["controller"].ToString(),
        Action = route.Values["action"].ToString(),
        Message = exp.Message,
        Stacktrace = exp.StackTrace,
        Updated = DateTime.Now
      };
      db.ErrorLogs.Add(err);
      db.SaveChanges();
      //filterContext.ExceptionHandled = true;
      //filterContext.Result = new ContentResult() { Content="例外は処理されました。"};

    }
  }
}
