using AForge.Video.DirectShow;
using AForge.Video;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System;
using ZXing;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace AttendanceManagement
{
    public partial class AttendanceQRForm : Form
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

        private MySqlConnection con;
        private FilterInfoCollection CaptureDevice;
        private VideoCaptureDevice FinalFrame;
        private VideoCaptureDevice FinalFrameTeacher;

        public AttendanceQRForm()
        {
            InitializeComponent();
            con = new MySqlConnection(@"server=localhost;database=user_info;userid=root;password=;");
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));

            timer3.Interval = 1000;
            timer3.Tick += new EventHandler(UpdateTimeLabel);
            timer3.Start();

            UpdateTimeLabel(null, null);
            UpdateDateLabel(null, null);
        }

        private void UpdateTimeLabel(object sender, EventArgs e)
        {
            time_label.Text = DateTime.Now.ToLongTimeString();
        }

        private void UpdateDateLabel(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToLongDateString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FinalFrame = new VideoCaptureDevice(CaptureDevice[comboBox1.SelectedIndex].MonikerString);
            FinalFrame.NewFrame += new NewFrameEventHandler(FinalFrame_NewFrame);
            FinalFrame.Start();
        }

        private void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox3.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void AttendanceQRForm_Load(object sender, EventArgs e)
        {
            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in CaptureDevice)
                comboBox1.Items.Add(device.Name);
            comboBox1.SelectedIndex = 0;

            comboBox2.Items.Add("Time In");
            comboBox2.Items.Add("Time Out");
            comboBox2.SelectedIndex = 0;

            date_label.Text = DateTime.Now.ToString("dd/MM/yyyy");
            time_label.Text = DateTime.Now.ToLongTimeString();
        }

        private void AttendanceQRForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FinalFrame.IsRunning)
                FinalFrame.Stop();
        }

        private bool isProcessingQR = false;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isProcessingQR) return;

            BarcodeReader reader = new BarcodeReader();
            Result result = reader.Decode((Bitmap)pictureBox3.Image);

            if (result != null)
            {
                string decode = result.ToString().Trim();
                id_text.Text = decode;

                isProcessingQR = true;

                try
                {
                    if (con.State != System.Data.ConnectionState.Open)
                    {
                        con.Open();
                    }

                    MySqlCommand coman = new MySqlCommand
                    {
                        Connection = con,
                        CommandText = "SELECT * FROM student_db WHERE studentid = @studentid"
                    };
                    coman.Parameters.AddWithValue("@studentid", decode);

                    using (MySqlDataReader dr = coman.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            name_text.Text = dr["fname"].ToString();
                            course_text.Text = dr["course"].ToString();
                            year_text.Text = dr["year"].ToString();
                            section_text.Text = dr["section"].ToString();

                            if (string.IsNullOrEmpty(name_text.Text) ||
                                string.IsNullOrEmpty(course_text.Text) ||
                                string.IsNullOrEmpty(year_text.Text) ||
                                string.IsNullOrEmpty(section_text.Text))
                            {
                                MessageBox.Show("Invalid QR code: Some information is missing.");
                                return;
                            }

                            byte[] img = (byte[])dr["photo"];
                            MemoryStream ms = new MemoryStream(img);
                            pictureBox2.Image = Image.FromStream(ms);
                        }
                        else
                        {
                            MessageBox.Show("Invalid QR code: Student not found.");
                            timer1.Stop();
                            return;
                        }
                    }

                    timer1.Stop();

                    timer2.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }

                    isProcessingQR = false;
                }
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();

            timer1.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                timer2.Stop();

                if (subjec_list.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a subject.");
                    return;
                }

                DateTime date;
                if (!DateTime.TryParseExact(date_label.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out date))
                {
                    MessageBox.Show("Invalid date format.");
                    return;
                }

                if (con == null)
                {
                    MessageBox.Show("Database connection is not initialized.");
                    return;
                }

                con.Open();

                MySqlCommand checkCommand = new MySqlCommand
                {
                    Connection = con,
                    CommandText = "SELECT COUNT(*) FROM attendance_db WHERE studentid = @studentid AND date = @date AND subject = @subject"
                };
                checkCommand.Parameters.AddWithValue("@studentid", id_text.Text);
                checkCommand.Parameters.AddWithValue("@date", date);
                checkCommand.Parameters.AddWithValue("@subject", subjec_list.SelectedItem.ToString());

                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Attendance already recorded for this subject today.");
                    return;
                }

                if (pictureBox2.Image == null)
                {
                    MessageBox.Show("No photo available to save.");
                    return;
                }

                MemoryStream ms = new MemoryStream();
                pictureBox2.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] photo = ms.ToArray();

                MySqlCommand coman = new MySqlCommand
                {
                    Connection = con,
                    CommandText = "INSERT INTO attendance_db (studentid, name, subject, course, year, section, date, timein, photo) VALUES (@studentid, @name, @subject, @course, @year, @section, @date, @timein, @photo)"
                };
                coman.Parameters.AddWithValue("@studentid", id_text.Text);
                coman.Parameters.AddWithValue("@name", name_text.Text);
                coman.Parameters.AddWithValue("@subject", subjec_list.SelectedItem.ToString());
                coman.Parameters.AddWithValue("@course", course_text.Text);
                coman.Parameters.AddWithValue("@year", year_text.Text);
                coman.Parameters.AddWithValue("@section", section_text.Text);
                coman.Parameters.AddWithValue("@date", date);
                coman.Parameters.AddWithValue("@timein", time_label.Text);
                coman.Parameters.AddWithValue("@photo", photo);
                coman.ExecuteNonQuery();

                MessageBox.Show("Attendance Recorded!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con != null && con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm mf = new MainForm();
            mf.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageTeacherForm manageteacherform = new ManageTeacherForm();
            manageteacherform.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            AttendanceQRForm attendanceQRForm = new AttendanceQRForm();
            attendanceQRForm.ShowDialog();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void date_label_Click_1(object sender, EventArgs e)
        {

        }

        private void time_label_Click(object sender, EventArgs e)
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

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void id_text_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void name_text_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void course_text_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void year_text_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void section_text_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

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

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void TeacherScanQR_btn_Click(object sender, EventArgs e)
        {
            if (pictureBox7.Image == null) return;

            BarcodeReader reader = new BarcodeReader();
            Result result = reader.Decode((Bitmap)pictureBox7.Image);

            if (result != null)
            {
                string decodedId = result.ToString().Trim();
                idnumber.Text = decodedId;

                try
                {
                    if (con.State != System.Data.ConnectionState.Open)
                    {
                        con.Open();
                    }

                    MySqlCommand coman = new MySqlCommand
                    {
                        Connection = con,
                        CommandText = "SELECT * FROM teacher_db WHERE id_no = @teacherid"
                    };
                    coman.Parameters.AddWithValue("@teacherid", decodedId);

                    using (MySqlDataReader dr = coman.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            teachername_text.Text = dr["name"].ToString();
                            department_text.Text = dr["department"].ToString();
                            email_text.Text = dr["email"].ToString();

                            byte[] img = (byte[])dr["photo"];
                            MemoryStream ms = new MemoryStream(img);
                            teacher_img.Image = Image.FromStream(ms);
                        }
                        else
                        {
                            MessageBox.Show("Invalid QR code: Teacher not found.");
                            return;
                        }
                    }

                    string attendanceType = comboBox2.SelectedItem.ToString();

                    if (attendanceType == "Time In")
                    {
                        if (!CanTimeIn(decodedId))
                        {
                            MessageBox.Show("You have already time in today.");
                            return;
                        }
                    }

                    RecordAttendance(decodedId, attendanceType);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("No valid QR code detected.");
            }
        }

        private bool CanTimeIn(string teacherId)
        {
            using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM teacherattendance_db WHERE id_no = @teacherid AND DATE(timein) = CURDATE()", con))
            {
                cmd.Parameters.AddWithValue("@teacherid", teacherId);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MessageBox.Show($"Found record: {reader["timein"]} on {reader["date"]}");
                        }
                        return false;
                    }
                    return true;
                }
            }
        }

        private void RecordAttendance(string teacherId, string attendanceType)
        {
            string commandText = "";

            if (attendanceType == "Time In")
            {            
                commandText = "INSERT INTO teacherattendance_db (id_no, name, department, email, timein, date) VALUES (@teacherid, @name, @department, @email, @timeIn, @attendanceDate)";
            }
            else if (attendanceType == "Time Out")
            {
                commandText = "UPDATE teacherattendance_db SET timeout = @timeOut WHERE id_no = @teacherid AND DATE(timein) = CURDATE()";
            }

            using (MySqlCommand coman = new MySqlCommand(commandText, con))
            {
                coman.Parameters.AddWithValue("@teacherid", teacherId);

                if (attendanceType == "Time In")
                {
                    string timeIn = DateTime.Now.ToString("HH:mm");
                    coman.Parameters.AddWithValue("@timeIn", timeIn);
                    coman.Parameters.AddWithValue("@name", teachername_text.Text);
                    coman.Parameters.AddWithValue("@department", department_text.Text);
                    coman.Parameters.AddWithValue("@email", email_text.Text);
                    coman.Parameters.AddWithValue("@attendanceDate", DateTime.Now.ToString("yyyy-MM-dd"));
                }
                else if (attendanceType == "Time Out")
                {
                    string timeOut = DateTime.Now.ToString("HH:mm");
                    coman.Parameters.AddWithValue("@timeOut", timeOut);
                }

                coman.ExecuteNonQuery();

                using (MySqlCommand checkCmd = new MySqlCommand("SELECT COUNT(*) FROM teacherattendance_db WHERE id_no = @teacherid AND DATE(timein) = CURDATE()", con))
                {
                    checkCmd.Parameters.AddWithValue("@teacherid", teacherId);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    MessageBox.Show($"Count after recording: {count}");
                }

                MessageBox.Show($"Attendance recorded: {attendanceType} for teacher {teacherId}.");
            }
        }

        private void TeacherOpenCam_btn_Click(object sender, EventArgs e)
        {
            FinalFrameTeacher = new VideoCaptureDevice(CaptureDevice[0].MonikerString);
            FinalFrameTeacher.NewFrame += new NewFrameEventHandler(FinalFrameTeacher_NewFrame);
            FinalFrameTeacher.Start();
        }

        private void FinalFrameTeacher_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox7.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
