using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;


namespace AttendanceManagement
{
    public partial class StudentInformationForm : Form
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

        public StudentInformationForm()
        {
            InitializeComponent();
            con.ConnectionString = @"server=localhost;database=user_info;userid=root;password=;";
            searchbar_text.KeyPress += searchbar_text_KeyPress;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));

            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(UpdateTimeLabel);
            timer1.Start();

            UpdateTimeLabel(null, null);
            UpdateDateLabel(null, null);
        }

        private void UpdateTimeLabel(object sender, EventArgs e)
        {
            time_label.Text = DateTime.Now.ToLongTimeString();
        }

        private void UpdateDateLabel(object sender, EventArgs e)
        {
            date_label.Text = DateTime.Now.ToLongDateString();
        }

        private void StudentInformationForm_Load(object sender, EventArgs e)
        {
            LoadAttendanceData();
        }

        private void LoadAttendanceData(string course = "", string year = "", string section = "", string searchText = "")
        {
            try
            {
                con.Open();
                MySqlCommand coman = new MySqlCommand();
                coman.Connection = con;

                string query = "SELECT * FROM student_db WHERE 1=1";

                if (!string.IsNullOrEmpty(course))
                    query += " AND course = @course";
                if (!string.IsNullOrEmpty(year))
                    query += " AND year = @year";
                if (!string.IsNullOrEmpty(section))
                    query += " AND section = @section";
                if (!string.IsNullOrEmpty(searchText))
                    query += " AND (studentid LIKE @searchText OR fname LIKE @searchText)";

                coman.CommandText = query;

                if (!string.IsNullOrEmpty(course))
                    coman.Parameters.AddWithValue("@course", course);
                if (!string.IsNullOrEmpty(year))
                    coman.Parameters.AddWithValue("@year", year);
                if (!string.IsNullOrEmpty(section))
                    coman.Parameters.AddWithValue("@section", section);
                if (!string.IsNullOrEmpty(searchText))
                    coman.Parameters.AddWithValue("@searchText", "%" + searchText + "%");

                MySqlDataAdapter da = new MySqlDataAdapter(coman);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No records found.");
                }
                else
                {
                    dataGridView1.Rows.Clear();

                    foreach (DataRow row in dt.Rows)
                    {
                        byte[] photoData = row["photo"] as byte[];
                        Image studentPhoto = null;

                        if (photoData != null)
                        {
                            using (MemoryStream ms = new MemoryStream(photoData))
                            {
                                studentPhoto = Image.FromStream(ms);
                            }
                        }

                        dataGridView1.Rows.Add(row["studentid"], row["fname"], row["course"], row["year"], row["section"], row["contact"], row["email"], row["gender"], row["birthdate"], row["religion"], row["citizen"], row["faddress"], studentPhoto);
                    }

                }

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ApplyFilters()
        {
            string selectedCourse = course_list.SelectedItem?.ToString();
            string selectedYear = year_list.SelectedItem?.ToString();
            string selectedSection = section_list.SelectedItem?.ToString();

            LoadAttendanceData(
                string.IsNullOrEmpty(selectedCourse) ? "" : selectedCourse,
                string.IsNullOrEmpty(selectedYear) ? "" : selectedYear,
                string.IsNullOrEmpty(selectedSection) ? "" : selectedSection
            );
        }

        private void course_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void year_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void section_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void searchbar_text_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string searchText = searchbar_text.Text.Trim();
                LoadAttendanceData(
                    "",
                    "",
                    "",
                    searchText
                );
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                StudentEditingForm studentEditingForm = new StudentEditingForm();

                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                studentEditingForm.StudentId = selectedRow.Cells["Column1"].Value.ToString();
                studentEditingForm.FirstName = selectedRow.Cells["Column2"].Value.ToString();
                studentEditingForm.Course = selectedRow.Cells["Column3"].Value.ToString();
                studentEditingForm.Year = selectedRow.Cells["Column4"].Value.ToString();
                studentEditingForm.Section = selectedRow.Cells["Column5"].Value.ToString();
                studentEditingForm.Contact = selectedRow.Cells["Column6"].Value.ToString();
                studentEditingForm.Email = selectedRow.Cells["Column7"].Value.ToString();
                studentEditingForm.Gender = selectedRow.Cells["Column8"].Value.ToString();
                studentEditingForm.Birthdate = selectedRow.Cells["Column9"].Value.ToString();
                studentEditingForm.Religion = selectedRow.Cells["Column10"].Value.ToString();
                studentEditingForm.Citizen = selectedRow.Cells["Column11"].Value.ToString();
                studentEditingForm.Address = selectedRow.Cells["Column12"].Value.ToString();

                if (selectedRow.Cells["Column13"].Value is byte[] photoData && photoData.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(photoData))
                    {
                        studentEditingForm.Photo = Image.FromStream(ms);
                    }
                }
                else
                {
                    studentEditingForm.Photo = null;
                }

                studentEditingForm.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentRegistrationForm srf = new StudentRegistrationForm();
            srf.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            AttendanceRecordForm arf = new AttendanceRecordForm();
            arf.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            AttendanceQRForm aqrf = new AttendanceQRForm();
            aqrf.ShowDialog();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            AttendanceRecordForm aqrf = new AttendanceRecordForm();
            aqrf.ShowDialog();
        }

        private void home_btn_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            AttendanceQRForm attendanceQRForm = new AttendanceQRForm();
            attendanceQRForm.ShowDialog();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            StudentRegistrationForm srf = new StudentRegistrationForm();
            srf.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            ManageTeacherForm manageTeacherForm = new ManageTeacherForm();
            manageTeacherForm.ShowDialog();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Logout Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                LoginForm loginForm = new LoginForm();
                loginForm.ShowDialog();

            }
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
    }
}
