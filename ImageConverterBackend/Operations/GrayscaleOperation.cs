using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImageConverterBackend.Operations
{
    public class GrayscaleOperation : IImageOperation
    {
        public void Apply(Image image)
        {
            image.Mutate(x => x.Grayscale());
        }
    }
}
