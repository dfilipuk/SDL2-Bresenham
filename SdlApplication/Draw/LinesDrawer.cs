using System;
using SdlApplication.Line;

namespace SdlApplication.Draw
{
    /// <summary>
    /// This class is only for testing Bresenham's algorithm
    /// </summary>
    public class LinesDrawer
    {
        private readonly int _offsetFromBorderPx = 10;
        private readonly ILineGenerator _lineGenerator;

        public LinesDrawer(ILineGenerator lineGenerator)
        {
            _lineGenerator = lineGenerator;
        }

        public void DrawLines(IntPtr renderer, int width, int height, int linesAmount)
        {
            int centerX = width / 2;
            int centerY = height / 2;
            int lineLength = Math.Min(centerX, centerY) - _offsetFromBorderPx;
            double step = 2 * Math.PI / linesAmount;
            double currentAngle = 0;

            for (int i = 0; i < linesAmount; i++)
            {
                int x = (int) Math.Round(lineLength * Math.Sin(currentAngle)) + centerX;
                int y = (int) Math.Round(lineLength * Math.Cos(currentAngle)) + centerY;
                _lineGenerator.DrawLine(renderer, centerX, centerY, x, y);
                currentAngle += step;
            }
        }
    }
}
