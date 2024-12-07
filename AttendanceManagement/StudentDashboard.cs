using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AttendanceManagement
{
    public partial class StudentDashboard : Form
    {
        private string studentid;

        public StudentDashboard(string studentid)
        {
            InitializeComponent();
            this.studentid = studentid;

            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(UpdateTimeLabel);
            timer1.Start();

            UpdateTimeLabel(null, null);
            UpdateDateLabel(null, null);
        }


        public void StudentDashboard_Load(object sender, EventArgs e)
        {
            LoadStudentProfile();
        }

        public void LoadStudentProfile()
        {
            using (MySqlConnection con = new MySqlConnection(@"server=localhost;database=user_info;userid=root;password=;"))
            {
                try
                {
                    con.Open();
                    string query = "SELECT * FROM student_db WHERE studentid = @studentid";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@studentid", studentid);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                login_name.Text = reader["fname"].ToString();
                                studentid_text.Text = reader["studentid"].ToString() ;

                                if (reader["photo"] != DBNull.Value)
                                {
                                    byte[] photoData = (byte[])reader["photo"];
                                    using (MemoryStream ms = new MemoryStream(photoData))
                                    {
                                        pictureBox3.Image = Image.FromStream(ms);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Student profile not found.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading student profile: {ex.Message}");
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
        private void pictureBox3_Click(object sender, EventArgs e)
        {
        }

        private void login_name_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentAttendanceRecord sar = new StudentAttendanceRecord(studentid);
            sar.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StudentAccountForm studentAccountForm = new StudentAccountForm(studentid);
            studentAccountForm.ShowDialog();
        }

        private void studentid_text_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            StudentAccountForm studentAccountForm = new StudentAccountForm(studentid);
            studentAccountForm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentAttendanceRecord studentAttendanceRecord = new StudentAttendanceRecord(studentid);
            studentAttendanceRecord.ShowDialog();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
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

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Logout Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                LoginForm loginForm = new LoginForm();
                loginForm.ShowDialog();
            }
        }
    }
}
