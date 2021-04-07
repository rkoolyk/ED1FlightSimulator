using System.ComponentModel;

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
       
    }
}