using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Runtime.InteropServices;     // DLL support

public class ModelDina
{
    [DllImport("C:\\Users\\miche\\Desktop\\university\\cpp\\Dll-tzvi\\x64\\Debug\\Dll-fg.dll")]

    public static extern IntPtr Create(String CSVfileName, String[] l, int size);

    [DllImport("C:\\Users\\miche\\Desktop\\university\\cpp\\Dll-tzvi\\x64\\Debug\\Dll-fg.dll")]
    public static extern float givesFloatTs(IntPtr obj, int line, String att);

    [DllImport("C:\\Users\\miche\\Desktop\\university\\cpp\\Dll-tzvi\\x64\\Debug\\Dll-fg.dll")]
    public static extern int getRowSize(IntPtr ts);

    public static XDocument XDoc;
    public static String[] l = new String[100];
    public static IntPtr ts;
    public static Dictionary<String, List<float>> newTsDict = new Dictionary<String, List<float>>();
    public ModelDina()
	{
	}
    public class Attribute
    {
        public String name { get; set; }
    }
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
        for(int j = 0 ; j < size1/2; j++)
        {
            SAttsList2.Add(SAttsList[j]);
        }
        return SAttsList2;
    }

    //user story 1 creating timeSeries and 
    public void Start()
    {
        Console.WriteLine("Hello and welcome to our awesome Flight Simulator!!!\n");
        Console.WriteLine("Please enter the path to your xml attribute file");
        String path1 = Console.ReadLine();
        Console.WriteLine("Please enter the path to your csv file");
        String path2 = Console.ReadLine(); 
        List<String> SAttsList = Parser(path1);
        int k;
        for (k = 0; k < SAttsList.Count(); k++)
        {
            l[k] = SAttsList[k];
        }
        ts = Create(path2, l, k);
        newTsDict = getDictionary(SAttsList, ts, path2);
    }

    Dictionary<String, List<float>> getDictionary(List<String> SAttsList,IntPtr ts,String path)
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
}
