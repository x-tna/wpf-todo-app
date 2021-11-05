using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ToDo.Commands
{
    public class ParameterCommand<TParam> : ICommand
    {
        private Action<TParam> _execute;
        private Predicate<TParam> _canExecute;

        public event EventHandler CanExecuteChanged;

        public ParameterCommand(Action<TParam> execute, Predicate<TParam> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;

        }

        public bool CanExecute(object parameter)
        {
            if (parameter is TParam todoItem)
            {
                return _canExecute.Invoke(todoItem);
            }
            else return false;
        }

        public void Execute(object parameter)
        {
            if(parameter is TParam todoItem)
            {
                _execute.Invoke(todoItem);
            }
            
        }

        public void RaisCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
