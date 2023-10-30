using System;
using System.Web.Mvc;

namespace MvcController.Extensions
{
  public class MyActionAttribute : ActionNameSelectorAttribute
  {
    public override bool IsValidName(ControllerContext controllerContext, string actionName, System.Reflection.MethodInfo methodInfo)
    {
      var name = methodInfo.Name;
      return name.EndsWith("Action") && String.Equals(actionName, name.Substring(0, name.Length - 6), StringComparison.OrdinalIgnoreCase);
    }
  }
}
