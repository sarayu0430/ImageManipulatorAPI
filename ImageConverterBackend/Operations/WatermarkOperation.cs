using SixLabors.Fonts;
using SixLabors.ImageSharp.PixelFormats;
using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace ImageConverterBackend.Operations
{
    public class WatermarkOperation : IImageOperation
    {
        private readonly string _text;
        private readonly float _fontSize;

        public WatermarkOperation(string text, float fontSize = 40f)
        {
            _text = text ?? throw new ArgumentNullException(nameof(text));
            _fontSize = Math.Max(20f, fontSize);
        }

        public void Apply(Image image)
        {
            if (image is null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            if (string.IsNullOrWhiteSpace(_text))
            {
                return;
            }

            var font = SystemFonts.CreateFont("Arial", _fontSize);

            // Measure the text using TextOptions (avoids RendererOptions)
            var textSize = TextMeasurer.MeasureBounds(_text, new TextOptions(font));

            // Calculate padding proportional to font size, ensure a minimum padding.
            float padding = MathF.Max(8f, _fontSize * 0.25f);

            // Position at bottom-right while staying inside the image bounds.
            float x = image.Width - textSize.Width - padding;
            float y = image.Height - textSize.Height - padding;

            if (x < padding) x = padding;
            if (y < padding) y = padding;

            var shadowColor = new Color(new Rgba32(0, 0, 0, 160));    // semi-transparent black
            var textColor = new Color(new Rgba32(255, 255, 255, 220)); // mostly opaque white

            image.Mutate(ctx =>
            {
                // subtle shadow for readability
                ctx.DrawText(_text, font, shadowColor, new PointF(x + 2, y + 2));
                ctx.DrawText(_text, font, textColor, new PointF(x, y));
            });
        }
    }
}