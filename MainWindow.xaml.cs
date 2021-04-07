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
    public partial class MainWindow
    {
        private IViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = new ViewModel();
            DataContext = vm;
            this.Elevator.Text = "<- Elevator ->";
            this.Aileron.Text = "<- Aileron ->";
            this.Throttle.Text = "<- Throttle ->";
            this.Rudder.Text = "<- Rudder ->";
           
            
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