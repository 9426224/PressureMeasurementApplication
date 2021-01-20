using PressureMeasurementApplication.Model;
using PressureMeasurementApplication.Utils;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PressureMeasurementApplication.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class DataProcessViewModel : ViewModelBase
    {
        public SQLiteDataContext Context;

        public ICommand GetDataCommand { get; }

        public DataProcessViewModel()
        {
            GetDataCommand = new AwaitableDelegateCommand(GetData);
        }

        public async Task GetData()
        {
            while (true)
            {
                DataModel.Instance.Data = (await SerialPortManager.Instance.ReadPort()).ToArray();

                if(DataModel.Instance.Data[2] == 0x00)
                {
                    switch(DataModel.Instance.Data[3]){
                        case 0x04: //下位机停止发送数据
                            break;
                        case 0x05: //下位机开始传输FLASH内容
                            break;
                        case 0x06: //下位机开始传输实时预览内容
                            break; 
                        default:
                            break;
                    }
                }
            }
        }
    }
}
