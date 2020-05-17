using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace OneTimetable
{
    public partial class MainForm : Form
    {
        private bool settingOpen;
        private bool SettingOpen
        {
            get { return settingOpen; }
            set
            {
                settingOpen = value; Size = new Size(
              (settingOpen ? formWideWeith : formThinWeith) * OneTimetableData.Description.Size / 100,
              Size.Height);
            }
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

            for (int i = 0, j = 0; i < timetable.Count && j < 9; i++)
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
                    SettingPanel.Controls[i].MouseEnter += new System.EventHandler(PictureBox_MouseEnter);
                    SettingPanel.Controls[i].MouseLeave += new System.EventHandler(PictureBox_MouseLeave);

                }
            }
        }

        private void PictureBox_MouseEnter(object sender, EventArgs e)
        {
            (sender as PictureBox).BackColor = SystemColors.ControlDark;
        }

        private void PictureBox_MouseLeave(object sender, EventArgs e)
        {
            (sender as PictureBox).BackColor = SystemColors.Control;
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
    }
}
