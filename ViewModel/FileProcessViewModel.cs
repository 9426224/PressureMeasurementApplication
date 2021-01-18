using PressureMeasurementApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressureMeasurementApplication.ViewModel
{
    public class FileProcessViewModel : ViewModelBase
    {
        public string data;

        public FileProcessViewModel()
        {
            SerialPortModel.Instance.OnDataReceived += ReadData;
            PrintStringData();
        }

        public void ReadData(object sender, string data)
            => this.data = data.ToString();

        public void PrintStringData()
        {
            while (true)
            {
                Console.WriteLine($"{data}\n");
            }
               
        }
    }
}
