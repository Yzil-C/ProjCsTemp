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
    /// Logique d'interaction pour Room.xaml
    /// </summary>
    public partial class Room : UserControl
    {
        public delegate void GameStarterEventHandler();

        public event GameStarterEventHandler IsGameStarted;

        public MainViewModel ViewModel => DataContext as MainViewModel;

        public Room()
        {
            InitializeComponent();
        }

        private void OnStart(object sender, RoutedEventArgs e)
        {
            IsGameStarted?.Invoke();
        }
    }
}
