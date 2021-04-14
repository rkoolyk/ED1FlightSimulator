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
        private bool started = false;
        private bool shouldPlay = false;
        private int imgNum = 0; //current place of simulation (number of frames passed)
        private List<KeyValuePair<float,float>> points = 
            new List<KeyValuePair<float, float>>(); //points to form the regression line graph
         private List<KeyValuePair<float,float>> allPoints = 
            new List<KeyValuePair<float, float>>(); //points of category/correlated category in last 30 timesteps 
         private List<KeyValuePair<float,float>> anomalyPoints = 
            new List<KeyValuePair<float, float>>(); 
        private UdpClient client = new UdpClient(5400);
        private int firstTimeFlag = 1; //first time pressing play/running simulation 
        private int maxVal = 1000; //number of lines in csv file (just initialized at arbitrary 1000)
        private float knobX = 50; //joystick--aileron
        private float knobY = 50; //joystick--elevator 
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
        private List<string> dataList = new List<string>(); //list of attributes to choose from (xml)
        private List<KeyValuePair<float, float>> mainGraphValues = new List<KeyValuePair<float, float>>();
        private List<KeyValuePair<float, float>> correlatedGraphValues = new List<KeyValuePair<float, float>>();
        private string category = " ";
        private string correlatedCategory = " ";
        private Dictionary<String, List<float>> dictionary; //keys--from xml, values--corresponding columns from csv
        private Dictionary<int, string> dictFile = new Dictionary<int, string>(); //key--line num, values--line in csv
        public event PropertyChangedEventHandler PropertyChanged;
        String AnomalyAlgorithm = " ";
        private IntPtr TimeSeries;
        private IntPtr AnomalyDetector;
        private DynamicLibraryLoader loader;

        System.Timers.Timer graphTimer; //how often to update graphs 

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

        //update graphs on timer, makes it run faster 
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {   
            UpdateGraphs();
            UpdatePoints();
            
        }
        //main loop to run the entire simulation 
        public void Start()
         {     
            //algorithm not chosen by user 
            if (AnomalyAlgorithm == " ")
            {
                GetPathAlgoDefault();
            } 
            GetPathRegFlight();
            AnomalyDetector = loader.AnomalyDetectionStarter(AnomalyAlgorithm, regFlightPath);
            started = true;
            try
             {  
                 //only connect to fg one time at the beginning 
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
            //range of throttle is between 0 and 1 
            List<float> throttleVals = dictionary["throttle"];
            Throttle = throttleVals[ImgNum] * 140;                                                
        }

        public void MoveRudder()
        {
            //range of rudder is between -1 and 1 
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

        public void UpdatePoints()
        {
            if (category == " " || correlatedCategory == " ")
            {
                return;
            }
            //creating a list of all points in last 30 frames for regression graph 
            List<KeyValuePair<float, float>> tempAllPoints = new List<KeyValuePair<float, float>>();
            int j = 0;
            if (imgNum - 30 >= 0)
            {
                j = imgNum - 30;
            }
            for (; j < imgNum; j++)
            {
                tempAllPoints.Add(new KeyValuePair<float, float>(dictionary[Category].ElementAt(j), dictionary[Correlated_Category].ElementAt(j)));
            }
            AllPoints = tempAllPoints;

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
            //get the csv file path according to what the user uploads, if we've already
            //gotten the xml path, create a timeseries to be used 
            csvPath = path;
            if (xmlPath != null)
            {
                CreateTimeseries();
                
            }
        }

        public void GetPathXML(string path)
        {   
            //get the xml file path according to what the user uploads, if we've already
            //gotten the csv path, create a timeseries to be used 
            xmlPath = path;
            Data_List = Parser(path);
            if (csvPath != null)
            {
                CreateTimeseries();
            }
        }

       public void GetPathAlgoDefault()
        {   
            //use simple anomaly detector algorithm as the default 
            GetPathAlgo("\\Algo1-Dll.dll");
        }

        public void GetPathAlgo(string algoPath)
        {   
            //adding the beginning of the path to create the full path to be sent 
            String path = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            path = Directory.GetParent(path).FullName;
            path = Directory.GetParent(path).FullName;
            path += algoPath;
            AnomalyAlgorithm = path;
            if (started)
            {
                SwitchAlgorithm();
            }
        }

        public void SwitchAlgorithm()
        {
            //switching the algorithm means we need to recreate the anomaly detector and update the anomalies accordingly
            AnomalyDetector = loader.AnomalyDetectionStarter(AnomalyAlgorithm, regFlightPath);
            if (started)
            {
                UpdateAnomalyGraph();
            }
        }

        public void GetFileDictionary()
        {
            //each spot in array has one line of csv file 
            string[] linesArray = System.IO.File.ReadAllLines(csvPath);
            //add each line as value to dictionary where key is the line number 
            for (int i = 0; i < linesArray.Length; i++)
            {
                if (!dictFile.ContainsKey(i)) { 
                    dictFile.Add(i, linesArray[i]);
                 }
            }
        }

        public void Previous()
        {   
            //sets back to beginning of simulation
            ImgNum = 0;
        }
        public void Rewind()
        {
            //rewind 100 frames or back to beginning if we're at less than 100
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
            //reinitialize everything to their starting points 
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
            //skip ahead 100 frames or to the end if we have less than 100 left 
            if (ImgNum < Max_Val + 100)
            {
                ImgNum += 100;
            } else
            {
                ImgNum = Max_Val;
            }

        }

        public void Next()
        {   //bring to end of simulation
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
                //lowest possible speed--0.25
                if (float.Parse(Play_Speed) < 0.25)
                {
                    pSpeed = 0.25F;
                }
            }
            //formula based on: regular speed is 10 images per second 
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

        public List<KeyValuePair<float,float>> AllPoints
        {
            get { return allPoints;}
            set
            {
                allPoints = value;
                onPropertyChanged("AllPoints");
            }
        }

        public List<KeyValuePair<float,float>> AnomalyPoints
        {
            get { return anomalyPoints;}
            set
            {
                anomalyPoints = value;
                onPropertyChanged("AnomalyPoints");
            }
        }

        public void UpdateAnomalyGraph()
        {
            List<float> animationPoints = loader.GetAnimationPoints(category, Correlated_Category);
            //drawing the regression line 
            List<KeyValuePair<float, float>> tempPoints = new List<KeyValuePair<float, float>>();
            for (int f = 0; f < animationPoints.Count(); f += 2)
            {
                float first = animationPoints[f];
                float second = animationPoints[f + 1];
                tempPoints.Add(new KeyValuePair<float, float>(first, second));
            }
            Points = tempPoints;
            List<float> TimeStepList = loader.GetRelevantTimesteps(category, correlatedCategory);
            UpdatePoints(); //draws the points as they update in real 
                            //drawing the anomaly points 
            List<KeyValuePair<float, float>> tempAnomalyPoints = new List<KeyValuePair<float, float>>();
            int size = TimeStepList.Count();
            //Debug.WriteLine("size: "+size+"\n");
            for (int k = 0; k < size; k++)
            {
                int index = (int)TimeStepList[k];
                tempAnomalyPoints.Add(new KeyValuePair<float, float>(dictionary[category].ElementAt(index), dictionary[Correlated_Category].ElementAt(index)));
            }
            AnomalyPoints = tempAnomalyPoints;
        }

        public string Category
        {
            get { return category; }
            set
            {
                //if simulation hasn't started, nothing to show
                if (!started)
                {
                    return;
                }
                category = value;
                List<float> data = dictionary[category];
                int i = 0;
                //temporary list to store points for graph 
                List<KeyValuePair<float, float>> dataPairs = new List<KeyValuePair<float, float>>();
                foreach (float f in data)
                {   
                    //x coordinate increases by 1 each time (time- current line in file),
                    //y value is the category's value in the file 
                    dataPairs.Add(new KeyValuePair<float, float>(i, f));
                    i++;
                }
                Main_Graph_Values = dataPairs;
                Correlated_Category = loader.FindCorrelation(Category);
                UpdateAnomalyGraph();
                onPropertyChanged("Category");
            }
        }

        public string Correlated_Category
        {
            get { return correlatedCategory; }
            set
            {
                correlatedCategory = value;
                //stores points to make graph of correlated feature 
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
