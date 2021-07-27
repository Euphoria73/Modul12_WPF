using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Modul11_UI_HW.Model
{
    class EmployeeInfo
    {
        private ObservableCollection<Employee> ListEmployees = new ObservableCollection<Employee>();
        
        public ObservableCollection<Employee> OutputAllManagers(ObservableCollection<Department> deps, int countDivisions)
        {
            for (int numDep = 0; numDep < deps.Count && numDep <= countDivisions; ++numDep)
            {
                foreach (Department item in deps)
                {
                    if (!ListEmployees.Contains(item.ManagerDepartment))
                    {
                        item.ManagerDepartment.Position = item.NameDepartment;
                        ListEmployees.Add(item.ManagerDepartment);
                    }
                }
                ListEmployees = OutputAllManagers(deps[numDep].Departments, countDivisions - 1); //применяю рекурсию для прохода по всему дереву организации
            }
            return ListEmployees;
        }

        public ObservableCollection<Employee> OutputAllWorkers(ObservableCollection<Department> deps, int countDivisions)
        {
            for (int numDep = 0; numDep < deps.Count && numDep <= countDivisions; ++numDep)
            {
                foreach (Department item in deps)
                {
                    if (!ListEmployees.Contains(item.ManagerDepartment))
                    {
                        item.ManagerDepartment.Position = item.NameDepartment;
                        ListEmployees.Add(item.ManagerDepartment);
                    }
                }
                ListEmployees = OutputAllManagers(deps[numDep].Departments, countDivisions - 1); //применяю рекурсию для прохода по всему дереву организации
            }
            return ListEmployees;
        }
    }
}
