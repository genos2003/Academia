using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using QRCoder;

namespace AttendanceManagement
{
    public partial class StudentRegistrationForm : Form
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
        public StudentRegistrationForm()
        {
            InitializeComponent();
            con.ConnectionString = @"server=localhost;database=user_info;userid=root;password=;";
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));

            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(UpdateTimeLabel);
            timer1.Start();

            UpdateTimeLabel(null, null);
            UpdateDateLabel(null,null);
        }

        private string Gender;
        private void UpdateTimeLabel(object sender, EventArgs e)
        {
            time_label.Text = DateTime.Now.ToLongTimeString();
        }
        private void UpdateDateLabel(object sender, EventArgs e)
        {
            date_label.Text = DateTime.Now.ToLongDateString();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(id_text.Text) ||
                string.IsNullOrWhiteSpace(username_text.Text) ||
                string.IsNullOrWhiteSpace(password_text.Text) ||
                string.IsNullOrWhiteSpace(name_text.Text) ||
                string.IsNullOrWhiteSpace(Gender) ||
                string.IsNullOrWhiteSpace(birthdate_cal.Text) ||
                string.IsNullOrWhiteSpace(religion_list.Text) ||
                string.IsNullOrWhiteSpace(citizen_list.Text) ||
                string.IsNullOrWhiteSpace(address_text.Text) ||
                string.IsNullOrWhiteSpace(contact_text.Text) ||
                string.IsNullOrWhiteSpace(email_text.Text) ||
                string.IsNullOrWhiteSpace(course_list.Text) ||
                string.IsNullOrWhiteSpace(year_list.Text) ||
                string.IsNullOrWhiteSpace(section_list.Text) ||
                pictureBox1.Image == null)
            {
                MessageBox.Show("Please fill in all fields and select a photo before proceeding.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool exists = false;
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                string checkQuery = "SELECT COUNT(*) FROM student_db WHERE studentid = @studentid";
                using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, con))
                {
                    checkCmd.Parameters.AddWithValue("@studentid", id_text.Text);
                    long count = (long)checkCmd.ExecuteScalar();
                    exists = (count > 0);
                }

                if (exists)
                {
                    MessageBox.Show("Student ID already exists. Please use a different ID.");
                    return;
                }

                // Save student data to database
                string insertUserQuery = "INSERT INTO student_acc (username, password, studentid) VALUES (@username, @password, @studentid)";
                using (MySqlCommand userCmd = new MySqlCommand(insertUserQuery, con))
                {
                    userCmd.Parameters.AddWithValue("@username", username_text.Text);
                    userCmd.Parameters.AddWithValue("@password", password_text.Text);
                    userCmd.Parameters.AddWithValue("@studentid", id_text.Text);
                    userCmd.ExecuteNonQuery();
                }

                QRCoder.QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(id_text.Text, QRCoder.QRCodeGenerator.ECCLevel.H);
                var qrCode = new QRCoder.QRCode(qrCodeData);
                pictureBox2.Image = qrCode.GetGraphic(100);

                using (MemoryStream ms = new MemoryStream())
                {
                    pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] photo = ms.ToArray();

                    string insertStudentQuery = "INSERT INTO student_db (studentid, fname, gender, birthdate, religion, citizen, faddress, contact, email, course, year, section, photo) " +
                                                "VALUES (@studentid, @fname, @gender, @birthdate, @religion, @citizen, @faddress, @contact, @email, @course, @year, @section, @photo)";

                    using (MySqlCommand studentCmd = new MySqlCommand(insertStudentQuery, con))
                    {
                        studentCmd.Parameters.AddWithValue("@studentid", id_text.Text);
                        studentCmd.Parameters.AddWithValue("@fname", name_text.Text);
                        studentCmd.Parameters.AddWithValue("@gender", Gender);
                        studentCmd.Parameters.AddWithValue("@birthdate", birthdate_cal.Text);
                        studentCmd.Parameters.AddWithValue("@religion", religion_list.Text);
                        studentCmd.Parameters.AddWithValue("@citizen", citizen_list.Text);
                        studentCmd.Parameters.AddWithValue("@faddress", address_text.Text);
                        studentCmd.Parameters.AddWithValue("@contact", contact_text.Text);
                        studentCmd.Parameters.AddWithValue("@email", email_text.Text);
                        studentCmd.Parameters.AddWithValue("@course", course_list.Text);
                        studentCmd.Parameters.AddWithValue("@year", year_list.Text);
                        studentCmd.Parameters.AddWithValue("@section", section_list.Text);
                        studentCmd.Parameters.AddWithValue("@photo", photo);

                        studentCmd.ExecuteNonQuery();
                    }
                }

                GenerateIDCard(name_text.Text, id_text.Text, pictureBox2.Image);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }

            SaveQRCodeImage();
        }

        private void GenerateIDCard(string studentName, string studentId, Image qrCodeImage, int qrCodeSize = 1300, int photoOffsetX = 0, int photoOffsetY = 0)
        {
            string frontIdPath = @"C:\Users\Karen Baguhin\source\repos\AttendanceManagement\Resources\idcardfront.png";
            string backIdPath = @"C:\Users\Karen Baguhin\source\repos\AttendanceManagement\Resources\idcardback.png";

            Bitmap frontId = null;
            Bitmap backId = null;

            try
            {
                frontId = new Bitmap(frontIdPath);
                backId = new Bitmap(backIdPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading images: {ex.Message}");
                return;
            }

            if (frontId == null || backId == null)
            {
                MessageBox.Show("Error: One or both images failed to load.");
                return;
            }

            using (Graphics g = Graphics.FromImage(frontId))
            {
                Font nameFont = new Font("Nirmala UI", 12, FontStyle.Bold);
                Font idFont = new Font("Nirmala UI", 12, FontStyle.Regular);
                Brush brush = Brushes.White;

                if (pictureBox1.Image != null)
                {
                    int photoWidth = 1000;
                    int photoHeight = 1000;

                    int photoX = (frontId.Width - photoWidth) / 2 + photoOffsetX;
                    int photoY = (frontId.Height - photoHeight) / 2 - photoHeight / 2 + photoOffsetY + 75;

                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddEllipse(photoX, photoY, photoWidth, photoHeight);
                        g.SetClip(path);
                        g.DrawImage(pictureBox1.Image, new Rectangle(photoX, photoY, photoWidth, photoHeight));
                        g.ResetClip();
                    }

                    float nameY = photoY + photoHeight + 100;
                    float idY = nameY + 350;

                    SizeF nameSize = g.MeasureString(studentName, nameFont);
                    SizeF idSize = g.MeasureString(studentId, idFont);

                    float nameX = (frontId.Width - nameSize.Width) / 2;
                    float idX = (frontId.Width - idSize.Width) / 2;

                    g.DrawString(studentName, nameFont, brush, new PointF(nameX, nameY));
                    g.DrawString(studentId, idFont, brush, new PointF(idX, idY));
                }

                string frontFilePath = $@"C:\Users\Karen Baguhin\Desktop\STUDENT ID\{studentId}_front.png";
                frontId.Save(frontFilePath, System.Drawing.Imaging.ImageFormat.Png);
            }

            using (Graphics gBack = Graphics.FromImage(backId))
            {
                if (qrCodeImage != null)
                {
                    int qrCodeWidth = qrCodeSize;
                    int qrCodeHeight = qrCodeSize;

                    int qrCodeX = (backId.Width - qrCodeWidth) / 2;
                    int qrCodeY = (backId.Height - qrCodeHeight) / 2 + 100;

                    gBack.DrawImage(qrCodeImage, new Rectangle(qrCodeX, qrCodeY, qrCodeWidth, qrCodeHeight));
                }
                else
                {
                    MessageBox.Show("Error: QR code image is null.");
                    return;
                }
            }

            string backFilePath = $@"C:\Users\Karen Baguhin\Desktop\STUDENT ID\{studentId}_back.png";
            backId.Save(backFilePath, System.Drawing.Imaging.ImageFormat.Png);

            MessageBox.Show("ID card generated successfully!");
        }
        private void SaveQRCodeImage()
        {
            string initialDirectory = @"C:\Users\Karen Baguhin\Desktop\STUDENT QR";
            using (SaveFileDialog dialog = new SaveFileDialog
            {
                InitialDirectory = initialDirectory,
                Filter = "PNG |*.png|JPEG|*.jpeg|BMP|*.bmp|GIF|*.gif"
            })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox2.Image.Save(dialog.FileName);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentInformationForm sif = new StudentInformationForm();
            sif.ShowDialog();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Hide();
            using (MainForm mf = new MainForm())
            {
                mf.ShowDialog();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Gender = "Male";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Gender = "Female";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fd = new OpenFileDialog())
            {
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(fd.FileName);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            AttendanceRecordForm arf = new AttendanceRecordForm();
            arf.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm lf = new LoginForm();
            lf.ShowDialog();
        }

        private void StudentRegistrationForm_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            AttendanceQRForm aqrf = new AttendanceQRForm();
            aqrf.ShowDialog();
        }

        private void username_text_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void password_text_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void id_text_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void name_text_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void birthdate_cal_ValueChanged(object sender, EventArgs e)
        {

        }

        private void religion_list_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void citizen_list_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void section_list_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void year_list_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void course_list_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void email_text_TextChanged(object sender, EventArgs e)
        {

        }

        private void contact_text_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            ClearAllControls(this);

            Gender = string.Empty;
        }

        private void ClearAllControls(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is TextBox)
                {
                    username_text.Text = string.Empty;
                    password_text.Text = string.Empty;
                    id_text.Text = string.Empty;
                    name_text.Text = string.Empty;
                    address_text.Text = string.Empty;
                    contact_text.Text = string.Empty;
                    email_text.Text = string.Empty;
                }
                else if (control is ComboBox)
                {
                    ((ComboBox)control).SelectedIndex = -1;
                }
                else if (control is PictureBox)
                {
                    ((PictureBox)control).Image = null;
                }
                else if (control is RadioButton)
                {
                    ((RadioButton)control).Checked = false;
                }
                else if (control is DateTimePicker)
                {
                    ((DateTimePicker)control).Value = DateTime.Now;
                }
                else if (control.HasChildren)
                {
                    ClearAllControls(control);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Hide();
            AttendanceQRForm attendanceQRForm = new AttendanceQRForm();
            attendanceQRForm.ShowDialog();
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            ManageTeacherForm manageteacherform = new ManageTeacherForm();
            manageteacherform.ShowDialog();
        }

        private void home_btn_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
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

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {

        }
    }
}
