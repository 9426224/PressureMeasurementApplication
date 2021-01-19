using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PressureMeasurementApplication.Utils
{
    public class DelegateCommand<T> : ICommand
    {
        private Action<T> executeMethod;
        private Predicate<T> canExecuteMethod;

        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, null)
        {
        }

        public DelegateCommand(Action<T> executeMethod, Predicate<T> canExecuteMethod)
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

        public void RaiseCanExecuteChanged()
            => CommandManager.InvalidateRequerySuggested();
        //    => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public class DelegateCommand : DelegateCommand<object>
    {
        public DelegateCommand(Action executeMethod)
            : base(x => executeMethod())
        {
        }

        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : base(x => executeMethod(), x => canExecuteMethod())
        {
        }
    }

    public interface IAsyncCommand : IAsyncCommand<object> { }

    public interface IAsyncCommand<T>
    {
        Task ExecuteAsync(T obj);
        bool CanExecute(object obj);
    }

    public class AwaitableDelegateCommand<T> : IAsyncCommand<T>, ICommand
    {
        private Func<T, Task> executeMethod;
        private DelegateCommand<T> delegateCommand;
        private bool isExecuting;

        public AwaitableDelegateCommand(Func<T, Task> executeMethod)
            : this(executeMethod, _ => true)
        {
        }

        public AwaitableDelegateCommand(Func<T, Task> executeMethod, Predicate<T> canExecuteMethod)
        {
            this.executeMethod = executeMethod;
            delegateCommand = new DelegateCommand<T>(x => { }, canExecuteMethod);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
            => !isExecuting && delegateCommand.CanExecute((T)parameter);


        public async void Execute(object parameter)
            => await ExecuteAsync((T)parameter);

        public async Task ExecuteAsync(T obj)
        {
            try
            {
                isExecuting = true;
                delegateCommand.RaiseCanExecuteChanged();
                await executeMethod(obj);
            }
            finally
            {
                isExecuting = false;
                delegateCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public class AwaitableDelegateCommand : AwaitableDelegateCommand<object>
    {
        public AwaitableDelegateCommand(Func<Task> executeMethod)
            : base(o => executeMethod())
        {
        }

        public AwaitableDelegateCommand(Func<Task> executeMethod, Func<bool> canExecuteMethod)
            : base(o => executeMethod(), o => canExecuteMethod())
        {
        }
    }
}
