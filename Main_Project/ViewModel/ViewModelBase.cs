using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Modul12.Project.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Событие, обновляющее информацию в View при изменении в Model
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyCanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// запускает PropertyChanged событие при изменении значения свойства и возвращает true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage">записанное значение</param>
        /// <param name="value">новое значение</param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            //если значение не изменилось, то PropertyChanged событие не запускает и возвращает false
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }
            storage = value;
            OnPropertyCanged(propertyName);
            return true;
        }
    }
}
