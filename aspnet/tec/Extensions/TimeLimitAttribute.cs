using System;
using System.Web.Mvc;

namespace MvcController.Extensions
{
  public class TimeLimitAttribute : FilterAttribute, IAuthorizationFilter
  {
    private DateTime _begin = DateTime.MinValue;
    private DateTime _end = DateTime.MaxValue;

    public String Begin
    {
      set
      {
        var b = DateTime.Parse(value);
        if (b >= this._end)
        {
          throw new ArgumentException("Begin parameter is invalid.");
        }
        this._begin = b;
      }
    }

    public String End
    {
      set
      {
        var e = DateTime.Parse(value);
        if (this._begin >= e)
        {
          throw new ArgumentException("End parameter is invalid.");
        }
        this._end = e;
      }
    }

    public TimeLimitAttribute(String begin, String end)
    {
      this.Begin = begin;
      this.End = end;
    }

    public void OnAuthorization(AuthorizationContext filterContext)
    {
      if (filterContext == null)
      {
        throw new ArgumentNullException("filterContext");
      }
      var current = DateTime.Now;
      if (current < this._begin || current > this._end)
      {
        var msg = String.Format("このページは{0}から{1}までの期間のみ有効です。", 
          this._begin.ToLongDateString(), this._end.ToLongDateString());
        throw new TimeLimitException(msg);
        // filterContext.Result = new ContentResult() { Content=msg };
      }
    }
  }

  [Serializable]
  public class TimeLimitException : Exception
  {
    public TimeLimitException(string message)
      : base(message)
    {
    }
  }
}