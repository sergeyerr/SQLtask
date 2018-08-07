using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace ConsoleApp1
{
    class Data
    {
        public readonly double Temperature, Pressure, Humidity;
        public readonly bool IsValid;
        public Data(double t, double p, double h, bool v)
        {
            Temperature = t;
            Pressure = p;
            Humidity = h;
            IsValid = v;
        }
    }
    class SQLWorker
    {
        public static Data GetDataFromCity(string city)
        {
            MySqlConnection connection = new MySqlConnection("server=127.0.0.1;port=8889; user=root; password=root; database=sampleAPIData; SslMode=none;");
            connection.Open();
            var query = connection.CreateCommand();
            query.CommandText = string.Format("SELECT temp, pressure, humidity FROM weather WHERE city='{0}' ORDER BY createdDate desc limit 1", city);
            MySqlDataReader reader = query.ExecuteReader();
            try
            {
                reader.Read();
                return new Data(reader.GetDouble(0), reader.GetDouble(1), reader.GetDouble(2), true);
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                return new Data(-1, -1, -1, false);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input City");
            string city = Console.ReadLine();
            Data data = SQLWorker.GetDataFromCity(city);
            if (data.IsValid)
            {
                Console.WriteLine("{0}\n Temperature:{1} Pressure:{2} Humidity:{3}\n", city, data.Temperature, data.Pressure, data.Humidity);
            }
            else
            {
                Console.WriteLine("No Data Available for the selected city\n");
            }
            Console.ReadKey();
        }
    }
}
