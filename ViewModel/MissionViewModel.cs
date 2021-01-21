using Microsoft.EntityFrameworkCore;
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
    public class MissionViewModel : ViewModelBase
    {
        public SQLiteDataContext dataContext;

        public ICommand GetDataCommand { get; }

        public MissionViewModel(SQLiteDataContext dataContext)
        {
            GetDataCommand = new AwaitableDelegateCommand(SerialPortProtocol.Instance.GetData);

            this.dataContext = dataContext;
        }
    }
}
