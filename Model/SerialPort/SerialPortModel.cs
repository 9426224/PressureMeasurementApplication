using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.IO;

namespace PressureMeasurementApplication.Model.SerialPort
{
    public class SerialPortModel : ModelBase<SerialPortModel>
    {
        private System.IO.Ports.SerialPort serialPort;
        private volatile bool keepReading;
        private Thread readThread;

        public SerialPortModel()
        {
            this.serialPort = new System.IO.Ports.SerialPort();
            keepReading = false;
            readThread = null;
        }

        /// <summary>
        /// 更新串口状态至事件订阅器
        /// </summary>
        public event EventHandler<string> OnStatusChanged;

        /// <summary>
        /// 更新从串口获取到的返回数据至事件订阅器
        /// </summary>
        public event EventHandler<string> OnDataReceived;

        /// <summary>
        /// 更新串口的连接状态至事件订阅器
        /// </summary>
        public event EventHandler<bool> OnSerialPortOpened;

        /// <summary>
        /// 如果串口确定连接则返回True
        /// </summary>
        public bool IsOpen { get { return serialPort.IsOpen;  } }

        /// <summary>
        /// 开启串口连接
        /// </summary>
        /// <param name="portName">串口名</param>
        /// <param name="baudRate">波特率</param>
        /// <param name="parity">校验位</param>
        /// <param name="dataBits">数据位</param>
        /// <param name="stopBits">停止位</param>
        /// <param name="handshake"></param>
        public void Open (
                string portName,
                int baudRate,
                Parity parity,
                int dataBits,
                StopBits stopBits,
                Handshake handshake)
        {
            Close();

            try
            {
                serialPort.PortName = portName;
                serialPort.BaudRate = baudRate;
                serialPort.Parity = parity;
                serialPort.DataBits = dataBits;
                serialPort.StopBits = stopBits;
                serialPort.Handshake = handshake;

                serialPort.ReadTimeout = 50;
                serialPort.WriteTimeout = 50;

                serialPort.Open();
                StartReading();
            }
            catch (IOException)
            {
                if (OnStatusChanged != null)
                    OnStatusChanged(this, string.Format("{0} 端口不存在。", portName));
            }
            catch (UnauthorizedAccessException)
            {
                if (OnStatusChanged != null)
                    OnStatusChanged(this, string.Format("{0} 端口正在使用中。", portName));
            }
            catch (Exception ex)
            {
                if (OnStatusChanged != null)
                    OnStatusChanged(this, "错误: " + ex.Message);
            }

            if (serialPort.IsOpen)
            {
                string sb = StopBits.None.ToString().Substring(0, 1);
                switch (serialPort.StopBits)
                {
                    case StopBits.One:
                        sb = "1"; break;
                    case StopBits.OnePointFive:
                        sb = "1.5"; break;
                    case StopBits.Two:
                        sb = "2"; break;
                    default:
                        break;
                }

                string p = serialPort.Parity.ToString().Substring(0, 1);
                string hs = serialPort.Handshake == Handshake.None ? "没有握手" : serialPort.Handshake.ToString();

                if (OnStatusChanged != null)
                    OnStatusChanged(this, string.Format(
                    "连接到 {0}: 波特率: {1} Bps, {2}{3}{4}, {5}。",
                    serialPort.PortName,
                    serialPort.BaudRate,
                    serialPort.DataBits,
                    p,
                    sb,
                    hs));

                if (OnSerialPortOpened != null)
                    OnSerialPortOpened(this, true);
            }
            else
            {
                if (OnStatusChanged != null)
                    OnStatusChanged(this, string.Format(
                    "{0} 端口正在使用中。",
                    portName));

                if (OnSerialPortOpened != null)
                    OnSerialPortOpened(this, false);
            }
        }

        /// <summary>
        /// 关闭串口连接
        /// </summary>
        public void Close()
        {
            StopReading();
            serialPort.Close();

            if(OnStatusChanged != null)
            {
                OnStatusChanged(this, "连接关闭。");
            }

            if(OnSerialPortOpened != null)
            {
                OnSerialPortOpened(this, false);
            }
        }

        /// <summary>
        /// 发送/写入数据至串口
        /// </summary>
        /// <param name="message"></param>
        public void SendString(string message)
        {
            if(serialPort.IsOpen)
            {
                try
                {
                    serialPort.Write(message);

                    if (OnStatusChanged != null)
                    {
                        OnStatusChanged(this, string.Format("消息已发送:{0}", message));
                    }
                }
                catch(Exception e)
                {
                    if(OnStatusChanged != null)
                    {
                        OnStatusChanged(this, string.Format("消息发送失败:{0}", e.Message));
                    }
                }
            }
        }

        /// <summary>
        /// 开始读取串口
        /// </summary>
        private void StartReading()
        {
            if(!keepReading)
            {
                keepReading = true;
                readThread = new Thread(ReadPort);
                readThread.Start();
            }
        }

        /// <summary>
        /// 停止读取串口
        /// </summary>
        private void StopReading()
        {
            if(keepReading)
            {
                keepReading = false;
                readThread.Join();
                readThread = null;
            }
        }

        /// <summary>
        /// 读取串口
        /// </summary>
        private void ReadPort()
        {
            while(keepReading)
            {
                if(serialPort.IsOpen)
                {
                    try
                    {
                        string data = serialPort.ReadLine();

                        if(OnDataReceived != null)
                        {
                            OnDataReceived(this, data);
                        }
                    }
                    catch (TimeoutException) { }
                }
                else
                {
                    TimeSpan waitTime = new TimeSpan(0, 0, 0, 0, 50);
                    Thread.Sleep(waitTime);
                }
            }
        }
    }
}
