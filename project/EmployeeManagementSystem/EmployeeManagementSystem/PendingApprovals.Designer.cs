namespace EmployeeManagementSystem
{
    partial class PendingApprovals
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblAction = new System.Windows.Forms.Label();
            this.txtAction = new System.Windows.Forms.TextBox();
            this.lblTarget = new System.Windows.Forms.Label();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.lblRequestedBy = new System.Windows.Forms.Label();
            this.txtRequestedBy = new System.Windows.Forms.TextBox();
            this.lblPayload = new System.Windows.Forms.Label();
            this.txtPayload = new System.Windows.Forms.TextBox();
            this.lblStatusFilter = new System.Windows.Forms.Label();
            this.cboStatusFilter = new System.Windows.Forms.ComboBox();
            this.btnApprove = new System.Windows.Forms.Button();
            this.btnReject = new System.Windows.Forms.Button();
            this.lblStatusMsg = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblGridTitle = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dgvApprovals = new System.Windows.Forms.DataGridView();
            this.lblCount = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvApprovals)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Controls.Add(this.lblAction);
            this.panel1.Controls.Add(this.txtAction);
            this.panel1.Controls.Add(this.lblTarget);
            this.panel1.Controls.Add(this.txtTarget);
            this.panel1.Controls.Add(this.lblRequestedBy);
            this.panel1.Controls.Add(this.txtRequestedBy);
            this.panel1.Controls.Add(this.lblPayload);
            this.panel1.Controls.Add(this.txtPayload);
            this.panel1.Controls.Add(this.lblStatusFilter);
            this.panel1.Controls.Add(this.cboStatusFilter);
            this.panel1.Controls.Add(this.btnApprove);
            this.panel1.Controls.Add(this.btnReject);
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
            this.lblTitle.Size = new System.Drawing.Size(144, 18);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "CHI TIẾT YÊU CẦU";
            // 
            // lblAction
            // 
            this.lblAction.AutoSize = true;
            this.lblAction.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblAction.Location = new System.Drawing.Point(14, 45);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new System.Drawing.Size(95, 14);
            this.lblAction.TabIndex = 1;
            this.lblAction.Text = "Loại hành động:";
            // 
            // txtAction
            // 
            this.txtAction.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtAction.Location = new System.Drawing.Point(14, 65);
            this.txtAction.Name = "txtAction";
            this.txtAction.ReadOnly = true;
            this.txtAction.Size = new System.Drawing.Size(192, 22);
            this.txtAction.TabIndex = 2;
            // 
            // lblTarget
            // 
            this.lblTarget.AutoSize = true;
            this.lblTarget.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblTarget.Location = new System.Drawing.Point(14, 95);
            this.lblTarget.Name = "lblTarget";
            this.lblTarget.Size = new System.Drawing.Size(64, 14);
            this.lblTarget.TabIndex = 3;
            this.lblTarget.Text = "Bảng đích:";
            // 
            // txtTarget
            // 
            this.txtTarget.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtTarget.Location = new System.Drawing.Point(14, 115);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.ReadOnly = true;
            this.txtTarget.Size = new System.Drawing.Size(192, 22);
            this.txtTarget.TabIndex = 4;
            // 
            // lblRequestedBy
            // 
            this.lblRequestedBy.AutoSize = true;
            this.lblRequestedBy.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblRequestedBy.Location = new System.Drawing.Point(14, 145);
            this.lblRequestedBy.Name = "lblRequestedBy";
            this.lblRequestedBy.Size = new System.Drawing.Size(90, 14);
            this.lblRequestedBy.TabIndex = 5;
            this.lblRequestedBy.Text = "Người yêu cầu:";
            // 
            // txtRequestedBy
            // 
            this.txtRequestedBy.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtRequestedBy.Location = new System.Drawing.Point(14, 165);
            this.txtRequestedBy.Name = "txtRequestedBy";
            this.txtRequestedBy.ReadOnly = true;
            this.txtRequestedBy.Size = new System.Drawing.Size(192, 22);
            this.txtRequestedBy.TabIndex = 6;
            // 
            // lblPayload
            // 
            this.lblPayload.AutoSize = true;
            this.lblPayload.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblPayload.Location = new System.Drawing.Point(14, 195);
            this.lblPayload.Name = "lblPayload";
            this.lblPayload.Size = new System.Drawing.Size(108, 14);
            this.lblPayload.TabIndex = 7;
            this.lblPayload.Text = "Nội dung thay đổi:";
            // 
            // txtPayload
            // 
            this.txtPayload.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.txtPayload.Location = new System.Drawing.Point(14, 215);
            this.txtPayload.Multiline = true;
            this.txtPayload.Name = "txtPayload";
            this.txtPayload.ReadOnly = true;
            this.txtPayload.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPayload.Size = new System.Drawing.Size(192, 90);
            this.txtPayload.TabIndex = 8;
            // 
            // lblStatusFilter
            // 
            this.lblStatusFilter.AutoSize = true;
            this.lblStatusFilter.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblStatusFilter.Location = new System.Drawing.Point(14, 315);
            this.lblStatusFilter.Name = "lblStatusFilter";
            this.lblStatusFilter.Size = new System.Drawing.Size(117, 14);
            this.lblStatusFilter.TabIndex = 9;
            this.lblStatusFilter.Text = "Lọc theo trạng thái:";
            // 
            // cboStatusFilter
            // 
            this.cboStatusFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatusFilter.Font = new System.Drawing.Font("Tahoma", 9F);
            this.cboStatusFilter.FormattingEnabled = true;
            this.cboStatusFilter.Items.AddRange(new object[] {
            "ALL",
            "PENDING",
            "APPROVED",
            "REJECTED"});
            this.cboStatusFilter.Location = new System.Drawing.Point(14, 335);
            this.cboStatusFilter.Name = "cboStatusFilter";
            this.cboStatusFilter.Size = new System.Drawing.Size(192, 22);
            this.cboStatusFilter.TabIndex = 10;
            // 
            // btnApprove
            // 
            this.btnApprove.BackColor = System.Drawing.Color.ForestGreen;
            this.btnApprove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApprove.FlatAppearance.BorderSize = 0;
            this.btnApprove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApprove.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnApprove.ForeColor = System.Drawing.Color.White;
            this.btnApprove.Location = new System.Drawing.Point(14, 375);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(92, 35);
            this.btnApprove.TabIndex = 11;
            this.btnApprove.Text = "Duyệt (✓)";
            this.btnApprove.UseVisualStyleBackColor = false;
            // 
            // btnReject
            // 
            this.btnReject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnReject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReject.FlatAppearance.BorderSize = 0;
            this.btnReject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReject.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnReject.ForeColor = System.Drawing.Color.White;
            this.btnReject.Location = new System.Drawing.Point(114, 375);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(92, 35);
            this.btnReject.TabIndex = 12;
            this.btnReject.Text = "Từ chối (X)";
            this.btnReject.UseVisualStyleBackColor = false;
            // 
            // lblStatusMsg
            // 
            this.lblStatusMsg.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblStatusMsg.ForeColor = System.Drawing.Color.Red;
            this.lblStatusMsg.Location = new System.Drawing.Point(14, 420);
            this.lblStatusMsg.Name = "lblStatusMsg";
            this.lblStatusMsg.Size = new System.Drawing.Size(192, 60);
            this.lblStatusMsg.TabIndex = 13;
            this.lblStatusMsg.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lblGridTitle);
            this.panel2.Controls.Add(this.txtSearch);
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Controls.Add(this.btnRefresh);
            this.panel2.Controls.Add(this.dgvApprovals);
            this.panel2.Controls.Add(this.lblCount);
            this.panel2.Location = new System.Drawing.Point(250, 15);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(428, 496);
            this.panel2.TabIndex = 1;
            // 
            // lblGridTitle
            // 
            this.lblGridTitle.AutoSize = true;
            this.lblGridTitle.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGridTitle.Location = new System.Drawing.Point(14, 15);
            this.lblGridTitle.Name = "lblGridTitle";
            this.lblGridTitle.Size = new System.Drawing.Size(160, 18);
            this.lblGridTitle.TabIndex = 0;
            this.lblGridTitle.Text = "Danh sách chờ duyệt";
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtSearch.Location = new System.Drawing.Point(174, 14);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(106, 22);
            this.txtSearch.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Location = new System.Drawing.Point(285, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(65, 26);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Tìm kiếm";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.Teal;
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(355, 12);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(60, 26);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Tải lại";
            this.btnRefresh.UseVisualStyleBackColor = false;
            // 
            // dgvApprovals
            // 
            this.dgvApprovals.AllowUserToAddRows = false;
            this.dgvApprovals.AllowUserToDeleteRows = false;
            this.dgvApprovals.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvApprovals.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(11)))), ((int)(((byte)(97)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvApprovals.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvApprovals.ColumnHeadersHeight = 30;
            this.dgvApprovals.EnableHeadersVisualStyles = false;
            this.dgvApprovals.Location = new System.Drawing.Point(14, 50);
            this.dgvApprovals.Name = "dgvApprovals";
            this.dgvApprovals.ReadOnly = true;
            this.dgvApprovals.RowHeadersVisible = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(8)))), ((int)(((byte)(138)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            this.dgvApprovals.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvApprovals.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvApprovals.Size = new System.Drawing.Size(400, 410);
            this.dgvApprovals.TabIndex = 4;
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic);
            this.lblCount.Location = new System.Drawing.Point(14, 470);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(90, 13);
            this.lblCount.TabIndex = 5;
            this.lblCount.Text = "Hien thi 0 ban ghi";
            // 
            // PendingApprovals
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Azure;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "PendingApprovals";
            this.Size = new System.Drawing.Size(692, 526);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvApprovals)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblAction;
        private System.Windows.Forms.TextBox txtAction;
        private System.Windows.Forms.Label lblTarget;
        private System.Windows.Forms.TextBox txtTarget;
        private System.Windows.Forms.Label lblRequestedBy;
        private System.Windows.Forms.TextBox txtRequestedBy;
        private System.Windows.Forms.Label lblPayload;
        private System.Windows.Forms.TextBox txtPayload;
        private System.Windows.Forms.Label lblStatusFilter;
        private System.Windows.Forms.ComboBox cboStatusFilter;
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.Button btnReject;
        private System.Windows.Forms.Label lblStatusMsg;

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblGridTitle;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dgvApprovals;
        private System.Windows.Forms.Label lblCount;
    }
}
