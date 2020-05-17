using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OneTimetable
{
    public partial class MainForm : Form
    {
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

            //同步在json中修改
            dayOrder[DayControl.SelectedIndex].Add((sender as Button).Text);

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
            UpdataData();
        }
    }
}
