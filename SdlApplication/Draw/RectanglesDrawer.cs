using System;
using SdlApplication.Line;
using SDL2;

namespace SdlApplication.Draw
{
    public class RectanglesDrawer
    {
        private readonly int _offsetFromBorderPx = 10;
        private readonly ILineGenerator _lineGenerator;

        public RectanglesDrawer(ILineGenerator lineGenerator)
        {
            _lineGenerator = lineGenerator;
        }

        public void DrawRectangles(IntPtr renderer, int width, int height, int rectanglesCount, double k)
        {
            int centerX, centerY, halfSideLength;
            double mu = GetMu(k, rectanglesCount);
            GetRectangleParameters(width, height, out centerX, out centerY, out halfSideLength);
            SDL.SDL_Point[] vertexes = GetInitialVertexes(centerX, centerY, halfSideLength);

            Draw(renderer, vertexes);
            for (int i = 0; i < rectanglesCount; i++)
            {
                vertexes = GetNewVertexes(mu, vertexes);
                Draw(renderer, vertexes);
            }
        }

        private void GetRectangleParameters(int width, int height, out int centerX, out int centerY, out int halfSideLength)
        {
            centerX = width / 2;
            centerY = height / 2;
            halfSideLength = Math.Min(centerX, centerY) - _offsetFromBorderPx;
        }

        private double GetMu(double k, int n)
        {
            double rotationAngle = k * Math.PI / (4 * n);
            return Math.Tan(rotationAngle) / (1 + Math.Tan(rotationAngle));
        }

        private SDL.SDL_Point[] GetInitialVertexes(int centerX, int centerY, int halfSide)
        {
            var result = new SDL.SDL_Point[]
            {
                new SDL.SDL_Point { x = halfSide + centerX, y = halfSide + centerY },
                new SDL.SDL_Point { x = -halfSide + centerX, y = halfSide + centerY },
                new SDL.SDL_Point { x = -halfSide + centerX, y = -halfSide + centerY },
                new SDL.SDL_Point { x = halfSide + centerX, y = -halfSide + centerY }
            };

            return result;
        }

        private SDL.SDL_Point[] GetNewVertexes(double mu, SDL.SDL_Point[] vertexes)
        {
            int vertexesCount = vertexes.Length;
            var result = new SDL.SDL_Point[vertexesCount];

            for (int currentVertex = 0; currentVertex < vertexesCount; currentVertex++)
            {
                int nextVertex = (currentVertex + 1) % vertexesCount;
                result[currentVertex] = new SDL.SDL_Point
                {
                    x = (int) Math.Round((1 - mu) * vertexes[currentVertex].x + mu * vertexes[nextVertex].x),
                    y = (int) Math.Round((1 - mu) * vertexes[currentVertex].y + mu * vertexes[nextVertex].y)
                };
            }

            return result;
        }

        private void Draw(IntPtr renderer, SDL.SDL_Point[] vertexes)
        {
            int vertexesCount = vertexes.Length;

            for (int currentVertex = 0; currentVertex < vertexesCount; currentVertex++)
            {
                int nextVertex = (currentVertex + 1) % vertexesCount;
                _lineGenerator.DrawLine(renderer, vertexes[currentVertex].x, vertexes[currentVertex].y, 
                    vertexes[nextVertex].x, vertexes[nextVertex].y);
            }
        }
    }
}
