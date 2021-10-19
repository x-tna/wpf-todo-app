using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ToDo.Commands
{
    class ActionCommand : ICommand
    {
        private Action _Execute;
        private Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public ActionCommand(Action Execute, Func<bool> CanExecute)
        {
            _Execute = Execute;
            _canExecute = CanExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute.Invoke();
        }

        public void Execute(object parameter)
        {
            _Execute.Invoke();
        }

        public void RaisCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
