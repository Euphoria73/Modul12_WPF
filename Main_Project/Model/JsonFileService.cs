using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Modul12.Project.Model
{
    interface IFileService
    {
        ObservableCollection<Department> Open(string fileName);
        void Save(string fileName, ObservableCollection<Department> departmentList);
    }

    class JsonFileService : IFileService
    {
        public ObservableCollection<Department> Open(string fileName)
        {
            ObservableCollection<Department> departments = new ObservableCollection<Department>();
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(ObservableCollection<Department>));
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                departments = jsonFormatter.ReadObject(fs) as ObservableCollection<Department>;
            }
            return departments;
        }

        public void Save(string fileName, ObservableCollection<Department> departmentList)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(ObservableCollection<Department>));
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, departmentList);
            }
        }
    }
}
