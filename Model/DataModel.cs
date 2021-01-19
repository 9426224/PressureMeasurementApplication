using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressureMeasurementApplication.Model
{
    public class DataModel : ModelBase<DataModel>
    {
        //存储读取到的原始byte类型数据
        public byte[] Data { get; set; }

        //以字符串方式读写数据
        public string DataString
        {
            get => Data.ToString();
            set => Data = Encoding.UTF8.GetBytes(value);
        }

    }
}
