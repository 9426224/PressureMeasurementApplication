using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Management;
using System.IO.Ports;
using System.Text.RegularExpressions;

namespace PressureMeasurementApplication.Model.SerialPort
{
    [Serializable]
    public class SerialPortsSettings : ModelBase<SerialPortsSettings>
    {
        //获取本机所有COM端口设备
        public Dictionary<string, string> GetPorts()
        {
            using var searcher = new ManagementObjectSearcher(@"SELECT DeviceID,Caption FROM Win32_PnPEntity Where pnpclass = 'Ports' ");
            using var moc = searcher.Get();

            return moc.OfType<ManagementBaseObject>().ToDictionary(
                x => x.GetPropertyValue("Caption").ToString() ,
                x => Regex.Match(x.GetPropertyValue("DeviceID").ToString(), @"^.+\((COM\d+)\)$").Groups[1].Value);
        }

        //获取所有可设置的波特率
        public Dictionary<string, int> GetBaudRates() 
            => new[] { 300, 600, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200, 230400 }.ToDictionary(x => $"{x} Baud", x => x);


        //获取所有校验位
        public Dictionary<string, Parity> GetParities()
        {
            return new Dictionary<string, Parity>()
            {
                {"无校验位", Parity.None },
                {"奇校验", Parity.Odd },
                {"偶校验", Parity.Even },
                {"校验位常0", Parity.Space },
                {"校验位常1", Parity.Mark },
            };
        }

        //获取所有数据位
        public Dictionary<int, int> GetDataBits()
            => new[] { 5, 6, 7, 8 }.ToDictionary(x => x, x => x);

        //获取所有停止位
        public Dictionary<string, StopBits> GetStopBits()
        {
            return new Dictionary<string, StopBits>()
            {
                //{"无停止位", StopBits.None },
                {"1位", StopBits.One },
                {"1.5位", StopBits.OnePointFive },
                {"2位", StopBits.Two },
            };
        }

        public Dictionary<string, Handshake> GetHandshakes()
        {
            return new Dictionary<string, Handshake>()
            {
                {"无", Handshake.None },
                {"RTS", Handshake.RequestToSend },
                {"RTS与XON/XOFF", Handshake.RequestToSendXOnXOff },
                {"XON/XOFF", Handshake.XOnXOff }
            };
        }
    }
}
