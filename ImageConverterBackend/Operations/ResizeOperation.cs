using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImageConverterBackend.Operations
{
    public class ResizeOperation : IImageOperation
    {
        private readonly int _width;
        private readonly int _height;

        public ResizeOperation(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public void Apply(Image image)
        {
            image.Mutate(x => x.Resize(_width, _height));
        }
    }
}
