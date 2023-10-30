using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcController.Extensions
{
  public class YMDBinder : IModelBinder
  {
    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
      var result = default(DateTime);
      try
      {
        result = new DateTime(
          GetYmd(bindingContext, "year"),
          GetYmd(bindingContext, "month"),
          GetYmd(bindingContext, "day")
        );
      } catch { }
      return result;
    }

    private int GetYmd(ModelBindingContext context, string type)
    {
      var result = 0;
      var value = context.ValueProvider.GetValue(
        string.Format("{0}.{1}", context.ModelName, type));
      try
      {
        result = (int)value.ConvertTo(typeof(int));
      } catch { }
      return result;
    }
  }
}