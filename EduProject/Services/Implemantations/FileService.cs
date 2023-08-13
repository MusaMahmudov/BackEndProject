using EduProject.Exceptions;
using EduProject.Services.Intefaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EduProject.Services.Implemantations
{
    public class FileService : IFileService
    {
        public async Task<string> CreateFileAsync(IFormFile file, string path)
        {
            if (!file.ContentType.Contains("image/"))
            {
                throw new FileTypeException("Only Images");
            }
            if(file.Length / 1024 > 600)
            {
                throw new FileSizeException("Size too much");
            }
            string FileName = $"{Guid.NewGuid()}-{file.FileName}";
            string ResultPath = Path.Combine(path, FileName);
            using (FileStream fileStream = new FileStream(ResultPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return FileName;

        }

        public  void DeteleFile(string path)
        {
            if(System.IO.File.Exists(path))
            {
                 System.IO.File.Delete(path);
            }
        }
    }
}
