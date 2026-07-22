using System;
using System.Windows.Forms;
using BLL;

namespace EmployeeManagementSystem
{
    public partial class Dashboard : UserControl
    {
        private readonly DashboardService _service = new DashboardService();

        public Dashboard()
        {
            InitializeComponent();
        }

        public void RefreshData()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)RefreshData);
                return;
            }

            try
            {
                var stats = _service.LoadDashboardStats();

                dashboard_TE.Text = stats.TotalEmployees.ToString();
                dashboard_AE.Text = stats.ActiveEmployees.ToString();
                dashboard_IE.Text = stats.InactiveEmployees.ToString();
                dashboard_TM.Text = stats.TerminatedEmployees.ToString();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Phiên đăng nhập",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Error Message",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}