using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lifethreadening.Base
{
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

        private void InvokeCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
