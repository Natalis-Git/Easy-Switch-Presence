
using System;
using System.Windows.Input;



namespace EasySwitchPresence
{

    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        
        private readonly Action _execute;  
        private readonly Func<bool> _canExecute;


        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }


        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);    
        }


        public bool CanExecute(object param)
        {
            return _canExecute == null ? true : _canExecute.Invoke();
        }


        public void Execute(object param)
        {
            _execute.Invoke();
        }
    }

}
