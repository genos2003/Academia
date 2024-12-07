namespace AttendanceManagement
{
    partial class StudentEditingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button6 = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.section_list = new System.Windows.Forms.ComboBox();
            this.year_list = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.course_list = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.email_text = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.contact_text = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.address_text = new System.Windows.Forms.RichTextBox();
            this.citizen_list = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.religion_list = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.birthdate_cal = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.name_text = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.id_text = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.addphoto_btn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.button6.FlatAppearance.BorderSize = 0;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.button6.Location = new System.Drawing.Point(277, 382);
            this.button6.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(67, 25);
            this.button6.TabIndex = 27;
            this.button6.Text = "UPDATE";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.label13.Location = new System.Drawing.Point(413, 107);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(54, 13);
            this.label13.TabIndex = 26;
            this.label13.Text = "SECTION:";
            // 
            // section_list
            // 
            this.section_list.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.section_list.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.section_list.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.section_list.FormattingEnabled = true;
            this.section_list.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.section_list.Location = new System.Drawing.Point(488, 103);
            this.section_list.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.section_list.Name = "section_list";
            this.section_list.Size = new System.Drawing.Size(112, 21);
            this.section_list.TabIndex = 25;
            // 
            // year_list
            // 
            this.year_list.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.year_list.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.year_list.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.year_list.FormattingEnabled = true;
            this.year_list.Items.AddRange(new object[] {
            "FIRST YEAR",
            "SECOND YEAR",
            "THIRD YEAR",
            "FOURTH YEAR"});
            this.year_list.Location = new System.Drawing.Point(488, 72);
            this.year_list.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.year_list.Name = "year_list";
            this.year_list.Size = new System.Drawing.Size(112, 21);
            this.year_list.TabIndex = 24;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.label12.Location = new System.Drawing.Point(413, 75);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(35, 13);
            this.label12.TabIndex = 23;
            this.label12.Text = "YEAR:";
            // 
            // course_list
            // 
            this.course_list.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.course_list.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.course_list.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.course_list.FormattingEnabled = true;
            this.course_list.Items.AddRange(new object[] {
            "BSIT",
            "BSBA",
            "BSCRIM",
            "BEEd",
            "BSEd",
            "BSA",
            "BSHM",
            "BSTM",
            "BSN",
            "BSMedTech",
            "BSPsych",
            "BSRadTech"});
            this.course_list.Location = new System.Drawing.Point(488, 40);
            this.course_list.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.course_list.Name = "course_list";
            this.course_list.Size = new System.Drawing.Size(112, 21);
            this.course_list.TabIndex = 22;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.label11.Location = new System.Drawing.Point(413, 45);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "COURSE:";
            // 
            // email_text
            // 
            this.email_text.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.email_text.Location = new System.Drawing.Point(232, 344);
            this.email_text.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.email_text.Name = "email_text";
            this.email_text.Size = new System.Drawing.Size(112, 20);
            this.email_text.TabIndex = 20;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.label10.Location = new System.Drawing.Point(131, 347);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 13);
            this.label10.TabIndex = 19;
            this.label10.Text = "EMAIL ADDRESS:";
            // 
            // contact_text
            // 
            this.contact_text.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.contact_text.Location = new System.Drawing.Point(232, 312);
            this.contact_text.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.contact_text.Name = "contact_text";
            this.contact_text.Size = new System.Drawing.Size(112, 20);
            this.contact_text.TabIndex = 18;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.label9.Location = new System.Drawing.Point(131, 315);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "CONTACT NO.";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.pictureBox2);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.section_list);
            this.groupBox1.Controls.Add(this.year_list);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.course_list);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.email_text);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.contact_text);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.address_text);
            this.groupBox1.Controls.Add(this.citizen_list);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.religion_list);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.birthdate_cal);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.name_text);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.id_text);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.addphoto_btn);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.groupBox1.Location = new System.Drawing.Point(0, 31);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox1.Size = new System.Drawing.Size(680, 419);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Student Info";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.button1.Location = new System.Drawing.Point(361, 382);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(67, 25);
            this.button1.TabIndex = 30;
            this.button1.Text = "REMOVE";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::AttendanceManagement.Properties.Resources.qricon;
            this.pictureBox2.Location = new System.Drawing.Point(404, 148);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(200, 200);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 29;
            this.pictureBox2.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.label8.Location = new System.Drawing.Point(131, 230);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "FULL ADDRESS:";
            // 
            // address_text
            // 
            this.address_text.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.address_text.Location = new System.Drawing.Point(232, 227);
            this.address_text.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.address_text.Name = "address_text";
            this.address_text.Size = new System.Drawing.Size(112, 70);
            this.address_text.TabIndex = 15;
            this.address_text.Text = "";
            // 
            // citizen_list
            // 
            this.citizen_list.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.citizen_list.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.citizen_list.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.citizen_list.FormattingEnabled = true;
            this.citizen_list.Items.AddRange(new object[] {
            "FILIPINO",
            "FOREIGN"});
            this.citizen_list.Location = new System.Drawing.Point(232, 196);
            this.citizen_list.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.citizen_list.Name = "citizen_list";
            this.citizen_list.Size = new System.Drawing.Size(112, 21);
            this.citizen_list.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.label7.Location = new System.Drawing.Point(131, 199);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "CITIZENSHIP:";
            // 
            // religion_list
            // 
            this.religion_list.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.religion_list.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.religion_list.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.religion_list.FormattingEnabled = true;
            this.religion_list.Items.AddRange(new object[] {
            "Christianity",
            "Catholic",
            "Seventh-day Adventist",
            "Iglesia ni Cristo",
            "Islam",
            "Jehovah\'s Witnesses",
            "Other"});
            this.religion_list.Location = new System.Drawing.Point(232, 164);
            this.religion_list.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.religion_list.Name = "religion_list";
            this.religion_list.Size = new System.Drawing.Size(112, 21);
            this.religion_list.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.label6.Location = new System.Drawing.Point(131, 166);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "RELIGION:";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.radioButton2.Location = new System.Drawing.Point(287, 104);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(61, 17);
            this.radioButton2.TabIndex = 11;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Female";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.radioButton1.Location = new System.Drawing.Point(232, 104);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(50, 17);
            this.radioButton1.TabIndex = 10;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Male";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.label5.Location = new System.Drawing.Point(131, 134);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "BIRTHDATE:";
            // 
            // birthdate_cal
            // 
            this.birthdate_cal.CalendarMonthBackground = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.birthdate_cal.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.birthdate_cal.Location = new System.Drawing.Point(232, 129);
            this.birthdate_cal.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.birthdate_cal.Name = "birthdate_cal";
            this.birthdate_cal.Size = new System.Drawing.Size(112, 20);
            this.birthdate_cal.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.label4.Location = new System.Drawing.Point(131, 107);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "GENDER:";
            // 
            // name_text
            // 
            this.name_text.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.name_text.Location = new System.Drawing.Point(232, 73);
            this.name_text.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.name_text.Name = "name_text";
            this.name_text.Size = new System.Drawing.Size(112, 20);
            this.name_text.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.label3.Location = new System.Drawing.Point(131, 75);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "FULLNAME:";
            // 
            // id_text
            // 
            this.id_text.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.id_text.Location = new System.Drawing.Point(232, 42);
            this.id_text.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.id_text.Name = "id_text";
            this.id_text.Size = new System.Drawing.Size(112, 20);
            this.id_text.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.label2.Location = new System.Drawing.Point(131, 45);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "STUDENT NO.";
            // 
            // addphoto_btn
            // 
            this.addphoto_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.addphoto_btn.FlatAppearance.BorderSize = 0;
            this.addphoto_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addphoto_btn.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addphoto_btn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.addphoto_btn.Location = new System.Drawing.Point(22, 129);
            this.addphoto_btn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.addphoto_btn.Name = "addphoto_btn";
            this.addphoto_btn.Size = new System.Drawing.Size(100, 25);
            this.addphoto_btn.TabIndex = 2;
            this.addphoto_btn.Text = "Add Photo";
            this.addphoto_btn.UseVisualStyleBackColor = false;
            this.addphoto_btn.Click += new System.EventHandler(this.addphoto_btn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AttendanceManagement.Properties.Resources.usericon;
            this.pictureBox1.Location = new System.Drawing.Point(22, 23);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.panel1.Controls.Add(this.pictureBox6);
            this.panel1.Controls.Add(this.pictureBox4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(680, 44);
            this.panel1.TabIndex = 6;
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = global::AttendanceManagement.Properties.Resources.minus;
            this.pictureBox6.Location = new System.Drawing.Point(619, 3);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(25, 25);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox6.TabIndex = 39;
            this.pictureBox6.TabStop = false;
            this.pictureBox6.Click += new System.EventHandler(this.pictureBox6_Click);
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::AttendanceManagement.Properties.Resources.close__2_;
            this.pictureBox4.Location = new System.Drawing.Point(650, 3);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(25, 25);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 36;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.Click += new System.EventHandler(this.pictureBox4_Click);
            // 
            // StudentEditingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 450);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "StudentEditingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StudentEditingForm";
            this.Load += new System.EventHandler(this.StudentEditingForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox section_list;
        private System.Windows.Forms.ComboBox year_list;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox course_list;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox email_text;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox contact_text;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RichTextBox address_text;
        private System.Windows.Forms.ComboBox citizen_list;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox religion_list;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker birthdate_cal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MaskedTextBox name_text;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MaskedTextBox id_text;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button addphoto_btn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.Button button1;
    }
}