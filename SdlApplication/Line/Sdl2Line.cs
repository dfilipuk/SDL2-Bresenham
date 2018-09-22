using System;
using SDL2;

namespace SdlApplication.Line
{
    public class Sdl2Line : ILineGenerator
    {
        public void DrawLine(IntPtr renderer, int x1, int y1, int x2, int y2)
        {
            SDL.SDL_RenderDrawLine(renderer, x1, y1, x2, y2);
        }
    }
}
