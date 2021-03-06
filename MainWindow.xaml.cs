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
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

        //moving slider to skip ahead in simulation
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            vm.VM_ImgNum = (int)((Slider)sender).Value;
        }

        //user can change the speed of simulation
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            vm.VM_Play_Speed = ((TextBox)sender).Text;
            
        }

        //choosing attribute (for showing graphs)--model needs to know about the selection
        private void FeatureChoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vm.VM_Category = (string)((ListBox)sender).SelectedItem;
        }

        //uploading a csv file upon click of button
	    private void LoadCsv_OnClick(object sender, RoutedEventArgs e)
        {
            string path = "";
		    OpenFileDialog openFileDialog = new OpenFileDialog();
		    if(openFileDialog.ShowDialog() == true) {
                 string sFilenames = "";
                 foreach (string sFilename in openFileDialog.FileNames) {
                 path += sFilename; 
                 }
                 vm.GetPathCSV(path);
            }
          
        }
        //uploading an xml file upon click of button
        private void LoadXml_OnClick(object sender, RoutedEventArgs e)
        {
            string path = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string sFilenames = "";
                foreach (string sFilename in openFileDialog.FileNames)
                {
                    path += sFilename;
                }
                vm.GetPathXML(path);
            }
        }

        //algorithm 1 dll according to users choice 
        private void Simple_Checked(object sender, EventArgs e)
        {
            string path = "\\Algo1-Dll.dll";
            vm.GetPathAlgo(path);
        }
        //algorithm 2 dll according to users choice 
        private void Circle_Checked(object sender, EventArgs e)
        {
            string path = "\\Algo2-Dll.dll";
            vm.GetPathAlgo(path);
        }

        //button clicked to start simulation
        private void StartSim_OnClick(object sender, RoutedEventArgs e)
        {
            vm.StartSim();
        }
        /*
         * Speed Buttons (play, pause, fast forward etc.)
         */
        private void Previous_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            vm.Previous();
        }

        private void Rewind_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            vm.Rewind();
        }

        private void Play_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            vm.Play();
        }

        private void Pause_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            vm.Pause();
        }

        private void Stop_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            vm.Stop();
        }

        private void FastForward_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            vm.FastForward();
        }

        private void Next_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            vm.Next();
        }
    }
}