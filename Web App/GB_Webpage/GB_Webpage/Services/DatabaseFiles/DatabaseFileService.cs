using Newtonsoft.Json;
using System.Reflection;

namespace GB_Webpage.Services.DatabaseFiles
{
    public class DatabaseFileService : IDatabaseFileService
    {

        private readonly ILogger<DatabaseFileService> _logger;
        private readonly IConfiguration _configuration;

        public DatabaseFileService(ILogger<DatabaseFileService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        private string GetDirFiles(string folder)
        {
            string pathToDir = $"{Environment.CurrentDirectory}/{_configuration["DatabaseStorage:MainFolder"]}/{folder}";
            string[] fileEntries = null;

            try
            {
                fileEntries = Directory.GetFiles(pathToDir);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogFormatterService.FormatException(ex, MethodBase.GetCurrentMethod()?.Name));
            }

            if (fileEntries?.Length > 0)
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
            TextWriter writer = null;

            try
            {
                if (Directory.Exists(path))
                {
                    path = $"{path}/{DateTime.Now.ToString("dd-MM-yyyy")}.json";
                }

                if (File.Exists(path))
                {
                    File.Delete(path);
                    _logger.LogInformation(LogFormatterService.FormatAction(MethodBase.GetCurrentMethod()?.Name, "File already exist - file deleted", $"Path to file='{path}'"));
                }

                string jsonData = JsonConvert.SerializeObject(data);
                writer = new StreamWriter(path, false);
                writer.Write(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogFormatterService.FormatException(ex, MethodBase.GetCurrentMethod()?.Name));
                return false;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }

            _logger.LogInformation(LogFormatterService.FormatAction(MethodBase.GetCurrentMethod()?.Name, "File has been saved.", $"Path to file='{path}'"));
            return true;
        }

        public T? ReadFile<T>(string folderName)
        {

            string path = GetDirFiles(folderName);
            TextReader reader = null;

            try
            {
                if (Directory.Exists(path))
                {
                    return default;
                }

                reader = new StreamReader(path);
                var fileContents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(fileContents);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogFormatterService.FormatException(ex, MethodBase.GetCurrentMethod()?.Name));
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
