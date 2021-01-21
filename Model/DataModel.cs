using PressureMeasurementApplication.Utils;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressureMeasurementApplication.Model
{
    public class DataModel : ModelBase<DataModel>
    {
        /// <summary>
        /// 获取或设置一个值，表示该数据所属任务计划的Id。
        /// </summary>
        public ulong MissionModelId { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示任务计划的单个数据。
        /// </summary>
        public byte Data { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示该数据的采集来源的传感器ID。
        /// </summary>
        public int SensorId { get; set; }

    }
}
