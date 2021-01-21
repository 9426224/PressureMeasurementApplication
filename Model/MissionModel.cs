using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressureMeasurementApplication.Model
{
    public class MissionModel :ModelBase<MissionModel>
    {
        /// <summary>
        /// 获取或设置一个值，表示任务计划的名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示任务计划的开始时间。
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示该任务计划中包含的传感器数量。
        /// </summary>
        public int SensorNumber { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示一组关联至该计划的测量数据的合集。
        /// </summary>
        public ICollection<DataModel> DataModels { get; set; } 
    }
}
