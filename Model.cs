using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ED1FlightSimulator
{
    public class Model : INotifyPropertyChanged
    {
        private float knobX = 10;
        private float knobY = 10;
        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public float KNOB_X
        {
            get { return knobX; }
            set
            {
                knobX = value;
                onPropertyChanged("KNOB_X");
            }
        }

        public float KNOB_Y
        {
            get { return knobY; }
            set
            {
                knobY = value;
                onPropertyChanged("KNOB_X");
            }
        }

        public double GetTop()
        {
            return 1;
        }

        public void SetTop()
        {
            
        }
    }
}