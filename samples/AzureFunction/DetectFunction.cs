using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using LibFaceDetection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;

namespace AzureFunction
{
    public static class DetectFunction
    {
        private static readonly ObjectPool<CnnFaceDetector> FaceDetectors;
        private static readonly IMemoryCache Cache = new MemoryCache(new MemoryCacheOptions { ExpirationScanFrequency = TimeSpan.FromSeconds(30) });
        private static readonly TimeSpan Duration = TimeSpan.FromMinutes(10);
        private const int MaxRequests = 5;

        static DetectFunction()
        {
            FaceDetectors = new DefaultObjectPoolProvider
            {
                MaximumRetained = Environment.ProcessorCount * 10
            }
                .Create<CnnFaceDetector>();
        }

        [FunctionName("Detect")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            // Check how many requests received from the same IP
            if (!ThrottleIp(req.HttpContext.Connection.RemoteIpAddress))
            {
                return new StatusCodeResult(429);
            }

            // Read image from request
            using var bitmap = new Bitmap(req.Body);

            CnnFaceDetector detector = null;
            try
            {
                // Get a detector from pool
                detector = FaceDetectors.Get();

                // Detect faces
                IReadOnlyList<CnnFaceDetected> result = detector.Detect(bitmap, new Size(200, 200));
                log.LogInformation("Found {faces} faces", result.Count);

                return new OkObjectResult(result);
            }
            finally
            {
                if (detector != null) FaceDetectors.Return(detector);
            }
        }

        private static bool ThrottleIp(IPAddress ipAddress)
        {
            // Read from cache
            int count = Cache.GetOrCreate(ipAddress, e =>
            {
                e.SetSlidingExpiration(Duration);
                return 0;
            });
            count++;
            Cache.Set(ipAddress, count, Duration);

            // Cannot call again
            if (count > MaxRequests)
            {
                return false;
            }

            return true;
        }
    }
}
