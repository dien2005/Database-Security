using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BLL;
using DTO;

namespace EmployeeManagementSystem
{
    public partial class AddEmployee : UserControl
    {
        private readonly EmployeeService _svc = new EmployeeService();
        private int? _selectedEmployeeId = null;
        private List<EmployeeDTO> _currentList = new List<EmployeeDTO>();

        public AddEmployee()
        {
            InitializeComponent();
        }

        private void AddEmployee_Load(object sender, EventArgs e)
        {
            ApplyRolePermissions();
            LoadLookups();
            LoadEmployees(null, null);
        }

        /// <summary>
        /// Ẩn các nút CRUD nếu user không có quyền hr_staff hoặc dept_manager.
        /// Chỉ 2 role trên mới được Thêm / Sửa / Xoá nhân viên.
        /// </summary>
        private void ApplyRolePermissions()
        {
            var user = BLL.SessionManager.CurrentUser;
            bool canEdit = user != null && (user.IsHrStaff || user.IsDeptManager);

            btnAdd.Visible = canEdit;
            btnUpdate.Visible = canEdit;
            btnDelete.Visible = canEdit;

            // Các field nhập liệu cũng chỉ cần khi có quyền edit
            // (vẫn hiển thị để xem thông tin khi click hàng, chỉ disable)
            txtFullName.ReadOnly = !canEdit;
            txtEmail.ReadOnly = !canEdit;
            txtPhone.ReadOnly = !canEdit;
            dtpHireDate.Enabled = canEdit;
            cboJob.Enabled = canEdit;
            cboDepartment.Enabled = canEdit;
            cboLocation.Enabled = canEdit;
            cboManager.Enabled = canEdit;
            cboStatus.Enabled = canEdit;

            if (!canEdit)
                lblFormTitle.Text = "  XEM THONG TIN NHAN VIEN  (ban khong co quyen chinh sua)";
        }

        /// <summary>
        /// Gọi khi MainForm chuyển sang tab này (được gọi từ bên ngoài).
        /// </summary>
        public void RefreshData()
        {
            if (InvokeRequired) { Invoke((MethodInvoker)RefreshData); return; }
            LoadLookups();
            LoadEmployees(null, null);
        }
        private void LoadLookups()
        {
            try
            {
                // ── Jobs ──────────────────────────────────────────
                var jobs = _svc.GetJobLookup();
                cboJob.DataSource = new List<LookupItemDTO>(jobs);
                cboJob.DisplayMember = "Text";
                cboJob.ValueMember = "Id";

                // ── Departments ──────────────────────────────────
                var depts = _svc.GetDepartmentLookup();
                cboDepartment.DataSource = new List<LookupItemDTO>(depts);
                cboDepartment.DisplayMember = "Text";
                cboDepartment.ValueMember = "Id";

                // Filter dept ComboBox — thêm "Tất cả" ở đầu
                var filterDepts = new List<LookupItemDTO> { new LookupItemDTO { Id = -1, Text = "-- Tất cả --" } };
                filterDepts.AddRange(depts);
                cboFilterDept.DataSource = filterDepts;
                cboFilterDept.DisplayMember = "Text";
                cboFilterDept.ValueMember = "Id";

                // ── Locations ────────────────────────────────────
                var locs = _svc.GetLocationLookup();
                cboLocation.DataSource = new List<LookupItemDTO>(locs);
                cboLocation.DisplayMember = "Text";
                cboLocation.ValueMember = "Id";

                // ── Managers (có thêm tuỳ chọn "Không có") ──────
                var mgrs = new List<LookupItemDTO> { new LookupItemDTO { Id = -1, Text = "-- Không có --" } };
                mgrs.AddRange(_svc.GetManagerLookup());
                cboManager.DataSource = mgrs;
                cboManager.DisplayMember = "Text";
                cboManager.ValueMember = "Id";

                // ── Status combobox ──────────────────────────────
                cboStatus.Items.Clear();
                cboStatus.Items.AddRange(new object[] { "ACTIVE", "INACTIVE", "TERMINATED" });
                cboStatus.SelectedIndex = 0;

                // ── Filter status ────────────────────────────────
                cboFilterStatus.Items.Clear();
                cboFilterStatus.Items.AddRange(new object[] { "-- Tất cả --", "ACTIVE", "INACTIVE", "TERMINATED" });
                cboFilterStatus.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ShowStatus($"Lỗi nạp dữ liệu: {ex.Message}", false);
            }
        }

