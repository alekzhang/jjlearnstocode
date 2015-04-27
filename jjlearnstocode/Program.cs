using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace jjlearnstocode
{
    class Program
    {
        static void Main(string[] args)
        {
            List<HistoricalStockData> data = DownloadData("data");
            

            Console.WriteLine(updays.Count);
            Console.WriteLine(downdays.Count);
            Console.ReadLine();
        }

        public static List<HistoricalStockData> DownloadData(string ticker)
        {
            List<HistoricalStockData> retval = new List<HistoricalStockData>();

            using (WebClient web = new WebClient())
            {
                string data = web.DownloadString(string.Format("http://ichart.finance.yahoo.com/table.csv?s={0}", ticker));

                data = data.Replace("r", "");

                string[] rows = data.Split('\n');

                //First row is headers so Ignore it
                for (int i = 1; i < rows.Length; i++)
                {
                    if (rows[i].Replace("n", "").Trim() == "") continue;

                    string[] cols = rows[i].Split(',');

                    HistoricalStockData hs = new HistoricalStockData();
                    hs.Date = Convert.ToDateTime(cols[0]);
                    hs.Open = Convert.ToDouble(cols[1]);
                    hs.High = Convert.ToDouble(cols[2]);
                    hs.Low = Convert.ToDouble(cols[3]);
                    hs.Close = Convert.ToDouble(cols[4]);
                    hs.Volume = Convert.ToDouble(cols[5]);
                    hs.AdjClose = Convert.ToDouble(cols[6]);

                    retval.Add(hs);
                }

                return retval;
            }
        }

        public static double Averagelist(List<double> anylist)
        {
            double avgup = 0;
            for(int i= 0; i < anylist.Count; i++)
            {
                avgup += anylist[i];
            }
            avgup = avgup / (double)anylist.Count;
            return avgup;
        }

        public static List<double> SimpleMovingAverage(List<HistoricalStockData> data, int numberOfReturnedDays, int numberOfSampleDays)
        {
            List<double> smaData = new List<double>();
            int x = 0;
            double sma = 0;

           while (x < numberOfReturnedDays)
           {
               for (int i = x; i < x + numberOfSampleDays; i++)
               {
                   sma += data[i].Close;
                  
               }

               sma = sma / numberOfSampleDays;
               smaData.Add(sma);
               sma = 0;
               x++;
           }

            return smaData;
        }

        public static List<double> ExponentialMovingAverage(List<HistoricalStockData> data, int numberOfReturnedDays, int numberOfSampleDays)
        {
            List<double> emaData = new List<double>();
            int x = 0;
            double DayISma = 0;
            double DayIEma = 0;
            double ema = 0;
            double alpha = 2 / (numberOfSampleDays + 1);

            for (int i = numberOfSampleDays + 1; i < numberOfSampleDays + numberOfSampleDays + 1; i++)
            {
                DayISma += data[i].Close;
                DayISma = DayISma / numberOfSampleDays;
            }

            DayIEma = (data[numberOfSampleDays].Close * alpha) + (DayISma * (1 - alpha));
    
            while (x < numberOfReturnedDays)
                {
                    for (int i = x; i < x + numberOfSampleDays; i++)
                    {
                        ema = DayIEma + (alpha * (data[i].Close - DayIEma));
                        emaData.Add(ema);
                        DayIEma = ema;
                    }
                x++;
                }

            return emaData;
        }

        public static List<double> MACD(List<HistoricalStockData> data, int numberOfReturnedDays, int smallSampleDayCount, int largeSampleDayCount, int diffEMASampleDayCount)
        {
            List<double> macdData = new List<double>();
            // jj code goes here
            return macdData;
        }

        public static List<Tuple<double, double>> DMI(List<HistoricalStockData> data, int numberOfReturnedDays)
        {
            List<Tuple<double, double>> dmiData = new List<Tuple<double, double>>();
            for(int i = 0; i < numberOfReturnedDays; i++)
            {
                double plusDI = 0.0;
                double minusDI = 0.0;

                // jj code goes here

                dmiData.Add(new Tuple<double, double>(plusDI, minusDI));
            }
            return dmiData;
        }

        public static List<double> RSI(List<HistoricalStockData> data, int numberOfReturnedDays, int numberOfSampleDays)
        {
            List<double> rsiData = new List<double>();
            double RSI = 0.0;
            double deltaa = 0.0;
            int x = 1;
            
            List<HistoricalStockData> updays = new List<HistoricalStockData>();
            List<HistoricalStockData> downdays = new List<HistoricalStockData>();
            List<double> deltaups = new List<double>();
            List<double> deltadowns = new List<double>();

            while (x <= numberOfReturnedDays)
            {
                for (int i = x; i < x + numberOfSampleDays; i++)
                {
                    if (data[i].Close >= data[i - 1].Close)
                    {
                        deltaa = data[i].Close - data[i - 1].Close;
                        deltaups.Add(deltaa);
                    }
                    else
                    {
                        deltaa = data[i - 1].Close - data[i].Close;
                        deltadowns.Add(deltaa);
                    }
                }
                RSI = 100.0 - (100.0 / (1.0 + (Averagelist(deltaups) / Averagelist(deltadowns))));
                Console.WriteLine(RSI);
                deltaups.Clear();
                deltadowns.Clear();
                x++;
            }
            for (int i = 1; i < x; i++)
            {
                if (data[i].Close >= data[i - 1].Close)
                {
                    updays.Add(data[i]);
                }
                else
                {
                    downdays.Add(data[i]);
                }
            }
            
            return rsiData;
        }
    }
}
