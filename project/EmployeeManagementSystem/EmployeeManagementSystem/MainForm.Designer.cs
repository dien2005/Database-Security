namespace EmployeeManagementSystem
{
    partial class MainForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.exit = new System.Windows.Forms.Label();
            this.logout_btn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.myProfile_btn = new System.Windows.Forms.Button();
            this.auditLog_btn = new System.Windows.Forms.Button();
            this.approvals_btn = new System.Windows.Forms.Button();
            this.department_btn = new System.Windows.Forms.Button();
            this.salary_btn = new System.Windows.Forms.Button();
            this.addEmployee_btn = new System.Windows.Forms.Button();
            this.dashboard_btn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dashboard1 = new EmployeeManagementSystem.Dashboard();
            this.salary1 = new EmployeeManagementSystem.Salary();
            this.pendingApprovals1 = new EmployeeManagementSystem.PendingApprovals();
            this.myProfile1 = new EmployeeManagementSystem.MyProfile();
            this.department1 = new EmployeeManagementSystem.Department();
            this.auditLog1 = new EmployeeManagementSystem.AuditLog();
            this.addEmployee1 = new EmployeeManagementSystem.AddEmployee();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.exit);
            this.panel1.Controls.Add(this.logout_btn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(940, 37);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(19, 7);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(238, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Employee Management System";
            // 
            // exit
            // 
            this.exit.AutoSize = true;
            this.exit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exit.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exit.ForeColor = System.Drawing.Color.White;
            this.exit.Location = new System.Drawing.Point(914, 7);
            this.exit.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(18, 17);
            this.exit.TabIndex = 0;
            this.exit.Text = "X";
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // logout_btn
            // 
            this.logout_btn.BackColor = System.Drawing.Color.LightGray;
            this.logout_btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.logout_btn.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.logout_btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(8)))), ((int)(((byte)(138)))));
            this.logout_btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(8)))), ((int)(((byte)(138)))));
            this.logout_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logout_btn.ImageIndex = 112;
            this.logout_btn.Location = new System.Drawing.Point(821, 5);
            this.logout_btn.Margin = new System.Windows.Forms.Padding(2);
            this.logout_btn.Name = "logout_btn";
            this.logout_btn.Size = new System.Drawing.Size(89, 27);
            this.logout_btn.TabIndex = 5;
            this.logout_btn.Text = "SIGN OUT";
            this.logout_btn.UseVisualStyleBackColor = false;
            this.logout_btn.Click += new System.EventHandler(this.logout_btn_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(94)))));
            this.panel2.Controls.Add(this.myProfile_btn);
            this.panel2.Controls.Add(this.auditLog_btn);
            this.panel2.Controls.Add(this.approvals_btn);
            this.panel2.Controls.Add(this.department_btn);
            this.panel2.Controls.Add(this.salary_btn);
            this.panel2.Controls.Add(this.addEmployee_btn);
            this.panel2.Controls.Add(this.dashboard_btn);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 37);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(248, 527);
            this.panel2.TabIndex = 1;
            // 
            // myProfile_btn
            // 
            this.myProfile_btn.BackColor = System.Drawing.Color.DarkCyan;
            this.myProfile_btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.myProfile_btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(8)))), ((int)(((byte)(138)))));
            this.myProfile_btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(8)))), ((int)(((byte)(138)))));
            this.myProfile_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.myProfile_btn.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.myProfile_btn.ForeColor = System.Drawing.Color.White;
            this.myProfile_btn.Location = new System.Drawing.Point(11, 435);
            this.myProfile_btn.Margin = new System.Windows.Forms.Padding(2);
            this.myProfile_btn.Name = "myProfile_btn";
            this.myProfile_btn.Size = new System.Drawing.Size(228, 39);
            this.myProfile_btn.TabIndex = 9;
            this.myProfile_btn.Text = "MY PROFILE";
            this.myProfile_btn.UseVisualStyleBackColor = false;
            this.myProfile_btn.Click += new System.EventHandler(this.myProfile_btn_Click);
            // 
            // auditLog_btn
            // 
            this.auditLog_btn.BackColor = System.Drawing.Color.DarkCyan;
            this.auditLog_btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.auditLog_btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(8)))), ((int)(((byte)(138)))));
            this.auditLog_btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(8)))), ((int)(((byte)(138)))));
            this.auditLog_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.auditLog_btn.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.auditLog_btn.ForeColor = System.Drawing.Color.White;
            this.auditLog_btn.Location = new System.Drawing.Point(11, 392);
            this.auditLog_btn.Margin = new System.Windows.Forms.Padding(2);
            this.auditLog_btn.Name = "auditLog_btn";
            this.auditLog_btn.Size = new System.Drawing.Size(228, 39);
            this.auditLog_btn.TabIndex = 8;
            this.auditLog_btn.Text = "AUDIT LOG";
            this.auditLog_btn.UseVisualStyleBackColor = false;
            this.auditLog_btn.Click += new System.EventHandler(this.auditLog_btn_Click);
            // 
            // approvals_btn
            // 
            this.approvals_btn.BackColor = System.Drawing.Color.DarkCyan;
            this.approvals_btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.approvals_btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(8)))), ((int)(((byte)(138)))));
            this.approvals_btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(8)))), ((int)(((byte)(138)))));
            this.approvals_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.approvals_btn.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.approvals_btn.ForeColor = System.Drawing.Color.White;
            this.approvals_btn.Location = new System.Drawing.Point(11, 349);
            this.approvals_btn.Margin = new System.Windows.Forms.Padding(2);
            this.approvals_btn.Name = "approvals_btn";
            this.approvals_btn.Size = new System.Drawing.Size(228, 39);
            this.approvals_btn.TabIndex = 7;
            this.approvals_btn.Text = "PENDING APPROVALS";
            this.approvals_btn.UseVisualStyleBackColor = false;
            this.approvals_btn.Click += new System.EventHandler(this.approvals_btn_Click);
            // 
            // department_btn
            // 
            this.department_btn.BackColor = System.Drawing.Color.DarkCyan;
            this.department_btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.department_btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(8)))), ((int)(((byte)(138)))));
            this.department_btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(8)))), ((int)(((byte)(138)))));
            this.department_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.department_btn.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.department_btn.ForeColor = System.Drawing.Color.White;
            this.department_btn.Location = new System.Drawing.Point(11, 306);
            this.department_btn.Margin = new System.Windows.Forms.Padding(2);
            this.department_btn.Name = "department_btn";
            this.department_btn.Size = new System.Drawing.Size(228, 39);
            this.department_btn.TabIndex = 6;
            this.department_btn.Text = "DEPARTMENT OVERVIEW";
            this.department_btn.UseVisualStyleBackColor = false;
            this.department_btn.Click += new System.EventHandler(this.department_btn_Click);
            // 
            // salary_btn
            // 
            this.salary_btn.BackColor = System.Drawing.Color.DarkCyan;
            this.salary_btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.salary_btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(8)))), ((int)(((byte)(138)))));
            this.salary_btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(8)))), ((int)(((byte)(138)))));
            this.salary_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.salary_btn.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.salary_btn.ForeColor = System.Drawing.Color.White;
            this.salary_btn.Location = new System.Drawing.Point(11, 263);
            this.salary_btn.Margin = new System.Windows.Forms.Padding(2);
            this.salary_btn.Name = "salary_btn";
            this.salary_btn.Size = new System.Drawing.Size(228, 39);
            this.salary_btn.TabIndex = 4;
            this.salary_btn.Text = "PAYROLL / SALARY VIEW";
            this.salary_btn.UseVisualStyleBackColor = false;
            this.salary_btn.Click += new System.EventHandler(this.salary_btn_Click);
            // 
            // addEmployee_btn
            // 
            this.addEmployee_btn.BackColor = System.Drawing.Color.DarkCyan;
            this.addEmployee_btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addEmployee_btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(8)))), ((int)(((byte)(138)))));
            this.addEmployee_btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(8)))), ((int)(((byte)(138)))));
            this.addEmployee_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addEmployee_btn.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addEmployee_btn.ForeColor = System.Drawing.Color.White;
            this.addEmployee_btn.Location = new System.Drawing.Point(11, 220);
            this.addEmployee_btn.Margin = new System.Windows.Forms.Padding(2);
            this.addEmployee_btn.Name = "addEmployee_btn";
            this.addEmployee_btn.Size = new System.Drawing.Size(228, 39);
            this.addEmployee_btn.TabIndex = 3;
            this.addEmployee_btn.Text = "EMPLOYEE MANAGEMENT";
            this.addEmployee_btn.UseVisualStyleBackColor = false;
            this.addEmployee_btn.Click += new System.EventHandler(this.addEmloyee_btn_Click);
            // 
            // dashboard_btn
            // 
            this.dashboard_btn.BackColor = System.Drawing.Color.DarkCyan;
            this.dashboard_btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dashboard_btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(8)))), ((int)(((byte)(138)))));
            this.dashboard_btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(8)))), ((int)(((byte)(138)))));
            this.dashboard_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dashboard_btn.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dashboard_btn.ForeColor = System.Drawing.Color.White;
            this.dashboard_btn.Location = new System.Drawing.Point(11, 177);
            this.dashboard_btn.Margin = new System.Windows.Forms.Padding(2);
            this.dashboard_btn.Name = "dashboard_btn";
            this.dashboard_btn.Size = new System.Drawing.Size(228, 39);
            this.dashboard_btn.TabIndex = 2;
            this.dashboard_btn.Text = "DASHBOARD";
            this.dashboard_btn.UseVisualStyleBackColor = false;
            this.dashboard_btn.Click += new System.EventHandler(this.dashboard_btn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::EmployeeManagementSystem.Properties.Resources.EMPL_M_S;
            this.pictureBox1.Location = new System.Drawing.Point(47, 15);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(149, 151);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dashboard1);
            this.panel3.Controls.Add(this.salary1);
            this.panel3.Controls.Add(this.pendingApprovals1);
            this.panel3.Controls.Add(this.myProfile1);
            this.panel3.Controls.Add(this.department1);
            this.panel3.Controls.Add(this.auditLog1);
            this.panel3.Controls.Add(this.addEmployee1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(248, 37);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(692, 527);
            this.panel3.TabIndex = 2;
            // 
            // dashboard1
            // 
            this.dashboard1.BackColor = System.Drawing.Color.White;
            this.dashboard1.Location = new System.Drawing.Point(-2, 0);
            this.dashboard1.Margin = new System.Windows.Forms.Padding(2);
            this.dashboard1.Name = "dashboard1";
            this.dashboard1.Size = new System.Drawing.Size(692, 526);
            this.dashboard1.TabIndex = 2;
            // 
            // salary1
            // 
            this.salary1.BackColor = System.Drawing.Color.Azure;
            this.salary1.Location = new System.Drawing.Point(-2, 0);
            this.salary1.Margin = new System.Windows.Forms.Padding(2);
            this.salary1.Name = "salary1";
            this.salary1.Size = new System.Drawing.Size(692, 526);
            this.salary1.TabIndex = 7;
            // 
            // pendingApprovals1
            // 
            this.pendingApprovals1.BackColor = System.Drawing.Color.Azure;
            this.pendingApprovals1.Location = new System.Drawing.Point(-2, 0);
            this.pendingApprovals1.Name = "pendingApprovals1";
            this.pendingApprovals1.Size = new System.Drawing.Size(692, 526);
            this.pendingApprovals1.TabIndex = 6;
            // 
            // myProfile1
            // 
            this.myProfile1.Location = new System.Drawing.Point(-2, 0);
            this.myProfile1.Name = "myProfile1";
            this.myProfile1.Size = new System.Drawing.Size(692, 526);
            this.myProfile1.TabIndex = 5;
            // 
            // department1
            // 
            this.department1.BackColor = System.Drawing.Color.Azure;
            this.department1.Location = new System.Drawing.Point(-2, 0);
            this.department1.Name = "department1";
            this.department1.Size = new System.Drawing.Size(692, 526);
            this.department1.TabIndex = 4;
            // 
            // auditLog1
            // 
            this.auditLog1.BackColor = System.Drawing.Color.Azure;
            this.auditLog1.Location = new System.Drawing.Point(-2, 0);
            this.auditLog1.Name = "auditLog1";
            this.auditLog1.Size = new System.Drawing.Size(692, 526);
            this.auditLog1.TabIndex = 3;
            // 
            // addEmployee1
            // 
            this.addEmployee1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.addEmployee1.Location = new System.Drawing.Point(0, 0);
            this.addEmployee1.Margin = new System.Windows.Forms.Padding(2);
            this.addEmployee1.Name = "addEmployee1";
            this.addEmployee1.Size = new System.Drawing.Size(692, 526);
            this.addEmployee1.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 564);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button dashboard_btn;
        private System.Windows.Forms.Button logout_btn;
        private System.Windows.Forms.Button salary_btn;
        private System.Windows.Forms.Button addEmployee_btn;
        private System.Windows.Forms.Panel panel3;
        private AddEmployee addEmployee1;
        private Dashboard dashboard1;
        private System.Windows.Forms.Button auditLog_btn;
        private System.Windows.Forms.Button approvals_btn;
        private System.Windows.Forms.Button department_btn;
        private System.Windows.Forms.Button myProfile_btn;
        private System.Windows.Forms.Label exit;
        private AuditLog auditLog1;
        private Department department1;
        private PendingApprovals pendingApprovals1;
        private MyProfile myProfile1;
        private Salary salary1;
    }
}