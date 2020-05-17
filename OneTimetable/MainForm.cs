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
        
        readonly AutoSizeForm asc;

        private readonly string JsonDataPath = "data.txt";

        private readonly int formThinWeith = 233;
        private readonly int formWideWeith;
        private readonly int formHeight;

        private Quicktype.OneTimetableData OneTimetableData;
        List<List<string>> dayOrder;

        public MainForm()
        {
            InitializeComponent();

            formWideWeith = Size.Width;
            formHeight = Size.Height;
#if !DEBUG
            SetToDesktop();
#endif

            InitPictureBoxColorEvent();
            InitAddClassEvent();

            LoadData();
            UpdataData();

            SettingOpen = false;

            asc = new AutoSizeForm();
            ChangeSize(OneTimetableData.Description.Size);
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
            if (dateTime.DayOfWeek == 0)
            {
                timetable = dayOrder[6];
            }
            else
            {
                timetable = dayOrder[(int)dateTime.DayOfWeek - 1];
            }
            ListShow(timetable);
        }



        private void MainForm_Load(object sender, EventArgs e)
        {

        }






        private void ChangeSize(int size)
        {
            asc.RenewControlRect(this);

            Size = new Size(settingOpen ? formWideWeith : formThinWeith * size / 100, formHeight * size / 100);

            asc.ControlAutoSize(this);
        }

        private void SizeChangeButton_Click(object sender, EventArgs e)
        {
            OneTimetableData.Description.Size = SizeTrackBar.Value;

            ChangeSize(SizeTrackBar.Value);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string json = Quicktype.Serialize.ToJson(OneTimetableData);
            System.IO.File.WriteAllText(JsonDataPath, json);
        }
    }
}
