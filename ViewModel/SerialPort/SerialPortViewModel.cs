using PressureMeasurementApplication.Model.SerialPort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Management;
using System.IO.Ports;
using PropertyChanged;

namespace PressureMeasurementApplication.ViewModel.SerialPort
{
    [AddINotifyPropertyChangedInterface]
    public class SerialPortViewModel : ViewModelBase 
    {        
        public Dictionary<string, string> PortNameList { get; private set; }
        public Dictionary<string, int> BaudRatesList { get; }
        public Dictionary<string, Parity> ParitiesList { get; }
        public Dictionary<int, int> DataBitsList { get; }
        public Dictionary<string, StopBits> StopBitsList { get; }

        public SerialPortViewModel()
        {
            PortNameList = SerialPortsSettings.Instance.GetPorts();
            BaudRatesList = SerialPortsSettings.Instance.GetBaudRates();
            ParitiesList = SerialPortsSettings.Instance.GetParities();
            DataBitsList = SerialPortsSettings.Instance.GetDataBits();
            StopBitsList = SerialPortsSettings.Instance.GetStopBits();
        }

        public string PortName { get; set; }
        public int BaudRate { get; set; }
        public Parity Parity { get; set; }
        public Parity DataBits { get; set; }
        public Parity StopBits { get; set; }

    }
}