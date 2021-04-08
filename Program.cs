using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;     // DLL support

 class HelloWorld
{
    [DllImport("C:\\Users\\miche\\Desktop\\university\\cpp\\Dll-tzvi\\x64\\Debug\\Dll-fg.dll")]
    //[DllImport("Dll-tzvi.dll")]
    //public static extern void DisplayHelloFromDLL();
    public static extern IntPtr Create(String CSVfileName);

    static void Main()
    {
        ModelDina m = new ModelDina();
        m.Start();
        //new Client();
        /*Console.WriteLine("Hello and welcome to our awesome Flight Simulator!!!\n");
        Console.WriteLine("Please enter the path to your xml attribute file");
        String path1= Console.ReadLine();
        Console.WriteLine("Please enter the path to your csv file");
        String path2 = Console.ReadLine();
        //DisplayHelloFromDLL(); 
        IntPtr ts = Create(path2);
        MODEL m;
        List <Attribute> l = Parser(path1);
        //call anomaly algorithm- start(ts)but need info for xml*/
    }

    //C:\\Users\\miche\\Desktop\\university\\cpp\\Tzvi-csharp\\Tzvi-csharp\\reg_flight.csv
    //C:\Users\\miche\\Desktop\\university\\cpp\\Tzvi-csharp\\Tzvi-csharp\\playback_small.xml
}