using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.IO;
using System.Threading.Tasks;

namespace PressureMeasurementApplication.Model
{
    public class SerialPortModel : ModelBase<SerialPortModel>
    {
        private System.IO.Ports.SerialPort serialPort;
        private byte[] buffer = new byte[1000];

        public bool DeviceMode ;

        private SerialPortModel()
        {
            this.serialPort = new System.IO.Ports.SerialPort();
        }

        /// <summary>
        /// 如果串口确定连接则返回True
        /// </summary>
        public bool IsOpen { get { return serialPort.IsOpen; } }

        /// <summary>
        /// 开启串口连接
        /// </summary>
        /// <param name="portName">串口名</param>
        /// <param name="baudRate">波特率</param>
        /// <param name="parity">校验位</param>
        /// <param name="dataBits">数据位</param>
        /// <param name="stopBits">停止位</param>
        /// <param name="handshake"></param>
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

                serialPort.ReadTimeout = 200;
                serialPort.WriteTimeout = 50;

                serialPort.Open();
            });

        }

        /// <summary>
        /// 关闭串口连接
        /// </summary>
        public async Task Close()
        {
            await Task.Run(() =>
            {
                serialPort.Close();
            });
        }

        /// <summary>
        /// 发送/写入数据至串口
        /// </summary>
        /// <param name="message"></param>
        public async Task SendData(Memory<byte> message)
        {
            if (serialPort.IsOpen)
            {
                await serialPort.BaseStream.WriteAsync(message);
            }
            throw new InvalidOperationException("串口未打开");
        }

        /// <summary>
        /// 读取串口
        /// </summary>
        public async Task<Memory<Byte>> ReadPort()
        {
            if (serialPort.IsOpen)
            {
                await serialPort.BaseStream.ReadAsync(buffer, 0, 1);

                var length = buffer[0];

                await serialPort.BaseStream.ReadAsync(buffer, 1, length);

                return new Memory<byte>(buffer, 0, length);
               
            }
            throw new InvalidOperationException("串口未打开");
        }

    }
}
