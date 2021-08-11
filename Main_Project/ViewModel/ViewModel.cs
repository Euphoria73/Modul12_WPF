using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Modul12.Project.Commands;
using Modul12.Project.Model;

namespace Modul12.Project.ViewModel
{
    class ViewModel : ViewModelBase
    {

        private string _title = "";

        /// <summary>
        /// Заголовок главного окна
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private Department selectedItem;

        /// <summary>
        /// Выбранный департамент в treeview
        /// </summary>
        public Department SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }

        private static ObservableCollection<Department> _myOrganization = new ObservableCollection<Department>();

        private readonly Organization structure = Organization.GetInstance; //использую lazy-синглтон структуры, т.к. в один момент времени мы можем работать только с одной организацией

        private readonly OrganizationSource organizationSource = OrganizationSource.GetInstance;

        private readonly DialogService dialog = new DialogService();
        /// <summary>
        /// Коллекция организации
        /// </summary>
        public ObservableCollection<Department> GetOrganization
        {
            get
            {
                return _myOrganization;
            }

            set => SetProperty(ref _myOrganization, value);
        }

        #region Команды управления программой


        #region Команда для создания новой организации
        public ICommand CreateCommand { get; }

        private bool CanCreateCommandExecute(object file) => true;

        //Создание структуры компании 
        public void OnCreateCommandExecuted(object file)
        {
            try
            {               
                structure.Populate(ref _myOrganization);
            }
            catch (Exception e)
            {
                dialog.ShowMessage($"Обратитесь в службу поддержки. Код ошибки {e.Message}");
            }            
        }
        #endregion

        #region Команда открытия базы данных организации в формате JSON
        public ICommand OpenCommand { get; } //Команда для открытия организации из файла

        private bool CanOpenCommandExecute(object path) => true;

        //для открытия записи организации в формате JSON
        public void OnOpenCommandExecuted(object path)
        {
            try
            {
                GetOrganization = organizationSource.OpenFromJSONFile();
            }
            catch (NullReferenceException)
            {
                dialog.ShowMessage("Обратитесь в службу поддержки");
            }
        }

        #endregion

        #region Команда кнопки открытия списка руководителей

        /// <summary>
        /// Привязка списка руководителей к ListManagersWindow
        /// </summary>
        public ObservableCollection<Employee> GetManagers
        {
            get
            {
                EmployeeInfo managers = new EmployeeInfo();
                return managers.OutputAllManagers(_myOrganization, _myOrganization[0].Departments.Count);
            }
        }

        public ICommand GetManagersCommand { get; }
        private bool CanGetManagersCommandExecute(object path) => path is Department; //делаю проверку привязки к SelectedItem в MainWindow

        public void OnGetManagersCommand(object path)
        {
            try
            {
                View.ListManagersWindow windowManagers = new View.ListManagersWindow(); //открываем окно со списком
                windowManagers.Title = "Список Руководителей";
                windowManagers.ShowDialog();
            }
            catch (Exception e)
            {

                dialog.ShowMessage($"Ошибка {e}, Обратитесь в службу поддержки");
            }
        }

        #endregion

        #region Команда кнопки открытия списка работников

        /// <summary>
        /// Привязка списка работников к ListWorkersWindow
        /// </summary>
        public ObservableCollection<Employee> GetWorkers
        {
            get
            {
                EmployeeInfo workers = new EmployeeInfo();
                return workers.OutputAllWorkers(_myOrganization, _myOrganization[0].Departments.Count);
            }
        }

        public ICommand GetWorkersCommand { get; }
        private bool CanGetWorkersCommandExecute(object path) => path is Department; //делаю проверку привязки к SelectedItem в MainWindow

        public void OnGetWorkersCommand(object path)
        {
            try
            {
                View.ListWorkersWindow windowManagers = new View.ListWorkersWindow();
                windowManagers.Title = "Список Работников";
                windowManagers.ShowDialog();
            }
            catch (Exception e)
            {

                dialog.ShowMessage($"Ошибка {e}, Обратитесь в службу поддержки");
            }
        }

        #endregion

        #region Команда сохранения организации в файл
        public ICommand SaveCommand { get; }

