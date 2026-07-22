using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BLL;
using DTO;

namespace EmployeeManagementSystem
{
    public partial class Salary : UserControl
    {
        // ── Services ──────────────────────────────────────────────────
        private readonly PayrollService _svc = new PayrollService();

        // ── State ─────────────────────────────────────────────────────
        private int? _selectedPayrollId = null;
        private List<PayrollDTO> _currentList = new List<PayrollDTO>();

        public Salary()
        {
            InitializeComponent();
        }

        // ══════════════════════════════════════════════════════════════
        // LOAD
        // ══════════════════════════════════════════════════════════════
        private void Salary_Load(object sender, EventArgs e)
        {
            InitFilters();
            LoadEmployeeLookup();
            ApplyRolePermissions();
            LoadPayroll();

            // Recalc net pay realtime
            txtBaseSalary.TextChanged += (s, args) => RecalcNetPay();
            txtBonus.TextChanged      += (s, args) => RecalcNetPay();
            txtAllowances.TextChanged += (s, args) => RecalcNetPay();
            txtDeductions.TextChanged += (s, args) => RecalcNetPay();
        }

        /// <summary>Gọi từ MainForm khi chuyển tab.</summary>
        public void RefreshData()
        {
            if (InvokeRequired) { Invoke((MethodInvoker)RefreshData); return; }
            LoadPayroll();
        }

        // ══════════════════════════════════════════════════════════════
        // PHÂN QUYỀN
        // ══════════════════════════════════════════════════════════════
        private void ApplyRolePermissions()
        {
            var user       = SessionManager.CurrentUser;
            bool canEdit   = user != null && (user.IsFinStaff || user.IsHrManager);
            bool canApprove = user != null && user.IsHrManager;

            btnAdd.Visible     = canEdit;
            btnUpdate.Visible  = canEdit;
            btnCancel.Visible  = canEdit;
            btnApprove.Visible = canApprove;

            cboEmployee.Enabled    = canEdit;
            txtPayPeriod.ReadOnly  = !canEdit;
            txtBaseSalary.ReadOnly = !canEdit;
            txtBonus.ReadOnly      = !canEdit;
            txtAllowances.ReadOnly = !canEdit;
            txtDeductions.ReadOnly = !canEdit;
            dtpPayDate.Enabled     = canEdit;
            cboPayStatus.Enabled   = canEdit;

            if (!canEdit)
                lblFormTitle.Text = "XEM PHIEU LUONG  (ban khong co quyen chinh sua)";
        }

        // ══════════════════════════════════════════════════════════════
        // INIT FILTERS & LOOKUPS
        // ══════════════════════════════════════════════════════════════
        private void InitFilters()
        {
            cboThang.Items.Clear();
            cboThang.Items.Add("-- Tat ca --");
            for (int i = 1; i <= 12; i++)
                cboThang.Items.Add(i.ToString("D2"));
            cboThang.SelectedIndex = 0;

            cboNam.Items.Clear();
            cboNam.Items.Add("-- Tat ca --");
            int cy = DateTime.Today.Year;
            for (int y = cy; y >= cy - 2; y--)
                cboNam.Items.Add(y.ToString());
            cboNam.SelectedIndex = 0;

            cboFilterStatus.Items.Clear();
            cboFilterStatus.Items.AddRange(new object[] {
                "-- Tat ca --", "PENDING", "APPROVED", "PAID", "CANCELLED"
            });
            cboFilterStatus.SelectedIndex = 0;

            cboPayStatus.Items.Clear();
            cboPayStatus.Items.AddRange(new object[] {
                "PENDING", "APPROVED", "PAID", "CANCELLED"
            });
            cboPayStatus.SelectedIndex = 0;
        }

        private void LoadEmployeeLookup()
        {
            try
            {
                var emps = _svc.GetEmployeeLookup();
                var list = new List<LookupItemDTO>();
                list.Add(new LookupItemDTO { Id = -1, Text = "-- Chon nhan vien --" });
                list.AddRange(emps);

                cboEmployee.DataSource    = list;
                cboEmployee.DisplayMember = "Text";
                cboEmployee.ValueMember   = "Id";
            }
            catch (Exception ex)
            {
                ShowStatus("Loi nap danh sach nhan vien: " + ex.Message, false);
            }
        }

