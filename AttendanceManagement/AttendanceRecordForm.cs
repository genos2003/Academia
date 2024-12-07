using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OfficeOpenXml;

namespace AttendanceManagement
{
    public partial class AttendanceRecordForm : Form
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

        public AttendanceRecordForm()
        {
            InitializeComponent();
            con.ConnectionString = @"server=localhost;database=user_info;userid=root;password=;";
            searchbar_text.KeyPress += searchbar_text_KeyPress;
            department_list.SelectedIndexChanged += department_list_SelectedIndexChanged;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string selectedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
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

        private void AttendanceRecordForm_Load(object sender, EventArgs e)
        {
            LoadAttendanceData();
            LoadTeacherAttendanceData();
        }

        private void LoadAttendanceData(string subject = "", string course = "", string year = "", string section = "", string searchText = "", DateTime? selectedDate = null)
        {
            try
            {
                con.Open();
                MySqlCommand coman = new MySqlCommand();
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
                    query += " AND (studentid LIKE @searchText OR name LIKE @searchText)";
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

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No records found.");
                }

                dataGridView1.Rows.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    string dateValue = Convert.ToDateTime(row["date"]).ToString("yyyy-MM-dd");

                    dataGridView1.Rows.Add(
                        row["studentid"],
                        row["name"],
                        row["subject"],
                        row["course"],
                        row["year"],
                        row["section"],
                        dateValue,
                        row["timein"]
                    );
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
            string selectedSubject = subject_list.SelectedItem?.ToString();
            string selectedDepartment = department_list.SelectedItem?.ToString();
            string searchText = searchbar_text.Text.Trim();
            DateTime? selectedDate = dateTimePicker1.Checked ? (DateTime?)dateTimePicker1.Value.Date : null;

            LoadAttendanceData(
                string.IsNullOrEmpty(selectedSubject) ? "" : selectedSubject,
                string.IsNullOrEmpty(selectedCourse) ? "" : selectedCourse,
                string.IsNullOrEmpty(selectedYear) ? "" : selectedYear,
                string.IsNullOrEmpty(selectedSection) ? "" : selectedSection,
                searchText,
                selectedDate
            );

            LoadTeacherAttendanceData(
    string.IsNullOrEmpty(selectedDepartment) ? "" : selectedDepartment,
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

        private void subjec_list_SelectedIndexChanged(object sender, EventArgs e)
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

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void home_btn_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.ShowDialog();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            StudentRegistrationForm sf = new StudentRegistrationForm();
            sf.ShowDialog();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            StudentInformationForm studentInformationForm = new StudentInformationForm();
            studentInformationForm.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            ManageTeacherForm manageTeacherForm = new ManageTeacherForm();
            manageTeacherForm.ShowDialog();
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void LoadTeacherAttendanceData(string department = "", string searchText = "", DateTime? selectedDate = null)
        {
            try
            {
                con.Open();
                MySqlCommand coman = new MySqlCommand();
                coman.Connection = con;

                string query = "SELECT * FROM teacherattendance_db WHERE 1=1";

                if (!string.IsNullOrEmpty(department))
                    query += " AND department LIKE @department";
                if (!string.IsNullOrEmpty(searchText))
                    query += " AND (id_no LIKE @searchText OR name LIKE @searchText)";
                if (selectedDate.HasValue)
                    query += " AND DATE(date) = @selectedDate";

                coman.CommandText = query;

                if (!string.IsNullOrEmpty(department))
                    coman.Parameters.AddWithValue("@department", "%" + department + "%");
                if (!string.IsNullOrEmpty(searchText))
                    coman.Parameters.AddWithValue("@searchText", "%" + searchText + "%");
                if (selectedDate.HasValue)
                    coman.Parameters.AddWithValue("@selectedDate", selectedDate.Value.ToString("yyyy-MM-dd"));

                MySqlDataAdapter da = new MySqlDataAdapter(coman);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No teacher records found.");
                }

                dataGridView2.Rows.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    string dateValue = Convert.ToDateTime(row["date"]).ToString("yyyy-MM-dd");

                    dataGridView2.Rows.Add(
                        row["id_no"],
                        row["name"],
                        row["department"],
                        row["timein"],
                        row["timeout"],
                        dateValue
                    );
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void department_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void generatereport_btn_Click(object sender, EventArgs e)
        {
            // Check if there are any rows in the DataGridView
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No data available to generate a report.");
                return;
            }

            // Create a save file dialog to select the location to save the report
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Workbook|*.xlsx";
            saveFileDialog.Title = "Save Attendance Report";
            saveFileDialog.FileName = "AttendanceReport_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Create a new Excel package
                    using (ExcelPackage package = new ExcelPackage())
                    {
                        // Create a new worksheet
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Attendance Report");

                        // Add a title
                        worksheet.Cells[1, 1].Value = "Attendance Report";
                        worksheet.Cells[1, 1].Style.Font.Bold = true;
                        worksheet.Cells[1, 1].Style.Font.Size = 16;
                        worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // Add date
                        worksheet.Cells[2, 1].Value = "Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        worksheet.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                        // Add a horizontal line (in Excel, we can't draw lines, so we will just skip this)

                        // Set header row
                        for (int i = 0; i < dataGridView1.Columns.Count; i++)
                        {
                            worksheet.Cells[4, i + 1].Value = dataGridView1.Columns[i].HeaderText; // Set header text
                            worksheet.Cells[4, i + 1].Style.Font.Bold = true; // Set header text to bold
                            worksheet.Cells[4, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; // Center align header text
                        }

                        // Loop through the DataGridView rows and write the data
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            DataGridViewRow row = dataGridView1.Rows[i];

                            if (row.IsNewRow) continue; // Skip the new row placeholder

                            for (int j = 0; j < dataGridView1.Columns.Count; j++)
                            {
                                worksheet.Cells[i + 5, j + 1].Value = row.Cells[j].Value?.ToString(); // Add data to the worksheet
                                worksheet.Cells[i + 5, j + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; // Center align cell text
                            }
                        }

                        // Add end of report note
                        worksheet.Cells[dataGridView1.Rows.Count + 6, 1].Value = "End of Report";

                        // Save the Excel package
                        FileInfo excelFile = new FileInfo(saveFileDialog.FileName);
                        package.SaveAs(excelFile);
                    }

                    // Inform the user the report has been generated
                    MessageBox.Show("Report generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error generating report: " + ex.Message);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count == 0)
            {
                MessageBox.Show("No data available to generate a report.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Workbook|*.xlsx";
            saveFileDialog.Title = "Save Teacher Report";
            saveFileDialog.FileName = "TeacherReport_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (ExcelPackage package = new ExcelPackage())
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Teacher Report");

                        worksheet.Cells[1, 1].Value = "Teacher Report";
                        worksheet.Cells[1, 1].Style.Font.Bold = true;
                        worksheet.Cells[1, 1].Style.Font.Size = 16;
                        worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        worksheet.Cells[2, 1].Value = "Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        worksheet.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                        for (int i = 0; i < dataGridView2.Columns.Count; i++)
                        {
                            worksheet.Cells[4, i + 1].Value = dataGridView2.Columns[i].HeaderText;
                            worksheet.Cells[4, i + 1].Style.Font.Bold = true;
                            worksheet.Cells[4, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }

                        for (int i = 0; i < dataGridView2.Rows.Count; i++)
                        {
                            DataGridViewRow row = dataGridView2.Rows[i];

                            if (row.IsNewRow) continue;

                            for (int j = 0; j < dataGridView2.Columns.Count; j++)
                            {
                                worksheet.Cells[i + 5, j + 1].Value = row.Cells[j].Value?.ToString();
                                worksheet.Cells[i + 5, j + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            }
                        }

                        for (int i = 0; i < dataGridView2.Columns.Count; i++)
                        {
                            worksheet.Column(i + 1).AutoFit();
                        }

                        FileInfo fi = new FileInfo(saveFileDialog.FileName);
                        package.SaveAs(fi);
                    }

                    MessageBox.Show("Teacher report generated successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error generating report: " + ex.Message);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            AttendanceQRForm form = new AttendanceQRForm();
            form.ShowDialog();
        }
    }
}
