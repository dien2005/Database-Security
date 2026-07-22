using System;
using System.Windows.Forms;
using BLL;

namespace EmployeeManagementSystem
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Bảo vệ: không cho vào MainForm nếu chưa login hợp lệ
            if (!SessionManager.IsLoggedIn || SessionManager.CurrentUser == null
                || !SessionManager.CurrentUser.IsValidEmployee)
            {
                MessageBox.Show("Phiên đăng nhập không hợp lệ.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Close();
                return;
            }

            var user = SessionManager.CurrentUser;
            this.Text = $"Employee Management System - {user.FullName} ({user.Username})";

            // Gate sidebar theo role
            addEmployee_btn.Enabled = true;

            salary_btn.Enabled = user.IsHrManager || user.IsFinStaff || user.IsDeptManager || user.IsHrStaff;
            // Chỉ trưởng phòng thường và trưởng phòng HR mới xem được
            department_btn.Enabled = user.IsDeptManager || user.IsHrManager;
            approvals_btn.Enabled = user.IsHrManager || user.IsDeptManager;
            auditLog_btn.Enabled = user.IsHrManager;
            ShowOnly(dashboard1);
            // dashboard_btn và myProfile_btn: ai cũng thấy, không cần gate

            dashboard1.RefreshData();
        }

        private void ShowOnly(Control controlToShow)
        {
            dashboard1.Visible = false;
            addEmployee1.Visible = false;
            salary1.Visible = false;
            department1.Visible = false;
            pendingApprovals1.Visible = false;
            auditLog1.Visible = false;
            myProfile1.Visible = false;

            controlToShow.Visible = true;
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void logout_btn_Click(object sender, EventArgs e)
        {
            DialogResult check =
                MessageBox.Show("Are you sure you want to logout?",
                "Confirmation Message",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (check == DialogResult.Yes)
            {
                // Đóng connection Oracle đang giữ context RBAC/VPD/OLS của user hiện tại
                if (SessionManager.Connection != null)
                {
                    if (SessionManager.Connection.State == System.Data.ConnectionState.Open)
                        SessionManager.Connection.Close();
                    SessionManager.Connection.Dispose();
                    SessionManager.Connection = null;
                }
                SessionManager.CurrentUser = null;

                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Close();   // Close chứ không Hide, để giải phóng hẳn MainForm cũ
            }
        }

        private void dashboard_btn_Click(object sender, EventArgs e)
        {
            ShowOnly(dashboard1);
            dashboard1.RefreshData();
        }
        private void addEmloyee_btn_Click(object sender, EventArgs e) => ShowOnly(addEmployee1);
        private void salary_btn_Click(object sender, EventArgs e) => ShowOnly(salary1);
        private void department_btn_Click(object sender, EventArgs e) => ShowOnly(department1);
        private void approvals_btn_Click(object sender, EventArgs e) => ShowOnly(pendingApprovals1);
        private void auditLog_btn_Click(object sender, EventArgs e) => ShowOnly(auditLog1);
        private void myProfile_btn_Click(object sender, EventArgs e) => ShowOnly(myProfile1);
    }
}