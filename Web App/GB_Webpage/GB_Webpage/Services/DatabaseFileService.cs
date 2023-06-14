using Newtonsoft.Json;

namespace GB_Webpage.Services
{
    public class DatabaseFileService : IDatabaseFileService
    {

        private readonly ILogger<DatabaseFileService> _logger;

        public DatabaseFileService(ILogger<DatabaseFileService> logger)
        {
            _logger = logger;
        }

        private string GetDirFiles(string folder)
        {
            string pathToDir = $"{Environment.CurrentDirectory}/DatabaseFiles/{folder}";
            string[] fileEntries = Directory.GetFiles(pathToDir);

            if (fileEntries.Length > 0)
            {
                return fileEntries[0];
            }
            else
            {
                return pathToDir;
            }

        }

        public bool SaveFile<T>(T data, string folderName)
        {

            string path = GetDirFiles(folderName);

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
                //writer = new StreamWriter(null, false);
                writer.Write(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogFormatterService.FormatException(ex));
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

        public T? ReadFile<T>(string folderName)
        {

            string path = GetDirFiles(folderName);

            if (Directory.Exists(path))
            {
                return default;
            }

            TextReader reader = null;
            try
            {
                reader = new StreamReader(path);
                var fileContents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(fileContents);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

        }
    }
}
