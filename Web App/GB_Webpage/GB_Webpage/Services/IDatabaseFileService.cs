namespace GB_Webpage.Services
{
    public interface IDatabaseFileService
    {
        public bool SaveFile<T>(T data, string folderName);
        public T? ReadFile<T>(string folderName);
    }
}
