using System;
using System.Windows.Input;

namespace Modul12.Project.Commands
{
    class RelayCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        /// <summary>
        /// Контроль событий в WPF
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value; //вызываем метод Delegate.Combine для добавления нового метода в список вызовов
            remove => CommandManager.RequerySuggested -= value; //вызываем метод Delegate.Remove для удаления метода из списка вызовов. 
                                                                //Если список пустой, то присвоится null
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(Execute));
            this.canExecute = canExecute;
        }

        /// <summary>
        /// выполняет логику команды
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            execute(parameter);
        }

        /// <summary>
        /// определяет, может ли команда выполняться
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter) => canExecute?.Invoke(parameter) ?? true; //Если не указан делегат считаем что команду можно выполнить
                                                                                           //в любом случае
    }

}

