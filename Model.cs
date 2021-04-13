using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;
using System.Xml.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Windows;
using System.Timers;
using System.Text;
using System.Windows.Controls.DataVisualization;
using System.Windows.Controls.DataVisualization.Charting;
using System.Collections.ObjectModel;

namespace ED1FlightSimulator
{
    public class Model : IModel 
    {   
        private bool shouldPlay = false;
        private int imgNum = 0;
        private List<KeyValuePair<float,float>> points = 
            new List<KeyValuePair<float, float>>();
         private List<KeyValuePair<float,float>> points2 = 
            new List<KeyValuePair<float, float>>();
         private List<KeyValuePair<float,float>> points3 = 
            new List<KeyValuePair<float, float>>();
  
        private UdpClient client = new UdpClient(5400);
        private int firstTimeFlag = 1;

       // private Stopwatch stopwatch;
        //private System.Timers.Timer t;
        
        private int maxVal = 1000;
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
        private string xmlPath = null;
        private string csvPath = null;
        private string timeSeriesPath = null;
        private string regFlightPath = null;
        private List<string> dataList = new List<string>();
        private List<KeyValuePair<float, float>> mainGraphValues = new List<KeyValuePair<float, float>>();
        private List<KeyValuePair<float, float>> correlatedGraphValues = new List<KeyValuePair<float, float>>();
        private List<KeyValuePair<float, float>> regressionGraph = null;
        private string category = " ";
        private string correlatedCategory = " ";
        private Dictionary<String, List<float>> dictionary;
        private Dictionary<int, string> dictFile = new Dictionary<int, string>();
        public event PropertyChangedEventHandler PropertyChanged;
        String AnomalyAlgorithm = " ";
        private IntPtr TimeSeries;
        private IntPtr AnomalyDetector;
        private DynamicLibraryLoader loader;

        System.Timers.Timer graphTimer;

