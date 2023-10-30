using System;
using System.Web.Mvc;

namespace MvcController.Extensions
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
  public class CheckOrderAttribute : ActionFilterAttribute
  {

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      filterContext.HttpContext.Response.Write(
          String.Format("<p>OnActionExecuting{0}</p>", this.Order));
      //if (this.Order == 2)
      //{
      //  throw new Exception("例外発生");
      //}
    }

    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
      filterContext.HttpContext.Response.Write(
          String.Format("<p>OnActionExecuted{0}</p>", this.Order));
      //filterContext.ExceptionHandled = true;
    }

    public override void OnResultExecuting(ResultExecutingContext filterContext)
    {
      filterContext.HttpContext.Response.Write(
          String.Format("<p>OnResultExecuting{0}</p>", this.Order));
    }

    public override void OnResultExecuted(ResultExecutedContext filterContext)
    {
      filterContext.HttpContext.Response.Write(
          String.Format("<p>OnResultExecuted{0}</p>", this.Order));
    }
  }
}