        // ══════════════════════════════════════════════════════════════
        // LOAD PAYROLL
        // ══════════════════════════════════════════════════════════════
        private void LoadPayroll()
        {
            try
            {
                string kw     = string.IsNullOrWhiteSpace(txtSearch.Text) ? null : txtSearch.Text.Trim();
                string period = BuildPayPeriod();
                string status = cboFilterStatus.SelectedItem != null
                    ? cboFilterStatus.SelectedItem.ToString()
                    : null;

                var result = _svc.Search(kw, period, status);
                if (!result.Success)
                {
                    ShowStatus(result.ErrorMessage, false);
                    return;
                }

                _currentList = result.Data;
                BindGrid(_currentList);
                ClearForm();
            }
            catch (Exception ex)
            {
                ShowStatus("Loi: " + ex.Message, false);
            }
        }

        private string BuildPayPeriod()
        {
            string thang = cboThang.SelectedItem != null ? cboThang.SelectedItem.ToString() : "";
            string nam   = cboNam.SelectedItem   != null ? cboNam.SelectedItem.ToString()   : "";
            if (thang.StartsWith("--") || nam.StartsWith("--")) return null;
            return nam + "-" + thang;
        }

        // ══════════════════════════════════════════════════════════════
        // BIND GRID
        // ══════════════════════════════════════════════════════════════
        private void BindGrid(List<PayrollDTO> list)
        {
            var table = new System.Data.DataTable();
            table.Columns.Add("payroll_id",  typeof(int));
            table.Columns.Add("Nhan vien",   typeof(string));
            table.Columns.Add("Phong ban",   typeof(string));
            table.Columns.Add("Ky luong",    typeof(string));
            table.Columns.Add("Luong CB",    typeof(string));
            table.Columns.Add("Thuong",      typeof(string));
            table.Columns.Add("Phu cap",     typeof(string));
            table.Columns.Add("Khau tru",    typeof(string));
            table.Columns.Add("Thuc linh",   typeof(string));
            table.Columns.Add("Trang thai",  typeof(string));
            table.Columns.Add("Nguoi duyet", typeof(string));

            foreach (var p in list)
            {
                table.Rows.Add(
                    p.PayrollId,
                    p.EmployeeName,
                    p.DeptName,
                    p.PayPeriod,
                    p.BaseSalary.ToString("N0"),
                    p.Bonus.ToString("N0"),
                    p.Allowances.ToString("N0"),
                    p.Deductions.ToString("N0"),
                    p.NetPay.ToString("N0"),
                    p.Status,
                    p.ApprovedBy
                );
            }

            dgvPayroll.DataSource = table;

            if (dgvPayroll.Columns.Contains("payroll_id"))
                dgvPayroll.Columns["payroll_id"].Visible = false;

            foreach (string col in new[] { "Luong CB", "Thuong", "Phu cap", "Khau tru", "Thuc linh" })
            {
                if (dgvPayroll.Columns.Contains(col))
                    dgvPayroll.Columns[col].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleRight;
            }

            dgvPayroll.CellFormatting -= DgvPayroll_CellFormatting;
            dgvPayroll.CellFormatting += DgvPayroll_CellFormatting;

            lblCount.Text = "Hien thi " + list.Count + " phieu luong";
        }

