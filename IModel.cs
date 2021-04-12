using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ED1FlightSimulator
{
    interface IModel : INotifyPropertyChanged
    {
        float KNOB_X { get; set; }
        float KNOB_Y { get; set; }
        string Height_Text { get; set; }
        string Speed_Text { get; set; }
        string Direction_Text { get; set; }
        string Yaw_Text { get; set; }
        string Roll_Text { get; set; }
        string Pitch_Text { get; set; }
        string Play_Speed { get; set; }
        string Time { get; set; }
        float Throttle { get; set; }
        float Rudder { get; set; }
        List<string> Data_List { get; set; }
        string Category { get; set; }
        List<KeyValuePair<float, float>> Main_Graph_Values { get; set; }
        string Correlated_Category { get; set; }
        List<KeyValuePair<float, float>> Correlated_Graph_Values { get; set; }
        void StartSim();
    }
}
