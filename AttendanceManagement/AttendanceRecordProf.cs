using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AttendanceManagement
{
    public partial class AttendanceRecordProf : Form
    {
        private string professorid;
        private MySqlConnection con;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        public AttendanceRecordProf(string professorid)
        {
            InitializeComponent();
            con = new MySqlConnection(@"server=localhost;database=user_info;userid=root;password=;");
            this.professorid = professorid;
            string selectedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
            searchbar_text.KeyPress += searchbar_text_KeyPress;
        }

        private void AttendanceRecordProf_Load(object sender, EventArgs e)
        {
            LoadAttendanceData();
            LoadProfessorProfile();
            searchbar_text.KeyPress += searchbar_text_KeyPress;
        }

        public void LoadProfessorProfile()
        {
            using (MySqlConnection con = new MySqlConnection(@"server=localhost;database=user_info;userid=root;password=;"))
            {
                try
                {
                    con.Open();
                    string query = "SELECT * FROM teacher_db WHERE id_no = @idno";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@idno", professorid);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                login_name.Text = reader["name"].ToString();
                                profid_text.Text = reader["id_no"].ToString();

                                if (reader["photo"] != DBNull.Value)
                                {
                                    byte[] photoData = (byte[])reader["photo"];
                                    using (MemoryStream ms = new MemoryStream(photoData))
                                    {
                                        pictureBox1.Image = Image.FromStream(ms);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Professor profile not found.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading professor profile: {ex.Message}");
                }
            }
        }

        private void LoadAttendanceData(string subject = "", string course = "", string year = "", string section = "", string searchText = "", DateTime? selectedDate = null)
        {
            try
            {
                con.Open();
                using (MySqlCommand coman = new MySqlCommand())
                {
                    coman.Connection = con;

                    string query = "SELECT * FROM attendance_db WHERE 1=1";

                    if (!string.IsNullOrEmpty(course))
                        query += " AND course = @course";
                    if (!string.IsNullOrEmpty(year))
                        query += " AND year = @year";
                    if (!string.IsNullOrEmpty(section))
                        query += " AND section = @section";
                    if (!string.IsNullOrEmpty(subject))
                        query += " AND subject = @subject";
                    if (!string.IsNullOrEmpty(searchText))
                        query += " AND (studentid LIKE @searchText OR name LIKE @searchText OR subject LIKE @searchText OR course LIKE @searchText OR year LIKE @searchText OR section LIKE @searchText)";
                    if (selectedDate.HasValue)
                        query += " AND DATE(date) = @selectedDate";

                    coman.CommandText = query;

                    if (!string.IsNullOrEmpty(course))
                        coman.Parameters.AddWithValue("@course", course);
                    if (!string.IsNullOrEmpty(year))
                        coman.Parameters.AddWithValue("@year", year);
                    if (!string.IsNullOrEmpty(section))
                        coman.Parameters.AddWithValue("@section", section);
                    if (!string.IsNullOrEmpty(subject))
                        coman.Parameters.AddWithValue("@subject", subject);
                    if (!string.IsNullOrEmpty(searchText))
                        coman.Parameters.AddWithValue("@searchText", "%" + searchText + "%");
                    if (selectedDate.HasValue)
                        coman.Parameters.AddWithValue("@selectedDate", selectedDate.Value.ToString("yyyy-MM-dd"));

                    MySqlDataAdapter da = new MySqlDataAdapter(coman);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.Rows.Clear();

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No records found.");
                    }
                    else
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            var dateValue = row["date"] != DBNull.Value ? Convert.ToDateTime(row["date"]).ToString("yyyy-MM-dd") : "N/A";
                            var timeinValue = row["timein"] != DBNull.Value ? Convert.ToDateTime(row["timein"]).ToString("HH:mm:ss") : "N/A";

                            dataGridView1.Rows.Add(row["studentid"], row["name"], row["subject"], row["course"], row["year"], row["section"], dateValue, timeinValue);
                        }
                    }
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
            string selectedCourse = course_list.SelectedItem?.ToString();
            string selectedYear = year_list.SelectedItem?.ToString();
            string selectedSection = section_list.SelectedItem?.ToString();
            string selectedSubject = subject_list.SelectedItem?.ToString();
            string searchText = searchbar_text.Text.Trim();
            DateTime? selectedDate = dateTimePicker1.Checked ? (DateTime?)dateTimePicker1.Value.Date : null;

            LoadAttendanceData(
                selectedSubject ?? "",
                selectedCourse ?? "",
                selectedYear ?? "",
                selectedSection ?? "",
                searchText,
                selectedDate
            );
        }

        private void searchbar_text_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                ApplyFilters();
            }
        }

        private void course_list_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilters();
        private void year_list_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilters();
        private void section_list_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilters();
        private void subject_list_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilters();
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) => ApplyFilters();

        private void home_btn_Click(object sender, EventArgs e)
        {
            this.Hide();
            ProfessorDashboard professorDashboard = new ProfessorDashboard(professorid);
            professorDashboard.ShowDialog();
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

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentInformationProf studentInformationProf = new StudentInformationProf(professorid);
            studentInformationProf.ShowDialog();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to close this form?", "Confirm Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                this.Close(); // Close the form
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }

}