        private void DgvPayroll_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvPayroll.Columns[e.ColumnIndex].Name == "Trang thai" && e.Value != null)
            {
                switch (e.Value.ToString())
                {
                    case "PENDING":
                        e.CellStyle.ForeColor = Color.FromArgb(230, 81, 0);
                        e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                        break;
                    case "APPROVED":
                        e.CellStyle.ForeColor = Color.FromArgb(21, 101, 192);
                        e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                        break;
                    case "PAID":
                        e.CellStyle.ForeColor = Color.FromArgb(46, 125, 50);
                        e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                        break;
                    case "CANCELLED":
                        e.CellStyle.ForeColor = Color.FromArgb(198, 40, 40);
                        e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                        break;
                }
            }
        }

        // ══════════════════════════════════════════════════════════════
        // GRID CLICK
        // ══════════════════════════════════════════════════════════════
        private void dgvPayroll_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvPayroll.Rows[e.RowIndex];
            _selectedPayrollId = Convert.ToInt32(row.Cells["payroll_id"].Value);

            PayrollDTO p = null;
            foreach (var item in _currentList)
            {
                if (item.PayrollId == _selectedPayrollId.Value)
                { p = item; break; }
            }
            if (p == null) return;

            if (cboEmployee.DataSource is List<LookupItemDTO>)
            {
                var empList = (List<LookupItemDTO>)cboEmployee.DataSource;
                foreach (var emp in empList)
                {
                    if (emp.Id == p.EmployeeId)
                    { cboEmployee.SelectedItem = emp; break; }
                }
            }

            txtPayPeriod.Text  = p.PayPeriod;
            txtBaseSalary.Text = p.BaseSalary.ToString("N0");
            txtBonus.Text      = p.Bonus.ToString("N0");
            txtAllowances.Text = p.Allowances.ToString("N0");
            txtDeductions.Text = p.Deductions.ToString("N0");
            txtNetPay.Text     = p.NetPay.ToString("N0");
            txtApprovedBy.Text = p.ApprovedBy ?? "";
            dtpPayDate.Value   = p.PayDate.HasValue ? p.PayDate.Value : DateTime.Today;

            if (cboPayStatus.Items.Contains(p.Status))
                cboPayStatus.SelectedItem = p.Status;

            lblFormTitle.Text = "SUA PHIEU LUONG  [ID: " + p.PayrollId + "]";
            ShowStatus("Da chon: " + p.EmployeeName + " - ky " + p.PayPeriod, true);
        }

        // ══════════════════════════════════════════════════════════════
        // SEARCH BAR EVENTS
        // ══════════════════════════════════════════════════════════════
        private void btnSearch_Click(object sender, EventArgs e) { LoadPayroll(); }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            cboThang.SelectedIndex        = 0;
            cboNam.SelectedIndex          = 0;
            cboFilterStatus.SelectedIndex = 0;
            LoadPayroll();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) LoadPayroll();
        }

        // ══════════════════════════════════════════════════════════════
        // BUTTONS
        // ══════════════════════════════════════════════════════════════
        private void btnAdd_Click(object sender, EventArgs e)
        {
            PayrollDTO dto;
            if (!BuildAndValidate(out dto)) return;

            var result = _svc.AddPayroll(dto);
            if (result.Success)
            {
                ShowStatus("Da tao phieu luong ID=" + result.NewId + " thanh cong!", true);
                LoadPayroll();
            }
            else
            {
                ShowStatus(result.ErrorMessage, false);
                MessageBox.Show(result.ErrorMessage, "Không thể tạo phiếu lương",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedPayrollId == null)
            { ShowStatus("Chon phieu luong can sua.", false); return; }

            PayrollDTO dto;
            if (!BuildAndValidate(out dto)) return;
            dto.PayrollId = _selectedPayrollId.Value;

            var result = _svc.UpdatePayroll(dto);
            if (result.Success)
            {
                ShowStatus("Yêu cầu thay đổi lương đã được gửi và chờ duyệt!", true);
                LoadPayroll();
            }
            else ShowStatus(result.ErrorMessage, false);
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (_selectedPayrollId == null)
            { ShowStatus("Chon phieu luong can phe duyet.", false); return; }

            PayrollDTO p = null;
            foreach (var item in _currentList)
            {
                if (item.PayrollId == _selectedPayrollId.Value)
                { p = item; break; }
            }
            string nm = p != null ? p.EmployeeName : "ID=" + _selectedPayrollId;

            var confirm = MessageBox.Show(
                "Phe duyet phieu luong cua '" + nm + "'?\n" +
                "Luu y: ban khong the phe duyet phieu do chinh minh tao (SoD).",
                "Xac nhan phe duyet",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            var result = _svc.ApprovePayroll(_selectedPayrollId.Value);
            if (result.Success)
            {
                ShowStatus("Da phe duyet phieu luong cua " + nm + "!", true);
                LoadPayroll();
            }
            else ShowStatus(result.ErrorMessage, false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_selectedPayrollId == null)
            { ShowStatus("Chon phieu luong can huy.", false); return; }

            PayrollDTO p = null;
            foreach (var item in _currentList)
            {
                if (item.PayrollId == _selectedPayrollId.Value)
                { p = item; break; }
            }
            string nm = p != null ? p.EmployeeName : "ID=" + _selectedPayrollId;

            var confirm = MessageBox.Show(
                "Xac nhan HUY phieu luong cua '" + nm + "'?",
                "Xac nhan huy",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            var result = _svc.CancelPayroll(_selectedPayrollId.Value);
            if (result.Success)
            {
                ShowStatus("Da huy phieu luong cua " + nm + ".", true);
                LoadPayroll();
            }
            else ShowStatus(result.ErrorMessage, false);
        }

        private void btnClear_Click(object sender, EventArgs e) { ClearForm(); }

        // ══════════════════════════════════════════════════════════════
        // HELPERS
        // ══════════════════════════════════════════════════════════════
        private bool BuildAndValidate(out PayrollDTO dto)
        {
            dto = null;

            int empId = -1;
            if (cboEmployee.SelectedValue is int)
                empId = (int)cboEmployee.SelectedValue;

            if (empId <= 0)
            { ShowStatus("Vui long chon nhan vien.", false); return false; }

            string rawBase = txtBaseSalary.Text.Replace(",", "").Replace(".", "");
            decimal baseSal;
            if (!decimal.TryParse(rawBase, out baseSal) || baseSal <= 0)
            { ShowStatus("Luong co ban phai la so nguyen duong.", false); return false; }

            decimal bonus, allow, deduct;
            decimal.TryParse(txtBonus.Text.Replace(",", ""),      out bonus);
            decimal.TryParse(txtAllowances.Text.Replace(",", ""), out allow);
            decimal.TryParse(txtDeductions.Text.Replace(",", ""), out deduct);

            dto = new PayrollDTO
            {
                EmployeeId   = empId,
                PayPeriod    = txtPayPeriod.Text.Trim(),
                BaseSalary   = baseSal,
                Bonus        = bonus,
                Allowances   = allow,
                Deductions   = deduct,
                PayDate      = dtpPayDate.Value.Date,
                Status       = cboPayStatus.SelectedItem != null
                    ? cboPayStatus.SelectedItem.ToString()
                    : "PENDING"
            };
            return true;
        }

        private void RecalcNetPay()
        {
            decimal b, bo, a, d;
            decimal.TryParse(txtBaseSalary.Text.Replace(",", ""), out b);
            decimal.TryParse(txtBonus.Text.Replace(",", ""),      out bo);
            decimal.TryParse(txtAllowances.Text.Replace(",", ""), out a);
            decimal.TryParse(txtDeductions.Text.Replace(",", ""), out d);
            txtNetPay.Text = (b + bo + a - d).ToString("N0");
        }

        private void ClearForm()
        {
            _selectedPayrollId = null;
            if (cboEmployee.Items.Count > 0)   cboEmployee.SelectedIndex = 0;
            if (cboPayStatus.Items.Count > 0)  cboPayStatus.SelectedIndex = 0;
            txtPayPeriod.Text  = "";
            txtBaseSalary.Text = "";
            txtBonus.Text      = "0";
            txtAllowances.Text = "0";
            txtDeductions.Text = "0";
            txtNetPay.Text     = "0";
            txtApprovedBy.Text = "";
            dtpPayDate.Value   = DateTime.Today;
            lblFormTitle.Text  = "THEM / SUA PHIEU LUONG";
            ShowStatus("", true);
        }

        private void ShowStatus(string msg, bool success)
        {
            lblStatus.Text      = msg ?? "";
            lblStatus.ForeColor = success
                ? Color.FromArgb(100, 255, 150)
                : Color.FromArgb(255, 100, 100);
        }
    }
}
