using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;

namespace EmployeeManagementSystem
{
    public partial class LoginForm : Form
    {
        private readonly AuthService _authService = new AuthService();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void login_signupBtn_Click(object sender, EventArgs e)
        {
            RegisterForm regForm = new RegisterForm();
            regForm.Show();
            this.Hide();
        }

        private void login_showPass_CheckedChanged(object sender, EventArgs e)
        {
            login_password.PasswordChar = login_showPass.Checked ? '\0' : '*';
        }

        private async void login_btn_Click(object sender, EventArgs e)
        {
            string username = login_username.Text.Trim();   // TODO: đổi "login_username" thành đúng tên control thật của bạn
            string password = login_password.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            login_btn.Enabled = false;
            Cursor = Cursors.WaitCursor;

            var result = await Task.Run(() => _authService.Login(username, password));

            Cursor = Cursors.Default;
            login_btn.Enabled = true;

            if (!result.Success)
            {
                MessageBox.Show(result.ErrorMessage, "Đăng nhập thất bại",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                login_password.Clear();
                login_password.Focus();
                return;
            }

            MainForm mainForm = new MainForm();
            //mainForm.FormClosed += (s, args) => Application.Exit();
            mainForm.Show();
            this.Hide();
        }
    }
}