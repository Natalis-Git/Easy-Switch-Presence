
using System;
using System.Globalization;
using System.Windows.Data;




namespace EasySwitchPresence
{

    /// <summary>
    /// This class is a basic value converter for booleans to be used with the settings UI radio buttons.
    /// This nonsense is unfortunately necessary to prevent bindings being screwed up by WPF; Without it, all bound
    /// values would be set back to false immediately after being set from a user click.
    /// </summary>
    public class BoolConverter : IValueConverter
    {
        public bool Inverse { get; set; }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;

            return Inverse ? !boolValue : boolValue;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;

            if (!boolValue)
            {
                return null;
            }

            return !Inverse;
        }
    }

}
