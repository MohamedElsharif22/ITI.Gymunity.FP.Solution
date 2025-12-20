using Microsoft.AspNetCore.Http;

namespace ITI.Gymunity.FP.Infrastructure.Contracts.ExternalServices
{
    public interface IFileUploadService
    {
        const string UserProfilePhotosFolder = "profile-photos";
        Task<string> UploadImageAsync(IFormFile file, string folder);
        bool DeleteImage(string filePath);
        bool IsValidImageFile(IFormFile file);
    }
}
