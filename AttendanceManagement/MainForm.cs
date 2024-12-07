using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AttendanceManagement
{
    public partial class MainForm : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );
        private MySqlConnection con = new MySqlConnection();

        public MainForm()
        {
            InitializeComponent();
            con.ConnectionString = @"server=localhost;database=user_info;userid=root;password=;";
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));

            timer1.Interval = 86400000;
            timer1.Tick += new EventHandler(UpdateDateLabel);
            timer1.Start();

            timer2.Interval = 1000;
            timer2.Tick += new EventHandler(UpdateTimeLabel);
            timer2.Start();

            UpdateDateLabel(null, null);
            UpdateTimeLabel(null, null);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string query_total = "SELECT COUNT(*) FROM student_db";

            using (MySqlConnection con = new MySqlConnection(@"server=localhost;database=user_info;userid=root;password=;"))
            {
                con.Open();

                using (MySqlCommand cmd = new MySqlCommand(query_total, con))
                {
                    Int32 totalStudents = Convert.ToInt32(cmd.ExecuteScalar());
                    student_no.Text = totalStudents.ToString();
                }

                string query_totalp = "SELECT COUNT(*) FROM teacher_db";

                using (MySqlCommand cmd = new MySqlCommand(query_totalp, con))
                {
                    Int32 totalStudents = Convert.ToInt32(cmd.ExecuteScalar());
                    prof_no.Text = totalStudents.ToString();
                }
                string todayDate = DateTime.Now.ToString("yyyy-MM-dd");

                string query_today = "SELECT COUNT(*) FROM attendance_db WHERE DATE(date) = @todayDate";

                using (MySqlCommand cmdr = new MySqlCommand(query_today, con))
                {
                    cmdr.Parameters.AddWithValue("@todayDate", todayDate);
                    Int32 attendanceCountToday = Convert.ToInt32(cmdr.ExecuteScalar());
                    attendance_no.Text = attendanceCountToday.ToString();
                }

                string query_ttoday = "SELECT COUNT(*) FROM teacherattendance_db WHERE DATE(date) = @todayDate";

                using (MySqlCommand cmdr = new MySqlCommand(query_ttoday, con))
                {
                    cmdr.Parameters.AddWithValue("@todayDate", todayDate);
                    Int32 attendanceCountToday = Convert.ToInt32(cmdr.ExecuteScalar());
                    label2.Text = attendanceCountToday.ToString();
                }
            }
        }

        private void UpdateDateLabel(object sender, EventArgs e)
        {
            date_label.Text = DateTime.Now.ToLongDateString();
        }

        private void UpdateTimeLabel(object sender, EventArgs e)
        {
            time_label.Text = DateTime.Now.ToLongTimeString();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm mf = new MainForm();
            mf.ShowDialog();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }

        private void date_label_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageTeacherForm mtf = new ManageTeacherForm();
            mtf.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            AttendanceQRForm aqrf = new AttendanceQRForm();
            aqrf.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentRegistrationForm srf = new StudentRegistrationForm();
            srf.ShowDialog();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentInformationForm sif = new StudentInformationForm();
            sif.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            AttendanceRecordForm arf = new AttendanceRecordForm();
            arf.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            AttendanceQRForm attendanceQRForm = new AttendanceQRForm();
            attendanceQRForm.ShowDialog();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            StudentRegistrationForm studentRegistrationForm = new StudentRegistrationForm();
            studentRegistrationForm.ShowDialog();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            StudentInformationForm studentInformationForm = new StudentInformationForm();
            studentInformationForm.ShowDialog();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            AttendanceRecordForm attendanceRecordForm = new AttendanceRecordForm();
            attendanceRecordForm.ShowDialog();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            ManageTeacherForm manageTeacherForm = new ManageTeacherForm();
            manageTeacherForm.ShowDialog();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Logout Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                LoginForm loginForm = new LoginForm();
                loginForm.ShowDialog();
            }
        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to close this form?",
                                     "Confirm Close",
                                     MessageBoxButtons.YesNo,
                                     MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }
    }
}
