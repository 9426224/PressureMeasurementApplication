using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace PressureMeasurementApplication.Model.SerialPort
{
    public class SerialPortModel : ModelBase
    {
        private System.IO.Ports.SerialPort serialPort;

        public SerialPortModel(string com)
        {
            this.serialPort = new System.IO.Ports.SerialPort(com);
        }

        //端口号
        public string PortName
        {
            get { return serialPort.PortName; }
            set { serialPort.PortName = value; }
        }

        //波特率
        public int BaudRate
        {
            get { return serialPort.BaudRate; }
            set { serialPort.BaudRate = value; }
        }

        //校验位
        public Parity Parity
        {
            get { return serialPort.Parity; }
            set { serialPort.Parity = value; }
        }

        //停止位
        public StopBits StopBits
        {
            get { return serialPort.StopBits; }
            set { serialPort.StopBits = value; }
        }

        //数据位
        public int DataBits
        {
            get { return serialPort.DataBits; }
            set { serialPort.DataBits = value; }
        } 

        //开启串口
        public bool Open()
        {
            serialPort.Open();
            return serialPort.IsOpen ? true : false;
        }

        //关闭串口
        public bool Close()
        {
            serialPort.Close();
            return serialPort.IsOpen ? false : true;
        }
    }
}
