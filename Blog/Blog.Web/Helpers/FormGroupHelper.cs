﻿namespace Blog.Web.Helpers
{
    using System;
    using System.IO;
    using System.Linq.Expressions;
    using System.Text.Encodings.Web;
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class FormGroupHelper
    {
        public static IHtmlContent FormGroupFor<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression)
        {
            using (var writer = new StringWriter())
            {
                var label = htmlHelper.LabelFor(expression);

                var editor = htmlHelper.EditorFor(expression, new { htmlAttributes = new { @class = "single-input-primary" } });
                var validationMessage = htmlHelper.ValidationMessageFor(expression, null, new { @class = "text-danger" });

                writer.Write("<div class=\"form-group\">");
                label.WriteTo(writer, HtmlEncoder.Default);
                editor.WriteTo(writer, HtmlEncoder.Default);
                validationMessage.WriteTo(writer, HtmlEncoder.Default);
                writer.Write("</div>");

                return new HtmlString(writer.ToString());
            }
        }
    }
}
