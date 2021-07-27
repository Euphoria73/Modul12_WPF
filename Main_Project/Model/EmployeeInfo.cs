using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Modul11_UI_HW.Model
{
    class EmployeeInfo
    {
        private ObservableCollection<Employee> ListEmployees = new ObservableCollection<Employee>();

        /// <summary>
        /// Возвращает список всех руководителей организации
        /// </summary>
        /// <param name="deps">департамент организации</param>
        /// <param name="countDivisions">количество подразделений(отделов) департамента</param>
        /// <returns></returns>
        public ObservableCollection<Employee> OutputAllManagers(ObservableCollection<Department> deps, int countDivisions)
        {
            for (int numberDepartment = 0; numberDepartment < deps.Count && numberDepartment <= countDivisions; ++numberDepartment)
            {
                foreach (Department department in deps)
                {
                    if (!ListEmployees.Contains(department.ManagerDepartment))
                    {
                        department.ManagerDepartment.Position = department.NameDepartment;
                        ListEmployees.Add(department.ManagerDepartment);
                    }
                }
                ListEmployees = OutputAllManagers(deps[numberDepartment].Departments, countDivisions - 1); //применяю рекурсию для прохода по всему дереву организации
            }
            return ListEmployees;
        }

        /// <summary>
        /// Возвращает список всех работников организации
        /// </summary>
        /// <param name="deps">департамент организации</param>
        /// <param name="countDivisions">количество подразделений(отделов) департамента</param>
        /// <returns></returns>
        public ObservableCollection<Employee> OutputAllWorkers(ObservableCollection<Department> deps, int countDivisions)
        {
            for (int numberDepartment = 0; numberDepartment < deps.Count && numberDepartment <= countDivisions; ++numberDepartment)
            {
                foreach (Department department in deps)
                {
                    foreach (var worker in department.Employees)
                    {
                        if (!ListEmployees.Contains(worker))
                        {
                            ListEmployees.Add(worker);
                        }
                    }
                }
                ListEmployees = OutputAllWorkers(deps[numberDepartment].Departments, countDivisions - 1); //применяю рекурсию для прохода по всему дереву организации
            }
            return ListEmployees;
        }
    }
}
