using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Shapes;

namespace ED1FlightSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            this.HeightText.Text = "0";
            this.SpeedText.Text = "0";
            this.DirText.Text = "0";
            this.Time.Text = "00:00:00";
            this.Elevator.Text = "<- Elevator ->";
            this.Aileron.Text = "<- Aileron ->";
            this.Throttle.Text = "<- Throttle ->";
            this.Rudder.Text = "<- Rudder ->";
            this.ThrottleSlider.Margin = new Thickness(0, 150, 0, 0);
            // ThrottleSlider.SetValue(Canvas.TopProperty, BoundTop);
            // RudderSlider.SetValue(Canvas.LeftProperty, BoundLeft);
            // CenterPos.SetValue(VisibilityProperty, Center);
            // UpPos.SetValue(VisibilityProperty, Up);
            // DownPos.SetValue(VisibilityProperty, Down);
            // LeftPos.SetValue(VisibilityProperty, Left);
            // RightPos.SetValue(VisibilityProperty, Right);
            // UpRightPos.SetValue(VisibilityProperty, UpRight);
            // UpLeftPos.SetValue(VisibilityProperty, UpLeft);
            // DownLeftPos.SetValue(VisibilityProperty, DownLeft);
            // DownRightPos.SetValue(VisibilityProperty, DownRight);
        }

        private double _boundTop = 150;

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
        
        private double _boundLeft = 150;

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

        private Visibility _center = Visibility.Visible;

        public Visibility Center
        {
            get
            {
                return _center;
            }
            set
            {
                if (_center != value)
                {
                    _center = value;
                }
                onPropertyChanged("Center");
            }
        }
        
        
        private Visibility _up = Visibility.Hidden;

        public Visibility Up
        {
            get
            {
                return _up;
            }
            set
            {
                if (_up != value)
                {
                    _up = value;
                }
                onPropertyChanged("Up");
            }
        }
        
        private Visibility _down = Visibility.Hidden;

        public Visibility Down
        {
            get
            {
                return _down;
            }
            set
            {
                if (_down != value)
                {
                    _down = value;
                }
                onPropertyChanged("Down");
            }
        }
        
        private Visibility _left = Visibility.Hidden;

        public Visibility Left
        {
            get
            {
                return _left;
            }
            set
            {
                if (_left != value)
                {
                    _left = value;
                }
                onPropertyChanged("Left");
            }
        }
        
        private Visibility _right = Visibility.Hidden;

        public Visibility Right
        {
            get
            {
                return _right;
            }
            set
            {
                if (_right != value)
                {
                    _right = value;
                }
                onPropertyChanged("Right");
            }
        }
        
        private Visibility _upRight = Visibility.Hidden;

        public Visibility UpRight
        {
            get
            {
                return _upRight;
            }
            set
            {
                if (_upRight != value)
                {
                    _upRight = value;
                }
                onPropertyChanged("UpRight");
            }
        }
        
        private Visibility _downRight = Visibility.Hidden;

        public Visibility DownRight
        {
            get
            {
                return _downRight;
            }
            set
            {
                if (_downRight != value)
                {
                    _downRight = value;
                }
                onPropertyChanged("DownRight");
            }
        }
        
        
        private Visibility _downLeft = Visibility.Hidden;

        public Visibility DownLeft
        {
            get
            {
                return _downLeft;
            }
            set
            {
                if (_downLeft != value)
                {
                    _downLeft = value;
                }
                onPropertyChanged("DownLeft");
            }
        }

        private Visibility _upLeft = Visibility.Hidden;

        public Visibility UpLeft
        {
            get
            {
                return _upLeft;
            }
            set
            {
                if (_upLeft != value)
                {
                    _upLeft = value;
                }
                onPropertyChanged("UpLeft");
            }
        }
        



        private void ArrowDown_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("!!!!");
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