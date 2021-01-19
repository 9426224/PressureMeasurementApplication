using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressureMeasurementApplication.Utils
{
    [AddINotifyPropertyChangedInterface]
    public class SingletonBase<T> where T : class
    {
        private static T InstanceCreator()
        {
            return Activator.CreateInstance(typeof(T), true) as T;
        }

        private static readonly Lazy<T> _Instance = new Lazy<T>(() => InstanceCreator());

        public static T Instance { get { return _Instance.Value; } }
    {
    }
}
