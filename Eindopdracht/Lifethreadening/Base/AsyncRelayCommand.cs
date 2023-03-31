using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lifethreadening.Base
{
    /// <summary>
    /// This class stores a async command to be executed as a command
    /// </summary>
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<Task> execute;
        private bool _isExecuting;

        private bool isExecuting
        {
            get
            {
                return _isExecuting;
            }
            set
            {
                _isExecuting = value;
                InvokeCanExecuteChanged();
            }
        }

        /// <summary>
        /// Creates an async relay command
        /// </summary>
        /// <param name="execute">The function to use as the command</param>
        public AsyncRelayCommand(Func<Task> execute)
        {
            this.execute = execute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return !isExecuting;
        }

        public async void Execute(object parameter)
        {
            await ExecuteAsync();
        }
        
        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns>The task that executes the command</returns>
        public async Task ExecuteAsync()
        {
            try
            {
                isExecuting = true;
                await execute();
            }
            finally
            {
                isExecuting = false;
            }
        }

        /// <summary>
        /// Changes the status of the invokability of this command
        /// </summary>
        private void InvokeCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
