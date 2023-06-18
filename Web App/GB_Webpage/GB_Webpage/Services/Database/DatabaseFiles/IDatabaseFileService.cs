namespace GB_Webpage.Services.Database.DatabaseFiles
{
    public interface IDatabaseFileService
    {
        public bool SaveFile<T>(T data, string folderName);
        public T? ReadFile<T>(string folderName);
    }
}
