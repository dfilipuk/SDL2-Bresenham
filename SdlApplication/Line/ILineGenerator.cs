using System;

namespace SdlApplication.Line
{
    public interface ILineGenerator
    {
        void DrawLine(IntPtr renderer, int x1, int y1, int x2, int y2);
    }
}
