using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Management;
using System.IO.Ports;

namespace PressureMeasurementApplication.Model.SerialPort
{
    [Serializable]
    public class SerialPortsSettings : ModelBase<SerialPortsSettings>
    {
        //设备ID
        public string DeviceID { get; set; }
        //设备描述
        public string Description { get; set; }
        //设备信息
        public string DeviceInfo { get; set; }
        //波特率名字
        public string BaudRateName { get; set; }
        //波特率值        
        public int BaudRateValue { get; set; }
        //校验名
        public string ParityName { get; set; }
        //校验值
        public Parity ParityValue { get; set; }

        //获取本机所有COM设备的详细信息
        public List<SerialPortsSettings> GetPorts()
        {
            var devicesLists = new List<SerialPortsSettings>();

            ManagementObjectCollection moc;
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_SerialPort"))
            {
                moc = searcher.Get();
            }

            foreach (var device in moc)
            {
                devicesLists.Add(new SerialPortsSettings()
                {
                    DeviceID = (string)device.GetPropertyValue("DeviceID"),
                    Description = (string)device.GetPropertyValue("Description"),
                    DeviceInfo = (string)device.GetPropertyValue("Description") + " (" + (string)device.GetPropertyValue("DeviceID") + ")"
                });
            }

            moc.Dispose();

            return devicesLists;
        }

        //获取所有可设置的波特率
        public List<SerialPortsSettings> GetBaudRates()
        {
            var baudRates = new List<SerialPortsSettings>();

            baudRates.Add(new SerialPortsSettings() { BaudRateName = "300 Baud", BaudRateValue = 300 });
            baudRates.Add(new SerialPortsSettings() { BaudRateName = "600 Baud", BaudRateValue = 600 });
            baudRates.Add(new SerialPortsSettings() { BaudRateName = "1200 Baud", BaudRateValue = 1200 });
            baudRates.Add(new SerialPortsSettings() { BaudRateName = "2400 Baud", BaudRateValue = 2400 });
            baudRates.Add(new SerialPortsSettings() { BaudRateName = "4800 Baud", BaudRateValue = 4800 });
            baudRates.Add(new SerialPortsSettings() { BaudRateName = "9600 Baud", BaudRateValue = 9600 });
            baudRates.Add(new SerialPortsSettings() { BaudRateName = "19200 Baud", BaudRateValue = 19200 });
            baudRates.Add(new SerialPortsSettings() { BaudRateName = "38400 Baud", BaudRateValue = 38400 });
            baudRates.Add(new SerialPortsSettings() { BaudRateName = "57600 Baud", BaudRateValue = 57600 });
            baudRates.Add(new SerialPortsSettings() { BaudRateName = "115200 Baud", BaudRateValue = 115200 });
            baudRates.Add(new SerialPortsSettings() { BaudRateName = "230400 Baud", BaudRateValue = 230400 });

            return baudRates;
        }

        //获取所有校验位
        public List<SerialPortsSettings> GetParities()
        {
            var parities = new List<SerialPortsSettings>();

            parities.Add(new SerialPortsSettings() { ParityName = "无校验位", ParityValue = Parity.None });
            parities.Add(new SerialPortsSettings() { ParityName = "奇校验", ParityValue = Parity.Odd });
            parities.Add(new SerialPortsSettings() { ParityName = "偶校验", ParityValue = Parity.Even });
            parities.Add(new SerialPortsSettings() { ParityName = "校验位常0", ParityValue = Parity.Space });
            parities.Add(new SerialPortsSettings() { ParityName = "校验位常1", ParityValue = Parity.Mark });

            return parities;
        }

        //获取数据位
        public static int[] GetDataBits = { 5, 6, 7, 8 };
    }
}
