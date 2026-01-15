using System.Text.Json.Serialization;

namespace ImageConverterBackend.Models
{
    public class ImageOperationDto
    {
        public IFormFile Image { get; set; }
        public ImageOperationType Operation { get; set; }   // resize, grayscale, rotate, watermark
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? Angle { get; set; }
        public string? WatermarkText { get; set; }

        public int? Quality { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ImageOperationType
    {
        [JsonPropertyName("resize")]
        Resize,
        [JsonPropertyName("grayscale")]
        Grayscale,
        [JsonPropertyName("rotate")]
        Rotate,
        [JsonPropertyName("watermark")]
        Watermark,
        [JsonPropertyName("compress")]
        Compress
    }
}
