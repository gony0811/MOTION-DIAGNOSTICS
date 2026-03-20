using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device
{
    internal class ImageProcessing
    {
        public static Mat DrawingCrosshair(Mat source)
        {
            var height = source.Height;
            var width = source.Width;
            Cv2.Line(source, new OpenCvSharp.Point(0, height / 2), new OpenCvSharp.Point(width, height / 2), Scalar.Red, 1);
            Cv2.Line(source, new OpenCvSharp.Point(width / 2, 0), new OpenCvSharp.Point(width / 2, height), Scalar.Red, 1);

            return source;
        }


    }
}
