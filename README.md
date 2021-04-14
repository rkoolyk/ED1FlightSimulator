# ED1FlightSimulator
1st milestone, Advanced Programming 2 -- Flight Simulator

**Features Implemented:**
* Dynamic loading of DLL based on algorithm picked
  * Simple Algorithm *(default)*
  * Minimum Circle Algorithm
* File Loading
  * CSV
  * XML
* Connection to FlightGear app via server **FlightGear must be opened prior to pressing *Start Simulation* in order to view the flight video**
* Buttons moving in real-time as the video plays
  * Joystick
  * Rudder bar
  * Throttle bar
  * Scroll bar
* Flight indices update in real-time as the video plays
  * Height
  * Speed
  * Direction
  * Yaw
  * Roll
  * Pitch
* Scroll bar to jump to any point in time of the video
* Timer that updates in real time as the simulation plays 
* Text Box allowing the user to change the speed at which the simulation plays 
* List of attributes, when upon selection present the user with the following graphs:
  * Status of the attribute selected (over the span of past 30 frames until current moment in video)
  * Status of the attribute that is most correlated to that which was selected (over the span of past 30 frames until current moment in video)
  * Regression Line between the selected attribute and the attribute most correlated to it
   * Points made from attribute and most correlated attribute from the last thirty timesteps update in real time
   * Points of anomaly are shown on the graph 
      

**Project Structure:**
 The application is built using the MVVM structure 
 * View
   *  MainWindow.xaml is responsible for the visual presentation of the application 
   *  MainWindow.xaml.cs, the code behind of the xaml
 * ViewModel
   *  IViewModel.cs, an interface which implements INotifyPropertyChanged and is implemented by the ViewModel class
   *  ViewModel.cs is responsible for the connection between the MainWindow and the Model  
 * Model
   *  IModel.cs, an interface which implements INotifyPropertyChanged and is implemented by the Model class
   *  Model.cs is responsible for the back end logic of the program 
   *  DynamicLibraryLoader.cs 
     *  Loads the functions of the DLLs in a separate class to be used without loading the DLL multiple times unnecessarily 

**Necessary Installations:**

**Instruction to Run Application:**
* Download the FlightGear application at https://flightgear.org/
* Right click on the application icon -> open file location -> leave the bin folder -> enter data folder -> enter protocol folder -> put xml file (playback_small.xml) into the folder 
* Upon opening the application, go to settings -> additional settings and write: --generic=socket,in,10,127.0.0.1,5400,udp,playback_small
--fdm=null
* To use the flight simulator, press "Fly" in the FlightGear application 
* When the application window pops up after running, press LoadCSV and LoadXML buttons to upload relevant files 
* Select an algorithm if wanted or the simple default algorithm will be used, this can be done at any stage of the simulation
* Press "Start Simulation"

**Additional Links:**

