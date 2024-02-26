using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PtWinOwnScr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private const int GWL_STYLE = -16;
        private const int WS_VISIBLE = 0x10000000;
        private const int WS_POPUP = unchecked((int)0x80000000);

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        static extern bool MoveWindow(IntPtr Handle, int x, int y, int w, int h, bool repaint);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        private Process _curProcess;


        private void button1_Click(object sender, EventArgs e)
        {
            applyMod("C:\\Program Files\\Notepad++\\notepad++.exe");
        }

        private void applyMod(String proc)
        {
            try
            {
                // Launch Notepad
                _curProcess = Process.Start(proc);
                // Wait for Notepad to open its window
                _curProcess.WaitForInputIdle();
                // Get the handle of Notepad's main window
                IntPtr appWin = _curProcess.MainWindowHandle;
                // Set Notepad's parent window to be this form's handle
                SetParent(appWin, this.Handle);
                // Set Notepad's style to be visible and remove the title bar
                //SetWindowLong(appWin, GWL_STYLE, WS_VISIBLE | WS_POPUP);
                // Move Notepad's window to the center of this form's client area
                MoveWindow(appWin, 0, 0, this.Width, this.Height, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void applyMod(IntPtr appWin)
        {
            try
            {
                // Set the window's parent window to be this form's handle
                SetParent(appWin, this.Handle);
                // Set the window's style to be visible and remove the title bar
                //SetWindowLong(appWin, GWL_STYLE, WS_VISIBLE | WS_POPUP);
                // Move the window to the center of this form's client area
                MoveWindow(appWin, 0, 0, this.Width, this.Height, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //if (_curProcess != null)
            //{
            //    MoveWindow(_curProcess.MainWindowHandle, 0, 0, this.Width, this.Height, true);
            //}
        }

        Boolean isFullScreen = false;

        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            if (isFullScreen)
            {
                this.TopMost = false;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.TopMost = true;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }
            isFullScreen = !isFullScreen;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            applyMod("notepad.exe");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // ask for process name in winforms
            var process = Prompt.ShowDialog("PROCPATH", "PAMGR");
            applyMod(process);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel1.Visible = !panel1.Visible;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Start a timer that ticks every 5 seconds
            Timer timer = new Timer();
            timer.Interval = 5000; // 5 seconds
            timer.Tick += (s, args) =>
            {
                // Stop the timer
                timer.Stop();
                // Get the handle of the currently focused window
                IntPtr focusedWin = GetForegroundWindow();
                // Apply the modification
                applyMod(focusedWin);
            };
            timer.Start();
        }
    }

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                TopMost = true
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
