using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace AttendanceManagement
{
    public partial class StudentAccountForm : Form
    {
        private string studentid;
        private MySqlConnection con;

        public StudentAccountForm(string studentid = null)
        {
            InitializeComponent();
            this.studentid = studentid;

            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(UpdateTimeLabel);
            timer1.Start();

            UpdateTimeLabel(null, null);
            UpdateDateLabel(null, null);

            con = new MySqlConnection(@"server=localhost;database=user_info;userid=root;password=;");
        }

        private void StudentAccountForm_Load(object sender, EventArgs e)
        {
            LoadStudentDataAndProfile();
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
                    cmd.Parameters.AddWithValue("@studentid", studentid);

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

        private void LoadStudentDataAndProfile()
        {
            if (string.IsNullOrEmpty(studentid))
            {
                MessageBox.Show("No student ID provided.");
                return;
            }

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
                            id_text.Text = reader["studentid"].ToString();
                            name_text.Text = reader["fname"].ToString();
                            birthdate_cal.Value = Convert.ToDateTime(reader["birthdate"]);
                            contact_text.Text = reader["contact"].ToString();
                            email_text.Text = reader["email"].ToString();
                            religion_list.Text = reader["religion"].ToString();
                            citizen_list.Text = reader["citizen"].ToString();
                            address_text.Text = reader["faddress"].ToString();
                            course_list.Text = reader["course"].ToString();
                            year_list.Text = reader["year"].ToString();
                            section_list.Text = reader["section"].ToString();

                            string gender = reader["gender"].ToString();
                            if (gender == "Male")
                            {
                                radioButton1.Checked = true;
                            }
                            else
                            {
                                radioButton2.Checked = true;
                            }

                            if (reader["photo"] != DBNull.Value)
                            {
                                byte[] photoData = (byte[])reader["photo"];
                                using (MemoryStream ms = new MemoryStream(photoData))
                                {
                                    Image studentImage = Image.FromStream(ms);
                                    pictureBox1.Image = studentImage;
                                    pictureBox3.Image = studentImage;
                                }
                            }
                            else
                            {
                                pictureBox1.Image = null;
                                pictureBox3.Image = null;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Student not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading student data: {ex.Message}");
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

        private void home_btn_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentDashboard studentDashboard = new StudentDashboard(studentid);
            studentDashboard.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentAttendanceRecord sar = new StudentAttendanceRecord(studentid);
            sar.ShowDialog();
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
                        coman.Parameters.AddWithValue("@photo", DBNull.Value);
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
                    catch (OutOfMemoryException)
                    {
                        MessageBox.Show("The selected file is not a valid image format.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading image: " + ex.Message);
                    }
                }
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

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Logout Confirmation", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                LoginForm signinForm = new LoginForm();
                signinForm.ShowDialog();
            }
        }
    }
}
