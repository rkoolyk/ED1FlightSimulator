// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents the view-model for the main window.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ED1FlightSimulator
{
    using OxyPlot;
    using OxyPlot.Series;

    /// <summary>
    /// Represents the view-model for the main window.
    /// </summary>
    public class MainViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel" /> class.
        /// </summary>
        /// 
        
            using System.Collections.Generic;

            using OxyPlot;

        public MainViewModel()
        {

                this.Title = "Example 2";
                this.Points = new List<DataPoint>
                              {
                                  new DataPoint(0, 4),
                                  new DataPoint(10, 13),
                                  new DataPoint(20, 15),
                                  new DataPoint(30, 16),
                                  new DataPoint(40, 12),
                                  new DataPoint(50, 12)
                              };
            }

            public string Title { get; private set; }

            public IList<DataPoint> Points { get; private set; }
        }

        /*
        // Create the plot model
        var tmp = new PlotModel { Title = "Simple example", Subtitle = "using OxyPlot" };

        // Create two line series (markers are hidden by default)
        var series1 = new LineSeries { Title = "Series 1", MarkerType = MarkerType.Circle };
        series1.Points.Add(new DataPoint(0, 0));
        series1.Points.Add(new DataPoint(10, 18));
        series1.Points.Add(new DataPoint(20, 12));
        series1.Points.Add(new DataPoint(30, 8));
        series1.Points.Add(new DataPoint(40, 15));

        var series2 = new LineSeries { Title = "Series 2", MarkerType = MarkerType.Square };
        series2.Points.Add(new DataPoint(0, 4));
        series2.Points.Add(new DataPoint(10, 12));
        series2.Points.Add(new DataPoint(20, 16));
        series2.Points.Add(new DataPoint(30, 25));
        series2.Points.Add(new DataPoint(40, 5));


        // Add the series to the plot model
        tmp.Series.Add(series1);
        tmp.Series.Add(series2);

        // Axes are created automatically if they are not defined

        // Set the Model property, the INotifyPropertyChanged event will make the WPF Plot control update its content
        this.VM_Model = tmp;
    }

    /// <summary>
    /// Gets the plot model.
    /// </summary>
    public PlotModel VM_Model { get; private set; }
        */
    }
}





