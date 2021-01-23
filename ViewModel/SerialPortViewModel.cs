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
using PressureMeasurementApplication.Utils.SerialPort;
using System.Threading.Tasks;

namespace PressureMeasurementApplication.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class SerialPortViewModel 
    {
        public Dictionary<string, string> PortNameList { get; private set; }
        public Dictionary<int, string> BaudRatesList { get; }
        public Dictionary<Parity, string> ParitiesList { get; }
        public Dictionary<int, int> DataBitsList { get; }
        public Dictionary<StopBits, string> StopBitsList { get; }
        public Dictionary<Handshake, string> HandshakeList { get; }

        public string PortName { get; set; }
        public int BaudRate { get; set; } = 115200;
        public Parity Parity { get; set; } = Parity.None;
        public int DataBits { get; set; } = 8;
        public StopBits StopBits { get; set; } = StopBits.One;
        public Handshake Handshake { get; set; } = Handshake.None;

        public ICommand OpenCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand RefreshCommand { get; }

        public SerialPortViewModel()
        {
            _ = RefreshPortNameList();
            BaudRatesList = Settings.Instance.GetBaudRates();
            ParitiesList = Settings.Instance.GetParities();
            DataBitsList = Settings.Instance.GetDataBits();
            StopBitsList = Settings.Instance.GetStopBits();
            HandshakeList = Settings.Instance.GetHandshakes();

            OpenCommand = new AwaitableDelegateCommand(OpenAsync);
            CloseCommand = new AwaitableDelegateCommand(CloseAsync);
            RefreshCommand = new AwaitableDelegateCommand(RefreshPortNameList);
        }

        /// <summary>
        /// 使用<see cref="Settings.GetPorts">GetPorts</see>刷新串口列表。
        /// </summary>
        /// <returns>代表异步获取串口列表的任务。</returns>
        public async Task RefreshPortNameList()
        {
            PortNameList = await Settings.Instance.GetPorts();
        }

        /// <summary>
        /// 使用<see cref="Manager.Open">Open</see>方法开启设备串口连接。
        /// </summary>
        /// <returns>代表异步开启设备的任务。</returns>
        public async Task OpenAsync()
        {
            if (!PortNameList.ContainsKey(PortName))
            {
                MessageBox.Show("错误:未选择串口设备！");
                return;
            }
            try
            {
                await Manager.Instance.Open(PortName, BaudRate, Parity, DataBits, StopBits, Handshake);
            }
            catch (Exception e)
            {
                MessageBox.Show("错误:" + e.Message);
            }
        }

        /// <summary>
        /// 使用<see cref="Manager.Close">Close</see>方法关闭设备。
        /// </summary>
        /// <returns>代表异步关闭设备的任务。</returns>
        public async Task CloseAsync()
        {
            await Manager.Instance.Close();
        }

    }
}