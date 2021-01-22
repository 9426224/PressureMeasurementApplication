using PressureMeasurementApplication.Utils;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace PressureMeasurementApplication.ViewModel
{
    public class MainViewModel
    {
        SQLiteDataContext dataContext;

        public MainViewModel(SQLiteDataContext dataContext, MissionViewModel missionViewModel,SerialPortViewModel serialPortViewModel)
        {
            this.dataContext = dataContext;

            MissionViewModel = missionViewModel;

            SerialPortViewModel = serialPortViewModel;
        }

        public MissionViewModel MissionViewModel { get; }

        public SerialPortViewModel SerialPortViewModel { get; }

    }
}
