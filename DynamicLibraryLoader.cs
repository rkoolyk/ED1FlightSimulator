using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
static class NativeMethods
{
    [DllImport("kernel32.dll")]
    public static extern IntPtr LoadLibrary(string dllToLoad);

    [DllImport("kernel32.dll")]
    public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

    [DllImport("kernel32.dll")]
    public static extern bool FreeLibrary(IntPtr hModule);
}

namespace ED1FlightSimulator
{
    public class DynamicLibraryLoader
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void findLinReg(IntPtr ts, ref float a, ref float b, String attA, String attB);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void MostCorrelatedFeature(IntPtr sad, [MarshalAs(UnmanagedType.LPStr)] String CSVfileName, [MarshalAs(UnmanagedType.LPArray)] String[] l, int size, [MarshalAs(UnmanagedType.LPStr)] String att, StringBuilder s);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void getTimeSteps(IntPtr sad, [MarshalAs(UnmanagedType.LPStr)] String CSVfileName, [MarshalAs(UnmanagedType.LPArray)] String[] l, int size, [MarshalAs(UnmanagedType.LPStr)] String oneWay, [MarshalAs(UnmanagedType.LPStr)] String otherWay, StringBuilder arr);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr CreateSAD();
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr Create(String CSVfileName, String[] l, int size);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate float givesFloatTs(IntPtr obj, int line, String att);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int getRowSize(IntPtr ts);

        private IntPtr AnomalyDetector;
        private IntPtr TimeSeries;
        private List<string> dataList;
        private Dictionary<String, String> correlations = new Dictionary<String, String>();
        private String csvPath;
        private String regFlightPath;
        private String xmlPath;
        IntPtr algoDLL;
        IntPtr pCreateSAD;
        IntPtr pMostCorrelatedFeature;
        IntPtr pFindLinReg;
        IntPtr timeSeriesDLL;
        IntPtr pCreate;
        IntPtr pgetRowSize;
        IntPtr pgivesFloatTs;


        public IntPtr AnomalyDetectionStater(String AnomalyAlgorithm, String regFlight)
        {
            regFlightPath = regFlight;
            algoDLL = NativeMethods.LoadLibrary(@AnomalyAlgorithm);
            pCreateSAD = NativeMethods.GetProcAddress(algoDLL, "CreateSAD");
            pMostCorrelatedFeature = NativeMethods.GetProcAddress(algoDLL, "MostCorrelatedFeature");
            pFindLinReg = NativeMethods.GetProcAddress(algoDLL, "findLinReg");
            CreateSAD CreateSAD = (CreateSAD)Marshal.GetDelegateForFunctionPointer(pCreateSAD, typeof(CreateSAD));
            AnomalyDetector = CreateSAD();
            Thread thread = new Thread(
                delegate ()
                {
                    CreateCorrelations();
                });
            thread.Start();
            //CreateCorrelations();
            return AnomalyDetector;
        }

        public  void LineReg(ref float a, ref float b, String category, String correlatedCategory)
        {
            findLinReg findLinReg =(findLinReg)Marshal.GetDelegateForFunctionPointer(pFindLinReg, typeof( findLinReg));
            findLinReg(TimeSeries, ref a, ref b, category, correlatedCategory);
        }
        public void CreateCorrelations()
        {
            MostCorrelatedFeature MostCorrelatedFeature = (MostCorrelatedFeature)Marshal.GetDelegateForFunctionPointer(pMostCorrelatedFeature, typeof(MostCorrelatedFeature));
            StringBuilder tmp = new StringBuilder();   
            foreach(String category in dataList)
            {
                MostCorrelatedFeature(AnomalyDetector, regFlightPath, dataList.ToArray(), dataList.Count(), category, tmp);
                correlations.Add(category, tmp.ToString());
            }
        }

        public String FindCorrelation(String category)
        {
            if (correlations.ContainsKey(category))
            {
                return correlations[category];
            }
            else
            {
                MostCorrelatedFeature MostCorrelatedFeature = (MostCorrelatedFeature)Marshal.GetDelegateForFunctionPointer(pMostCorrelatedFeature, typeof(MostCorrelatedFeature));
                StringBuilder tmp = new StringBuilder();
                MostCorrelatedFeature(AnomalyDetector, regFlightPath, dataList.ToArray(), dataList.Count(), category, tmp);
                return tmp.ToString();
            }
        }

        public  IntPtr CreateTS(String timeSeriesPath, String csvPath, List<string> data)
        {
            timeSeriesDLL = NativeMethods.LoadLibrary(@timeSeriesPath);
            pCreate = NativeMethods.GetProcAddress(timeSeriesDLL, "Create");
            pgetRowSize = NativeMethods.GetProcAddress(timeSeriesDLL, "getRowSize");
            pgivesFloatTs = NativeMethods.GetProcAddress(timeSeriesDLL, "givesFloatTs");
            Create Create = (Create)Marshal.GetDelegateForFunctionPointer(pCreate, typeof(Create));
            TimeSeries = Create(csvPath, data.ToArray(), data.Count());
            dataList = data;
            return TimeSeries;
        }
        public Dictionary<String, List<float>> GetDictionary()
        {
            Dictionary<String, List<float>> tsDic = new Dictionary<String, List<float>>();
            getRowSize getRowSize = (getRowSize)Marshal.GetDelegateForFunctionPointer(pgetRowSize, typeof(getRowSize));
            givesFloatTs givesFloatTs = (givesFloatTs)Marshal.GetDelegateForFunctionPointer(pgivesFloatTs, typeof(givesFloatTs));
            int size = getRowSize(TimeSeries);
            for (int i = 0; i < dataList.Count(); i++)
            {
                List<float> f = new List<float>();
                tsDic.Add(dataList[i], f);
                for (int j = 0; j < size; j++)
                {
                    tsDic[dataList[i]].Add(givesFloatTs(TimeSeries, j, dataList[i]));
                }
            }
            return tsDic;
        }
    }


    }

