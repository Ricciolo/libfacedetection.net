using System;
using System.Runtime.InteropServices;
using Xunit;

namespace LibFaceDetection.Test
{
    public class UnitTest
    {
        [Fact]
        public void Test1()
        {
            //@"..\..\..\src\out\build\x64-Debug\native\libfacedetection.net"
            //if (NativeLibrary.TryLoad("libfacedetection.net", typeof(Class1).Assembly, DllImportSearchPath.AssemblyDirectory,  out IntPtr handle))
            //{

            //}

            int test = Class1.Test("ciao", 1, 3);
        }
    }
}
