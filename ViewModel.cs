using System.ComponentModel;
using System.Runtime.CompilerServices;

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
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        private double _boundTop;
        public double BoundTop
        {
            get
            {
                return model.GetTop();
            }

            set
            {
                model.SetTop();
                // if (_boundTop != value)
                // {
                //     _boundTop = value;
                // }
                //
                // onPropertyChanged("BoundTop");
            }
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
    }
    
}