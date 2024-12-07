using MySql.Data.MySqlClient;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace AttendanceManagement
{
    public partial class StudentEditingForm : Form
    {
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public string Birthdate { get; set; }
        public string Religion { get; set; }
        public string Citizen { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Course { get; set; }
        public string Year { get; set; }
        public string Section { get; set; }
        public Image Photo { get; set; }

        public StudentEditingForm()
        {
            InitializeComponent();
        }

        private void StudentEditingForm_Load(object sender, EventArgs e)
        {
            LoadStudentData(StudentId);
            GenerateQRCode(StudentId);
        }

        private void LoadStudentData(string studentId)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(@"server=localhost;database=user_info;userid=root;password=;"))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT fname, gender, birthdate, religion, citizen, faddress, contact, email, course, year, section, photo FROM student_db WHERE studentid = @studentid", con);
                    cmd.Parameters.AddWithValue("@studentid", studentId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            id_text.Text = studentId;
                            name_text.Text = reader["fname"].ToString();
                            birthdate_cal.Text = reader["birthdate"].ToString();
                            religion_list.Text = reader["religion"].ToString();
                            citizen_list.Text = reader["citizen"].ToString();
                            address_text.Text = reader["faddress"].ToString();
                            contact_text.Text = reader["contact"].ToString();
                            email_text.Text = reader["email"].ToString();
                            course_list.Text = reader["course"].ToString();
                            year_list.Text = reader["year"].ToString();
                            section_list.Text = reader["section"].ToString();

                            string gender = reader["gender"].ToString();
                            radioButton1.Checked = gender == "Male";
                            radioButton2.Checked = gender == "Female";

                            if (reader["photo"] != DBNull.Value)
                            {
                                byte[] imageBytes = (byte[])reader["photo"];
                                using (MemoryStream ms = new MemoryStream(imageBytes))
                                {
                                    Photo = Image.FromStream(ms);
                                    pictureBox1.Image = Photo;
                                }
                            }
                            else
                            {
                                pictureBox1.Image = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading student data: " + ex.Message);
            }
        }

        private void GenerateQRCode(string studentId)
        {
            try
            {
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                {
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(studentId, QRCodeGenerator.ECCLevel.Q);
                    using (QRCode qrCode = new QRCode(qrCodeData))
                    {
                        Bitmap qrCodeImage = qrCode.GetGraphic(20);

                        pictureBox2.Image = qrCodeImage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating QR code: " + ex.Message);
            }
        }


        private void button6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(name_text.Text) || string.IsNullOrWhiteSpace(contact_text.Text) || string.IsNullOrWhiteSpace(email_text.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            try
            {
                using (MySqlConnection con = new MySqlConnection(@"server=localhost;database=user_info;userid=root;password=;"))
                {
                    con.Open();
                    MySqlCommand coman = new MySqlCommand
                    {
                        Connection = con,
                        CommandText = "UPDATE student_db SET fname=@fname, gender=@gender, birthdate=@birthdate, religion=@religion, citizen=@citizen, faddress=@faddress, contact=@contact, email=@email, course=@course, year=@year, section=@section, photo=@photo WHERE studentid=@studentid"
                    };

                    coman.Parameters.AddWithValue("@studentid", StudentId);
                    coman.Parameters.AddWithValue("@fname", name_text.Text);
                    coman.Parameters.AddWithValue("@gender", radioButton1.Checked ? "Male" : "Female");
                    coman.Parameters.AddWithValue("@birthdate", birthdate_cal.Text);
                    coman.Parameters.AddWithValue("@religion", religion_list.Text);
                    coman.Parameters.AddWithValue("@citizen", citizen_list.Text);
                    coman.Parameters.AddWithValue("@faddress", address_text.Text);
                    coman.Parameters.AddWithValue("@contact", contact_text.Text);
                    coman.Parameters.AddWithValue("@email", email_text.Text);
                    coman.Parameters.AddWithValue("@course", course_list.Text);
                    coman.Parameters.AddWithValue("@year", year_list.Text);
                    coman.Parameters.AddWithValue("@section", section_list.Text);

                    if (pictureBox1.Image != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            pictureBox1.Image.Save(ms, ImageFormat.Jpeg);
                            byte[] imageBytes = ms.ToArray();
                            coman.Parameters.AddWithValue("@photo", imageBytes);
                        }
                    }
                    else
                    {
                        byte[] existingPhoto = GetExistingPhoto(con, StudentId);
                        coman.Parameters.AddWithValue("@photo", existingPhoto != null ? (object)existingPhoto : DBNull.Value);
                    }

                    coman.ExecuteNonQuery();
                    MessageBox.Show("Data updated successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating data: " + ex.Message);
            }
        }

        private byte[] GetExistingPhoto(MySqlConnection con, string studentId)
        {
            using (MySqlCommand cmd = new MySqlCommand("SELECT photo FROM student_db WHERE studentid = @studentid", con))
            {
                cmd.Parameters.AddWithValue("@studentid", studentId);
                object result = cmd.ExecuteScalar();
                return result != DBNull.Value ? (byte[])result : null;
            }
        }

        private void remove_btn_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
        }

        private void addphoto_btn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                openFileDialog.Title = "Select a Photo";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading image: " + ex.Message);
                    }
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void button1_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to remove this student?",
                                                 "Confirm Remove",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                RemoveStudentFromDatabase(StudentId);
            }
        }

        private void RemoveStudentFromDatabase(string studentId)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(@"server=localhost;database=user_info;userid=root;password=;"))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM student_db WHERE studentid = @studentid", con);
                    cmd.Parameters.AddWithValue("@studentid", studentId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Student removed successfully.");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Student not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error removing student: " + ex.Message);
            }
        }
    }
}
