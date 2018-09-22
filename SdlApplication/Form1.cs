using System;
using System.Windows.Forms;
using SdlApplication.Window;

namespace SdlApplication
{
    public partial class Form1 : Form
    {
        private readonly SdlWindow _sdlWindow;

        public Form1()
        {
            InitializeComponent();
            _sdlWindow = new SdlWindow("SDL DEMO", 640, 480);
            _sdlWindow.Open();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Hide();
            Close();
        }
    }
}
