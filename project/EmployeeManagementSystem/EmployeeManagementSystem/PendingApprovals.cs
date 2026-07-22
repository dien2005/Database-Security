using System;
using System.Data;
using System.Windows.Forms;
using BLL;

namespace EmployeeManagementSystem
{
    public partial class PendingApprovals : UserControl
    {
        private readonly PendingApprovalService _service;
        private int _selectedApprovalId = -1;

        public PendingApprovals()
        {
            InitializeComponent();
            _service = new PendingApprovalService();

            this.Load += PendingApprovals_Load;
            
            if (btnSearch != null) btnSearch.Click += btnSearch_Click;
            if (btnRefresh != null) btnRefresh.Click += btnRefresh_Click;
            if (btnApprove != null) btnApprove.Click += btnApprove_Click;
            if (btnReject != null) btnReject.Click += btnReject_Click;
            if (dgvApprovals != null) dgvApprovals.CellClick += dgvApprovals_CellClick;
            if (cboStatusFilter != null) cboStatusFilter.SelectedIndexChanged += cboStatusFilter_SelectedIndexChanged;
        }

        private void PendingApprovals_Load(object sender, EventArgs e)
        {
            if (cboStatusFilter != null && cboStatusFilter.Items.Count > 0)
            {
                // Mặc định chọn PENDING
                cboStatusFilter.SelectedIndex = 1; 
            }
            LoadData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (txtSearch != null) txtSearch.Text = "";
            LoadData();
        }

        private void cboStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string kw = txtSearch?.Text;
                string status = cboStatusFilter?.SelectedItem?.ToString();
                
                var dt = _service.Search(kw, status);
                if (dgvApprovals == null) return;
                
                dgvApprovals.DataSource = dt;
                
                // Format lưới
                if (dgvApprovals.Columns["approval_id"] != null)
                {
                    dgvApprovals.Columns["approval_id"].HeaderText = "ID";
                    dgvApprovals.Columns["action_type"].HeaderText = "Loại yêu cầu";
                    dgvApprovals.Columns["target_table"].HeaderText = "Bảng đích";
                    dgvApprovals.Columns["target_id"].HeaderText = "ID Đích";
                    dgvApprovals.Columns["requested_by"].HeaderText = "Người yêu cầu";
                    dgvApprovals.Columns["request_time"].HeaderText = "Thời gian Y/C";
                    dgvApprovals.Columns["payload"].Visible = false; // Ẩn cột JSON, chỉ hiện bên trái
                    dgvApprovals.Columns["approved_by_1"].HeaderText = "Người duyệt 1";
                    dgvApprovals.Columns["approval_time_1"].Visible = false;
                    dgvApprovals.Columns["approved_by_2"].HeaderText = "Người duyệt 2";
                    dgvApprovals.Columns["approval_time_2"].Visible = false;
                    dgvApprovals.Columns["status"].HeaderText = "Trạng thái";
                    dgvApprovals.Columns["final_action_time"].Visible = false;
                    dgvApprovals.Columns["notes"].Visible = false;
                }
                
                if (lblCount != null)
                    lblCount.Text = $"Hiển thị {dt.Rows.Count} yêu cầu";
                if (lblStatusMsg != null)
                    lblStatusMsg.Text = "";
                    
                ClearInputs();
            }
            catch (Exception ex)
            {
                if (lblStatusMsg != null)
                    lblStatusMsg.Text = "Lỗi tải dữ liệu: " + ex.Message;
                else
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvApprovals_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvApprovals != null)
            {
                DataGridViewRow row = dgvApprovals.Rows[e.RowIndex];
                
                if (row.Cells["approval_id"].Value != DBNull.Value)
                    _selectedApprovalId = Convert.ToInt32(row.Cells["approval_id"].Value);
                else
                    _selectedApprovalId = -1;

                if (txtAction != null)
                    txtAction.Text = row.Cells["action_type"].Value?.ToString();
                    
                if (txtTarget != null)
                {
                    string tbl = row.Cells["target_table"].Value?.ToString();
                    string tid = row.Cells["target_id"].Value?.ToString();
                    txtTarget.Text = $"{tbl} (ID: {tid})";
                }
                    
                if (txtRequestedBy != null)
                    txtRequestedBy.Text = row.Cells["requested_by"].Value?.ToString();
                    
                if (txtPayload != null)
                {
                    // JSON có thể được format lại cho đẹp nếu muốn, tạm thời hiện thô
                    txtPayload.Text = row.Cells["payload"].Value?.ToString();
                }
            }
        }

        private void ClearInputs()
        {
            _selectedApprovalId = -1;
            if (txtAction != null) txtAction.Text = "";
            if (txtTarget != null) txtTarget.Text = "";
            if (txtRequestedBy != null) txtRequestedBy.Text = "";
            if (txtPayload != null) txtPayload.Text = "";
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (_selectedApprovalId <= 0)
            {
                MessageBox.Show("Vui lòng chọn một yêu cầu để duyệt!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dialogResult = MessageBox.Show($"Bạn có chắc chắn muốn duyệt yêu cầu ID {_selectedApprovalId} không?", 
                                               "Xác nhận Duyệt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (dialogResult == DialogResult.Yes)
            {
                var result = _service.Approve(_selectedApprovalId);
                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            if (_selectedApprovalId <= 0)
            {
                MessageBox.Show("Vui lòng chọn một yêu cầu để từ chối!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dialogResult = MessageBox.Show($"Bạn có chắc chắn muốn TỪ CHỐI yêu cầu ID {_selectedApprovalId} không?", 
                                               "Xác nhận Từ chối", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            
            if (dialogResult == DialogResult.Yes)
            {
                var result = _service.Reject(_selectedApprovalId, "Từ chối bởi quản lý.");
                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
