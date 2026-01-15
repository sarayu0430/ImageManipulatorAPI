using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using ImageConverterBackend.Models;
using ImageConverterBackend.Operations;
using ImageConverterBackend.Utilities;

namespace ImageConverterBackend.Services
{
    public class ImageProcessingService : IImageProcessingService
    {
        public async Task<string> ProcessAsync(ImageOperationDto request)
        {
            using var stream = request.Image.OpenReadStream();
            using var image = await Image.LoadAsync(stream);

            IImageOperation operation = request.Operation switch
            {
                ImageOperationType.Resize => new ResizeOperation(request.Width.Value, request.Height.Value),
                ImageOperationType.Grayscale => new GrayscaleOperation(),
                ImageOperationType.Rotate => new RotateOperation(request.Angle.Value),
                ImageOperationType.Watermark => new WatermarkOperation(request.WatermarkText),
                ImageOperationType.Compress => new CompressOperation(request.Quality.Value),
                _ => throw new Exception("Invalid operation")
            };
            operation.Apply(image);

            using var output = new MemoryStream();
            await image.SaveAsync(output, new PngEncoder());

            var id = image.GetHashCode().ToString();

            InMemoryImageStore.SaveImage(image, id);

            return id;
        }
        public byte[] GetImage(string id)
        {
            var storedImage = InMemoryImageStore.GetImageInMemoryById(id);
            if (storedImage is null)
            {
                return null;
            }

            using var output = new MemoryStream();
            storedImage.Save(output, new PngEncoder());
            return output.ToArray();
        }

        public ImageMetaData GetMetadata(string id)
        {
            Image storedImage = InMemoryImageStore.GetImageInMemoryById(id);
            if (storedImage is null)
            {
                return null;
            }
            using var ms = new MemoryStream();
            storedImage.Save(ms, new PngEncoder());
            double sizeKb = ms.Length / 1024.0;
            return new ImageMetaData()
            {
                Id = id,
                Width = storedImage.Width,
                Height = storedImage.Height,
                SizeKb = sizeKb,
            };
        }

        public void Delete(string id)
        {
            InMemoryImageStore.DeleteImageInMemoryById(id);
        }
    }

}
