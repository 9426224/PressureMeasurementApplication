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
using System.Windows.Input;
using PressureMeasurementApplication.Utils;

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
        public Dictionary<string, Handshake> HandshakeList { get; }

        public ICommand OpenCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand RefreshCommand { get; }

        public SerialPortViewModel()
        {
            PortNameList = SerialPortsSettings.Instance.GetPorts();
            BaudRatesList = SerialPortsSettings.Instance.GetBaudRates();
            ParitiesList = SerialPortsSettings.Instance.GetParities();
            DataBitsList = SerialPortsSettings.Instance.GetDataBits();
            StopBitsList = SerialPortsSettings.Instance.GetStopBits();
            HandshakeList = SerialPortsSettings.Instance.GetHandshakes();

            OpenCommand = new RelayCommand(Open, () => true);
            CloseCommand = new RelayCommand(Close, () => true);
            RefreshCommand = new RelayCommand(RefreshPortNameList, () => true);
        }

        public string PortName { get; set; }
        public int BaudRate { get; set; }
        public Parity Parity { get; set; }
        public int DataBits { get; set; }
        public StopBits StopBits { get; set; }
        public Handshake Handshake { get; set; }

        //刷新端口列表
        public void RefreshPortNameList()
        {
            PortNameList = SerialPortsSettings.Instance.GetPorts();
        }

        //开启设备
        public void Open()
        {
            SerialPortModel.Instance.Open(PortName, BaudRate, Parity, DataBits, StopBits, Handshake);
        }

        //关闭设备
        public void Close()
        {
            SerialPortModel.Instance.Close();
        }

    }
}