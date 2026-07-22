namespace EmployeeManagementSystem
{
    partial class Department
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblDeptName = new System.Windows.Forms.Label();
            this.txtDeptName = new System.Windows.Forms.TextBox();
            this.lblDeptCode = new System.Windows.Forms.Label();
            this.txtDeptCode = new System.Windows.Forms.TextBox();
            this.lblManager = new System.Windows.Forms.Label();
            this.cboManager = new System.Windows.Forms.ComboBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.cboLocation = new System.Windows.Forms.ComboBox();
            this.lblParentDept = new System.Windows.Forms.Label();
            this.cboParentDept = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.lblStatusMsg = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblGridTitle = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dgvDepartments = new System.Windows.Forms.DataGridView();
            this.lblCount = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDepartments)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.lblDeptName);
            this.panel1.Controls.Add(this.txtDeptName);
            this.panel1.Controls.Add(this.lblDeptCode);
            this.panel1.Controls.Add(this.txtDeptCode);
            this.panel1.Controls.Add(this.lblManager);
            this.panel1.Controls.Add(this.cboManager);
            this.panel1.Controls.Add(this.lblLocation);
            this.panel1.Controls.Add(this.cboLocation);
            this.panel1.Controls.Add(this.lblParentDept);
            this.panel1.Controls.Add(this.cboParentDept);
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Controls.Add(this.cboStatus);
            this.panel1.Controls.Add(this.lblStatusMsg);
            this.panel1.Location = new System.Drawing.Point(14, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(223, 496);
            this.panel1.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(191, 18);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "THÔNG TIN PHÒNG BAN";
            // 
            // lblDeptName
            // 
            this.lblDeptName.AutoSize = true;
            this.lblDeptName.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblDeptName.Location = new System.Drawing.Point(14, 45);
            this.lblDeptName.Name = "lblDeptName";
            this.lblDeptName.Size = new System.Drawing.Size(96, 14);
            this.lblDeptName.TabIndex = 1;
            this.lblDeptName.Text = "Tên phòng ban:";
            // 
            // txtDeptName
            // 
            this.txtDeptName.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtDeptName.Location = new System.Drawing.Point(14, 65);
            this.txtDeptName.Name = "txtDeptName";
            this.txtDeptName.Size = new System.Drawing.Size(192, 22);
            this.txtDeptName.TabIndex = 2;
            // 
            // lblDeptCode
            // 
            this.lblDeptCode.AutoSize = true;
            this.lblDeptCode.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblDeptCode.Location = new System.Drawing.Point(14, 95);
            this.lblDeptCode.Name = "lblDeptCode";
            this.lblDeptCode.Size = new System.Drawing.Size(89, 14);
            this.lblDeptCode.TabIndex = 3;
            this.lblDeptCode.Text = "Mã phòng ban:";
            // 
            // txtDeptCode
            // 
            this.txtDeptCode.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtDeptCode.Location = new System.Drawing.Point(14, 115);
            this.txtDeptCode.Name = "txtDeptCode";
            this.txtDeptCode.Size = new System.Drawing.Size(192, 22);
            this.txtDeptCode.TabIndex = 4;
            // 
            // lblManager
            // 
            this.lblManager.AutoSize = true;
            this.lblManager.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblManager.Location = new System.Drawing.Point(14, 145);
            this.lblManager.Name = "lblManager";
            this.lblManager.Size = new System.Drawing.Size(91, 14);
            this.lblManager.TabIndex = 5;
            this.lblManager.Text = "Trưởng phòng:";
            // 
            // cboManager
            // 
            this.cboManager.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboManager.Font = new System.Drawing.Font("Tahoma", 9F);
            this.cboManager.FormattingEnabled = true;
            this.cboManager.Location = new System.Drawing.Point(14, 165);
            this.cboManager.Name = "cboManager";
            this.cboManager.Size = new System.Drawing.Size(192, 22);
            this.cboManager.TabIndex = 6;
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblLocation.Location = new System.Drawing.Point(14, 195);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(57, 14);
            this.lblLocation.TabIndex = 7;
            this.lblLocation.Text = "Địa điểm:";
            // 
            // cboLocation
            // 
            this.cboLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocation.Font = new System.Drawing.Font("Tahoma", 9F);
            this.cboLocation.FormattingEnabled = true;
            this.cboLocation.Location = new System.Drawing.Point(14, 215);
            this.cboLocation.Name = "cboLocation";
            this.cboLocation.Size = new System.Drawing.Size(192, 22);
            this.cboLocation.TabIndex = 8;
            // 
            // lblParentDept
            // 
            this.lblParentDept.AutoSize = true;
            this.lblParentDept.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblParentDept.Location = new System.Drawing.Point(14, 245);
            this.lblParentDept.Name = "lblParentDept";
            this.lblParentDept.Size = new System.Drawing.Size(93, 14);
            this.lblParentDept.TabIndex = 9;
            this.lblParentDept.Text = "Phòng ban cha:";
            // 
            // cboParentDept
            // 
            this.cboParentDept.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboParentDept.Font = new System.Drawing.Font("Tahoma", 9F);
            this.cboParentDept.FormattingEnabled = true;
            this.cboParentDept.Location = new System.Drawing.Point(14, 265);
            this.cboParentDept.Name = "cboParentDept";
            this.cboParentDept.Size = new System.Drawing.Size(192, 22);
            this.cboParentDept.TabIndex = 10;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblStatus.Location = new System.Drawing.Point(14, 295);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(67, 14);
            this.lblStatus.TabIndex = 11;
            this.lblStatus.Text = "Trạng thái:";
            // 
            // cboStatus
            // 
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.Font = new System.Drawing.Font("Tahoma", 9F);
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Items.AddRange(new object[] {
            "ACTIVE",
            "INACTIVE",
            "DISSOLVED"});
            this.cboStatus.Location = new System.Drawing.Point(14, 315);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(192, 22);
            this.cboStatus.TabIndex = 12;
            // 
            // lblStatusMsg
            // 
            this.lblStatusMsg.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblStatusMsg.ForeColor = System.Drawing.Color.Red;
            this.lblStatusMsg.Location = new System.Drawing.Point(14, 450);
            this.lblStatusMsg.Name = "lblStatusMsg";
            this.lblStatusMsg.Size = new System.Drawing.Size(192, 35);
            this.lblStatusMsg.TabIndex = 17;
            this.lblStatusMsg.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lblGridTitle);
            this.panel2.Controls.Add(this.dgvDepartments);
            this.panel2.Controls.Add(this.lblCount);
            this.panel2.Location = new System.Drawing.Point(250, 15);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(428, 496);
            this.panel2.TabIndex = 1;
            // 
            // lblGridTitle
            // 
            this.lblGridTitle.AutoSize = true;
            this.lblGridTitle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lblGridTitle.Location = new System.Drawing.Point(14, 15);
            this.lblGridTitle.Name = "lblGridTitle";
            this.lblGridTitle.Size = new System.Drawing.Size(182, 19);
            this.lblGridTitle.TabIndex = 0;
            this.lblGridTitle.Text = "Danh sách phòng ban";
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.Teal;
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(14, 356);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(192, 28);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            // 
            // dgvDepartments
            // 
            this.dgvDepartments.AllowUserToAddRows = false;
            this.dgvDepartments.AllowUserToDeleteRows = false;
            this.dgvDepartments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDepartments.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(11)))), ((int)(((byte)(97)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDepartments.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvDepartments.ColumnHeadersHeight = 30;
            this.dgvDepartments.EnableHeadersVisualStyles = false;
            this.dgvDepartments.Location = new System.Drawing.Point(14, 50);
            this.dgvDepartments.Name = "dgvDepartments";
            this.dgvDepartments.ReadOnly = true;
            this.dgvDepartments.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 9F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(8)))), ((int)(((byte)(138)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            this.dgvDepartments.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvDepartments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDepartments.Size = new System.Drawing.Size(400, 410);
            this.dgvDepartments.TabIndex = 2;
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic);
            this.lblCount.Location = new System.Drawing.Point(14, 470);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(106, 13);
            this.lblCount.TabIndex = 3;
            this.lblCount.Text = "Hien thi 0 phong ban";
            // 
            // Department
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Azure;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Department";
            this.Size = new System.Drawing.Size(692, 526);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDepartments)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDeptName;
        private System.Windows.Forms.TextBox txtDeptName;
        private System.Windows.Forms.Label lblDeptCode;
        private System.Windows.Forms.TextBox txtDeptCode;
        private System.Windows.Forms.Label lblManager;
        private System.Windows.Forms.ComboBox cboManager;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.ComboBox cboLocation;
        private System.Windows.Forms.Label lblParentDept;
        private System.Windows.Forms.ComboBox cboParentDept;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.Label lblStatusMsg;

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblGridTitle;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dgvDepartments;
        private System.Windows.Forms.Label lblCount;
    }
}
