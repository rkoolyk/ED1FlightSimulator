using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;
using System.Xml.Linq;
using System.Runtime.InteropServices;
using OxyPlot;
using OxyPlot.Series;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ED1FlightSimulator
{
    public class Model : IModel //INotifyPropertyChanged
    {   
        private bool shouldPlay = false;
        private int imgNum = 0;

        private UdpClient client = new UdpClient(5400);
        private StreamReader s;
        private int firstTimeFlag = 1;
       
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
        private List<string> dataList = new List<string>();
        private Dictionary<String, List<float>> dictionary;
        public event PropertyChangedEventHandler PropertyChanged;
        String AnomalyAlgorithm;
        private IntPtr TimeSeries;
        private PlotModel mainGraph = null;
        private PlotModel correlatedGraph = null;
        private PlotModel regressionGraph = null;
        private string category = "slats";//starting off with slats to check

        private void onPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Start()
         {

             try
             {  

                 //UdpClient client = new UdpClient(5400);
                 if (firstTimeFlag == 1)
                {
                    client.Connect("localhost", 5400);
                    s = new StreamReader(File.OpenRead(csvPath));
                }
                 firstTimeFlag = 0;
                 //StreamReader s = new StreamReader(File.OpenRead(csvPath));
                 Thread thread = new Thread(
                 delegate()
                 {
                     //while(true)
                     //{
                         while(shouldPlay == true && imgNum < dictionary["throttle"].Count()
                            && !s.EndOfStream)
                         {    
                              var newline = s.ReadLine();
                              String eol = "\n\r";
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
                              Thread.Sleep(SleepTime());
                              imgNum++;
                         } 
                     //}

                  } );
                  thread.Start();
                }
               
             catch (Exception e)
             {
                 Console.WriteLine(e.ToString());
             }
            /*Thread thread = new Thread(
                delegate()
                {
                    while (shouldPlay == true && imgNum < dictionary["throttle"].Count())
                    {
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
                        Thread.Sleep(SleepTime());
                        imgNum++;
                    }
                }

                );
            thread.Start();*/
        }

        public void StartSim()
        {   
            shouldPlay = true;
            Start();
        }

        public void MoveThrottle()
        {
            
            List<float> throttleVals = dictionary["throttle"];
            Throttle = throttleVals[imgNum] * 140;
                                                              
        }

        public void MoveRudder()
        {
            List<float> rudderVals = dictionary["rudder"];
            Rudder = rudderVals[imgNum] * 70 + 70;
            
        }

        public void MoveAileron()
        {
            List<float> aileronVals = dictionary["aileron"];
            if (aileronVals[imgNum] * 100 + 50 < 100 && aileronVals[imgNum] * 100 + 50 > 0)
            {
                 KNOB_X = aileronVals[imgNum] * 100 + 50;
            }
            else if (aileronVals[imgNum] * 100 + 50 > 100)
            {
                KNOB_X = 100;
            }
            else if (aileronVals[imgNum] * 100 + 50 < 0)
            {
                KNOB_X = 0;
            }
           
        }

        public void MoveElevator()
        {
            List<float> elevatorVals = dictionary["elevator"];
            if (elevatorVals[imgNum] * 100 + 50 > 0 && elevatorVals[imgNum] * 100 + 50 < 100)
            {
                 KNOB_Y = elevatorVals[imgNum] * 100 + 50;
            }
            else if (elevatorVals[imgNum] * 100 + 50 > 100)
            {
                KNOB_Y = 100;
            }
            else if (elevatorVals[imgNum] * 100 + 50 < 0)
            {
                KNOB_Y = 0;
            }
           
        }

        public void UpdateHeight()
        {
            List<float> heightVals = dictionary["altitude-ft"];
            Height_Text = heightVals[imgNum].ToString();
        }

        public void UpdateSpeed()
        {
            List<float> speedVals = dictionary["airspeed-kt"];
            Speed_Text = speedVals[imgNum].ToString();
        }

        public void UpdateDirection()
        {
            List<float> dirVals = dictionary["heading-deg"];
            Direction_Text = dirVals[imgNum].ToString();
           
        }

        public void UpdateYaw()
        {
            List<float> yawVals = dictionary["side-slip-deg"];
            Yaw_Text = yawVals[imgNum].ToString();

        }

        public void UpdateRoll()
        {
            List<float> rollVals = dictionary["roll-deg"];
            Roll_Text = rollVals[imgNum].ToString();

        }

        public void UpdatePitch()
        {
            List<float> pitchVals = dictionary["pitch-deg"];
            Pitch_Text = pitchVals[imgNum].ToString();

        }


        public void GetPathCSV(string path)
        {
            csvPath = path;
            if (xmlPath != null)
            {
                TimeSeries = Create(csvPath, dataList.ToArray(), Data_List.Count());
                dictionary = getDictionary(dataList, TimeSeries);
                //MoveAileron();
                //Start();
                
            }
        }

        public void GetPathXML(string path)
        {
            xmlPath = path;
            Data_List = Parser(path);
            if (csvPath != null)
            {
                TimeSeries = Create(csvPath, dataList.ToArray(), Data_List.Count());
                dictionary = getDictionary(dataList, TimeSeries);
            }
        }

        public void Previous()
        {

        }
        public void Rewind()
        {
            if (imgNum > 100)
            {
                imgNum -= 100;
            } else
            {
                imgNum = 0;
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
            imgNum = 0;

        }

        public void FastForward()
        {
            int numOfLines = 100;
            if (imgNum < numOfLines + 10)
            {
                imgNum += 10;
            } else
            {
                imgNum = numOfLines;
            }

        }

        public void Next()
        {

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
            float pSpeed = float.Parse(Play_Speed);
            return (int) (1000 / (10 * pSpeed));
        }

        public string Category
        {
            get { return category; }
            set
            {
                category = value;
                onPropertyChanged("Category");
                PlotModel tmp = new PlotModel { };
                LineSeries line = new LineSeries { Title = category, MarkerType = MarkerType.Circle };
                List<float> data = dictionary[category];
                int i = 0;
                foreach (float f in data)
                {
                    line.Points.Add(new DataPoint(i, f));
                    i++;
                }
                tmp.Series.Add(line);
                Main_Graph = tmp;
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

        [DllImport("C:\\Users\\rayra\\Source\\Repos\\rkoolyk\\ED1FlightSimulator\\Dll-fg.dll")]

        public static extern IntPtr Create(String CSVfileName, String[] l, int size);

        [DllImport("C:\\Users\\rayra\\Source\\Repos\\rkoolyk\\ED1FlightSimulator\\Dll-fg.dll")]
        public static extern float givesFloatTs(IntPtr obj, int line, String att);

        [DllImport("C:\\Users\\rayra\\Source\\Repos\\rkoolyk\\ED1FlightSimulator\\Dll-fg.dll")]
        public static extern int getRowSize(IntPtr ts);

        [DllImport("C:\\Users\\rayra\\Source\\Repos\\rkoolyk\\ED1FlightSimulator\\Dll-fg.dll")]
        public static extern void findLinReg(IntPtr ts, ref float a, ref float b, String attA, String attB);

        Dictionary<String, List<float>> getDictionary(List<String> SAttsList, IntPtr ts)
        {
            Dictionary<String, List<float>> tsDic = new Dictionary<String, List<float>>();

            int size = getRowSize(ts);
            for (int i = 0; i < SAttsList.Count(); i++)
            {
                List<float> f = new List<float>();
                tsDic.Add(SAttsList[i], f);
                for (int j = 0; j < size; j++)
                {
                    tsDic[SAttsList[i]].Add(givesFloatTs(ts, j, SAttsList[i]));
                }

            }
            return tsDic;
        }

        public PlotModel Main_Graph
        {
            get
            {
                return mainGraph;
            }
            set
            {
                mainGraph = value;
                mainGraph.InvalidatePlot(true);
                onPropertyChanged("Main_Graph");
            }
        }

    }
}