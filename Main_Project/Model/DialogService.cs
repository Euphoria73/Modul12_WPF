using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace Modul11_UI_HW.Model
{
    public interface IDialogService
    {
        /// <summary>
        /// Путь к файлу
        /// </summary>
        string FilePath { get; set; }
        /// <summary>
        /// Открытие файла
        /// </summary>
        /// <returns></returns>
        bool OpenFileDialog();
        /// <summary>
        /// Сохранение файла
        /// </summary>
        /// <returns></returns>
        bool SaveFileDialog();
        /// <summary>
        /// Вывод сообщения
        /// </summary>
        /// <param name="message"></param>
        void ShowMessage(string message);
    }

    public class DialogService : IDialogService
    {
        public string FilePath { get; set; }
        public bool OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }
            return false;
        }

        public bool SaveFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
                return true;
            }
            return false;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
