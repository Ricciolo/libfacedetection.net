using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace LibFaceDetection
{
    internal class Interop
    {
        private const string DllName = "libfacedetection.net";

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int DetectCallback(in FaceDetected faceDetected);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Ctor();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Dtor(IntPtr instance);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Detect(IntPtr instance, IntPtr data, bool rgb2bgr, int width, int height, int step, DetectCallback callback);

        [StructLayout(LayoutKind.Sequential)]
        public struct FaceDetected
        {
            public int x, y;
            public int width, height;
            public float confidence;
        }
    }
}
