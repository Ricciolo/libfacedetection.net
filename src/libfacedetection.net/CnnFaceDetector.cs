using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace LibFaceDetection
{
    public class CnnFaceDetector : IDisposable
    {
        private readonly IntPtr _instance;

        public CnnFaceDetector()
        {
            _instance = Interop.Ctor();
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

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

        ~CnnFaceDetector()
        {
            ReleaseUnmanagedResources();
        }
    }
}