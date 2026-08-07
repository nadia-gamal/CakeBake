using Microsoft.AspNetCore.Http;

namespace CakeBake.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string?> UploadImageAsync(IFormFile file);
    }
}