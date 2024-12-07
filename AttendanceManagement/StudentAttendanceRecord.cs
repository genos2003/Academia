using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace AttendanceManagement
{
    public partial class StudentAttendanceRecord : Form
    {
        private MySqlConnection con = new MySqlConnection(@"server=localhost;database=user_info;userid=root;password=;");
        private string studentId;

        public StudentAttendanceRecord(string studentId)
        {
            InitializeComponent();
            this.studentId = studentId;

            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(UpdateTimeLabel);
            timer1.Start();

            UpdateTimeLabel(null, null);
            UpdateDateLabel(null, null);
        }

        private void StudentAttendanceRecord_Load(object sender, EventArgs e)
        {
            LoadAttendanceData();
            LoadStudentProfile();
        }

        private void LoadStudentProfile()
        {
            try
            {
                con.Open();
                string query = "SELECT * FROM student_db WHERE studentid = @studentid";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@studentid", studentId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            login_name.Text = reader["fname"].ToString();
                            studentid_text.Text = reader["studentid"].ToString();

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
            finally
            {
                con.Close();
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
        private void LoadAttendanceData(string subject = "", DateTime? date = null)
        {
            try
            {
                con.Open();
                MySqlCommand coman = new MySqlCommand();
                coman.Connection = con;

                string query = "SELECT * FROM attendance_db WHERE studentid = @studentId";

                if (!string.IsNullOrEmpty(subject))
                    query += " AND subject = @subject";

                if (date.HasValue)
                    query += " AND DATE(date) = @date";

                coman.CommandText = query;
                coman.Parameters.AddWithValue("@studentId", studentId);

                if (!string.IsNullOrEmpty(subject))
                    coman.Parameters.AddWithValue("@subject", subject);

                if (date.HasValue)
                    coman.Parameters.AddWithValue("@date", date.Value.Date.ToString("yyyy-MM-dd"));

                MySqlDataAdapter da = new MySqlDataAdapter(coman);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.Rows.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    string dateValue = Convert.ToDateTime(row["date"]).ToString("yyyy-MM-dd");

                    dataGridView1.Rows.Add(row["studentid"], row["name"], row["subject"], row["course"], row["year"], row["section"], dateValue, row["timein"]);
                }

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No attendance records found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void ApplyFilters()
        {
            string selectedSubject = subject_list.SelectedItem?.ToString();
            DateTime selectedDate = dateTimePicker1.Value.Date;

            LoadAttendanceData(string.IsNullOrEmpty(selectedSubject) ? "" : selectedSubject, selectedDate);
        }

        private void subject_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            ApplyFilters();
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

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            StudentAccountForm studentAccountForm = new StudentAccountForm(studentId);
            studentAccountForm.ShowDialog();
        }

        private void home_btn_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentDashboard studentDashboard = new StudentDashboard(studentId);
            studentDashboard.ShowDialog();
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

        private void button1_Click_1(object sender, EventArgs e)
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
