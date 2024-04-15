using System;
using System.Threading;
using System.Windows.Forms;

namespace AutoClicker
{
    public partial class MainForm : Form
    {
        private bool autoClicking = false;
        private int clickInterval = 100; // Time in milliseconds between clicks

        public MainForm()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (!autoClicking)
            {
                autoClicking = true;
                startButton.Text = "Stop";
                StartAutoClick();
            }
            else
            {
                autoClicking = false;
                startButton.Text = "Start";
            }
        }

        private void StartAutoClick()
        {
            Thread clickThread = new Thread(() =>
            {
                while (autoClicking)
                {
                    Cursor.Position = new System.Drawing.Point(Cursor.Position.X, Cursor.Position.Y);
                    Thread.Sleep(clickInterval);
                    MouseEvent(MouseEventFlags.LeftDown | MouseEventFlags.LeftUp);
                }
            });
            clickThread.Start();
        }

        private void MouseEvent(MouseEventFlags flags)
        {
            NativeMethods.mouse_event((int)flags, 0, 0, 0, UIntPtr.Zero);
        }
    }

    [Flags]
    public enum MouseEventFlags
    {
        LeftDown = 0x00000002,
        LeftUp = 0x00000004,
        RightDown = 0x00000008,
        RightUp = 0x00000010
    }

    internal static class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, UIntPtr dwExtraInfo);
    }
}
