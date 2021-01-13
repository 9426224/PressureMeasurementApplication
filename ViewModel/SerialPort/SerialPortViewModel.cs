using PressureMeasurementApplication.Model.SerialPort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;

namespace PressureMeasurementApplication.ViewModel.SerialPort
{
    public class SerialPortViewModel : ViewModelBase, INotifyPropertyChanged
    {        
        public SerialPortModel serialPort;

        public Dictionary<int, string> _portNameList;

        public SerialPortViewModel()
        {
            serialPort = new SerialPortModel("COM0");

            GetPortNameList();
        }

        public string PortName
        {
            get
            {
                return serialPort.PortName;
            }

            set
            {
                Regex r = new Regex("^C+O+M+[0-9]{1,2}");
                if (value == r.Match(value).Value)
                {
                    serialPort.PortName = value;
                    RaisePropertyChanged("PortName");
                }
                else
                {
                    MessageBox.Show("请重新输入对应COM序号!","错误");
                }
            }
        }

        public Dictionary<int, string> PortNameList 
        { 
            get { return _portNameList; } 
        }

        private void GetPortNameList()
        {
            _portNameList = new Dictionary<int, string>();

            _portNameList.Add(-1, "请选择");

            for (int i = 0; i < 15; i++)
            {
                _portNameList.Add(i, "COM" + i.ToString());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// INotifyPropertyChanged实现通知消息:成员内容已改变
        /// </summary>
        /// <param name="propertyName"></param>
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}