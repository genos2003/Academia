using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AttendanceManagement
{
    public partial class StudentInformationProf : Form
    {
        private string professorId;
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        private MySqlConnection con = new MySqlConnection(@"server=localhost;database=user_info;userid=root;password=;");

        public StudentInformationProf(string professorId)
        {
            InitializeComponent();
            this.professorId = professorId;
            searchbar_text.KeyPress += SearchbarText_KeyPress;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
        }

        private void StudentInformationProf_Load(object sender, EventArgs e)
        {
            LoadAttendanceData();
            LoadProfessorProfile();
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
                        cmd.Parameters.AddWithValue("@idno", professorId); // Use professorId here

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


        private void LoadAttendanceData(string course = "", string year = "", string section = "", string searchText = "")
        {
            try
            {
                con.Open();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = con;

                    string query = "SELECT * FROM student_db WHERE 1=1";

                    if (!string.IsNullOrEmpty(course))
                        query += " AND course = @course";
                    if (!string.IsNullOrEmpty(year))
                        query += " AND year = @year";
                    if (!string.IsNullOrEmpty(section))
                        query += " AND section = @section";
                    if (!string.IsNullOrEmpty(searchText))
                        query += " AND (studentid LIKE @searchText OR fname LIKE @searchText)";

                    command.CommandText = query;

                    if (!string.IsNullOrEmpty(course))
                        command.Parameters.AddWithValue("@course", course);
                    if (!string.IsNullOrEmpty(year))
                        command.Parameters.AddWithValue("@year", year);
                    if (!string.IsNullOrEmpty(section))
                        command.Parameters.AddWithValue("@section", section);
                    if (!string.IsNullOrEmpty(searchText))
                        command.Parameters.AddWithValue("@searchText", "%" + searchText + "%");

                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No records found.");
                    }
                    else
                    {
                        // Clear existing rows before binding new data
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

                            dataGridView1.Rows.Add(
                                row["studentid"],
                                row["fname"],
                                row["course"],
                                row["year"],
                                row["section"],
                                row["contact"],
                                row["email"],
                                row["gender"],
                                row["birthdate"],
                                row["religion"],
                                row["citizen"],
                                row["faddress"],
                                studentPhoto
                            );
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

            LoadAttendanceData(
                selectedCourse ?? "",
                selectedYear ?? "",
                selectedSection ?? ""
            );
        }

        private void course_list_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilters();
        private void year_list_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilters();
        private void section_list_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilters();

        private void SearchbarText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string searchText = searchbar_text.Text.Trim();
                LoadAttendanceData("", "", "", searchText);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                OpenStudentEditingForm(selectedRow);
            }
        }

        private void OpenStudentEditingForm(DataGridViewRow selectedRow)
        {
            using (StudentEditingForm studentEditingForm = new StudentEditingForm())
            {
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

        private void NavigateToForm(Form form)
        {
            this.Hide();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e) => NavigateToForm(new StudentRegistrationForm());
        private void button4_Click(object sender, EventArgs e) => NavigateToForm(new AttendanceRecordForm());
        private void button1_Click(object sender, EventArgs e) => NavigateToForm(new AttendanceQRForm());
        private void button4_Click_1(object sender, EventArgs e) => NavigateToForm(new AttendanceRecordProf(professorId));
        private void home_btn_Click(object sender, EventArgs e) => NavigateToForm(new ProfessorDashboard(professorId));

        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Logout Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                NavigateToForm(new LoginForm());
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