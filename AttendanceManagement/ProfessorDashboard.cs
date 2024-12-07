using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AttendanceManagement
{
    public partial class ProfessorDashboard : Form
    {
        private string professorid;


        public ProfessorDashboard(string professorid)
        {
            InitializeComponent();
            this.professorid = professorid;
        }

        private void ProfessorDashboard_Load(object sender, EventArgs e)
        {
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
                                        pictureBox3.Image = Image.FromStream(ms);
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

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            AttendanceRecordProf attendanceRecordProf = new AttendanceRecordProf(professorid);
            attendanceRecordProf.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentInformationProf studentInformationProf = new StudentInformationProf(professorid);
            studentInformationProf.ShowDialog();
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