        private void onPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Model()
        {
            loader = new DynamicLibraryLoader();
            graphTimer = new System.Timers.Timer(150);
            graphTimer.Elapsed += OnTimedEvent;
            graphTimer.AutoReset = true;
            graphTimer.Enabled = true;
            
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {   
            //Debug.WriteLine("GOT HERE\n");
            UpdateGraphs();
            
        }

        public void Start()
         {      
            if (AnomalyAlgorithm == " ")
            {
                GetPathAlgoDefault();
            }
             
            GetPathRegFlight();
            AnomalyDetector = loader.AnomalyDetectionStarter(AnomalyAlgorithm, regFlightPath);
            try
             {  

                 if (firstTimeFlag == 1)
                 {
                    client.Connect("localhost", 5400);

                 }

                 firstTimeFlag = 0;
                 Thread thread = new Thread(
                 delegate()
                 {
                    
                     
                         while(shouldPlay == true && ImgNum < dictionary["throttle"].Count())
                         {    
                              TimeSpan timeSpan = TimeSpan.FromSeconds(ImgNum / 10);
                              Time = timeSpan.ToString();
                              string newline = dictFile[ImgNum];
                              String eol = "\r\n";
                              Byte[] sendBytes = System.Text.Encoding.ASCII.GetBytes(newline + eol);
                              client.Send(sendBytes, sendBytes.Length);
                                                       
                              MoveThrottle();
                              MoveRudder();
                              MoveAileron();
                              MoveElevator();
                              UpdateHeight();
                              UpdateSpeed();
                              UpdateDirection();
                              UpdateYaw();
                              UpdateRoll();
                              UpdatePitch();
                              //UpdateGraphs();
                              
                              Thread.Sleep(SleepTime());
                              ImgNum++;

                              } 


                  } );
                  thread.Start();
                }
               
             catch (Exception e)
             {
                 Console.WriteLine(e.ToString());
             }
            
        }


        public void StartSim()
        {   
            shouldPlay = true;
            Start();
        }

        

        public void MoveThrottle()
        {
            
            List<float> throttleVals = dictionary["throttle"];
            Throttle = throttleVals[ImgNum] * 140;
                                                              
        }

        public void MoveRudder()
        {
            List<float> rudderVals = dictionary["rudder"];
            Rudder = rudderVals[ImgNum] * 70 + 70;
            
        }

        public void MoveAileron()
        {
            List<float> aileronVals = dictionary["aileron"];
            KNOB_X = aileronVals[ImgNum] * 50 + 50;
            
        }

        public void MoveElevator()
        {
            List<float> elevatorVals = dictionary["elevator"];
            KNOB_Y = elevatorVals[ImgNum] * 50 + 50;
            
        }

        public void UpdateHeight()
        {
            List<float> heightVals = dictionary["altitude-ft"];
            Height_Text = heightVals[ImgNum].ToString();
        }

        public void UpdateSpeed()
        {
            List<float> speedVals = dictionary["airspeed-kt"];
            Speed_Text = speedVals[ImgNum].ToString();
        }

        public void UpdateDirection()
        {
            List<float> dirVals = dictionary["heading-deg"];
            Direction_Text = dirVals[ImgNum].ToString();
           
        }

        public void UpdateYaw()
        {
            List<float> yawVals = dictionary["side-slip-deg"];
            Yaw_Text = yawVals[ImgNum].ToString();

        }

        public void UpdateRoll()
        {
            List<float> rollVals = dictionary["roll-deg"];
            Roll_Text = rollVals[ImgNum].ToString();

        }

        public void UpdatePitch()
        {
            List<float> pitchVals = dictionary["pitch-deg"];
            Pitch_Text = pitchVals[ImgNum].ToString();

        }

        public void UpdateGraphs()
        {
            if (category == " ")
            {
                return;
            }
            List<float> data = dictionary[category];
            int i = 0;
            List<KeyValuePair<float, float>> dataPairs = new List<KeyValuePair<float, float>>();
            foreach (float f in data)
            {
                if (i >= imgNum - 30 && i <= imgNum)
                {
                    dataPairs.Add(new KeyValuePair<float, float>(i, f));
                }
                i++;
            }
            Main_Graph_Values = dataPairs;
            dataPairs = new List<KeyValuePair<float, float>>();
            if (correlatedCategory == " ")
            {
                for (i = 0; i < 30; i++)
                {
                    dataPairs.Add(new KeyValuePair<float, float>(0, 0));
                }
            }
            else
            {
                data = dictionary[correlatedCategory];
                i = 0;
                dataPairs = new List<KeyValuePair<float, float>>();
                foreach (float f in data)
                {
                    if (i >= imgNum - 30 && i <= imgNum)
                    {
                        dataPairs.Add(new KeyValuePair<float, float>(i, f));
                    }
                    i++;
                }
            }
            Correlated_Graph_Values = dataPairs;
        }

        public void CreateTimeseries()
        {
            timeSeriesPath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            timeSeriesPath = Directory.GetParent(timeSeriesPath).FullName;
            timeSeriesPath = Directory.GetParent(timeSeriesPath).FullName;
            timeSeriesPath += "\\Dll-fg.dll";    
            TimeSeries = loader.CreateTS(timeSeriesPath,  csvPath, dataList);
            dictionary = loader.GetDictionary();
            GetFileDictionary();
            Max_Val = dictionary["throttle"].Count();

        }

        public void GetPathRegFlight()
        {
            regFlightPath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            regFlightPath = Directory.GetParent(regFlightPath).FullName;
            regFlightPath = Directory.GetParent(regFlightPath).FullName;
            regFlightPath += "\\reg_flight.csv";
        }

        public void GetPathCSV(string path)
        {
            csvPath = path;
            if (xmlPath != null)
            {
                CreateTimeseries();
                
            }
        }

        public void GetPathXML(string path)
        {
            xmlPath = path;
            Data_List = Parser(path);
            if (csvPath != null)
            {
                CreateTimeseries();
            }
        }

       public void GetPathAlgoDefault()
        {
            /*String path = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            path = Directory.GetParent(path).FullName;
            path = Directory.GetParent(path).FullName;
            path += "\\Algo1-Dll.dll";*/
            GetPathAlgo("\\Algo1-Dll.dll");
        }

        public void GetPathAlgo(string algoPath)
        {
            String path = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            path = Directory.GetParent(path).FullName;
            path = Directory.GetParent(path).FullName;
            path += algoPath;

            //alg = new StringAlgo(path);
            AnomalyAlgorithm = path;
            if (AnomalyDetector == null)
            {
                AnomalyDetector = loader.AnomalyDetectionStarter(AnomalyAlgorithm, regFlightPath);
            }
        }

        public void GetFileDictionary()
        {
         
            string[] linesArray = System.IO.File.ReadAllLines(csvPath);
            for (int i = 0; i < linesArray.Length; i++)
            {
                dictFile.Add(i, linesArray[i]);
            }

        }

        public void Previous()
        {
            ImgNum = 0;
        }
        public void Rewind()
        {
            if (ImgNum > 100)
            {
                ImgNum -= 100;
            } else
            {
                ImgNum = 0;
            }
        }
        public void Play()
        {
            shouldPlay = true;
            Start();
        }

        public void Pause()
        {
            shouldPlay = false;
           

        }

        public void Stop()
        {   
            firstTimeFlag = 1;
            shouldPlay = false;
            Height_Text = "0";
            Speed_Text = "0";
            Direction_Text = "0";
            Yaw_Text = "0";
            Roll_Text = "0";
            Pitch_Text = "0";
            KNOB_X = 50;
            KNOB_Y = 50;
            Throttle = 0;
            Rudder = 0;
            Time = "00:00:00";
            ImgNum = 0;

        }

        public void FastForward()
        {
            
            if (ImgNum < Max_Val + 100)
            {
                ImgNum += 100;
            } else
            {
                ImgNum = Max_Val;
            }

        }

        public void Next()
        {
            ImgNum = Max_Val;
        }

        public int Max_Val
        {
            get { return maxVal; }
            set
            {
                maxVal = value;
                onPropertyChanged("Max_Val");
            }
        }

        public int ImgNum
        {
            get { return imgNum; }
            set
            {
                imgNum = value;
                onPropertyChanged("ImgNum");
            }
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
        public List<string> Data_List
        {
            get { return dataList; }
            set
            {
                dataList = value;
                onPropertyChanged("Data_List");
            }
        }

        public int SleepTime()
        {   
            float pSpeed;
            if (Play_Speed == "")
            {
                pSpeed = 1;
            }
            else{
                pSpeed = float.Parse(Play_Speed);
                
                if (float.Parse(Play_Speed) < 0.25)
                {
                    pSpeed = 0.25F;
                }
            }
            return (int) (1000 / (10 * pSpeed));
        }

        public List<KeyValuePair<float,float>> Points
        {
            get { return points;}
            set
            {
                points = value;
                onPropertyChanged("Points");
            }
        }

        public List<KeyValuePair<float,float>> Points2
        {
            get { return points2;}
            set
            {
                points2 = value;
                onPropertyChanged("Points2");
            }
        }

        public List<KeyValuePair<float,float>> Points3
        {
            get { return points3;}
            set
            {
                points3 = value;
                onPropertyChanged("Points3");
            }
        }

        public string Category
        {
            get { return category; }
            set
            {
                category = value;
                List<float> data = dictionary[category];
                int i = 0;
                if (category == "engine-pump")
                {
                    Console.WriteLine("Found the problem!");
                }

                List<KeyValuePair<float, float>> dataPairs = new List<KeyValuePair<float, float>>();
                foreach (float f in data)
                {
                    dataPairs.Add(new KeyValuePair<float, float>(i, f));
                    i++;
                }
                /*IntPtr pDll = NativeMethods.LoadLibrary(@AnomalyAlgorithm);

                IntPtr pAddressOfFunctionToCall2 = NativeMethods.GetProcAddress(pDll, "MostCorrelatedFeature");
 
                MostCorrelatedFeature MostCorrelatedFeature =(MostCorrelatedFeature)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall2, typeof(MostCorrelatedFeature));*/

                Main_Graph_Values = dataPairs;
                //StringBuilder s = new StringBuilder();
                //MostCorrelatedFeature(AnomalyDetector, regFlightPath, dataList.ToArray(), dataList.Count, category, s);
                //Correlated_Category = s.ToString();
                Correlated_Category = loader.FindCorrelation(Category);

                //IntPtr pDll = NativeMethods.LoadLibrary(@AnomalyAlgorithm);
                /*IntPtr pAddressOfFunctionToCall1 = NativeMethods.GetProcAddress(pDll, "findLinReg");
                findLinReg findLinReg =(findLinReg)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall1, typeof( findLinReg));
                findLinReg(TimeSeries, ref a, ref b, category, Correlated_Category);*/
                //loader.LineReg(ref a, ref b, Category, Correlated_Category);

                List<float> animationPoints = loader.GetAnimationPoints(category, Correlated_Category);

                List<KeyValuePair<float, float>> tempPoints = new List<KeyValuePair<float, float>>();
                for(int f = 0; f < animationPoints.Count(); f += 2)
                {
                    float first = animationPoints[f];
                    float second = animationPoints[f+1];
                    tempPoints.Add(new KeyValuePair<float, float>(first, second));
                }
                Points = tempPoints;
                /*tempPoints.Add(new KeyValuePair<float, float>(0, b));
                if (a != 0)
                {
                    tempPoints.Add(new KeyValuePair<float, float>((-b) / a, 0));
                }
                else
                {
                    tempPoints.Add(new KeyValuePair<float, float>(1, a + b));
                }*/

                //Points = tempPoints;
                /**List<float> TimeStepList = loader.GetRelevantTimesteps(category, correlatedCategory);
                List<KeyValuePair<float, float>> tempPoints2 = new List<KeyValuePair<float, float>>();
                List<KeyValuePair<float, float>> tempPoints3 = new List<KeyValuePair<float, float>>();
                int size = dictionary[category].Count();
                for (int j = 0; i < size; i++)
                {
                    tempPoints2.Add(new KeyValuePair<float, float>(dictionary[category].ElementAt(i), dictionary[Correlated_Category].ElementAt(i)));
                }
                int size2 = TimeStepList.Count();
                for (int j = 0; i < size2; i++)
                {
                    int index = (int)TimeStepList[i];
                    tempPoints3.Add(new KeyValuePair<float, float>(dictionary[category].ElementAt(index), dictionary[Correlated_Category].ElementAt(index)));
                }*/

                onPropertyChanged("Category");
            }
        }

        public string Correlated_Category
        {
            get { return correlatedCategory; }
            set
            {
                correlatedCategory = value;
                

                
                List<KeyValuePair<float, float>> dataPairs = new List<KeyValuePair<float, float>>();
                if (correlatedCategory != " ")
                {
                    List<float> data = dictionary[correlatedCategory];
                    int i = 0;
                    foreach (float f in data)
                    {
                        dataPairs.Add(new KeyValuePair<float, float>(i, f));
                        i++;
                    }
                }
                correlatedGraphValues = dataPairs;
                onPropertyChanged("Correlated_Category");
            }
        }

        public List<KeyValuePair<float, float>> Main_Graph_Values
        {
            get
            {
                return mainGraphValues;
            }
            set
            {
                mainGraphValues = value;
                onPropertyChanged("Main_Graph_Values");
            }
        }

        public List<KeyValuePair<float, float>> Correlated_Graph_Values
        {
            get
            {
                return correlatedGraphValues;
            }
            set
            {
                correlatedGraphValues = value;
                onPropertyChanged("Correlated_Graph_Values");
            }
        }

        public class Attribute
        {
            public String name { get; set; }
        }

        public static XDocument XDoc;

        //Method for parsing the xml file and finding the attributes
        public static List<String> Parser(String path)
        {

            List<Attribute> AttsList = new List<Attribute>();
            try
            {
                //Console.WriteLine("\nNow Loading: {0}\n", UserPath);
                XDoc = XDocument.Load(@path);
            }
            catch (Exception err)
            {
                Console.WriteLine("An Exception has been caught:");
                Console.WriteLine(err);
                Environment.Exit(1);
            }
            // Build a LINQ query, and run through the XML building
            // the PersonObjects
            var query = from xml in XDoc.Descendants("chunk")
                        select new Attribute
                        {
                            name = (string)xml.Element("name"),

                        };
            AttsList = query.ToList();
            List<String> SAttsList = new List<String>();
            int i = 0;
            for (i = 0; i < AttsList.Count(); i++)
            {
                if (SAttsList.Contains(AttsList[i].name))
                {
                    SAttsList.Add(AttsList[i].name);
                    SAttsList[i] = SAttsList[i] + 2;
                }
                else
                {
                    SAttsList.Add(AttsList[i].name);
                }

            }
            List<String> SAttsList2 = new List<String>();
            int size1 = SAttsList.Count();

            //removing second half bc we want only first half of list
            for (int j = 0; j < size1 / 2; j++)
            {
                SAttsList2.Add(SAttsList[j]);
            }
            return SAttsList2;
        }
    }
}
