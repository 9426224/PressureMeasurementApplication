using System;
using System.Collections.Generic;
using System.Text;

namespace PressureMeasurementApplication.Model
{
    public class ModelBase <T> where T : class
    {
        private static T InstanceCreator()
        {
           return Activator.CreateInstance(typeof(T), true) as T;
        }

        private static readonly Lazy<T> _Instance = new Lazy<T>(() => InstanceCreator());        
        
        public static T Instance { get { return _Instance.Value; } }

    }
}
