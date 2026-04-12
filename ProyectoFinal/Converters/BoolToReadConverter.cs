using System.Globalization;

namespace ProyectoFinal.Converters;

public class BoolToReadConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isRead)
            return isRead ? "✓ Leído" : "⏳ Pendiente";
        return "⏳ Pendiente";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}