        private bool CanSaveCommandExecute(object path)
        {
            string f_text = organizationSource.SerializeToJson(GetOrganization);
            if (f_text == null || f_text == "[]") return false; //проверяем, что структура не пустая
            return true;
        }

        //Сохранение организации в формате JSON
        public void OnSaveCommandExecutedAsync(object path)
        {
            organizationSource.SaveToJsonFile(path, GetOrganization);
        }

        #endregion

        #region Команда добавления сотрудника в выбранный департамент
        public ICommand AddEmployeeCommand { get; }

        private bool CanAddEmployeeCommandExecute(object path) => path is Department;

        public void OnAddEmployeeCommandExecuted(object path)
        {
            try
            {
                int numAddingEmployees = 1; //по умолчанию добавляет 1 сотрудника. TODO: можно добавить вывод текстбокса для ввода требуемого пользователем числа добавляемых сотрудников
                SelectedItem.AddNewEmployee(numAddingEmployees);
                structure.RefreshSalary(_myOrganization);
                dialog.ShowMessage("Работник успешно добавлен!");
            }
            catch (NullReferenceException)
            {
                dialog.ShowMessage("Выберите департамент, в который хотите добавить работника");
            }
            catch (Exception e)
            {
                dialog.ShowMessage($"Ошибка: {e.Message} => {e.GetType()}, передайте информацию в отдел технической поддержки для исправления");
            }
        }
        #endregion     

        #region Команда добавления нового департамента в выбранный департамент
        public ICommand AddDepartmentCommand { get; }

        private bool CanAddDepartmentCommandExecute(object path) => path is Department;

        //для открытия записи организации в формате JSON
        public void OnAddDepartmentCommandExecuted(object path)
        {
            try
            {
                if (selectedItem == _myOrganization[0])
                {
                    selectedItem.AddDepartment(new Department($"Департамент {selectedItem.Departments.Count + 1}", 2));
                }
                else
                {
                    int lastNumberDepartment = selectedItem.Departments.Count + 1;
                    selectedItem.AddDepartment(new Department($"{selectedItem.NameDepartment + "." + lastNumberDepartment}", 3));
                }
                structure.RefreshSalary(_myOrganization);
            }
            catch (NullReferenceException)
            {
                dialog.ShowMessage("Выберите родительский департамент, в который хотите добавить новый департамент");
            }
            catch (Exception e)
            {
                dialog.ShowMessage($"Ошибка: {e.Message} => {e.GetType()}, передайте информацию в отдел технической поддержки для исправления");
            }
        }

        #endregion

        #region Команда удаления департамента из организации
        public ICommand DeleteDepartmentCommand { get; }

        private bool CanDeleteDepartmentCommandExecute(object path) => path is Department;

        public void OnDeleteDepartmentCommandExecuted(object path)
        {
            //главный департамент не трогаем/запрет на удаление
            if (selectedItem.ParentDepartment == null)
            {
                return;
            }
            selectedItem.ParentDepartment.Departments.Remove(selectedItem); //использую встроенный метод удаления департамента
            structure.RefreshSalary(_myOrganization);
        }

        #endregion

        #endregion

        /// <summary>
        /// Модель представления для связи между визуальной частью и моделью
        /// </summary>
        public ViewModel()
        {
            CreateCommand = new RelayCommand(OnCreateCommandExecuted, CanCreateCommandExecute);
            OpenCommand = new RelayCommand(OnOpenCommandExecuted, CanOpenCommandExecute);
            SaveCommand = new RelayCommand(OnSaveCommandExecutedAsync, CanSaveCommandExecute);
            AddEmployeeCommand = new RelayCommand(OnAddEmployeeCommandExecuted, CanAddEmployeeCommandExecute);
            AddDepartmentCommand = new RelayCommand(OnAddDepartmentCommandExecuted, CanAddDepartmentCommandExecute);
            DeleteDepartmentCommand = new RelayCommand(OnDeleteDepartmentCommandExecuted, CanDeleteDepartmentCommandExecute);
            GetManagersCommand = new RelayCommand(OnGetManagersCommand, CanGetManagersCommandExecute);
            GetWorkersCommand = new RelayCommand(OnGetWorkersCommand, CanGetWorkersCommandExecute);
        }


    }
}
