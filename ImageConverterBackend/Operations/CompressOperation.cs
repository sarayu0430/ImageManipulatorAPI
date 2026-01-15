using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImageConverterBackend.Operations
{
    public class CompressOperation : IImageOperation
    {
        private readonly int _quality;

        public CompressOperation(int quality)
        {
            // ensure quality is in a sane range 1..100
            _quality = Math.Clamp(quality, 1, 100);
        }

        public void Apply(Image image)
        {
            if (image is null)
            {
                throw new ArgumentNullException(nameof(image));
            }


            // Quality here controls an aggressive but safe downscale.
            // 100 => no change, lower values reduce dimensions which typically reduces file size.
            if (_quality >= 100)
            {
                return;
            }

            // Keep a sensible minimum scale to avoid producing a 1x1 image accidentally.
            const float minScale = 0.25f;

            // Map quality (1..100) to scale (minScale..1.0)
            float scale = minScale + (_quality / 100f) * (1f - minScale);

            if (scale < 1f)
            {
                int newWidth = Math.Max(1, (int)Math.Round(image.Width * scale));
                int newHeight = Math.Max(1, (int)Math.Round(image.Height * scale));

                image.Mutate(ctx => ctx.Resize(newWidth, newHeight));
            }
        }
    }

}
