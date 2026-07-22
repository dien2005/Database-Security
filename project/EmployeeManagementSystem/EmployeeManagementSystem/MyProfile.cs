using System;
using System.Windows.Forms;
using BLL;
using DTO;

namespace EmployeeManagementSystem
{
    public partial class MyProfile : UserControl
    {
        private readonly EmployeeService _empService = new EmployeeService();
        private int _currentEmployeeId;

        public MyProfile()
        {
            InitializeComponent();
            this.Load += MyProfile_Load;
            btnSave.Click += BtnSave_Click;
        }

        private void MyProfile_Load(object sender, EventArgs e)
        {
            if (SessionManager.CurrentUser == null || SessionManager.CurrentUser.EmployeeId == null) return;
            _currentEmployeeId = SessionManager.CurrentUser.EmployeeId.Value;
            LoadProfile();
        }

        private void LoadProfile()
        {
            var result = _empService.GetProfile(_currentEmployeeId);
            if (result.Success && result.Data != null)
            {
                var emp = result.Data;
                lblFullName.Text = emp.FullName;
                lblDepartment.Text = !string.IsNullOrEmpty(emp.DeptName) ? emp.DeptName : emp.DepartmentId.ToString();
                lblJobTitle.Text = !string.IsNullOrEmpty(emp.JobTitle) ? emp.JobTitle : emp.JobId.ToString();
                
                txtEmail.Text = emp.Email;
                txtPhone.Text = emp.Phone;
            }
            else
            {
                MessageBox.Show(result.ErrorMessage ?? "Lỗi tải thông tin.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Email không được để trống.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = _empService.UpdateProfile(_currentEmployeeId, email, phone);
            if (result.Success)
            {
                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(result.ErrorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
