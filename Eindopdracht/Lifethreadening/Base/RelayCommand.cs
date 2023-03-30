using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lifethreadening.Base
{
    /// <summary>
    /// The ralay command is used to store a function as a command
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action _execute;

        /// <summary>
        /// Creates a new Relaycommand
        /// </summary>
        /// <param name="execute">The function the command wil execute</param>
        public RelayCommand(Action execute)
        {
            _canExecute = null;
            _execute = execute;
        }

        /// <summary>
        /// Creates a new Relaycommand with a value indicating wether it is executable or not
        /// </summary>
        /// <param name="execute">The function the command wil execute</param>
        /// <param name="canExecute">Wether the function can be executed</param>
        public RelayCommand(Action execute, Predicate<object> canExecute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute.Invoke();
        }
    }
}
