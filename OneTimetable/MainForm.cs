using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OneTimetable
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
#if !DEBUG
            SetToDeskTop();
#endif
            PictureBoxColor();
        }
        private void PictureBoxColor()
        {
            for (int i = 0; i < flowLayoutPanel2.Controls.Count; i++)
            {
                if (flowLayoutPanel2.Controls[i].GetType() == typeof(PictureBox))
                {
                    flowLayoutPanel2.Controls[i].MouseEnter += new System.EventHandler(pictureBox_MouseEnter);
                    flowLayoutPanel2.Controls[i].MouseLeave += new System.EventHandler(pictureBox_MouseLeave);

                }
            }
        }


        #region 将窗体钉在桌面上
        private void SetToDeskTop()
        {
            try
            {
                if (Environment.OSVersion.Version.Major < 6)
                {
                    base.SendToBack();
                    IntPtr hWndNewParent = Win32.FindWindow("Progman", null);
                    Win32.SetParent(base.Handle, hWndNewParent);
                }
                else
                {
                    IntPtr desktopHwnd = GetDesktopPtr();
                    IntPtr ownHwnd = base.Handle;
                    IntPtr result = Win32.SetParent(ownHwnd, desktopHwnd);

                }

            }
            catch (ApplicationException exx)
            {
                MessageBox.Show(this, exx.Message, "Pin to Desktop");
            }
        }

        private IntPtr GetDesktopPtr()
        {
            //http://blog.csdn.net/mkdym/article/details/7018318
            // 情况一
            IntPtr hwndWorkerW = IntPtr.Zero;
            IntPtr hShellDefView = IntPtr.Zero;
            IntPtr hwndDesktop = IntPtr.Zero;
            IntPtr hProgMan = Win32.FindWindow("ProgMan", null);
            if (hProgMan != IntPtr.Zero)
            {
                hShellDefView = Win32.FindWindowEx(hProgMan, IntPtr.Zero, "SHELLDLL_DefView", null);
                if (hShellDefView != IntPtr.Zero)
                {
                    hwndDesktop = Win32.FindWindowEx(hShellDefView, IntPtr.Zero, "SysListView32", null);
                }
            }
            if (hwndDesktop != IntPtr.Zero) return hwndDesktop;

            // 情况二
            while (hwndDesktop == IntPtr.Zero)
            {//必须存在桌面窗口层次  
                hwndWorkerW = Win32.FindWindowEx(IntPtr.Zero, hwndWorkerW, "WorkerW", null);//获得WorkerW类的窗口  
                if (hwndWorkerW == IntPtr.Zero) break;//未知错误
                hShellDefView = Win32.FindWindowEx(hwndWorkerW, IntPtr.Zero, "SysListView32", null);
                if (hShellDefView == IntPtr.Zero) continue;
                hwndDesktop = Win32.FindWindowEx(hShellDefView, IntPtr.Zero, "SysListView32", null);
            }
            return hwndDesktop;
        }


        private void MainForm_Activated(object sender, EventArgs e)
        {
#if DEBUG
            return;
#endif
            if (Environment.OSVersion.Version.Major >= 6)
            {
                Win32.SetWindowPos(base.Handle, 1, 0, 0, 0, 0, Win32.SE_SHUTDOWN_PRIVILEGE);
            }

        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
#if DEBUG
            return;
#endif

            if (Environment.OSVersion.Version.Major >= 6)
            {
                Win32.SetWindowPos(base.Handle, 1, 0, 0, 0, 0, Win32.SE_SHUTDOWN_PRIVILEGE);
            }
        }
        #endregion

        //窗口移动
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            Win32.ReleaseCapture();
            Win32.SendMessage(this.Handle, Win32.WM_SYSCOMMAND, Win32.SC_MOVE + Win32.HTCAPTION, 0);//窗体移动

        }

        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            pictureBox.BackColor = System.Drawing.SystemColors.ControlDark;
        }
        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            pictureBox.BackColor = System.Drawing.SystemColors.Control;
        }

        private void ButtomExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ButtomInfo_Click(object sender, EventArgs e)
        {
            string text = @"
Made by Kelatte
开源在Github : https://github.com/1205691775/OneTimetable
";
            MessageBox.Show(text);
        }
    }
}
