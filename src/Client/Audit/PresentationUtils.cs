using System;
using System.Globalization;
using System.Linq;

namespace TDL.Client.Audit
{
    public static class PresentationExtensions
    {
        public static string ToDisplayableString(this object[] items) =>
            string.Join(", ", items.Select(ToDisplayableString));

        public static string ToDisplayableString(this object item)
        {
            var itemString = item.ToString();

            if (IsMultiLineString(itemString))
                itemString = SuppressExtraLines(itemString);

            if (IsNotNumeric(item))
                itemString = AddQuotes(itemString);

            return itemString;
        }

        private static bool IsMultiLineString(string value) =>
            value.Contains("\n");

        private static bool IsNotNumeric(object item) =>
            !IsNumeric(item);

        private static bool IsNumeric(object item) =>
            item != null &&
            double.TryParse(Convert.ToString(item, CultureInfo.InvariantCulture), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out double _);

        private static string SuppressExtraLines(string value)
        {
            var parts = value.Split('\n');
            var plural = parts.Length > 1 ? "s" : string.Empty;

            return $"{parts[0]} .. ( more line{plural})";
        }

        private static string AddQuotes(string value) =>
            $@"""{value}""";
    }
}
