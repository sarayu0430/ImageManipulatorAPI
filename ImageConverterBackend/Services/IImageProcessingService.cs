using SixLabors.ImageSharp.Metadata;
using ImageConverterBackend.Models;

namespace ImageConverterBackend.Services
{
    public interface IImageProcessingService
    {
        Task<string> ProcessAsync(ImageOperationDto request);
        byte[] GetImage(string id);
        ImageMetaData GetMetadata(string id);
        void Delete(string id);
    }
}
