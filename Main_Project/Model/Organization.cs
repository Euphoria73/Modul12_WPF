﻿using System.Collections.ObjectModel;


namespace Modul12.Project.Model
{
    sealed class Organization
    {
        private Organization()
        {
        }
        /// <summary>
        /// Синглтон структуры организации
        /// </summary>
        /// <returns></returns>
        public static Organization GetInstance { get { return NestedOrganization.instance; } }

        /// <summary>
        /// вложенный класс для предотвращения преждевременного и неконтролируемого создания экземпляра класса Organization
        /// </summary>
        private class NestedOrganization
        {
            static NestedOrganization()
            {
            }
            internal static readonly Organization instance = new Organization();

        }

        /// <summary>
        /// Заполнение новой организации псевдослучайными данными
        /// </summary>
        /// <param name="deps"></param>
        public void Populate(ref ObservableCollection<Department> deps)
        {
            if (deps.Count == 0)
            {
                deps.Add(new Department
                (
                    "Default organization", 0
                ));
                PopulateOrganization(deps[0].Departments, "Department ", 0);
                foreach (var item in deps)
                {
                    deps[0].ManagerDepartment.Salary += item.ManagerDepartment.ManagerGetSalary(item);
                }
            }
            else return;
        }

        /// <summary>
        /// Заполнение структуры данными
        /// </summary>
        /// <param name="deps"></param>
        /// <param name="nameDepartment"></param>
        /// <param name="countDivisions"></param>
        private void PopulateOrganization(ObservableCollection<Department> deps, string nameDepartment, int countDivisions)
        {
            if (countDivisions == 0)
            {
                return;
            }
            else
            {
                for (int i = 0; i < countDivisions; i++)
                {
                    deps.Add(new Department(nameDepartment + $"{i + 1}", 2));

                    foreach (var item in deps)
                    {
                        item.ManagerDepartment.Salary += item.ManagerDepartment.ManagerGetSalary(item);
                    }

                    PopulateOrganization(deps[i].Departments, deps[i].NameDepartment + ".", countDivisions - 1);
                }
            }
        }

        /// <summary>
        /// Метод обновления зарплаты руководства
        /// </summary>
        /// <param name="organization"></param>
        public void RefreshSalary(ObservableCollection<Department> organization)
        {
            foreach (var item in organization) //пересчитываем зарплаты руководства
            {
                item.ManagerDepartment.ManagerGetSalary(item);
            }
        }


    }
}
