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
            double RSI = 0.0;
            int x = 1;
            List<HistoricalStockData> AAPL = DownloadData("AAPL");
            List<HistoricalStockData> updays = new List<HistoricalStockData>();
            List<HistoricalStockData> downdays = new List<HistoricalStockData>();

            while (x <= 300)
            {
           

                for (int i = x; i < x + 30; i++)
                {
                    if (AAPL[i].Close >= AAPL[i - 1].Close)
                        updays.Add(AAPL[i]);
                    else
                        downdays.Add(AAPL[i]);
                }
                RSI = 100.0 - (100.0 / (1.0 + (Averageup(updays) / Averageup(downdays))));
                Console.WriteLine(RSI);
                updays.Clear();
                downdays.Clear();
                x++;
            }

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
           
        
        public static double Averageup(List<HistoricalStockData> updays)
            {
                double avgup = 0;
                for(int i= 0; i < updays.Count; i++)
                {
                    avgup += updays[i].Close;
                }
                avgup = avgup / (double)updays.Count;
                return avgup;
            }   
        }
}
