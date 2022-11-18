namespace Client
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxLogin = new System.Windows.Forms.TextBox();
            this.labelUser = new System.Windows.Forms.Label();
            this.labelLogin = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.buttonRegister = new System.Windows.Forms.Button();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.listBoxNotes = new System.Windows.Forms.ListBox();
            this.labelNotes = new System.Windows.Forms.Label();
            this.labelNote = new System.Windows.Forms.Label();
            this.labelNoteDate = new System.Windows.Forms.Label();
            this.labelNoteCaption = new System.Windows.Forms.Label();
            this.labelNoteContent = new System.Windows.Forms.Label();
            this.textBoxNoteDate = new System.Windows.Forms.TextBox();
            this.textBoxNoteCaption = new System.Windows.Forms.TextBox();
            this.textBoxNoteContent = new System.Windows.Forms.TextBox();
            this.buttonNoteCreate = new System.Windows.Forms.Button();
            this.buttonNoteUpdate = new System.Windows.Forms.Button();
            this.buttonNoteDelete = new System.Windows.Forms.Button();
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.SuspendLayout();
            // 
            // textBoxLogin
            // 
            this.textBoxLogin.Location = new System.Drawing.Point(988, 101);
            this.textBoxLogin.Name = "textBoxLogin";
            this.textBoxLogin.Size = new System.Drawing.Size(150, 31);
            this.textBoxLogin.TabIndex = 0;
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Location = new System.Drawing.Point(988, 47);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(147, 25);
            this.labelUser.TabIndex = 1;
            this.labelUser.Text = "Log in or register";
            // 
            // labelLogin
            // 
            this.labelLogin.AutoSize = true;
            this.labelLogin.Location = new System.Drawing.Point(883, 104);
            this.labelLogin.Name = "labelLogin";
            this.labelLogin.Size = new System.Drawing.Size(56, 25);
            this.labelLogin.TabIndex = 2;
            this.labelLogin.Text = "Login";
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(883, 157);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(87, 25);
            this.labelPassword.TabIndex = 3;
            this.labelPassword.Text = "Password";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(988, 151);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(150, 31);
            this.textBoxPassword.TabIndex = 4;
            // 
            // buttonRegister
            // 
            this.buttonRegister.Location = new System.Drawing.Point(883, 210);
            this.buttonRegister.Name = "buttonRegister";
            this.buttonRegister.Size = new System.Drawing.Size(112, 34);
            this.buttonRegister.TabIndex = 5;
            this.buttonRegister.Text = "Register";
            this.buttonRegister.UseVisualStyleBackColor = true;
            this.buttonRegister.Click += new System.EventHandler(this.buttonRegister_Click);
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(1023, 210);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(112, 34);
            this.buttonLogin.TabIndex = 6;
            this.buttonLogin.Text = "Log in";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // listBoxNotes
            // 
            this.listBoxNotes.FormattingEnabled = true;
            this.listBoxNotes.ItemHeight = 25;
            this.listBoxNotes.Location = new System.Drawing.Point(414, 75);
            this.listBoxNotes.Name = "listBoxNotes";
            this.listBoxNotes.Size = new System.Drawing.Size(394, 254);
            this.listBoxNotes.TabIndex = 8;
            this.listBoxNotes.SelectedIndexChanged += new System.EventHandler(this.listBoxNotes_SelectedIndexChanged);
            // 
            // labelNotes
            // 
            this.labelNotes.AutoSize = true;
            this.labelNotes.Location = new System.Drawing.Point(414, 47);
            this.labelNotes.Name = "labelNotes";
            this.labelNotes.Size = new System.Drawing.Size(86, 25);
            this.labelNotes.TabIndex = 9;
            this.labelNotes.Text = "My notes";
            // 
            // labelNote
            // 
            this.labelNote.AutoSize = true;
            this.labelNote.Location = new System.Drawing.Point(118, 47);
            this.labelNote.Name = "labelNote";
            this.labelNote.Size = new System.Drawing.Size(51, 25);
            this.labelNote.TabIndex = 10;
            this.labelNote.Text = "Note";
            // 
            // labelNoteDate
            // 
            this.labelNoteDate.AutoSize = true;
            this.labelNoteDate.Location = new System.Drawing.Point(12, 101);
            this.labelNoteDate.Name = "labelNoteDate";
            this.labelNoteDate.Size = new System.Drawing.Size(49, 25);
            this.labelNoteDate.TabIndex = 11;
            this.labelNoteDate.Text = "Date";
            // 
            // labelNoteCaption
            // 
            this.labelNoteCaption.AutoSize = true;
            this.labelNoteCaption.Location = new System.Drawing.Point(12, 151);
            this.labelNoteCaption.Name = "labelNoteCaption";
            this.labelNoteCaption.Size = new System.Drawing.Size(74, 25);
            this.labelNoteCaption.TabIndex = 12;
            this.labelNoteCaption.Text = "Caption";
            // 
            // labelNoteContent
            // 
            this.labelNoteContent.AutoSize = true;
            this.labelNoteContent.Location = new System.Drawing.Point(12, 201);
            this.labelNoteContent.Name = "labelNoteContent";
            this.labelNoteContent.Size = new System.Drawing.Size(75, 25);
            this.labelNoteContent.TabIndex = 13;
            this.labelNoteContent.Text = "Content";
            // 
            // textBoxNoteDate
            // 
            this.textBoxNoteDate.Location = new System.Drawing.Point(118, 98);
            this.textBoxNoteDate.Name = "textBoxNoteDate";
            this.textBoxNoteDate.Size = new System.Drawing.Size(223, 31);
            this.textBoxNoteDate.TabIndex = 14;
            // 
            // textBoxNoteCaption
            // 
            this.textBoxNoteCaption.Location = new System.Drawing.Point(118, 148);
            this.textBoxNoteCaption.Name = "textBoxNoteCaption";
            this.textBoxNoteCaption.Size = new System.Drawing.Size(223, 31);
            this.textBoxNoteCaption.TabIndex = 15;
            // 
            // textBoxNoteContent
            // 
            this.textBoxNoteContent.Location = new System.Drawing.Point(118, 198);
            this.textBoxNoteContent.Multiline = true;
            this.textBoxNoteContent.Name = "textBoxNoteContent";
            this.textBoxNoteContent.Size = new System.Drawing.Size(223, 131);
            this.textBoxNoteContent.TabIndex = 16;
            // 
            // buttonNoteCreate
            // 
            this.buttonNoteCreate.Location = new System.Drawing.Point(12, 346);
            this.buttonNoteCreate.Name = "buttonNoteCreate";
            this.buttonNoteCreate.Size = new System.Drawing.Size(112, 34);
            this.buttonNoteCreate.TabIndex = 17;
            this.buttonNoteCreate.Text = "Create";
            this.buttonNoteCreate.UseVisualStyleBackColor = true;
            this.buttonNoteCreate.Click += new System.EventHandler(this.buttonNoteCreate_Click);
            // 
            // buttonNoteUpdate
            // 
            this.buttonNoteUpdate.Location = new System.Drawing.Point(130, 346);
            this.buttonNoteUpdate.Name = "buttonNoteUpdate";
            this.buttonNoteUpdate.Size = new System.Drawing.Size(112, 34);
            this.buttonNoteUpdate.TabIndex = 18;
            this.buttonNoteUpdate.Text = "Update";
            this.buttonNoteUpdate.UseVisualStyleBackColor = true;
            this.buttonNoteUpdate.Click += new System.EventHandler(this.buttonNoteUpdate_Click);
            // 
            // buttonNoteDelete
            // 
            this.buttonNoteDelete.Location = new System.Drawing.Point(248, 346);
            this.buttonNoteDelete.Name = "buttonNoteDelete";
            this.buttonNoteDelete.Size = new System.Drawing.Size(112, 34);
            this.buttonNoteDelete.TabIndex = 19;
            this.buttonNoteDelete.Text = "Delete";
            this.buttonNoteDelete.UseVisualStyleBackColor = true;
            this.buttonNoteDelete.Click += new System.EventHandler(this.buttonNoteDelete_Click);
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(883, 346);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 20;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1247, 654);
            this.Controls.Add(this.monthCalendar1);
            this.Controls.Add(this.buttonNoteDelete);
            this.Controls.Add(this.buttonNoteUpdate);
            this.Controls.Add(this.buttonNoteCreate);
            this.Controls.Add(this.textBoxNoteContent);
            this.Controls.Add(this.textBoxNoteCaption);
            this.Controls.Add(this.textBoxNoteDate);
            this.Controls.Add(this.labelNoteContent);
            this.Controls.Add(this.labelNoteCaption);
            this.Controls.Add(this.labelNoteDate);
            this.Controls.Add(this.labelNote);
            this.Controls.Add(this.labelNotes);
            this.Controls.Add(this.listBoxNotes);
            this.Controls.Add(this.buttonLogin);
            this.Controls.Add(this.buttonRegister);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.labelLogin);
            this.Controls.Add(this.labelUser);
            this.Controls.Add(this.textBoxLogin);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox textBoxLogin;
        private Label labelUser;
        private Label labelLogin;
        private Label labelPassword;
        private TextBox textBoxPassword;
        private Button buttonRegister;
        private Button buttonLogin;
        private ListBox listBoxNotes;
        private Label labelNotes;
        private Label labelNote;
        private Label labelNoteDate;
        private Label labelNoteCaption;
        private Label labelNoteContent;
        private TextBox textBoxNoteDate;
        private TextBox textBoxNoteCaption;
        private TextBox textBoxNoteContent;
        private Button buttonNoteCreate;
        private Button buttonNoteUpdate;
        private Button buttonNoteDelete;
        private MonthCalendar monthCalendar1;
    }
}