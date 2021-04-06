using System.ComponentModel;

namespace ED1FlightSimulator
{
    interface IViewModel : INotifyPropertyChanged
    {
        float VM_KNOB_X { get; }
        float VM_KNOB_Y { get; }
    }
}