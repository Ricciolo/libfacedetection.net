using System;
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

        private CnnFaceDetected(float confidence, Rectangle rectangle)
        {
            Confidence = confidence;
            Rectangle = rectangle;
        }

        /// <summary>
        /// Returns a scaled version of the detected face
        /// </summary>
        /// <param name="scaleX">Scale to multiply for x and width</param>
        /// <param name="scaleY">Scale to multiply for x and height</param>
        /// <returns></returns>
        public CnnFaceDetected Scale(float scaleX, float scaleY)
        {
            var rectangle = new Rectangle(
                Convert.ToInt32(Rectangle.X * scaleX),
                Convert.ToInt32(Rectangle.Y * scaleY),
                Convert.ToInt32(Rectangle.Width * scaleX),
                Convert.ToInt32(Rectangle.Height * scaleY));
            return new CnnFaceDetected(Confidence, rectangle);
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