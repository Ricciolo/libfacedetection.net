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
        }

        [Fact]
        public void ResizeFace()
        {
            using var detector = new CnnFaceDetector();
            
            using (var bitmap = new Bitmap("faceh.jpg"))
            {
                var result = detector.Detect(bitmap, new Size(100, 100));
                Assert.Equal(1, result.Count);
            }

            using (var bitmap = new Bitmap("facev.jpg"))
            {
                var result = detector.Detect(bitmap, new Size(100, 100));
                Assert.Equal(1, result.Count);
            }
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
