using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace LibFaceDetection
{
    /// <summary>
    /// Detector which uses Convolutional Neural Networks in order to detect faces into an image
    /// </summary>
    /// <remarks>To improve performances, please cache the object and reuse it multiple times</remarks>
    public class CnnFaceDetector : IDisposable
    {
        private readonly IntPtr _instance;

        /// <summary>
        /// Creates an instance of the detector. Allocates the resources needed for the detection process
        /// </summary>
        public CnnFaceDetector()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                throw new InvalidOperationException("Only Windows and Linux are supported right now");

            _instance = Interop.Ctor();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Detects faces contained inside the bitmap and scale it if needed
        /// </summary>
        /// <param name="bitmap">The image to analyze</param>
        /// <param name="maxSize"></param>
        /// <remarks>This functions is not thread safety</remarks>
        /// <returns></returns>
        public IReadOnlyList<CnnFaceDetected> Detect(Bitmap bitmap, Size maxSize)
        {
            float? scaleX = null, scaleY = null;

            bool disposeBitmap = false;

            // Resize image if needed
            try
            {
                if (bitmap.Width > maxSize.Width || bitmap.Height > maxSize.Height)
                {
                    int width, height;
                    if (bitmap.Height > bitmap.Width)
                    {
                        // Use height as max
                        height = maxSize.Height;
                        width = height * bitmap.Width / bitmap.Height;
                    }
                    else
                    {
                        // Use width as max
                        width = maxSize.Width;
                        height = width * bitmap.Height / bitmap.Width;
                    }

                    scaleX = bitmap.Width / (float) width;
                    scaleY = bitmap.Height / (float) height;
                    bitmap = new Bitmap(bitmap, width, height);
                    disposeBitmap = true;
                }

                IReadOnlyList<CnnFaceDetected> result = Detect(bitmap);

                // Apply scale, if needed
                if (scaleX.HasValue)
                {
                    result = result.Select(r => r.Scale(scaleX.Value, scaleY.Value)).ToArray();
                }

                return result;
            }
            finally
            {
                if (disposeBitmap)
                {
                    bitmap.Dispose();
                }
            }
        }

        /// <summary>
        /// Detects faces contained inside the bitmap
        /// </summary>
        /// <param name="bitmap">The image to analyze</param>
        /// <remarks>This functions is not thread safety</remarks>
        /// <returns></returns>
        public IReadOnlyList<CnnFaceDetected> Detect(Bitmap bitmap)
        {
            BitmapData bitmapData = null;
            try
            {
                bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                return Detect(bitmapData.Scan0, true, bitmapData.Width, bitmapData.Height, bitmapData.Stride);
            }
            finally
            {
                if (bitmapData != null)
                {
                    bitmap.UnlockBits(bitmapData);
                }
            }
        }

        /// <summary>
        /// Detects faces contained inside the bitmap. This overload allows to pass a pointer to an image
        /// </summary>
        /// <param name="bitmap">The pointer to the bitmap in BGR or RGB format</param>
        /// <param name="rgbToBgr">Indicates if pixels must be converted from rgb to bgr format. BGR format is required thus you can use this parameter in order to convert from RGB</param>
        /// <param name="width">Width of the bitmap</param>
        /// <param name="height">Height of the bitmap</param>
        /// <param name="stride">Stride (width * bytes per pixel + route) of the bitmap</param>
        /// <remarks>This functions is not thread safety</remarks>
        /// <returns></returns>
        public IReadOnlyList<CnnFaceDetected> Detect(IntPtr bitmap, bool rgbToBgr, int width, int height, int stride)
        {
            var result = new List<Interop.FaceDetected>();

            int Callback(in Interop.FaceDetected faceDetected)
            {
                result.Add(faceDetected);
                return 1;
            }

            int n = Interop.Detect(_instance, bitmap, rgbToBgr, width, height, stride, Callback);

            return result.Select(r => new CnnFaceDetected(r)).ToArray();
        }

        private void ReleaseUnmanagedResources()
        {
            Interop.Dtor(_instance);
        }

        /// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
        ~CnnFaceDetector() => ReleaseUnmanagedResources();
    }
}
