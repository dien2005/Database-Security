using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BLL;

namespace EmployeeManagementSystem
{
    public partial class Department : UserControl
    {
        private readonly DepartmentService _service;

        public Department()
        {
            InitializeComponent();
            _service = new DepartmentService();
            
            // Wire events
            this.Load += Department_Load;
            
            if (this.btnRefresh != null)
                this.btnRefresh.Click += btnRefresh_Click;
                
            if (this.dgvDepartments != null)
                this.dgvDepartments.CellClick += dgvDepartments_CellClick;
        }

        private void Department_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var dt = _service.SearchDepartments();
                if (dgvDepartments == null) return;
                
                dgvDepartments.DataSource = dt;
                
                if (dgvDepartments.Columns["department_id"] != null)
                {
                    dgvDepartments.Columns["department_id"].HeaderText = "ID";
                    dgvDepartments.Columns["dept_name"].HeaderText = "Tên phòng";
                    dgvDepartments.Columns["dept_code"].HeaderText = "Mã phòng";
                    dgvDepartments.Columns["manager_id"].Visible = false;
                    dgvDepartments.Columns["manager_name"].HeaderText = "Trưởng phòng";
                    dgvDepartments.Columns["location_id"].Visible = false;
                    dgvDepartments.Columns["location_name"].HeaderText = "Địa điểm";
                    dgvDepartments.Columns["parent_dept_id"].Visible = false;
                    dgvDepartments.Columns["parent_dept_name"].HeaderText = "Phòng ban cha";
                    dgvDepartments.Columns["status"].HeaderText = "Trạng thái";
                    dgvDepartments.Columns["created_at"].Visible = false;
                }
                
                if (lblCount != null)
                    lblCount.Text = $"Hiển thị {dt.Rows.Count} phòng ban";
                    
                if (lblStatusMsg != null)
                    lblStatusMsg.Text = "";
            }
            catch (Exception ex)
            {
                if (lblStatusMsg != null)
                    lblStatusMsg.Text = "Lỗi tải dữ liệu: " + ex.Message;
                else
                    MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvDepartments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvDepartments != null)
            {
                DataGridViewRow row = dgvDepartments.Rows[e.RowIndex];
                
                if (txtDeptName != null)
                    txtDeptName.Text = row.Cells["dept_name"].Value?.ToString();
                    
                if (txtDeptCode != null)
                    txtDeptCode.Text = row.Cells["dept_code"].Value?.ToString();
                
                if (cboManager != null)
                {
                    cboManager.Items.Clear();
                    if (row.Cells["manager_name"].Value != DBNull.Value)
                    {
                        cboManager.Items.Add(row.Cells["manager_name"].Value.ToString());
                        cboManager.SelectedIndex = 0;
                    }
                }
                
                if (cboLocation != null)
                {
                    cboLocation.Items.Clear();
                    if (row.Cells["location_name"].Value != DBNull.Value)
                    {
                        cboLocation.Items.Add(row.Cells["location_name"].Value.ToString());
                        cboLocation.SelectedIndex = 0;
                    }
                }
                
                if (cboParentDept != null)
                {
                    cboParentDept.Items.Clear();
                    if (row.Cells["parent_dept_name"].Value != DBNull.Value)
                    {
                        cboParentDept.Items.Add(row.Cells["parent_dept_name"].Value.ToString());
                        cboParentDept.SelectedIndex = 0;
                    }
                }
                
                if (cboStatus != null)
                {
                    string status = row.Cells["status"].Value?.ToString();
                    if (!string.IsNullOrEmpty(status))
                    {
                        if (cboStatus.Items.Contains(status))
                        {
                            cboStatus.SelectedItem = status;
                        }
                        else
                        {
                            cboStatus.Items.Clear();
                            cboStatus.Items.Add(status);
                            cboStatus.SelectedIndex = 0;
                        }
                    }
                }
            }
        }
    }
}
