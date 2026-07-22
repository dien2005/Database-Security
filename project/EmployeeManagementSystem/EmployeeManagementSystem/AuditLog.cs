using System;
using System.Data;
using System.Windows.Forms;
using BLL;

namespace EmployeeManagementSystem
{
    public partial class AuditLog : UserControl
    {
        private readonly AuditLogService _auditLogService = new AuditLogService();

        public AuditLog()
        {
            InitializeComponent();
            SetupComboBox();
            AttachEvents();
        }

        private void SetupComboBox()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("All");
            comboBox1.Items.Add("SELECT");
            comboBox1.Items.Add("INSERT");
            comboBox1.Items.Add("UPDATE");
            comboBox1.Items.Add("DELETE");
            comboBox1.SelectedIndex = 0;
        }

        private void AttachEvents()
        {
            this.Load         += AuditLog_Load;
            searchbtn.Click   += (s, e) => LoadData();
            button1.Click     += (s, e) => LoadData();   // FILTER
            button2.Click     += (s, e) => LoadData();   // APPLY DATE
        }

        private void AuditLog_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            string keyword = searchtxt.Text.Trim();
            string action  = comboBox1.SelectedItem?.ToString() == "All"
                             ? ""
                             : comboBox1.SelectedItem?.ToString() ?? "";

            DateTime? startDate = dateTimePicker1.Checked ? dateTimePicker1.Value.Date : (DateTime?)null;
            DateTime? endDate   = dateTimePicker2.Checked ? dateTimePicker2.Value.Date : (DateTime?)null;

            var result = _auditLogService.Search(keyword, action, startDate, endDate);

            if (result.Success)
            {
                dataGridView1.DataSource = result.Data;
                SetColumnHeaders();
                dataGridView1.ClearSelection();
            }
            else
            {
                // ORA-00942 = không có quyền xem bảng → thông báo thân thiện
                if (result.ErrorMessage != null && result.ErrorMessage.Contains("ORA-00942"))
                {
                    dataGridView1.DataSource = null;
                    MessageBox.Show(
                        "Bạn không có quyền xem Audit Log.\nChức năng này chỉ dành cho HR Manager và Ban Giám Đốc.",
                        "Không đủ quyền",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(result.ErrorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void SetColumnHeaders()
        {
            if (dataGridView1.Columns.Count == 0) return;

            var headers = new System.Collections.Generic.Dictionary<string, string>
            {
                { "AUDIT_ID",      "ID" },
                { "EVENT_TIME",    "Thời gian" },
                { "DB_USER",       "Người dùng" },
                { "OBJECT_SCHEMA", "Schema" },
                { "OBJECT_NAME",   "Đối tượng" },
                { "POLICY_NAME",   "Policy" },
                { "ACTION_NAME",   "Hành động" },
                { "SQL_TEXT",      "Câu SQL" },
                { "SYNC_TIME",     "Đồng bộ lúc" },
            };

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (headers.TryGetValue(col.Name.ToUpper(), out string label))
                    col.HeaderText = label;
            }
        }
    }
}
