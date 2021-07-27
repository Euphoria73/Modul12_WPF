using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;

namespace Modul12.Project.Model
{
    interface IOrganizationSource<T>
    {
        T OpenFromJSONFile();
        string SerializeToJson(T organization);
        void SaveToJsonFile(object fileName, T organization);
    }

    class OrganizationSource : IOrganizationSource<ObservableCollection<Department>>
    {
        private OrganizationSource()
        {
        }

        public static OrganizationSource GetInstance { get { return NestedOrganizationSource.instance; } }

        /// <summary>
        /// вложенный класс для предотвращения преждевременного создания экземпляра класса OrganizationSource
        /// </summary>
        private class NestedOrganizationSource
        {
            static NestedOrganizationSource()
            {
            }

            internal static readonly OrganizationSource instance = new OrganizationSource();
        }

        /// <summary>
        /// Метод открытия файла с сохраненной структурой организации
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Department> OpenFromJSONFile()
        {
            var dialog = new DialogService();
            //var dlg = new OpenFileDialog
            //{
            //    Title = "Открыть файл",
            //    Filter = "Файл json (*.json)|*.json",
            //    InitialDirectory = Environment.CurrentDirectory,
            //    RestoreDirectory = true
            //};
            if (dialog.OpenFileDialog() == false) return new ObservableCollection<Department>();

            var file = dialog.FilePath;

            using StreamReader reader = File.OpenText(file);
            var fileText = reader.ReadToEnd();
            var deserializeJson = JsonConvert.DeserializeObject<ObservableCollection<Department>>(fileText);
            return deserializeJson;
        }

        /// <summary>
        /// Сериализация структуры организации
        /// </summary>
        /// <param name="organization">организация</param>
        /// <returns></returns>
        public string SerializeToJson(ObservableCollection<Department> organization)
        {
            string text = JsonConvert.SerializeObject(organization, Formatting.Indented,
                              new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return text;
        }

        /// <summary>
        /// Метод сохранения структуры организации в Json файл 
        /// </summary>
        /// <param name="fileName">имя файла</param>
        /// <param name="organization">организация</param>
        public async void SaveToJsonFile(object fileName, ObservableCollection<Department> organization)
        {
            string f_text = SerializeToJson(organization);

            var f_name = fileName as string;

            if (f_name == null)
            {
                var dialog = new DialogService();
                //var dialog = new SaveFileDialog
                //{
                //    Title = "Сохранение файла",
                //    Filter = "Файл json (*.json)|*.json",
                //    InitialDirectory = Environment.CurrentDirectory,
                //    RestoreDirectory = true
                //};

                if (dialog.SaveFileDialog() != true) return;
                f_name = dialog.FilePath;
            }

            using var writer = new StreamWriter(new FileStream(f_name, FileMode.Create, FileAccess.Write));
            await writer.WriteAsync(f_text).ConfigureAwait(false);
        }
    }
}
