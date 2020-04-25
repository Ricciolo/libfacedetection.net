using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Xunit;
using Xunit.Abstractions;

namespace LibFaceDetection.Test
{
    public class CnnFaceDetectorTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public CnnFaceDetectorTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void MultipleFaces()
        {
            using var bitmap = new Bitmap("faces.jpg");

            using var detector = new CnnFaceDetector();

            var stopwatch = Stopwatch.StartNew();
            IReadOnlyList<CnnFaceDetected> result = detector.Detect(bitmap);
            stopwatch.Stop();

            _testOutputHelper.WriteLine("Elapsed {0}", stopwatch.Elapsed.ToString());

            Assert.Equal(49, result.Count);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                foreach (var faceDetected in result)
                {
                    graphics.DrawRectangle(new Pen(Color.Red, 2), faceDetected.Rectangle);
                    graphics.DrawString((faceDetected.Confidence * 100).ToString("0"), SystemFonts.DefaultFont, Brushes.Red, faceDetected.Rectangle);
                }
            }

            bitmap.Save("faces_out.jpg");
        }

        [Fact]
        public void Benchmark()
        {
            using var bitmap = new Bitmap("faces.jpg");

            using var detector = new CnnFaceDetector();

            var stopwatch = Stopwatch.StartNew();
            for (int x = 0; x < 10; x++)
            {
                IReadOnlyList<CnnFaceDetected> result = detector.Detect(bitmap);
            }

            stopwatch.Stop();

            _testOutputHelper.WriteLine("Elapsed {0}. {1:0.#} per second", stopwatch.Elapsed.ToString(), stopwatch.Elapsed.TotalSeconds / 100);
        }
    }
}
