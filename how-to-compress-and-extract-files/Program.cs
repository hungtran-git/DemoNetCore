using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace how_to_compress_and_extract_files
{
    class Program
    {
        static void Main(string[] args)
        {
            string startPath = @".\start";
            string zipPath = @".\result.zip";
            string extractPath = @".\extract";

            if (File.Exists(zipPath)) File.Delete(zipPath);
            if (Directory.Exists(extractPath)) Directory.Delete(extractPath, true);

            ZipFile.CreateFromDirectory(startPath, zipPath);
            ZipFile.ExtractToDirectory(zipPath, extractPath);

            var memory = new MemoryStream();
            using (var stream = new FileStream(zipPath,FileMode.Open)) {
                stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var extension = Path.GetExtension(zipPath);
            var mimetype = GetMimeType()[extension];

            Console.ReadKey();
        }

        public static Dictionary<string, string> GetMimeType()
        {
            return new Dictionary<string, string>() {
                { ".zip", "application/zip" }
            };
        }
    }
}
