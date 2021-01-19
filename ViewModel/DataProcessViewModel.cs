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
        public DataModel DataModel => DataModel.Instance;

        public DataContext dataContext;

        public ICommand ReadDeviceDataCommand { get; }
        public ICommand AddDataToLocalCommand { get; }

        public DataProcessViewModel()
        {
            ReadDeviceDataCommand = new AwaitableDelegateCommand(ReadDeviceData);
        }

        public async Task ReadDeviceData()
        {
            while(true)
            {
                await GetData();
            }
        }

        public async Task GetData()
            =>DataModel.Data = (await SerialPortManager.Instance.ReadPort()).ToArray();

    }
}
