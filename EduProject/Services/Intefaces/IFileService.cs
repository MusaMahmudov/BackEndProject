namespace EduProject.Services.Intefaces
{
    public interface IFileService
    {
        Task<string> CreateFileAsync(IFormFile file,string path);

         void DeteleFile(string path);
    }
}
