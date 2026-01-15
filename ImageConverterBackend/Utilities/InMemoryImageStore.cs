using SixLabors.ImageSharp.Formats.Png;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

namespace ImageConverterBackend.Utilities
{
    public static class InMemoryImageStore
    {
        private static readonly string StorageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Storage");

        public static void SaveImage(Image image, string id)
        {
            if (image is null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("Id must be provided", nameof(id));
            }

            Directory.CreateDirectory(StorageFolder);

            var filePath = Path.Combine(StorageFolder, id + ".png");

            // Save synchronously to avoid holding the incoming Image reference — caller may dispose it.
            using var fs = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            image.Save(fs, new PngEncoder());
        }

        public static IEnumerable<Image> GetAllImagesInMemory()
        {
            if (!Directory.Exists(StorageFolder))
            {
                return Enumerable.Empty<Image>();
            }

            var files = Directory.EnumerateFiles(StorageFolder, "*.png");
            var images = new List<Image>();
            foreach (var f in files)
            {
                try
                {
                    images.Add(Image.Load(f));
                }
                catch
                {
                    // skip corrupted/unloadable files
                }
            }

            return images;
        }

        public static Image GetImageInMemoryById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var filePath = Path.Combine(StorageFolder, id + ".png");
            if (!File.Exists(filePath))
            {
                return null;
            }

            return Image.Load(filePath);
        }

        public static void DeleteImageInMemoryById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return;
            }

            var filePath = Path.Combine(StorageFolder, id + ".png");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}