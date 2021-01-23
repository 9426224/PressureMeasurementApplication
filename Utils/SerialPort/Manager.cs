using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.IO;
using System.Threading.Tasks;

namespace PressureMeasurementApplication.Utils.SerialPort
{
    public class Manager : SingletonBase<Manager>
    {
        /// <summary>
        /// 实例化SerialPort。
        /// </summary>
        private System.IO.Ports.SerialPort serialPort;

        /// <summary>
        /// 缓存接收数据使用的Byte数组。长度为70000是考虑到长度域最多允许65535个字符单次输入。
        /// </summary>
        private byte[] buffer = new byte[70000];

        private Manager()
        {
            this.serialPort = new System.IO.Ports.SerialPort();
        }

        /// <summary>
        /// 返回<see cref="SerialPort.IsOpen">IsOpen</see>，True表示串口已开启，False表示串口未开启。
        /// </summary>
        public bool IsOpen { get { return serialPort.IsOpen; } }

        /// <summary>
        /// 通过<see cref="SerialPort.Open">Open</see>方法开启串口连接，进行该操作前会使用<see cref="Manager.Close">Close</see>方法关闭所有串口通信。
        /// </summary>
        /// <param name="portName">串口名:<see cref="SerialPort.PortName"/></param>
        /// <param name="baudRate">波特率:<see cref="SerialPort.BaudRate"/></param>
        /// <param name="parity">校验位:<see cref="SerialPort.Parity"/></param>
        /// <param name="dataBits">数据位:<see cref="SerialPort.DataBits"/></param>
        /// <param name="stopBits">停止位:<see cref="SerialPort.StopBits"/></param>
        /// <param name="handshake">握手协议:<see cref="SerialPort.Handshake"/></param>
        public async Task Open(
                string portName,
                int baudRate,
                Parity parity,
                int dataBits,
                StopBits stopBits,
                Handshake handshake)
        {
            await Close();

            await Task.Run(() =>
            {
                serialPort.PortName = portName;
                serialPort.BaudRate = baudRate;
                serialPort.Parity = parity;
                serialPort.DataBits = dataBits;
                serialPort.StopBits = stopBits;
                serialPort.Handshake = handshake;

                serialPort.ReadTimeout = 1000;
                serialPort.WriteTimeout = 50;

                serialPort.Open();
            });

        }

        /// <summary>
        /// 通过<see cref="SerialPort.Close">Close</see>方法关闭串口连接。
        /// </summary>
        public async Task Close()
        {
            await Task.Run(() =>
            {
                serialPort.Close();
            });
        }

        /// <summary>
        /// 通过<see cref="SerialPort.BaseStream">BaseStream</see>中的<see cref="WriteAsync"/>方法发送/写入数据至串口。
        /// </summary>
        /// <param name="message">外部返回的需要发送至串口的消息数组。</param>
        public async Task SendData(Memory<byte> message)
        {
            if (serialPort.IsOpen)
            {
                await serialPort.BaseStream.WriteAsync(message);
                return;
            }
            throw new InvalidOperationException("串口未打开");
        }

        /// <summary>
        /// 通过<see cref="SerialPort.BaseStream">BaseStream</see>中的<see cref="ReadAsync"/>方法读取串口数据并保存至<see cref="Manager.buffer">Buffer</see>中。
        /// </summary>
        public async Task<Memory<Byte>> ReadPort()
        {
            if (serialPort.IsOpen)
            {

                while (true)
                {
                    await serialPort.BaseStream.ReadAsync(buffer, 1 , 1);

                    if (buffer[0] == Protocol.Instance.StartBit[0]
                        && buffer[1] == Protocol.Instance.StartBit[1])
                    {
                        break;
                    }

                    buffer[0] = buffer[1];
                }

                await serialPort.BaseStream.ReadAsync(buffer, 2, 3);
                await serialPort.BaseStream.ReadAsync(buffer, 5, buffer[4] + 3);

                return new Memory<byte>(buffer, 0, buffer[4] + 8);

            }
            throw new InvalidOperationException("串口未打开");
        }

    }
}
