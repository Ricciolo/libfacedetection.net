using System.Diagnostics;
using System.Drawing;

namespace LibFaceDetection
{
    [DebuggerDisplay("{Rectangle} - {Confidence}")]
    public class CnnFaceDetected
    {
        internal CnnFaceDetected(Interop.FaceDetected faceDetected)
        {
            Rectangle = new Rectangle(faceDetected.x, faceDetected.y, faceDetected.width, faceDetected.height);
            Confidence = faceDetected.confidence;
        }

        public float Confidence { get; }

        public Rectangle Rectangle { get; }
    }
}