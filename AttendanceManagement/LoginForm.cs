using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AttendanceManagement
{
    public partial class LoginForm : Form
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

        public LoginForm()
        {
            InitializeComponent();
            con.ConnectionString = @"server=localhost;database=user_info;userid=root;password=;";
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to the database: {ex.Message}");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selectedRole = login_list.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedRole))
            {
                MessageBox.Show("Please select a role before logging in.");
                return;
            }

            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;

                if (selectedRole == "Admin")
                {
                    cmd.CommandText = "SELECT * FROM login_acc WHERE username = @username AND password = @password";
                }
                else if (selectedRole == "Professor")
                {
                    cmd.CommandText = "SELECT * FROM professor_acc WHERE username = @username AND password = @password";
                }
                else if (selectedRole == "Student")
                {
                    cmd.CommandText = "SELECT * FROM student_acc WHERE username = @username AND password = @password";
                }

                cmd.Parameters.AddWithValue("@username", user_text.Text);
                cmd.Parameters.AddWithValue("@password", password_text.Text);

                using (MySqlDataReader rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        if (selectedRole == "Admin")
                        {
                            MessageBox.Show("Welcome Admin");
                            this.Hide();
                            MainForm adminForm = new MainForm();
                            adminForm.ShowDialog();
                        }
                        else if (selectedRole == "Professor")
                        {
                            string professorid = rd["id_no"].ToString();
                            MessageBox.Show("Welcome Professor");
                            this.Hide();
                            ProfessorDashboard dashboard = new ProfessorDashboard(professorid);
                            dashboard.ShowDialog();
                        }
                        else if (selectedRole == "Student")
                        {
                            string studentid = rd["studentid"].ToString();
                            MessageBox.Show("Welcome Student");
                            this.Hide();
                            StudentDashboard studentForm = new StudentDashboard(studentid);
                            studentForm.ShowDialog();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Incorrect Username or Password");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during login: {ex.Message}");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void user_text_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void password_text_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

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
    }
}
