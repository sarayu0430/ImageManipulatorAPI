using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImageConverterBackend.Operations
{
    public class RotateOperation : IImageOperation
    {
        private readonly int _angle;

        public RotateOperation(int angle)
        {
            _angle = angle;
        }

        public void Apply(Image image)
        {
            image.Mutate(x => x.Rotate(_angle));
        }
    }
}
