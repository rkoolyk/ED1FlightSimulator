using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace ED1FlightSimulator
{
    //implements the ViewModel interface (who implements INotifyPropertyChanged)
    public class ViewModel : IViewModel
    {
        public Model model;

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
        //So we can alert the view when there is a change 
        private void onPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /*
         * The ViewModel is used as a pipe from View to Model so calls functions in Model 
         */
        public void StartSim()
        {
            model.StartSim();
        }

        public void Previous()
        {
            model.Previous();
        }

        public void Rewind()
        {
            model.Rewind();
        }

        public void Play()
        {
            model.Play();
        }

        public void Pause()
        {
            model.Pause();
        }

        public void Stop()
        {
            model.Stop();
        }

        public void FastForward()
        {
            model.FastForward();
        }

        public void Next()
        {
            model.Next();
        }

        public void GetPathCSV(string path)
        {
            model.GetPathCSV(path);
        }
        public void GetPathXML(string path)
        {
            model.GetPathXML(path);
        } 
        
        public void GetPathAlgo(string path)
        {
            model.GetPathAlgo(path);
        }


        /*
         * Characteristics are bound to the Model ones, return the ones from Model 
         */
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
        public string VM_Height_Text
        {
            get
            {
                return model.Height_Text;
            }
        }
        public string VM_Speed_Text
        {
            get
            {
                return model.Speed_Text;
            }
        }
        public string VM_Direction_Text
        {
            get
            {
                return model.Direction_Text;
            }
        }
        public string VM_Yaw_Text
        {
            get
            {
                return model.Yaw_Text;
            }
        }
        public string VM_Roll_Text
        {
            get
            {
                return model.Roll_Text;
            }
        }
        public string VM_Pitch_Text
        {
            get
            {
                return model.Pitch_Text;
            }
        }
        public string VM_Play_Speed
        {
            get
            {
                return model.Play_Speed;
            }
            set
            {
                model.Play_Speed = value;
            }
        }
        public string VM_Time
        {
            get
            {
                return model.Time;
            }
        }
        public float VM_Throttle
        {
            get
            {
                return model.Throttle;
            }
        }
        public float VM_Rudder
        {
            get
            {
                return model.Rudder;
            }
        }
         public List<string> VM_Data_List
         {
            get
            {
                return model.Data_List;
            }
        }
        public List<KeyValuePair<float, float>> VM_Main_Graph_Values
        {
            get
            { return model.Main_Graph_Values; }
        }

        public List<KeyValuePair<float,float>> VM_Points
        {
            get
            {
                return model.Points;
            }
        }

        public List<KeyValuePair<float,float>> VM_AllPoints
        {
            get
            {
                return model.AllPoints;
            }
        }

        public List<KeyValuePair<float,float>> VM_AnomalyPoints
        {
            get
            {
                return model.AnomalyPoints;
            }
        }

        public string VM_Category
        {
            get
            {
                return model.Category;
            }
            set
            {
                model.Category = value;
            }
        }
        public List<KeyValuePair<float, float>> VM_Correlated_Graph_Values
        {
            get { return model.Correlated_Graph_Values; }
        }

        public string VM_Correlated_Category
        {
            get
            {
                return model.Correlated_Category;
            }
        }

        
        public int VM_ImgNum
        {
            get
            {
                return model.ImgNum;
            }
            set
            {
                model.ImgNum = value;
            }
        }

        public int VM_Max_Val
        {
            get
            {
                return model.Max_Val;
            }
        }
    }
    
}