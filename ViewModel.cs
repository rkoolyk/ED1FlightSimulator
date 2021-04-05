using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ED1FlightSimulator
{
    public class ViewModel : INotifyPropertyChanged
    {
        Model model;

        ViewModel(Model m)
        {
            this.model = m;
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
    }
    
}