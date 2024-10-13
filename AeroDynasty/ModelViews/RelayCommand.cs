using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AeroDynasty.ModelViews
{
    // RelayCommand class implementing the ICommand interface
    // This allows the use of commands in WPF and other XAML-based applications
    public class RelayCommand : ICommand
    {
        // Fields to store the action to execute and the condition to check if it can be executed
        private readonly Action<object> _execute;              // The action to execute when the command is invoked
        private readonly Func<object, bool> _canExecute;       // The condition to determine if the command can be executed

        // Constructor accepting the action and an optional condition
        // 'canExecute' is optional, and defaults to always true if not provided
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;                               // Assign the action to execute
            _canExecute = canExecute ?? (p => true);          // Default 'canExecute' to always return true if not provided
        }

        // Overload constructor for parameterless commands
        public RelayCommand(Action execute, Func<bool> canExecute = null)
            : this(param => execute(), canExecute == null ? (Func<object, bool>)null : param => canExecute())
        {
        }

        // Determines if the command can execute by evaluating the 'canExecute' function
        // If 'canExecute' is null, the command can always execute
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        // Executes the action with the provided parameter when the command is invoked
        public void Execute(object parameter)
        {
            _execute(parameter);                              // Call the stored action, passing the provided parameter
        }

        // Event to handle automatic updates for when the executable state of the command changes
        // CommandManager.RequerySuggested raises this event when WPF re-evaluates the CanExecute state
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }  // Subscribe to the CommandManager's requery event
            remove { CommandManager.RequerySuggested -= value; } // Unsubscribe from the requery event
        }
    }

}
