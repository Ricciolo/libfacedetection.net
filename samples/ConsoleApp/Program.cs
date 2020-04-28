using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using LibFaceDetection;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter path to an image to load: ");
            string path = Console.ReadLine();

            // Load bitmap from path
            IReadOnlyList<CnnFaceDetected> result;
            using (var bitmap = new Bitmap(path))
            {
                // Detect faces
                using var detector = new CnnFaceDetector();
                result = detector.Detect(bitmap);
            }

            // Apply rectangles to faces
            using var image = Image.FromFile(path);
            using var graphics = Graphics.FromImage(image);
            foreach (var faceDetected in result)
            {
                graphics.DrawRectangle(new Pen(Color.Red, 2), faceDetected.Rectangle);
                graphics.DrawString((faceDetected.Confidence * 100).ToString("0"), SystemFonts.DefaultFont, Brushes.Red, faceDetected.Rectangle);
            }

            // Save new image
            string output = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")) + ".png";
            image.Save(output, ImageFormat.Png);

            // Open image
            Process.Start(new ProcessStartInfo(output) { UseShellExecute = true });
        }
    }
}