        private void LoadEmployees(string keyword, int? deptId)
        {
            try
            {
                var result = _svc.Search(keyword, deptId);
                if (!result.Success)
                {
                    ShowStatus(result.ErrorMessage, false);
                    return;
                }
                var list = result.Data;
                string statusFilter = cboFilterStatus.SelectedItem?.ToString();
                if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "-- Tất cả --")
                    list = list.Where(e => e.Status == statusFilter).ToList();

                _currentList = list;
                BindGrid(list);
                ClearForm();
            }
            catch (Exception ex)
            {
                ShowStatus($"Lỗi: {ex.Message}", false);
            }
        }

        private void BindGrid(List<EmployeeDTO> list)
        {
            // Tạo DataSource ẩn employee_id (cột 0) để lấy khi click
            var table = new System.Data.DataTable();
            table.Columns.Add("employee_id", typeof(int));
            table.Columns.Add("Họ tên", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Điện thoại", typeof(string));
            table.Columns.Add("Chức danh", typeof(string));
            table.Columns.Add("Phòng ban", typeof(string));
            table.Columns.Add("Địa điểm", typeof(string));
            table.Columns.Add("Quản lý", typeof(string));
            table.Columns.Add("Ngày vào", typeof(string));
            table.Columns.Add("Trạng thái", typeof(string));

            foreach (var e in list)
            {
                table.Rows.Add(
                    e.EmployeeId,
                    e.FullName,
                    e.Email,
                    e.Phone ?? "",
                    e.JobTitle ?? "",
                    e.DeptName ?? "",
                    e.LocationName ?? "",
                    e.ManagerName ?? "",
                    e.HireDate.ToString("dd/MM/yyyy"),
                    e.Status
                );
            }

            dgvEmployees.DataSource = table;

            // Ẩn cột employee_id
            if (dgvEmployees.Columns.Contains("employee_id"))
                dgvEmployees.Columns["employee_id"].Visible = false;

            // Tô màu cột Trạng thái
            dgvEmployees.CellFormatting += DgvEmployees_CellFormatting;

            lblCount.Text = $"Hiển thị {list.Count} nhân viên";
        }

        // Tô màu badge theo status
        private void DgvEmployees_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvEmployees.Columns[e.ColumnIndex].Name == "Trạng thái" && e.Value != null)
            {
                switch (e.Value.ToString())
                {
                    case "ACTIVE":
                        e.CellStyle.ForeColor = Color.FromArgb(46, 125, 50);
                        e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                        break;
                    case "INACTIVE":
                        e.CellStyle.ForeColor = Color.FromArgb(230, 81, 0);
                        e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                        break;
                    case "TERMINATED":
                        e.CellStyle.ForeColor = Color.FromArgb(198, 40, 40);
                        e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                        break;
                }
            }
        }
        private void dgvEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvEmployees.Rows[e.RowIndex];
            _selectedEmployeeId = Convert.ToInt32(row.Cells["employee_id"].Value);

            // Tìm trong cache
            var emp = _currentList.FirstOrDefault(x => x.EmployeeId == _selectedEmployeeId);
            if (emp == null) return;

            // Điền TextBox
            txtFullName.Text = emp.FullName;
            txtEmail.Text = emp.Email;
            txtPhone.Text = emp.Phone ?? "";
            dtpHireDate.Value = emp.HireDate;

            // Điền ComboBox bằng Value (Id)
            SetComboValue(cboJob, emp.JobId);
            SetComboValue(cboDepartment, emp.DepartmentId);
            SetComboValue(cboLocation, emp.LocationId);
            SetComboValue(cboManager, emp.ManagerId ?? -1);

            if (cboStatus.Items.Contains(emp.Status))
                cboStatus.SelectedItem = emp.Status;

            ShowStatus($"Đã chọn: {emp.FullName}", true);
            lblFormTitle.Text = $"✏  SỬA NHÂN VIÊN  [ID: {emp.EmployeeId}]";
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string kw = txtSearch.Text.Trim();
            int? deptId = null;

            if (cboFilterDept.SelectedValue is int)
            {
                int selectedId = (int)cboFilterDept.SelectedValue;
                if (selectedId > 0)
                    deptId = selectedId;
            }

            LoadEmployees(string.IsNullOrEmpty(kw) ? null : kw, deptId);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            cboFilterDept.SelectedIndex = 0;
            cboFilterStatus.SelectedIndex = 0;
            LoadEmployees(null, null);
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                btnSearch_Click(sender, EventArgs.Empty);
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            var dto    = BuildDTO();
            var result = _svc.AddEmployee(dto);

            if (result.Success)
            {
                ShowStatus("Da them nhan vien thanh cong!", true);
                LoadEmployees(null, null);
            }
            else
            {
                ShowStatus(result.ErrorMessage, false);
            }
        }

        // ── UPDATE ───────────────────────────────────────────────────
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedEmployeeId == null)
            {
                ShowStatus("Chon nhan vien trong bang de sua.", false);
                return;
            }
            if (!ValidateForm()) return;

            var dto = BuildDTO();
            dto.EmployeeId = _selectedEmployeeId.Value;

            var result = _svc.UpdateEmployee(dto);

            if (result.Success)
            {
                ShowStatus("Cap nhat thanh cong!", true);
                LoadEmployees(null, null);
            }
            else
            {
                ShowStatus(result.ErrorMessage, false);
            }
        }

        // ── DELETE (soft) ────────────────────────────────────────────
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedEmployeeId == null)
            {
                ShowStatus("Chon nhan vien trong bang de xoa.", false);
                return;
            }

            var emp = _currentList.FirstOrDefault(x => x.EmployeeId == _selectedEmployeeId);
            string name = (emp != null) ? emp.FullName : "ID=" + _selectedEmployeeId;

            var confirm = MessageBox.Show(
                "Xac nhan doi trang thai '" + name + "' thanh TERMINATED?",
                "Xac nhan",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            var result = _svc.DeleteEmployee(_selectedEmployeeId.Value);

            if (result.Success)
            {
                ShowStatus("Da chuyen trang thai " + name + " -> TERMINATED.", true);
                LoadEmployees(null, null);
            }
            else
            {
                ShowStatus(result.ErrorMessage, false);
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                ShowStatus("Vui long nhap ho ten.", false);
                txtFullName.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                ShowStatus("Vui long nhap email.", false);
                txtEmail.Focus();
                return false;
            }
            if (cboJob.SelectedValue == null || (int)cboJob.SelectedValue <= 0)
            {
                ShowStatus("Vui long chon chuc danh.", false);
                return false;
            }
            if (cboDepartment.SelectedValue == null || (int)cboDepartment.SelectedValue <= 0)
            {
                ShowStatus("Vui long chon phong ban.", false);
                return false;
            }
            if (cboLocation.SelectedValue == null || (int)cboLocation.SelectedValue <= 0)
            {
                ShowStatus("Vui long chon dia diem.", false);
                return false;
            }
            return true;
        }

        private EmployeeDTO BuildDTO()
        {
            int managerId = -1;
            if (cboManager.SelectedValue is int)
                managerId = (int)cboManager.SelectedValue;

            return new EmployeeDTO
            {
                FullName = txtFullName.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Phone = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim(),
                HireDate = dtpHireDate.Value.Date,
                JobId = (int)cboJob.SelectedValue,
                DepartmentId = (int)cboDepartment.SelectedValue,
                LocationId = (int)cboLocation.SelectedValue,
                ManagerId = managerId > 0 ? (int?)managerId : null,
                Status = cboStatus.SelectedItem != null ? cboStatus.SelectedItem.ToString() : "ACTIVE"
            };
        }

        private void ClearForm()
        {
            _selectedEmployeeId = null;
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            dtpHireDate.Value = DateTime.Today;

            if (cboJob.Items.Count > 0) cboJob.SelectedIndex = 0;
            if (cboDepartment.Items.Count > 0) cboDepartment.SelectedIndex = 0;
            if (cboLocation.Items.Count > 0) cboLocation.SelectedIndex = 0;
            if (cboManager.Items.Count > 0) cboManager.SelectedIndex = 0;
            if (cboStatus.Items.Count > 0) cboStatus.SelectedIndex = 0;

            lblFormTitle.Text = "THEM / SUA NHAN VIEN";
            ShowStatus("", true);
        }

        private void ShowStatus(string msg, bool success)
        {
            lblStatus.Text = msg ?? "";
            lblStatus.ForeColor = success
                ? Color.FromArgb(100, 255, 150)
                : Color.FromArgb(255, 100, 100);
        }
        private void SetComboValue(ComboBox cb, int id)
        {
            if (cb.DataSource is List<LookupItemDTO> list)
            {
                var item = list.FirstOrDefault(x => x.Id == id);
                if (item != null) cb.SelectedItem = item;
            }
        }
    }
}