namespace EmployeeManagementSystem
{
    partial class Salary
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblThang = new System.Windows.Forms.Label();
            this.cboThang = new System.Windows.Forms.ComboBox();
            this.lblNam = new System.Windows.Forms.Label();
            this.cboNam = new System.Windows.Forms.ComboBox();
            this.lblStatusFilter = new System.Windows.Forms.Label();
            this.cboFilterStatus = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dgvPayroll = new System.Windows.Forms.DataGridView();
            this.pnlForm = new System.Windows.Forms.Panel();
            this.pnlFormHeader = new System.Windows.Forms.Panel();
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.cboEmployee = new System.Windows.Forms.ComboBox();
            this.lblPayPeriod = new System.Windows.Forms.Label();
            this.txtPayPeriod = new System.Windows.Forms.TextBox();
            this.lblBaseSalary = new System.Windows.Forms.Label();
            this.txtBaseSalary = new System.Windows.Forms.TextBox();
            this.lblBonus = new System.Windows.Forms.Label();
            this.txtBonus = new System.Windows.Forms.TextBox();
            this.lblAllowances = new System.Windows.Forms.Label();
            this.txtAllowances = new System.Windows.Forms.TextBox();
            this.lblDeductions = new System.Windows.Forms.Label();
            this.txtDeductions = new System.Windows.Forms.TextBox();
            this.lblNetPay = new System.Windows.Forms.Label();
            this.txtNetPay = new System.Windows.Forms.TextBox();
            this.lblPayDate = new System.Windows.Forms.Label();
            this.dtpPayDate = new System.Windows.Forms.DateTimePicker();
            this.lblPayStatus = new System.Windows.Forms.Label();
            this.cboPayStatus = new System.Windows.Forms.ComboBox();
            this.lblApprovedBy = new System.Windows.Forms.Label();
            this.txtApprovedBy = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnApprove = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.pnlHeader.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayroll)).BeginInit();
            this.pnlForm.SuspendLayout();
            this.pnlFormHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(94)))));
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblCount);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(692, 50);
            this.pnlHeader.TabIndex = 2;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(16, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(337, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "     PAYROLL / SALARY MANAGEMENT";
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(220)))));
            this.lblCount.Location = new System.Drawing.Point(530, 16);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(0, 15);
            this.lblCount.TabIndex = 1;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(250)))));
            this.pnlSearch.Controls.Add(this.lblSearch);
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.lblThang);
            this.pnlSearch.Controls.Add(this.cboThang);
            this.pnlSearch.Controls.Add(this.lblNam);
            this.pnlSearch.Controls.Add(this.cboNam);
            this.pnlSearch.Controls.Add(this.lblStatusFilter);
            this.pnlSearch.Controls.Add(this.cboFilterStatus);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.btnRefresh);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 50);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(692, 68);
            this.pnlSearch.TabIndex = 1;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSearch.Location = new System.Drawing.Point(10, 14);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(66, 15);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Nhan vien:";
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtSearch.Location = new System.Drawing.Point(78, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(148, 24);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearch_KeyPress);
            // 
            // lblThang
            // 
            this.lblThang.AutoSize = true;
            this.lblThang.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblThang.Location = new System.Drawing.Point(232, 13);
            this.lblThang.Name = "lblThang";
            this.lblThang.Size = new System.Drawing.Size(44, 15);
            this.lblThang.TabIndex = 2;
            this.lblThang.Text = "Thang:";
            // 
            // cboThang
            // 
            this.cboThang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboThang.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboThang.Location = new System.Drawing.Point(280, 9);
            this.cboThang.Name = "cboThang";
            this.cboThang.Size = new System.Drawing.Size(111, 25);
            this.cboThang.TabIndex = 3;
            // 
            // lblNam
            // 
            this.lblNam.AutoSize = true;
            this.lblNam.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNam.Location = new System.Drawing.Point(397, 13);
            this.lblNam.Name = "lblNam";
            this.lblNam.Size = new System.Drawing.Size(36, 15);
            this.lblNam.TabIndex = 4;
            this.lblNam.Text = "Nam:";
            // 
            // cboNam
            // 
            this.cboNam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNam.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboNam.Location = new System.Drawing.Point(435, 9);
            this.cboNam.Name = "cboNam";
            this.cboNam.Size = new System.Drawing.Size(115, 25);
            this.cboNam.TabIndex = 5;
            // 
            // lblStatusFilter
            // 
            this.lblStatusFilter.AutoSize = true;
            this.lblStatusFilter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatusFilter.Location = new System.Drawing.Point(10, 42);
            this.lblStatusFilter.Name = "lblStatusFilter";
            this.lblStatusFilter.Size = new System.Drawing.Size(65, 15);
            this.lblStatusFilter.TabIndex = 6;
            this.lblStatusFilter.Text = "Trang thai:";
            // 
            // cboFilterStatus
            // 
            this.cboFilterStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilterStatus.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboFilterStatus.Location = new System.Drawing.Point(78, 37);
            this.cboFilterStatus.Name = "cboFilterStatus";
            this.cboFilterStatus.Size = new System.Drawing.Size(148, 25);
            this.cboFilterStatus.TabIndex = 7;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(8)))), ((int)(((byte)(137)))));
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(556, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(78, 30);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "Tim kiem";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(642, 5);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(36, 30);
            this.btnRefresh.TabIndex = 9;
            this.btnRefresh.Text = "O";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // dgvPayroll
            // 
            this.dgvPayroll.AllowUserToAddRows = false;
            this.dgvPayroll.AllowUserToDeleteRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(240)))), ((int)(((byte)(254)))));
            this.dgvPayroll.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvPayroll.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPayroll.BackgroundColor = System.Drawing.Color.White;
            this.dgvPayroll.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(11)))), ((int)(((byte)(97)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPayroll.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvPayroll.ColumnHeadersHeight = 32;
            this.dgvPayroll.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(8)))), ((int)(((byte)(137)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPayroll.DefaultCellStyle = dataGridViewCellStyle9;
            this.dgvPayroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPayroll.EnableHeadersVisualStyles = false;
            this.dgvPayroll.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(235)))));
            this.dgvPayroll.Location = new System.Drawing.Point(0, 118);
            this.dgvPayroll.MultiSelect = false;
            this.dgvPayroll.Name = "dgvPayroll";
            this.dgvPayroll.ReadOnly = true;
            this.dgvPayroll.RowHeadersVisible = false;
            this.dgvPayroll.RowTemplate.Height = 26;
            this.dgvPayroll.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPayroll.Size = new System.Drawing.Size(692, 168);
            this.dgvPayroll.TabIndex = 0;
            this.dgvPayroll.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPayroll_CellClick);
            // 
            // pnlForm
            // 
            this.pnlForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.pnlForm.Controls.Add(this.pnlFormHeader);
            this.pnlForm.Controls.Add(this.lblEmployee);
            this.pnlForm.Controls.Add(this.cboEmployee);
            this.pnlForm.Controls.Add(this.lblPayPeriod);
            this.pnlForm.Controls.Add(this.txtPayPeriod);
            this.pnlForm.Controls.Add(this.lblBaseSalary);
            this.pnlForm.Controls.Add(this.txtBaseSalary);
            this.pnlForm.Controls.Add(this.lblBonus);
            this.pnlForm.Controls.Add(this.txtBonus);
            this.pnlForm.Controls.Add(this.lblAllowances);
            this.pnlForm.Controls.Add(this.txtAllowances);
            this.pnlForm.Controls.Add(this.lblDeductions);
            this.pnlForm.Controls.Add(this.txtDeductions);
            this.pnlForm.Controls.Add(this.lblNetPay);
            this.pnlForm.Controls.Add(this.txtNetPay);
            this.pnlForm.Controls.Add(this.lblPayDate);
            this.pnlForm.Controls.Add(this.dtpPayDate);
            this.pnlForm.Controls.Add(this.lblPayStatus);
            this.pnlForm.Controls.Add(this.cboPayStatus);
            this.pnlForm.Controls.Add(this.lblApprovedBy);
            this.pnlForm.Controls.Add(this.txtApprovedBy);
            this.pnlForm.Controls.Add(this.btnAdd);
            this.pnlForm.Controls.Add(this.btnUpdate);
            this.pnlForm.Controls.Add(this.btnApprove);
            this.pnlForm.Controls.Add(this.btnCancel);
            this.pnlForm.Controls.Add(this.btnClear);
            this.pnlForm.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlForm.Location = new System.Drawing.Point(0, 286);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(692, 240);
            this.pnlForm.TabIndex = 3;
            // 
            // pnlFormHeader
            // 
            this.pnlFormHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(120)))));
            this.pnlFormHeader.Controls.Add(this.lblFormTitle);
            this.pnlFormHeader.Controls.Add(this.lblStatus);
            this.pnlFormHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFormHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlFormHeader.Name = "pnlFormHeader";
            this.pnlFormHeader.Size = new System.Drawing.Size(692, 30);
            this.pnlFormHeader.TabIndex = 0;
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblFormTitle.ForeColor = System.Drawing.Color.White;
            this.lblFormTitle.Location = new System.Drawing.Point(10, 6);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(175, 17);
            this.lblFormTitle.TabIndex = 0;
            this.lblFormTitle.Text = "THEM / SUA PHIEU LUONG";
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(150)))));
            this.lblStatus.Location = new System.Drawing.Point(832, 7);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(445, 18);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEmployee
            // 
            this.lblEmployee.AutoSize = true;
            this.lblEmployee.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblEmployee.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.lblEmployee.Location = new System.Drawing.Point(12, 43);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(71, 15);
            this.lblEmployee.TabIndex = 1;
            this.lblEmployee.Text = "Nhan vien *";
            // 
            // cboEmployee
            // 
            this.cboEmployee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEmployee.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboEmployee.Location = new System.Drawing.Point(100, 38);
            this.cboEmployee.Name = "cboEmployee";
            this.cboEmployee.Size = new System.Drawing.Size(243, 25);
            this.cboEmployee.TabIndex = 2;
            // 
            // lblPayPeriod
            // 
            this.lblPayPeriod.AutoSize = true;
            this.lblPayPeriod.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPayPeriod.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.lblPayPeriod.Location = new System.Drawing.Point(12, 73);
            this.lblPayPeriod.Name = "lblPayPeriod";
            this.lblPayPeriod.Size = new System.Drawing.Size(62, 15);
            this.lblPayPeriod.TabIndex = 3;
            this.lblPayPeriod.Text = "Ky luong *";
            // 
            // txtPayPeriod
            // 
            this.txtPayPeriod.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtPayPeriod.Location = new System.Drawing.Point(100, 68);
            this.txtPayPeriod.Name = "txtPayPeriod";
            this.txtPayPeriod.Size = new System.Drawing.Size(150, 24);
            this.txtPayPeriod.TabIndex = 4;
            // 
            // lblBaseSalary
            // 
            this.lblBaseSalary.AutoSize = true;
            this.lblBaseSalary.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblBaseSalary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.lblBaseSalary.Location = new System.Drawing.Point(12, 103);
            this.lblBaseSalary.Name = "lblBaseSalary";
            this.lblBaseSalary.Size = new System.Drawing.Size(88, 15);
            this.lblBaseSalary.TabIndex = 5;
            this.lblBaseSalary.Text = "Luong co ban *";
            // 
            // txtBaseSalary
            // 
            this.txtBaseSalary.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtBaseSalary.Location = new System.Drawing.Point(100, 98);
            this.txtBaseSalary.Name = "txtBaseSalary";
            this.txtBaseSalary.Size = new System.Drawing.Size(170, 24);
            this.txtBaseSalary.TabIndex = 6;
            // 
            // lblBonus
            // 
            this.lblBonus.AutoSize = true;
            this.lblBonus.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblBonus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.lblBonus.Location = new System.Drawing.Point(12, 133);
            this.lblBonus.Name = "lblBonus";
            this.lblBonus.Size = new System.Drawing.Size(49, 15);
            this.lblBonus.TabIndex = 7;
            this.lblBonus.Text = "Thuong";
            // 
            // txtBonus
            // 
            this.txtBonus.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtBonus.Location = new System.Drawing.Point(100, 127);
            this.txtBonus.Name = "txtBonus";
            this.txtBonus.Size = new System.Drawing.Size(170, 24);
            this.txtBonus.TabIndex = 8;
            // 
            // lblAllowances
            // 
            this.lblAllowances.AutoSize = true;
            this.lblAllowances.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblAllowances.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.lblAllowances.Location = new System.Drawing.Point(12, 163);
            this.lblAllowances.Name = "lblAllowances";
            this.lblAllowances.Size = new System.Drawing.Size(50, 15);
            this.lblAllowances.TabIndex = 9;
            this.lblAllowances.Text = "Phu cap";
            // 
            // txtAllowances
            // 
            this.txtAllowances.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtAllowances.Location = new System.Drawing.Point(100, 156);
            this.txtAllowances.Name = "txtAllowances";
            this.txtAllowances.Size = new System.Drawing.Size(170, 24);
            this.txtAllowances.TabIndex = 10;
            // 
            // lblDeductions
            // 
            this.lblDeductions.AutoSize = true;
            this.lblDeductions.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblDeductions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.lblDeductions.Location = new System.Drawing.Point(353, 42);
            this.lblDeductions.Name = "lblDeductions";
            this.lblDeductions.Size = new System.Drawing.Size(55, 15);
            this.lblDeductions.TabIndex = 11;
            this.lblDeductions.Text = "Khau tru";
            // 
            // txtDeductions
            // 
            this.txtDeductions.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtDeductions.Location = new System.Drawing.Point(438, 34);
            this.txtDeductions.Name = "txtDeductions";
            this.txtDeductions.Size = new System.Drawing.Size(240, 24);
            this.txtDeductions.TabIndex = 12;
            // 
            // lblNetPay
            // 
            this.lblNetPay.AutoSize = true;
            this.lblNetPay.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblNetPay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.lblNetPay.Location = new System.Drawing.Point(353, 72);
            this.lblNetPay.Name = "lblNetPay";
            this.lblNetPay.Size = new System.Drawing.Size(57, 15);
            this.lblNetPay.TabIndex = 13;
            this.lblNetPay.Text = "Thuc linh";
            // 
            // txtNetPay
            // 
            this.txtNetPay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(250)))), ((int)(((byte)(230)))));
            this.txtNetPay.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.txtNetPay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(94)))), ((int)(((byte)(32)))));
            this.txtNetPay.Location = new System.Drawing.Point(438, 64);
            this.txtNetPay.Name = "txtNetPay";
            this.txtNetPay.ReadOnly = true;
            this.txtNetPay.Size = new System.Drawing.Size(240, 24);
            this.txtNetPay.TabIndex = 14;
            this.txtNetPay.Text = "0";
            // 
            // lblPayDate
            // 
            this.lblPayDate.AutoSize = true;
            this.lblPayDate.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPayDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.lblPayDate.Location = new System.Drawing.Point(353, 102);
            this.lblPayDate.Name = "lblPayDate";
            this.lblPayDate.Size = new System.Drawing.Size(88, 15);
            this.lblPayDate.TabIndex = 15;
            this.lblPayDate.Text = "Ngay tra luong";
            // 
            // dtpPayDate
            // 
            this.dtpPayDate.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dtpPayDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPayDate.Location = new System.Drawing.Point(468, 94);
            this.dtpPayDate.Name = "dtpPayDate";
            this.dtpPayDate.Size = new System.Drawing.Size(210, 24);
            this.dtpPayDate.TabIndex = 16;
            // 
            // lblPayStatus
            // 
            this.lblPayStatus.AutoSize = true;
            this.lblPayStatus.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPayStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.lblPayStatus.Location = new System.Drawing.Point(353, 132);
            this.lblPayStatus.Name = "lblPayStatus";
            this.lblPayStatus.Size = new System.Drawing.Size(62, 15);
            this.lblPayStatus.TabIndex = 17;
            this.lblPayStatus.Text = "Trang thai";
            // 
            // cboPayStatus
            // 
            this.cboPayStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPayStatus.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboPayStatus.Location = new System.Drawing.Point(438, 124);
            this.cboPayStatus.Name = "cboPayStatus";
            this.cboPayStatus.Size = new System.Drawing.Size(240, 25);
            this.cboPayStatus.TabIndex = 18;
            // 
            // lblApprovedBy
            // 
            this.lblApprovedBy.AutoSize = true;
            this.lblApprovedBy.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblApprovedBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.lblApprovedBy.Location = new System.Drawing.Point(353, 162);
            this.lblApprovedBy.Name = "lblApprovedBy";
            this.lblApprovedBy.Size = new System.Drawing.Size(75, 15);
            this.lblApprovedBy.TabIndex = 19;
            this.lblApprovedBy.Text = "Nguoi duyet";
            // 
            // txtApprovedBy
            // 
            this.txtApprovedBy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.txtApprovedBy.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtApprovedBy.Location = new System.Drawing.Point(453, 154);
            this.txtApprovedBy.Name = "txtApprovedBy";
            this.txtApprovedBy.ReadOnly = true;
            this.txtApprovedBy.Size = new System.Drawing.Size(225, 24);
            this.txtApprovedBy.TabIndex = 20;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(213, 197);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(78, 30);
            this.btnAdd.TabIndex = 21;
            this.btnAdd.Text = "+ Them";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(81)))), ((int)(((byte)(0)))));
            this.btnUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(299, 197);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(78, 30);
            this.btnUpdate.TabIndex = 22;
            this.btnUpdate.Text = "Cap nhat";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnApprove
            // 
            this.btnApprove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.btnApprove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApprove.FlatAppearance.BorderSize = 0;
            this.btnApprove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApprove.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnApprove.ForeColor = System.Drawing.Color.White;
            this.btnApprove.Location = new System.Drawing.Point(385, 197);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(88, 30);
            this.btnApprove.TabIndex = 23;
            this.btnApprove.Text = "Phe duyet";
            this.btnApprove.UseVisualStyleBackColor = false;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(481, 197);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 30);
            this.btnCancel.TabIndex = 24;
            this.btnCancel.Text = "Huy phieu";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(577, 197);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(78, 30);
            this.btnClear.TabIndex = 25;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // Salary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.dgvPayroll);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlForm);
            this.Name = "Salary";
            this.Size = new System.Drawing.Size(692, 526);
            this.Load += new System.EventHandler(this.Salary_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayroll)).EndInit();
            this.pnlForm.ResumeLayout(false);
            this.pnlForm.PerformLayout();
            this.pnlFormHeader.ResumeLayout(false);
            this.pnlFormHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        // ── Control declarations ───────────────────────────────────────
        private System.Windows.Forms.Panel        pnlHeader;
        private System.Windows.Forms.Label        lblTitle;
        private System.Windows.Forms.Label        lblCount;

        private System.Windows.Forms.Panel        pnlSearch;
        private System.Windows.Forms.Label        lblSearch;
        private System.Windows.Forms.TextBox      txtSearch;
        private System.Windows.Forms.Label        lblThang;
        private System.Windows.Forms.ComboBox     cboThang;
        private System.Windows.Forms.Label        lblNam;
        private System.Windows.Forms.ComboBox     cboNam;
        private System.Windows.Forms.Label        lblStatusFilter;
        private System.Windows.Forms.ComboBox     cboFilterStatus;
        private System.Windows.Forms.Button       btnSearch;
        private System.Windows.Forms.Button       btnRefresh;

        private System.Windows.Forms.DataGridView dgvPayroll;

        private System.Windows.Forms.Panel        pnlForm;
        private System.Windows.Forms.Panel        pnlFormHeader;
        private System.Windows.Forms.Label        lblFormTitle;
        private System.Windows.Forms.Label        lblStatus;

        private System.Windows.Forms.Label        lblEmployee;
        private System.Windows.Forms.ComboBox     cboEmployee;
        private System.Windows.Forms.Label        lblPayPeriod;
        private System.Windows.Forms.TextBox      txtPayPeriod;
        private System.Windows.Forms.Label        lblBaseSalary;
        private System.Windows.Forms.TextBox      txtBaseSalary;
        private System.Windows.Forms.Label        lblBonus;
        private System.Windows.Forms.TextBox      txtBonus;
        private System.Windows.Forms.Label        lblAllowances;
        private System.Windows.Forms.TextBox      txtAllowances;

        private System.Windows.Forms.Label        lblDeductions;
        private System.Windows.Forms.TextBox      txtDeductions;
        private System.Windows.Forms.Label        lblNetPay;
        private System.Windows.Forms.TextBox      txtNetPay;
        private System.Windows.Forms.Label        lblPayDate;
        private System.Windows.Forms.DateTimePicker dtpPayDate;
        private System.Windows.Forms.Label        lblPayStatus;
        private System.Windows.Forms.ComboBox     cboPayStatus;
        private System.Windows.Forms.Label        lblApprovedBy;
        private System.Windows.Forms.TextBox      txtApprovedBy;

        private System.Windows.Forms.Button       btnAdd;
        private System.Windows.Forms.Button       btnUpdate;
        private System.Windows.Forms.Button       btnApprove;
        private System.Windows.Forms.Button       btnCancel;
        private System.Windows.Forms.Button       btnClear;
    }
}
