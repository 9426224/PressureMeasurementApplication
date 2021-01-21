using PressureMeasurementApplication.Utils;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;

namespace PressureMeasurementApplication.Model
{
    [AddINotifyPropertyChangedInterface]
    public class ModelBase<T> where T : class
    {
        /// <summary>
        /// 唯一ID。
        /// </summary>
        public ulong Id { get; set; } 

        private static T InstanceCreator()
        {
            return Activator.CreateInstance(typeof(T), true) as T;
        }

        private static readonly Lazy<T> _Instance = new Lazy<T>(() => InstanceCreator());

        public static T Instance { get { return _Instance.Value; } }
    }

    [AddINotifyPropertyChangedInterface]
    public class ModelBase
    {
        public ulong Id { get; set; }
    }
}
