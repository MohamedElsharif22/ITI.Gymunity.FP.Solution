using Microsoft.AspNetCore.Http;

namespace ITI.Gymunity.FP.Application.Contracts.ExternalServices
{
    public interface IFileUploadService
    {
        const string UserProfilePhotosFolder = "images/profile-photos";
        Task<string> UploadImageAsync(IFormFile file, string folder);
        bool DeleteImage(string filePath);
        bool IsValidImageFile(IFormFile file);
    }
}
