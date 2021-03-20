using GarticPhone.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace GarticPhone.Utilities
{
    class SetGameToolsVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var win = parameter?.ToString();

            if(value is GarticWindow verif)
            {
                switch(verif)
                {
                    case GarticWindow.FirstTextWindow when (win == "TimerControl" || win == "RoundControl"):

                    case GarticWindow.PaintWindow when (win == "TimerControl" || win == "RoundControl"):

                    case GarticWindow.GuessDrawWindow when (win == "TimerControl" || win == "RoundControl"):

                        return Visibility.Visible;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
