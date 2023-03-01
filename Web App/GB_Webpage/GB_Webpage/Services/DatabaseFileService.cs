using Newtonsoft.Json;

namespace GB_Webpage.Services
{
    public class DatabaseFileService
    {

        private readonly string _pathToDir;

        public DatabaseFileService()
        {
            _pathToDir = $"{Environment.CurrentDirectory}/DatabaseFile";
        }

        private string GetDirFiles()
        {
            string[] fileEntries = Directory.GetFiles(_pathToDir);

            if (fileEntries.Length > 0)
            {
                return fileEntries[0];
            }
            else
            {
                return _pathToDir;
            }

        }

        public bool SaveFile<T>(T data)
        {

            string path = GetDirFiles();

            if (Directory.Exists(path))
            {
                path = $"{path}/{DateTime.Now.ToString("dd-MM-yyyy")}.json";
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            TextWriter writer = null;
            try
            {
                string jsonData = JsonConvert.SerializeObject(data);
                writer = new StreamWriter(path, false);
                writer.Write(jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }

            return true;
        }

    }
}
