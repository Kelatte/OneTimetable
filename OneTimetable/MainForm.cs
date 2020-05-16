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
        private string JsonDataPath = "data.txt";

        private readonly int formThinWeith = 233;
        private readonly int formWideWeith;

        private Quicktype.OneTimetableData OneTimetableData;
        List<List<string>> dayOrder;

        public MainForm()
        {
            InitializeComponent();

            formWideWeith = Size.Width;
#if !DEBUG
            SetToDesktop();
#endif
            InitPictureBoxColorEvent();
            InitAddClassEvent();

            LoadData();
            UpdataData();

            SettingOpen = false;
        }

        private void InitAddClassEvent()
        {
            for (int i = 0; i < AddClassPanel.Controls.Count; i++)
            {
                AddClassPanel.Controls[i].Click += new System.EventHandler(SelectClass);
            }
        }

        private void SelectClass(object sender, EventArgs e)
        {

            if (ClassBox.Items.Count >= 9)
            {
                return;
            }
            ClassBox.Items.Add((sender as Button).Text);

            dayOrder[DayControl.SelectedIndex].Add((sender as Button).Text);

        }

        private void LoadData()
        {
            try
            {
                string json = System.IO.File.ReadAllText(JsonDataPath);
                OneTimetableData = Quicktype.OneTimetableData.FromJson(json);
            }
            catch (Exception e)
            {

                throw e;
            }

            dayOrder = new List<List<string>> { OneTimetableData.Data.Monday, OneTimetableData.Data.Thursday,
            OneTimetableData.Data.Wednesday,OneTimetableData.Data.Thursday,OneTimetableData.Data.Friday,
            OneTimetableData.Data.Saturday, OneTimetableData.Data.Sunday};
        }

        private void UpdataData()
        {
            DateTime dateTime = DateTime.Now;
            List<string> timetable = new List<string>();
            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    timetable = OneTimetableData.Data.Sunday;
                    break;
                case DayOfWeek.Monday:
                    timetable = OneTimetableData.Data.Monday;
                    break;
                case DayOfWeek.Tuesday:
                    timetable = OneTimetableData.Data.Tuesday;
                    break;
                case DayOfWeek.Wednesday:
                    timetable = OneTimetableData.Data.Wednesday;
                    break;
                case DayOfWeek.Thursday:
                    timetable = OneTimetableData.Data.Thursday;
                    break;
                case DayOfWeek.Friday:
                    timetable = OneTimetableData.Data.Friday;
                    break;
                case DayOfWeek.Saturday:
                    timetable = OneTimetableData.Data.Saturday;
                    break;
            }
            ListShow(timetable);
        }

        private void ListShow(List<string> timetable)
        {
            for (int i = 0; i < TimetablePanel.Controls.Count; i++)
            {
                if (TimetablePanel.Controls[i].GetType() == typeof(Label))
                    TimetablePanel.Controls[i].Text = "";
            }

            if (timetable.Count == 0)
            {
                if (TimetablePanel.Controls[0].GetType() == typeof(Label))
                TimetablePanel.Controls[0].Text = "无课";
            }

            for (int i = 0,j = 0; i < timetable.Count && j < 9; i++)
            {
                if (TimetablePanel.Controls[i].GetType() == typeof(Label))
                {
                    TimetablePanel.Controls[i].Text = timetable[j++];

                }

            }
        }

        private void InitPictureBoxColorEvent()
        {
            for (int i = 0; i < SettingPanel.Controls.Count; i++)
            {
                if (SettingPanel.Controls[i].GetType() == typeof(PictureBox))
                {
                    SettingPanel.Controls[i].MouseEnter += new System.EventHandler(pictureBox_MouseEnter);
                    SettingPanel.Controls[i].MouseLeave += new System.EventHandler(pictureBox_MouseLeave);

                }
            }
        }


        #region 将窗体钉在桌面上
        private void SetToDesktop()
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

        private void ButtonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ButtonInfo_Click(object sender, EventArgs e)
        {
            string text = 
@"Made by Kelatte
开源在Github : https://github.com/1205691775/OneTimetable";
            MessageBox.Show(text);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private bool settingOpen;
        private bool SettingOpen { get { return settingOpen; } 
            set { settingOpen = value; Size = new Size(settingOpen ? formWideWeith : formThinWeith, Size.Height); } 
        }

        private void ButtonSettings_Click(object sender, EventArgs e)
        {
            SettingOpen = !SettingOpen;

            if (settingOpen)
            {
                LoadClassBox(0);
                DayControl.SelectedIndex = 0;
            }
        }
        private void LoadClassBox(int index)
        {


            ClassBox.Items.Clear();

            for (int i = 0; i < dayOrder[index].Count; i++)
            {
                ClassBox.Items.Add(dayOrder[index][i]);
            }
        }

        private void ClearClass_Click(object sender, EventArgs e)
        {
            ClassBox.Items.Clear();
            dayOrder[DayControl.SelectedIndex].Clear();
        }

        private void DayControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClassBox(DayControl.SelectedIndex);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {

            string json = Quicktype.Serialize.ToJson(OneTimetableData);
            System.IO.File.WriteAllText(JsonDataPath, json);

            LoadData();
            UpdataData();
        }
    }
}
