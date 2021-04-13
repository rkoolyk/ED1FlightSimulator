using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;

namespace ED1FlightSimulator
{
    interface IViewModel : INotifyPropertyChanged
    {
        
        float VM_KNOB_X { get; }
        float VM_KNOB_Y { get; }
        string VM_Height_Text { get;}
        string VM_Speed_Text { get;}
        string VM_Direction_Text { get;}
        string VM_Yaw_Text { get;}
        string VM_Roll_Text { get;}
        string VM_Pitch_Text { get;}
        string VM_Play_Speed { get; set;}
        string VM_Time { get;}
        float VM_Throttle { get;}
        float VM_Rudder { get;}
        List<string> VM_Data_List { get;}

        List<KeyValuePair<float,float>> VM_Points { get;}
        List<KeyValuePair<float,float>> VM_Points2 { get;}
        List<KeyValuePair<float,float>> VM_Points3 { get;}

        int VM_ImgNum { get; set; }
        int VM_Max_Val { get; }

        List<KeyValuePair<float, float>> VM_Main_Graph_Values { get; }
        string VM_Category { get; set; }
        List<KeyValuePair<float, float>> VM_Correlated_Graph_Values { get; }
        string VM_Correlated_Category { get; }

        void Previous();
        void Rewind();
        void Play();
        void Pause();
        void Stop();
        void FastForward();
        void Next();

        void GetPathCSV(string path);
        void GetPathXML(string path);
        void GetPathAlgo(string path);
        void StartSim();
       
    }
}