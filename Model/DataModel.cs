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
        //存储读取到的原始byte类型数据。
        [AlsoNotifyFor(nameof(DataString))]
        public byte[] Data { get; set; }

        //以字符串方式读写数据。
        public string DataString
        {
            get => Data is null? "null" : string.Join("", Data.Select(x=>x.ToString("x2")));
        }

    }
}
