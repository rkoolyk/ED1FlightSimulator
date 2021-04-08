using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Axes;

namespace ED1FlightSimulator
{
    public class ViewModel : IViewModel
    {
        private Model model;

        public ViewModel()
        {
            this.model = new Model();
            this.model.PropertyChanged +=
                delegate(object o, PropertyChangedEventArgs e)
                {
                    onPropertyChanged("VM_" + e.PropertyName);
                };
            PlotModel = new PlotModel();
        }
        public PlotModel PlotModel
        {
            get { return plotModel; }
            set { plotModel = value; onPropertyChanged("PlotModel"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private PlotModel plotModel;
        private void onPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Previous()
        {
            model.Previous();
        }

        public void Rewind()
        {
            model.Rewind();
        }

        public void Play()
        {
            model.Play();
        }

        public void Pause()
        {
            model.Pause();
        }

        public void Stop()
        {
            model.Stop();
        }

        public void FastForward()
        {
            model.FastForward();
        }

        public void Next()
        {
            model.Next();
        }


        public float VM_KNOB_X
        {
            get { return model.KNOB_X; }
        }

        public float VM_KNOB_Y
        {
            get
            {
                return model.KNOB_Y;
            }
        }
        public string VM_Height_Text
        {
            get
            {
                return model.Height_Text;
            }
        }
        public string VM_Speed_Text
        {
            get
            {
                return model.Speed_Text;
            }
        }
        public string VM_Direction_Text
        {
            get
            {
                return model.Direction_Text;
            }
        }
        public string VM_Yaw_Text
        {
            get
            {
                return model.Yaw_Text;
            }
        }
        public string VM_Roll_Text
        {
            get
            {
                return model.Roll_Text;
            }
        }
        public string VM_Pitch_Text
        {
            get
            {
                return model.Pitch_Text;
            }
        }
        public string VM_Play_Speed
        {
            get
            {
                return model.Play_Speed;
            }
            set
            {
                model.Play_Speed = value;
            }
        }
        public string VM_Time
        {
            get
            {
                return model.Time;
            }
        }
        public float VM_Throttle
        {
            get
            {
                return model.Throttle;
            }
        }
        public float VM_Rudder
        {
            get
            {
                return model.Rudder;
            }
        }
         public List<string> VM_Data_List
        {
            get
            {
                return model.Data_List;
            }
        }

        public void GetPathCSV(string path)
        {
            model.GetPathCSV(path);
        }
        public void GetPathXML(string path)
        {
            model.GetPathXML(path);
        }

        /*private void SetUpModel()
        {
            PlotModel.LegendTitle = "Legend";
            PlotModel.LegendOrientation = LegendOrientation.Horizontal;
            PlotModel.LegendPlacement = LegendPlacement.Outside;
            PlotModel.LegendPosition = LegendPosition.TopRight;
            PlotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
            PlotModel.LegendBorder = OxyColors.Black;

            var dateAxis = new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "dd/MM/yy HH:mm", MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, IntervalLength = 80 };
            PlotModel.Axes.Add(dateAxis);
            var valueAxis = new LinearAxis { Position = AxisPosition.Left, Minimum = 0, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Value" };
            PlotModel.Axes.Add(valueAxis);
        }

        private void LoadData()
        {
            List<float> dataRecords = Data.GetData();

            var dataPerDetector = measurements.GroupBy(m => m.DetectorId).ToList();

            foreach (var data in dataPerDetector)
            {
                var lineSerie = new LineSeries
                {
                    StrokeThickness = 2,
                    MarkerSize = 3,
                    MarkerStroke = colors[data.Key],
                    MarkerType = markerTypes[data.Key],
                    CanTrackerInterpolatePoints = false,
                    Title = string.Format("Detector {0}", data.Key),
                    Smooth = false,
                };

                data.ToList().ForEach(d => lineSerie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(d.DateTime), d.Value)));
                PlotModel.Series.Add(lineSerie);
            }
        }*/

       
    }
    
}