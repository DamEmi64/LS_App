using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Base
{
    public static class TemplateFormatter
    {
        private static readonly Regex PlaceholderWithFormatRegex =
            new(@"\{(\w+):([^}]+)\}", RegexOptions.Compiled);

        private static readonly Regex PlaceholderRegex =
            new(@"\{(\w+)\}", RegexOptions.Compiled);

        public static string Format(string template, object values)
        {
            var type = values.GetType();

            // Replace formatted placeholders first
            template = PlaceholderWithFormatRegex.Replace(template, match =>
            {
                var propertyName = match.Groups[1].Value;
                var format = match.Groups[2].Value;

                var property = type.GetProperty(
                    propertyName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                if (property == null)
                    return match.Value;

                var value = property.GetValue(values);
                return value?.ToFormatString(format) ?? string.Empty;
            });

            // Replace simple placeholders
            return PlaceholderRegex.Replace(template, match =>
            {
                var propertyName = match.Groups[1].Value;

                var property = type.GetProperty(
                    propertyName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                if (property == null)
                    return match.Value;

                var value = property.GetValue(values);
                return value?.ToString() ?? string.Empty;
            });
        }

        private static string? ToFormatString(this object value, string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                return value.ToString();

            switch (format.ToLowerInvariant())
            {
                case "joinbyline":
                    if (value is IEnumerable enumerable && value is not string)
                        return string.Join(Environment.NewLine, enumerable.Cast<object>());
                    return value.ToString();

                case "joinbycomma":
                    if (value is IEnumerable enumerable2 && value is not string)
                        return string.Join(",", enumerable2.Cast<object>());
                    return value.ToString();

                default:
                    return value is IFormattable formattable
                        ? formattable.ToString(format, CultureInfo.InvariantCulture)
                        : value.ToString();
            }
        }
    }
}
