using GarticPhone.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GarticPhone.ViewModels
{
    public delegate void TimerTickEventHandler(float currentEvolution, bool isFinished);

    public class MainViewModel : ViewModelBase
    {
        //Windows Property to apply logic
        public static Dictionary<GarticWindow, float> TIMING_INF = new Dictionary<GarticWindow, float>
        {
            { GarticWindow.FirstTextWindow, 3},
            { GarticWindow.PaintWindow, 5},
            { GarticWindow.GuessDrawWindow, 10}
        };

        private GarticWindow currentWindow = GarticWindow.PreRoom;

        public GarticWindow CurrentWindow
        {
            get { return currentWindow; }
            set
            {
                currentWindow = value;
                OnPropertyChanged(nameof(CurrentWindow));
            }
        }


        //TimerGestion
        private DispatcherTimer _dispatcherTimer;
        private float _currentTime = 0;
        public event TimerTickEventHandler TimerTick;

        public void StartTimer()
        {
            _dispatcherTimer = new DispatcherTimer();
            
            _dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            _dispatcherTimer.Start();
        }

        public void ResetTimer()
        {
            _currentTime = 0;
        }

        public void StopTimer()
        {
            _dispatcherTimer.Stop();
            _dispatcherTimer.Tick -= new EventHandler(DispatcherTimer_Tick);
            _dispatcherTimer = null;
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {

            bool finished = false;
            if (_currentTime < TIMING_INF[CurrentWindow])
            {
                _currentTime += 1;
            }
            else
            {
                finished = true;
                ChangeWindow();
            }
            if (TIMING_INF.ContainsKey(CurrentWindow))
                TimerTick?.Invoke(_currentTime / TIMING_INF[CurrentWindow], finished);
        }



        //RoundGestion
        private int _currentRound;

        public int Round
        {
            get { return _currentRound; }
            set
            {
                _currentRound = value;
                LabelRound = $"{_currentRound}/{_totalRound}";
            }
        }

        private static int _totalRound;

        private string _labelRound;

        public string LabelRound
        {
            get { return _labelRound; }
            set
            {
                _labelRound = value;
                OnPropertyChanged(nameof(LabelRound));
            }
        }


        //RoomGestion
        private ObservableCollection<User> users = new ObservableCollection<User>();


        public ObservableCollection<User> Users
        {
            get { return users; }
            set
            {
                users = value;
            }
        }

        public void OnGameStarted()
        {
            StartTimer();
            _totalRound = Users.Count + 3;
            Round = 1;
            LabelRound = $"{Round}/{_totalRound}";
            CurrentWindow = GarticWindow.FirstTextWindow;
        }


        //New User & PreRoom Gestion
        public static List<string> IMAGE_INF = new List<string>
        {
            "/GarticPhone;component/GarticWindows/Resource/UserIcons/icon1.png",
            "/GarticPhone;component/GarticWindows/Resource/UserIcons/icon2.png",
            "/GarticPhone;component/GarticWindows/Resource/UserIcons/icon3.png"
        };

        private static Random rnd = new Random();

        private static int index = rnd.Next(IMAGE_INF.Count);

        private string _username;

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        private string _imageSource = IMAGE_INF[index];

        public string ImageSource
        {
            get { return _imageSource; }
            set 
            { 
                _imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }

        public void OnUserChoosen()
        {
            User currentUser = new User(Username, ImageSource);
            users.Add(currentUser);
            CurrentWindow = GarticWindow.Room;
        }

        public void OnNextIconSelected()
        {
            index++;
            ImageSource = IMAGE_INF[index % IMAGE_INF.Count];
        }

        public void OnPreviousIconSelected()
        {
            index--;
            ImageSource = IMAGE_INF[index % IMAGE_INF.Count];
        }

        
        //PaintGestion
        public void OnDrawFinished()
        {
            ChangeWindow();
        }

        //Changing Window Gestion
        private void ChangeWindow()
        {
            
            if(Round < _totalRound)
            {
                ResetTimer();
                Round++;
                if (CurrentWindow == GarticWindow.FirstTextWindow)
                    CurrentWindow = GarticWindow.PaintWindow;

                else if (CurrentWindow == GarticWindow.GuessDrawWindow)
                    CurrentWindow = GarticWindow.PaintWindow;
                else
                    CurrentWindow = GarticWindow.GuessDrawWindow;
            }
            else
            {
                StopTimer();
                CurrentWindow = GarticWindow.RecapWindow;
            }
        }

    }
}

