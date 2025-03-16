using System.Globalization;


namespace MauiClient.Utilities
{
    class DateConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return StaticDateConverter.Convert(value, targetType, parameter, culture);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return StaticDateConverter.ConvertBack(value, targetType, parameter, culture);
        }
    }

    public static class StaticDateConverter
    {
        public static DateTime Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateOnly date)
            {
                return date.ToDateTime(TimeOnly.MinValue);
            }

            return DateTime.Now;
        }

        public static DateOnly ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                return DateOnly.FromDateTime(date);
            }

            return DateOnly.FromDateTime(DateTime.Now);
        }
    }
}
