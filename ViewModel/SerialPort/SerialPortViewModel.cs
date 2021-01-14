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

namespace PressureMeasurementApplication.ViewModel.SerialPort
{
    public class SerialPortViewModel : ViewModelBase 
    {        
        public List<SerialPortsSettings> PortNameList { get; private set; }
        public List<SerialPortsSettings> BaudRatesList { get; }
        public List<SerialPortsSettings> ParitiesList { get; }

        public SerialPortViewModel()
        {
            PortNameList = SerialPortsSettings.Instance.GetPorts();

            BaudRatesList = SerialPortsSettings.Instance.GetBaudRates();

            ParitiesList = SerialPortsSettings.Instance.GetParities();
        }

        public string PortName
        {
            get { return SerialPortModel.Instance.PortName; }
            set
            {
                Regex r = new Regex("^C+O+M+[0-9]{1,2}");
                if (value != null && value == r.Match(value).Value)
                {
                    SerialPortModel.Instance.PortName = value;
                    RaisePropertyChanged("PortName");
                }
            }
        }

        public int BaudRate
        {
            get { return SerialPortModel.Instance.BaudRate; }
            set 
            {
                SerialPortModel.Instance.BaudRate = value;
                RaisePropertyChanged("BaudRate");
            }
        }

        public Parity Parity
        {
            get { return SerialPortModel.Instance.Parity; }
            set
            {
                SerialPortModel.Instance.Parity = value;
                RaisePropertyChanged("Parity");
            }
        }

    }
}