using System.Collections.Specialized;
using System.Globalization;
using System.Web.Mvc;

namespace MvcController.Extensions
{
  public class HttpCookieValueProviderFactory : ValueProviderFactory
  {
    public override IValueProvider GetValueProvider(
      ControllerContext controllerContext)
    {
      var list = new NameValueCollection();

      var cookies = controllerContext.HttpContext.Request.Cookies;
      foreach (var key in cookies.AllKeys)
      {
        list.Add(key, cookies[key].Value);
      }

      return new NameValueCollectionValueProvider(
        list, CultureInfo.CurrentCulture);
    }
  }
}