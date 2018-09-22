using System;
using System.Collections.Generic;
using SDL2;

namespace SdlApplication.Line
{
    public class BresenhamLine : ILineGenerator
    {
        public void DrawLine(IntPtr renderer, int x1, int y1, int x2, int y2)
        {
            int deltaX = Math.Abs(x2 - x1);
            int deltaY = Math.Abs(y2 - y1);
            int xDirection = x2 >= x1 ? 1 : -1;
            int yDirection = y2 >= y1 ? 1 : -1;
            var points = new List<SDL.SDL_Point>();
            points.Add(new SDL.SDL_Point { x = x1, y = y1 });

            // k = dy / dx
            // Multiply all values by dx to work with integer values

            if (deltaX >= deltaY)
            {
                int currentError = (deltaY << 1) - deltaX; // 2k-1
                int errorIncreaseWhenChangeY = (deltaY - deltaX) << 1; // 2k-2
                int errocIncreaseWhenNotChangeY = deltaY << 1; // 2k

                for (int i = 1, x = x1 + xDirection, y = y1; i <= deltaX; i++, x += xDirection)
                {
                    if (currentError > 0)
                    {
                        y += yDirection;
                        currentError += errorIncreaseWhenChangeY;
                    }
                    else
                    {
                        currentError += errocIncreaseWhenNotChangeY;
                    }

                    points.Add(new SDL.SDL_Point { x = x, y = y });
                }
            }
            else
            {
                int currentError = (deltaX << 1) - deltaY; // 2k-1
                int errorIncreaseWhenChangeX = (deltaX - deltaY) << 1; // 2k-2
                int errocIncreaseWhenNotChangeX = deltaX << 1; // 2k

                for (int i = 1, x = x1, y = y1 + yDirection; i <= deltaY; i++, y += yDirection)
                {
                    if (currentError > 0)
                    {
                        x += xDirection;
                        currentError += errorIncreaseWhenChangeX;
                    }
                    else
                    {
                        currentError += errocIncreaseWhenNotChangeX;
                    }

                    points.Add(new SDL.SDL_Point { x = x, y = y });
                }
            }

            SDL.SDL_RenderDrawPoints(renderer, points.ToArray(), points.Count);
        }
    }
}
