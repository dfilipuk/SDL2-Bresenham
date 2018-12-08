using System;
using System.Threading;
using SdlApplication.Draw;
using SdlApplication.Line;
using SDL2;

namespace SdlApplication.Window
{
    public class SdlWindow
    {
        private readonly int _renderLoopTimeoutMs = 10;

        private readonly int _screenWidth;
        private readonly int _screenHeight;
        private readonly string _title;
        private readonly LinesDrawer _linesDrawer;
        private readonly RectanglesDrawer _rectanglesDrawer;

        private IntPtr _renderer;
        private IntPtr _window;

        public SdlWindow(string title, int screenWidth, int screenHeight)
        {
            _title = title;
            _screenHeight = screenHeight;
            _screenWidth = screenWidth;

            var lineGenerator = new BresenhamLine();

            _linesDrawer = new LinesDrawer(lineGenerator);
            _rectanglesDrawer = new RectanglesDrawer(lineGenerator);
        }

        public void Open()
        {
            var thred = new Thread(() =>
            {
                SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING);
                _window = SDL.SDL_CreateWindow(_title, SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED,
                    _screenWidth, _screenHeight, SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
                _renderer = SDL.SDL_CreateRenderer(_window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

                WindowProcedure();

                SDL.SDL_DestroyRenderer(_renderer);
                SDL.SDL_DestroyWindow(_window);
                SDL.SDL_Quit();
            });
            thred.Start();
            thred.Join();
        }

        private void WindowProcedure()
        {
            bool exit = false;
            while (!exit)
            {
                SDL.SDL_Event sdlEvent;
                SDL.SDL_PollEvent(out sdlEvent);
                switch (sdlEvent.type)
                {
                    case SDL.SDL_EventType.SDL_QUIT:
                    {
                        exit = true;
                        break;
                    }
                    case SDL.SDL_EventType.SDL_KEYDOWN:
                    {
                        var key = sdlEvent.key;
                        switch (key.keysym.sym)
                        {
                            case SDL.SDL_Keycode.SDLK_q:
                                _rectanglesDrawer.DecreaseMu();
                                break;
                            case SDL.SDL_Keycode.SDLK_e:
                                _rectanglesDrawer.IncreaseMu();
                                break;
                        }
                        break;
                    }
                }
                Draw();
                Thread.Sleep(_renderLoopTimeoutMs);
            }
        }

        // Формат цвета в HEX коде:
        //     0xRRGGBB00
        //  где R: от 00 до FF
        //      G: от 00 до FF
        //      B: от 00 до FF 
        private void Draw()
        {
            SDL.SDL_SetRenderDrawColor(_renderer, 255, 255, 255, 255);
            SDL.SDL_RenderClear(_renderer);
            SDL.SDL_SetRenderDrawColor(_renderer, 0, 0, 0, 0);

            int width, height;
            SDL.SDL_GetWindowSize(_window, out width, out height);

            _rectanglesDrawer.DrawRectangles(_renderer, width, height, 10);

            SDL.SDL_RenderPresent(_renderer);
        }
    }
}
