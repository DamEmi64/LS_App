using System.Text.RegularExpressions;

namespace Base
{
    public static class TemplateFormatter
    {
        private static readonly Regex PlaceholderRegex = new(@"\{(\w+)\}", RegexOptions.Compiled);

        /// <summary>
        /// Replaces {PropertyName} placeholders in a template with values from the given object's properties.
        /// </summary>
        public static string Format(string template, object values)
        {
            var type = values.GetType();

            return PlaceholderRegex.Replace(template, match =>
            {
                string propertyName = match.Groups[1].Value;
                var property = type.GetProperty(propertyName);

                if (property is null)
                {
                    // Leave unknown placeholders untouched rather than throwing
                    return match.Value;
                }

                var value = property.GetValue(values);
                return value?.ToString() ?? string.Empty;
            });
        }
    }
}
