using PressureMeasurementApplication.Model;
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
using System.Threading.Tasks;

namespace PressureMeasurementApplication.ViewModel
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
            _ = RefreshPortNameList();
            BaudRatesList = SerialPortsSettings.Instance.GetBaudRates();
            ParitiesList = SerialPortsSettings.Instance.GetParities();
            DataBitsList = SerialPortsSettings.Instance.GetDataBits();
            StopBitsList = SerialPortsSettings.Instance.GetStopBits();
            HandshakeList = SerialPortsSettings.Instance.GetHandshakes();

            OpenCommand = new AwaitableDelegateCommand(OpenAsync);
            CloseCommand = new AwaitableDelegateCommand(CloseAsync);
            RefreshCommand = new AwaitableDelegateCommand(RefreshPortNameList, () => true);
        }

        public string PortName { get; set; }
        public int BaudRate { get; set; }
        public Parity Parity { get; set; }
        public int DataBits { get; set; }
        public StopBits StopBits { get; set; }
        public Handshake Handshake { get; set; }

        //刷新端口列表
        public async Task RefreshPortNameList()
        {
            PortNameList = await SerialPortsSettings.Instance.GetPorts();
        }

        //开启设备
        public async Task OpenAsync()
        {
            await SerialPortModel.Instance.Open(PortName, BaudRate, Parity, DataBits, StopBits, Handshake);
            var buffer = await SerialPortModel.Instance.ReadPort();
        }

        //关闭设备
        public async Task CloseAsync()
        {
            await SerialPortModel.Instance.Close();
        }

    }
}