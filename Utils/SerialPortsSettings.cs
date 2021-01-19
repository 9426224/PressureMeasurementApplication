using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Management;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PressureMeasurementApplication.Utils;

namespace PressureMeasurementApplication.Utils
{
    [Serializable]
    public class SerialPortsSettings : SingletonBase<SerialPortsSettings>
    {
        /// <summary>
        /// 返回一个Hashmap，存储使用<see cref="ManagementObjectSearcher"/>从<see cref="Win32_PnPEntity"/>中获取到的所有串口类型设备的合集。
        /// </summary>
        /// <returns><see cref="Dictionary{string, string}"/></returns>
        public async Task<Dictionary<string, string>> GetPorts()
        {
            using var searcher = new ManagementObjectSearcher(@"SELECT DeviceID,Caption FROM Win32_PnPEntity Where pnpclass = 'Ports' ");

            return await Task.Run(() =>
                searcher.Get().OfType<ManagementBaseObject>().ToDictionary(
                    x => Regex.Match(x.GetPropertyValue("Caption").ToString(), @"^.+\((COM\d+)\)$").Groups[1].Value,
                    x => x.GetPropertyValue("Caption").ToString())
                );
            
        }

        /// <summary>
        /// 返回一个Hashmap，存储<see cref="SerialPort.BaudRate">BaudRate</see>中的推荐设置的波特率的合集。
        /// </summary>
        /// <returns><see cref="Dictionary{TKey, TValue}"/></returns>
        public Dictionary<int, string> GetBaudRates() 
            => new[] { 300, 600, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200, 230400 }.ToDictionary(x => x, x => $"{x} Baud");


        /// <summary>
        /// 返回一个Hashmap，存储<see cref="Parity"/>中的所有校验位合集。
        /// </summary>
        /// <returns><see cref="Dictionary{Parity, string}"/></returns>
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

        /// <summary>
        /// 返回一个Hashmap，存储<see cref="SerialPort.DataBits">DataBits</see>中的所有数据位合集。
        /// </summary>
        /// <returns><see cref="Dictionary{int, int}"/></returns>
        public Dictionary<int, int> GetDataBits()
            => new[] { 5, 6, 7, 8 }.ToDictionary(x => x, x => x);

        /// <summary>
        /// 返回一个Hashmap，存储<see cref="StopBits"/>中的所有停止位合集。
        /// </summary>
        /// <returns><see cref="Dictionary{StopBits, string}"/></returns>
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

        /// <summary>
        /// 返回一个Hashmap，存储<see cref="Handshake"/>中的所有握手状态。
        /// </summary>
        /// <returns><see cref="Dictionary{Handshake, string}"/></returns>
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
