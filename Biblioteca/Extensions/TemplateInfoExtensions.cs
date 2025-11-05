using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BibliotecaUNAPEC.Extensions
{
    public static class TemplateInfoExtensions
    {
        public static string GetFullHtmlFieldId(this TemplateInfo templateInfo, string name)
        {
            if (templateInfo is null) throw new ArgumentNullException(nameof(templateInfo));
            var fullName = templateInfo.GetFullHtmlFieldName(name ?? string.Empty) ?? string.Empty;
            return fullName.Replace('.', '_');
        }
    }
}