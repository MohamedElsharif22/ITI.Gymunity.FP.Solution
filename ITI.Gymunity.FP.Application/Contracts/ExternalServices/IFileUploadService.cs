using Microsoft.AspNetCore.Http;

namespace ITI.Gymunity.FP.Application.Contracts.ExternalServices
{
    public interface IFileUploadService
    {
        Task<string> UploadImageAsync(IFormFile file, string folder);
        bool DeleteImage(string filePath);
        bool IsValidImageFile(IFormFile file);
    }
}
