﻿using System;
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

        private void LoadXml_OnClick(object sender, RoutedEventArgs e)
        {
            string path = "";
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if(openFileDialog.ShowDialog() == true) {
                string sFilenames = "";
                foreach (string sFilename in openFileDialog.FileNames) {
                    path += sFilename; 
                }
                vm.GetPathXML(path);
            }
        }

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