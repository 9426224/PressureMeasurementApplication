using PressureMeasurementApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressureMeasurementApplication.ViewModel
{
    public class DataProcessViewModel : ViewModelBase
    {
        public DataProcessViewModel()
        {

        }

        public async Task GetData()
            =>DataModel.Instance.Data = (await SerialPortModel.Instance.ReadPort()).ToArray();

    }
}
