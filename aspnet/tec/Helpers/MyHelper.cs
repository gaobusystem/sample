using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using System.Web.Mvc.Html;

namespace MvcView.Helpers
{
  public static class MyHelper
  {
    public static IHtmlString Mailto(String address, String linktext)
    {
      return MvcHtmlString.Create(
        String.Format("<a href=\"mailto:{0}\">{1}</a>",
          HttpUtility.HtmlAttributeEncode(address),
          HttpUtility.HtmlAttributeEncode(linktext)));
    }

    public static IHtmlString Image(this HtmlHelper helper, String src, String alt)
    {
      return MvcHtmlString.Create(
        String.Format("<img src=\"{0}\" alt=\"{1}\" />",
        HttpUtility.HtmlAttributeEncode(
          UrlHelper.GenerateContentUrl(src, helper.ViewContext.HttpContext)),
        HttpUtility.HtmlAttributeEncode(alt)));
    }

    public static IHtmlString Video(
    this HtmlHelper helper, String src, Object htmlAttrs)
    {
      var builder = new TagBuilder("video");
      builder.MergeAttribute("src",
        UrlHelper.GenerateContentUrl(src, helper.ViewContext.HttpContext));
      builder.MergeAttribute("controls", "controls");
      builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttrs));
      return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
    }

    public static IHtmlString RadioButtonListFor<TModel, TProperty>(
      this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> exp,
      IEnumerable<SelectListItem> list, Object htmlAttrs)
    {
      var sb = new StringBuilder();
      var name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(
          ExpressionHelper.GetExpressionText(exp));
      var value = ModelMetadata.FromLambdaExpression(exp, helper.ViewData).Model.ToString();

      int i = 1;
      foreach (var item in list)
      {
        var id = String.Format("{0}_{1}", name, i++);

        var label = new TagBuilder("label");
        label.MergeAttributes(
          HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttrs));
        label.InnerHtml = helper.RadioButton(name, item.Value, (item.Value == value), new { id = id }).ToString();
        label.InnerHtml += item.Text;
        sb.Append(label.ToString(TagRenderMode.Normal));
      }

      return MvcHtmlString.Create(sb.ToString());
    }

    public static IHtmlString TemplatedMessage(this HtmlHelper helper, Func<string, HelperResult> template, string message)
    {
      return template(message);
    }
  }
}