using PressureMeasurementApplication.Model.SerialPort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PressureMeasurementApplication.ViewModel.SerialPort
{
    public class SerialPortViewModel : ViewModelBase
    {
        public SerialPortViewModel()
        {
            SerialPortModel serialPort = new SerialPortModel("COM1");
        }


    }
}