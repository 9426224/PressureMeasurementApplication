using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PressureMeasurementApplication.Utils
{
    public class RelayCommand<T> : ICommand
    {
        private Action<T> executeMethod;
        private Predicate<T> canExecuteMethod;

        public RelayCommand(Action<T> executeMethod)
            : this(executeMethod, null)
        {
        }

        public RelayCommand(Action<T> executeMethod, Predicate<T> canExecuteMethod)
        {
            this.canExecuteMethod = canExecuteMethod;
            this.executeMethod = executeMethod;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
            => canExecuteMethod == null ? true : canExecuteMethod((T)parameter);

        public void Execute(object parameter)
            => executeMethod((T)parameter);
    }

    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action executeMethod)
            : base(x => executeMethod())
        {
        }

        public RelayCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : base(x => executeMethod(), x => canExecuteMethod())
        {
        }
    }
}
