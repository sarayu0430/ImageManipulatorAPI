using SixLabors.ImageSharp;

namespace ImageConverterBackend.Operations
{
    public interface IImageOperation
    {
        void Apply(Image image);
    }
}
