
using GarticPhone.ModelsToDraw;
using GarticPhone.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GarticPhone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    

    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel => DataContext as MainViewModel;


        public MainWindow()
		{
			InitializeComponent();
        }

        private void OnDrawFinished() => ViewModel.OnDrawFinished();

        private void OnUserChoosen() => ViewModel.OnUserChoosen();

        private void OnNextIconSelected() => ViewModel.OnNextIconSelected();

        private void OnPreviousIconSelected() => ViewModel.OnPreviousIconSelected();

        private void OnGameStarted() => ViewModel.OnGameStarted();
    }
}
