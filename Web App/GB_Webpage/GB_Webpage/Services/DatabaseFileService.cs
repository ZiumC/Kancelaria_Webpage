using GB_Webpage.Models;
using Newtonsoft.Json;

namespace GB_Webpage.Services
{
    public class DatabaseFileService
    {

        private readonly string _path;

        public DatabaseFileService()
        {
            _path = "";
        }

        public bool SaveFile<T>(T data)
        {

            string jsonData = JsonConvert.SerializeObject(data);

            if (File.Exists(_path))
            {
                File.Delete(_path);
            }

            TextWriter writer = null;
            try
            {
                writer = new StreamWriter(_path, false);
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
