using GarticPhone.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace GarticPhone.Utilities
{
    public class SetWindowVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var win = parameter?.ToString();

            if(value is GarticWindow verif)
            {
                switch(verif)
                {
                    case GarticWindow.PaintWindow when win == "PaintWindow":

                    case GarticWindow.PreRoom when win == "PreRoom":

                    case GarticWindow.Room when win == "Room":

                    case GarticWindow.FirstTextWindow when win == "FirstTextWindow":

                    case GarticWindow.GuessDrawWindow when win == "GuessDrawWindow":

                    case GarticWindow.RecapWindow when win == "RecapWindow":

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
