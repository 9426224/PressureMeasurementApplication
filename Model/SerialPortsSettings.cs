using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Management;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PressureMeasurementApplication.Model
{
    [Serializable]
    public class SerialPortsSettings : ModelBase<SerialPortsSettings>
    {
        //获取本机所有COM端口设备
        public async Task<Dictionary<string, string>> GetPorts()
        {
            using var searcher = new ManagementObjectSearcher(@"SELECT DeviceID,Caption FROM Win32_PnPEntity Where pnpclass = 'Ports' ");

            return await Task.Run(() =>
                searcher.Get().OfType<ManagementBaseObject>().ToDictionary(
                    x => Regex.Match(x.GetPropertyValue("Caption").ToString(), @"^.+\((COM\d+)\)$").Groups[1].Value,
                    x => x.GetPropertyValue("Caption").ToString())
                );
            
        }

        //获取所有可设置的波特率
        public Dictionary<int, string> GetBaudRates() 
            => new[] { 300, 600, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200, 230400 }.ToDictionary(x => x, x => $"{x} Baud");


        //获取所有校验位
        public Dictionary<Parity, string> GetParities()
        {
            return new Dictionary<Parity, string>()
            {
                {Parity.None, "无校验位"},
                {Parity.Odd, "奇校验"},
                {Parity.Even, "偶校验"},
                {Parity.Space, "校验位常0"},
                {Parity.Mark, "校验位常1"},
            };
        }

        //获取所有数据位
        public Dictionary<int, int> GetDataBits()
            => new[] { 5, 6, 7, 8 }.ToDictionary(x => x, x => x);

        //获取所有停止位
        public Dictionary<StopBits, string> GetStopBits()
        {
            return new Dictionary<StopBits, string>()
            {
                //{"无停止位", StopBits.None },
                {StopBits.One, "1位"},
                {StopBits.OnePointFive, "1.5位"},
                {StopBits.Two, "2位"},
            };
        }

        public Dictionary<Handshake, string> GetHandshakes()
        {
            return new Dictionary<Handshake, string>()
            {
                {Handshake.None, "无"},
                {Handshake.RequestToSend, "RTS"},
                {Handshake.RequestToSendXOnXOff, "RTS与XON/XOFF"},
                {Handshake.XOnXOff, "XON/XOFF"}
            };
        }
    }
}
