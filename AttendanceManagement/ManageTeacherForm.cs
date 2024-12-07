using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRCoder;

namespace AttendanceManagement
{
    public partial class ManageTeacherForm : Form
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

        public ManageTeacherForm()
        {
            InitializeComponent();
            con.ConnectionString = @"server=localhost;database=user_info;userid=root;password=;";
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
            GenerateUniqueId();

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

        private string Gender;
        private byte[] photo;

        private void GenerateUniqueId()
        {
            Random random = new Random();
            string id;

            do
            {
                string year = random.Next(10, 100).ToString();
                string month = random.Next(1, 13).ToString("D2");
                string day = random.Next(1, 32).ToString("D2");

                id = string.Format("{0}-{1}-{2}", day, month, "20" + year);
            }
            while (!IsUniqueId(id));

            id_text.Text = id;
        }

        private bool IsUniqueId(string id)
        {
            string query = "SELECT COUNT(*) FROM teacher_db WHERE id_no = @id";
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count == 0; // If count is 0, the ID is unique
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking ID uniqueness: " + ex.Message);
                return false;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void ManageTeacherForm_Load(object sender, EventArgs e)
        {
            LoadTeacherData();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            string id = id_text.Text;
            string name = name_text.Text;
            string contact = contact_text.Text;
            string email = email_text.Text;
            string department = section_list.SelectedItem?.ToString();
            string username = username_text.Text;
            string password = password_text.Text;

            Gender = radioButton1.Checked ? "Male" : radioButton2.Checked ? "Female" : null;

            string checkQuery = "SELECT COUNT(*) FROM teacher_db WHERE id_no = @id";

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                using (MySqlCommand cmdCheck = new MySqlCommand(checkQuery, con))
                {
                    cmdCheck.Parameters.AddWithValue("@id", id);
                    int existingRecords = Convert.ToInt32(cmdCheck.ExecuteScalar());

                    if (existingRecords > 0)
                    {
                        MessageBox.Show("This ID number already exist.");
                        return;
                    }
                }

                if (photo == null)
                {
                    MessageBox.Show("Please select a photo before saving.");
                    return;
                }

                string query1 = "INSERT INTO teacher_db (id_no, name, gender, department, email, contact, photo) VALUES (@id, @name, @gender, @department, @email, @contact, @photo)";
                string query2 = "INSERT INTO professor_acc (username, password, id_no) VALUES (@username, @password, @id)";

                using (MySqlCommand cmd1 = new MySqlCommand(query1, con))
                {
                    cmd1.Parameters.AddWithValue("@id", id);
                    cmd1.Parameters.AddWithValue("@name", name);
                    cmd1.Parameters.AddWithValue("@gender", Gender);
                    cmd1.Parameters.AddWithValue("@department", department);
                    cmd1.Parameters.AddWithValue("@email", email);
                    cmd1.Parameters.AddWithValue("@contact", contact);
                    cmd1.Parameters.AddWithValue("@photo", photo.Length > 0 ? photo : (object)DBNull.Value);

                    cmd1.ExecuteNonQuery();
                }

                using (MySqlCommand cmd2 = new MySqlCommand(query2, con))
                {
                    cmd2.Parameters.AddWithValue("@username", username);
                    cmd2.Parameters.AddWithValue("@password", password);
                    cmd2.Parameters.AddWithValue("@id", id);

                    cmd2.ExecuteNonQuery();
                }

                MessageBox.Show("Teacher data and account saved successfully!");

                GenerateUniqueId();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }


        private void GenerateQRCode(string idNo)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(idNo, QRCodeGenerator.ECCLevel.Q);

                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    Bitmap qrCodeImage = qrCode.GetGraphic(100, Color.Black, Color.White, true);

                    int borderSize = 5;
                    Bitmap croppedImage = new Bitmap(qrCodeImage.Width - borderSize * 2, qrCodeImage.Height - borderSize * 2);

                    using (Graphics g = Graphics.FromImage(croppedImage))
                    {
                        g.Clear(Color.White);
                        g.DrawImage(qrCodeImage, new Rectangle(0, 0, croppedImage.Width, croppedImage.Height),
                                    new Rectangle(borderSize, borderSize, croppedImage.Width, croppedImage.Height),
                                    GraphicsUnit.Pixel);
                    }

                    string qrCodePath = @"C:\Users\Karen Baguhin\Desktop\libraryqrcode\" + idNo + "_QR.png";
                    croppedImage.Save(qrCodePath, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }

        private void GenerateIDCard(string idNo, string name)
        {
            string frontIdPath = @"C:\Users\Karen Baguhin\source\repos\AttendanceManagement\Resources\1.png";
            string backIdPath = @"C:\Users\Karen Baguhin\source\repos\AttendanceManagement\Resources\2.png";

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
                Font nameFont = new Font("Nirmala UI", 15, FontStyle.Bold);
                Font idFont = new Font("Nirmala UI", 12, FontStyle.Regular);
                Brush brush = Brushes.Black;

                if (pictureBox1.Image != null)
                {
                    int photoWidth = 1000;
                    int photoHeight = 1000;

                    int photoX = (frontId.Width - photoWidth) / 2;
                    int photoY = (frontId.Height - photoHeight) / 2 - photoHeight / 2 + 75;

                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddEllipse(photoX, photoY, photoWidth, photoHeight);
                        g.SetClip(path);
                        g.DrawImage(pictureBox1.Image, new Rectangle(photoX, photoY, photoWidth, photoHeight));
                        g.ResetClip();
                    }

                    float nameY = photoY + photoHeight + 100;
                    float idY = nameY + 350;

                    SizeF nameSize = g.MeasureString(name, nameFont);
                    SizeF idSize = g.MeasureString(idNo, idFont);

                    float nameX = (frontId.Width - nameSize.Width) / 2;
                    float idX = (frontId.Width - idSize.Width) / 2;

                    g.DrawString(name, nameFont, brush, new PointF(nameX, nameY));
                    g.DrawString(idNo, idFont, brush, new PointF(idX, idY));
                }
                else
                {
                    MessageBox.Show("No photo available in pictureBox1.");
                }
            }

            string qrCodeImagePath = @"C:\Users\Karen Baguhin\Desktop\libraryqrcode\" + idNo + "_QR.png";
            if (!File.Exists(qrCodeImagePath))
            {
                MessageBox.Show("QR code image not found at path: " + qrCodeImagePath);
                return;
            }

            using (Image qrCodeImage = Image.FromFile(qrCodeImagePath))
            {
                using (Graphics gBack = Graphics.FromImage(backId))
                {
                    int qrCodeSize = 1400;
                    int qrCodeWidth = qrCodeSize;
                    int qrCodeHeight = qrCodeSize;

                    int qrCodeX = (backId.Width - qrCodeWidth) / 2;
                    int qrCodeY = (backId.Height - qrCodeHeight) / 2 + 100;

                    gBack.DrawImage(qrCodeImage, new Rectangle(qrCodeX, qrCodeY, qrCodeWidth, qrCodeHeight));
                }
            }

            string frontIdSavePath = @"C:\Users\Karen Baguhin\Desktop\PROFESSOR ID\" + idNo + "_FrontID.png";
            string backIdSavePath = @"C:\Users\Karen Baguhin\Desktop\PROFESSOR ID\" + idNo + "_BackID.png";

            frontId.Save(frontIdSavePath, System.Drawing.Imaging.ImageFormat.Png);
            backId.Save(backIdSavePath, System.Drawing.Imaging.ImageFormat.Png);

            MessageBox.Show("ID card generated successfully!");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(fd.FileName);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        photo = ms.ToArray();
                    }
                }
            }
        }

        private void LoadTeacherData()
        {
            string query = "SELECT id_no, name, gender, contact, email, department, photo FROM teacher_db";

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    byte[] photoData = row["photo"] as byte[];
                    Image img = null;
                    if (photoData != null && photoData.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(photoData))
                        {
                            img = Image.FromStream(ms);
                        }
                    }

                    int rowIndex = dataGridView1.Rows.Add(
                        row["id_no"],
                        row["name"],
                        row["gender"],
                        row["department"],
                        row["email"],
                        row["contact"]
                    );

                    dataGridView1.Rows[rowIndex].Cells["Column7"].Value = img;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                string teacherId = row.Cells["Column1"].Value.ToString(); // ID

                id_text.Text = teacherId; // ID
                name_text.Text = row.Cells["Column2"].Value.ToString(); // Name
                contact_text.Text = row.Cells["Column5"].Value.ToString(); // Contact
                email_text.Text = row.Cells["Column4"].Value.ToString(); // Email
                section_list.SelectedItem = row.Cells["Column3"].Value.ToString(); // Department

                // Check if the image cell contains a valid image
                if (row.Cells["Column7"].Value is Image)
                {
                    pictureBox1.Image = (Image)row.Cells["Column7"].Value;
                }
                else
                {
                    pictureBox1.Image = null; // No image found
                }

                // Set gender radio buttons
                if (row.Cells["Column6"].Value.ToString() == "Male")
                {
                    radioButton1.Checked = true;
                }
                else if (row.Cells["Column6"].Value.ToString() == "Female")
                {
                    radioButton2.Checked = true;
                }

                // Now retrieve username and password from professor_acc table using id_no
                string query = "SELECT username, password FROM professor_acc WHERE id_no = @id";

                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", teacherId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                username_text.Text = reader["username"].ToString();
                                password_text.Text = reader["password"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("No account found for the selected teacher.");
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
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
        }

            private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            AttendanceRecordForm form = new AttendanceRecordForm();
            form.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentInformationForm form = new StudentInformationForm();
            form.ShowDialog();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentRegistrationForm studentRegistrationForm = new StudentRegistrationForm();
            studentRegistrationForm.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            AttendanceQRForm form = new AttendanceQRForm();
            form.ShowDialog();
        }
        private void home_btn_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.ShowDialog();
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void remove_btn_Click(object sender, EventArgs e)
        {
            // Get the ID from the id_text textbox
            string selectedTeacherId = id_text.Text.Trim();

            if (string.IsNullOrEmpty(selectedTeacherId))
            {
                MessageBox.Show("Please enter an ID to remove.");
                return;
            }

            // Check if the teacher exists
            string checkTeacherExistsQuery = "SELECT COUNT(*) FROM teacher_db WHERE id_no = @id";

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                using (MySqlCommand checkCmd = new MySqlCommand(checkTeacherExistsQuery, con))
                {
                    checkCmd.Parameters.AddWithValue("@id", selectedTeacherId);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    // If the teacher does not exist, show a message and return
                    if (count == 0)
                    {
                        MessageBox.Show("No teacher found with the specified ID.");
                        return;
                    }
                }

                // Confirm deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete this teacher?",
                                                      "Delete Confirmation",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // SQL queries to delete from both tables
                    string deleteFromTeacherDbQuery = "DELETE FROM teacher_db WHERE id_no = @id";
                    string deleteFromProfessorAccQuery = "DELETE FROM professor_acc WHERE id_no = @id";

                    // Delete from teacher_db
                    using (MySqlCommand cmd1 = new MySqlCommand(deleteFromTeacherDbQuery, con))
                    {
                        cmd1.Parameters.AddWithValue("@id", selectedTeacherId);
                        cmd1.ExecuteNonQuery();
                    }

                    // Delete from professor_acc
                    using (MySqlCommand cmd2 = new MySqlCommand(deleteFromProfessorAccQuery, con))
                    {
                        cmd2.Parameters.AddWithValue("@id", selectedTeacherId);
                        cmd2.ExecuteNonQuery();
                    }

                    // Refresh the DataGridView to reflect changes
                    LoadTeacherData();
                    MessageBox.Show("Teacher removed successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void update_btn_Click(object sender, EventArgs e)
        {
            // Step 1: Retrieve updated data from the form fields
            string teacherId = id_text.Text;
            string name = name_text.Text;
            string contact = contact_text.Text;
            string email = email_text.Text;
            string department = section_list.SelectedItem?.ToString();
            string gender = radioButton1.Checked ? "Male" : "Female"; // Assuming radio buttons for gender selection
            string username = username_text.Text;
            string password = password_text.Text;

            // Check if all fields are filled
            if (string.IsNullOrEmpty(teacherId) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(contact) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(department) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill all the fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Step 2: Prepare the SQL update queries
            string updateTeacherQuery = "UPDATE teacher_db SET name = @name, contact = @contact, email = @email, department = @department, gender = @gender WHERE id_no = @id";
            string updateProfessorQuery = "UPDATE professor_acc SET username = @username, password = @password WHERE id_no = @id";

            try
            {
                // Step 3: Execute the update queries
                using (MySqlCommand cmd1 = new MySqlCommand(updateTeacherQuery, con))
                {
                    cmd1.Parameters.AddWithValue("@id", teacherId);
                    cmd1.Parameters.AddWithValue("@name", name);
                    cmd1.Parameters.AddWithValue("@contact", contact);
                    cmd1.Parameters.AddWithValue("@email", email);
                    cmd1.Parameters.AddWithValue("@department", department);
                    cmd1.Parameters.AddWithValue("@gender", gender);

                    // Open connection and execute the teacher update query
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    int rowsAffectedTeacher = cmd1.ExecuteNonQuery();

                    if (rowsAffectedTeacher > 0)
                    {
                        // Now update the professor account if teacher info was successfully updated
                        using (MySqlCommand cmd2 = new MySqlCommand(updateProfessorQuery, con))
                        {
                            cmd2.Parameters.AddWithValue("@username", username);
                            cmd2.Parameters.AddWithValue("@password", password);
                            cmd2.Parameters.AddWithValue("@id", teacherId);

                            int rowsAffectedProfessor = cmd2.ExecuteNonQuery();

                            if (rowsAffectedProfessor > 0)
                            {
                                MessageBox.Show("Teacher and account information updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Username and password update failed. Please check the details.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Teacher not found or no changes made.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., database errors)
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Step 5: Close the connection
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

    }
}
