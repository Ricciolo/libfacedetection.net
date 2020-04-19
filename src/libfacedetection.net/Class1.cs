using System.Runtime.InteropServices;

namespace LibFaceDetection
{
    public class Class1
    {
        private const string DllName = "libfacedetection.net.dll";

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern int Test(string s, int a, int b);

    }
}
