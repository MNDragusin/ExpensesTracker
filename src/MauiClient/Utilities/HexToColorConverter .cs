using System.Diagnostics;
using System.Globalization;


namespace MauiClient.Utilities
{
    class HexToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return HexConverter.Convert(value, targetType, parameter, culture);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return HexConverter.ConvertBack(value, targetType, parameter, culture);
        }
    }

    public static class HexConverter
    {
        public static object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string hex && !string.IsNullOrWhiteSpace(hex))
            {
                try
                {
                    return Color.FromArgb(hex);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return Colors.Transparent;
                }
            }

            return Colors.Transparent;
        }

        public static object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
