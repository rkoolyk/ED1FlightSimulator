using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ED1FlightSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private IViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = new ViewModel();
            DataContext = vm;
            this.HeightText.Text = "0";
            this.SpeedText.Text = "0";
            this.DirText.Text = "0";
            this.Time.Text = "00:00:00";
            this.Elevator.Text = "<- Elevator ->";
            this.Aileron.Text = "<- Aileron ->";
            this.Throttle.Text = "<- Throttle ->";
            this.Rudder.Text = "<- Rudder ->";
            this.ThrottleSlider.Margin = new Thickness(0, BoundTop, 0, 0);
            this.RudderSlider.Margin = new Thickness(BoundLeft, 0, 0, 0);
            this.Yaw.Text = "0";
            this.Roll.Text = "0";
            this.Pitch.Text = "0";
            
        }

        private double _boundTop = 135;

        public double BoundTop
        {
            get
            {
                return _boundTop;
            }

            set
            {
                if (_boundTop != value)
                {
                    _boundTop = value;
                }

                onPropertyChanged("BoundTop");
            }
        }
        
        private double _boundLeft = 135;

        public double BoundLeft
        {
            get
            {
                return _boundLeft;
            }

            set
            {
                if (_boundLeft != value)
                {
                    _boundLeft = value;
                }

                onPropertyChanged("BoundLeft");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void LoadCsv_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("CSV!!!!!!!!");
        }

        private void LoadXml_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("XML!!!!!!!!");
        }

        private void PlaySpeed_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("SELECTION CHANGED");
        }
    }
}