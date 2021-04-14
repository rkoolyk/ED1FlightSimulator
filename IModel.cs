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
        List<KeyValuePair<float,float>> Points { get; set;}
        List<KeyValuePair<float,float>> AllPoints { get; set;}
        List<KeyValuePair<float,float>> AnomalyPoints { get; set;}
        int Max_Val { get; set;}
        int ImgNum { get; set;}

        void StartSim();
        void MoveThrottle();
        void MoveRudder();
        void MoveAileron();
        void MoveElevator();
        void UpdateHeight();
        void UpdateSpeed();
        void UpdateDirection();
        void UpdateYaw();
        void UpdatePitch();
        void UpdateRoll();
        void UpdatePoints();
        void UpdateGraphs();
        void CreateTimeseries();
        void GetPathRegFlight();
        void GetPathCSV(string path);
        void GetPathXML(string path);
        void GetPathAlgoDefault();
        void GetPathAlgo(string path);
        void GetFileDictionary();
        void Previous();
        void Rewind();
        void Play();
        void Pause();
        void Stop();
        void FastForward();
        void Next();
        int SleepTime();
    }
}
