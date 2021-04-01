using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
            ThrottleSlider.SetValue(Canvas.TopProperty, BoundTop);
            RudderSlider.SetValue(Canvas.LeftProperty, BoundLeft);
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


        private void ArrowDown_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("!!!!");
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}