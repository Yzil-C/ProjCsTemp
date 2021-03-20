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

namespace GarticPhone.Utilities
{
    /// <summary>
    /// Logique d'interaction pour RoundControl.xaml
    /// </summary>
    public partial class RoundControl : UserControl
    {
        public delegate void RoundFinished();

        public event RoundFinished IsRoundFinished;

        public RoundControl()
        {
            InitializeComponent();
        }




    }
}
