using GarticPhone.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GarticPhone.GarticWindows
{
    /// <summary>
    /// Logique d'interaction pour PreRoom.xaml
    /// </summary>
    public partial class PreRoom : UserControl
    {
        public delegate void UserChoiceEventHandler();

        public event UserChoiceEventHandler UserChoosen;

        public delegate void NextIconEventHandler();

        public event NextIconEventHandler IsNextIconSelected;

        public delegate void PreviousIconEventHandler();

        public event PreviousIconEventHandler IsPreviousIconSelected;

        public MainViewModel ViewModel => DataContext as MainViewModel;

        public PreRoom()
        {
            InitializeComponent();
        }

        private void Validation(object sender, RoutedEventArgs e)
        {
            UserChoosen?.Invoke();
        }

        private void NextIconSelected(object sender, RoutedEventArgs e)
        {
            IsNextIconSelected?.Invoke();
        }

        private void PreviousIconSelected(object sender, RoutedEventArgs e)
        {
            IsPreviousIconSelected?.Invoke();
        }
    }
}
