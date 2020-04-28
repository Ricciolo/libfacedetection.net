using System.Diagnostics;
using System.Drawing;

namespace LibFaceDetection
{
    /// <summary>
    /// Represents an identified face. This object is returned after an image has been analyzed
    /// </summary>
    [DebuggerDisplay("{Rectangle} - {Confidence}")]
    public class CnnFaceDetected
    {
        internal CnnFaceDetected(Interop.FaceDetected faceDetected)
        {
            Rectangle = new Rectangle(faceDetected.x, faceDetected.y, faceDetected.width, faceDetected.height);
            Confidence = faceDetected.confidence;
        }

        /// <summary>
        /// Gets the confidence between 0 to 1 of the found face. 0 is low and 1 is high
        /// </summary>
        public float Confidence { get; }

        /// <summary>
        /// Gets the bounding box of the found face
        /// </summary>
        public Rectangle Rectangle { get; }
    }
}