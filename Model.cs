using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ED1FlightSimulator
{
    public class Model : INotifyPropertyChanged
    {
        private float knobX = 50;
        private float knobY = 50;
        private string heightText = "0";
        private string speedText = "0";
        private string directionText = "0";
        private string yawText = "0";
        private string rollText = "0";
        private string pitchText = "0";
        private string playSpeed = "1.0";
        private string time = "00:00:00";
        private float throttle = 0;
        private float rudder = 0;
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
                onPropertyChanged("KNOB_Y");
            }
        }
        public string Height_Text
        {
            get { return heightText; }
            set
            {
                heightText = value;
                onPropertyChanged("Height_Text");
            }
        }
        public string Speed_Text
        {
            get { return speedText; }
            set
            {
                speedText = value;
                onPropertyChanged("Speed_Text");
            }
        }
        public string Direction_Text
        {
            get { return directionText; }
            set
            {
                directionText = value;
                onPropertyChanged("Direction_Text");
            }
        }

        public string Yaw_Text
        {
            get { return yawText; }
            set
            {
                yawText = value;
                onPropertyChanged("Yaw_Text");
            }
        }
        public string Roll_Text
        {
            get { return rollText; }
            set
            {
                rollText = value;
                onPropertyChanged("Roll_Text");
            }
        }
        public string Pitch_Text
        {
            get { return pitchText; }
            set
            {
                pitchText = value;
                onPropertyChanged("Pitch_Text");
            }
        }
        public string Play_Speed
        {
            get { return playSpeed; }
            set
            {
                playSpeed = value;
                onPropertyChanged("Play_Speed");
            }
        }
        public string Time
        {
            get { return time; }
            set
            {
                time = value;
                onPropertyChanged("Time");
            }
        }
        public float Throttle
        {
            get { return throttle; }
            set
            {
                throttle = value;
                onPropertyChanged("Throttle");
            }
        }
         public float Rudder
        {
            get { return rudder; }
            set
            {
                rudder = value;
                onPropertyChanged("Rudder");
            }
        }
       
    }
}