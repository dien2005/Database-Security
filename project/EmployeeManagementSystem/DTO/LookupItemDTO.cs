namespace DTO
{
    /// <summary>
    /// Cặp Id/Name dùng chung cho mọi ComboBox tra cứu
    /// (cboJob, cboDepartment, cboLocation, cboManager).
    /// Set DisplayMember="Text", ValueMember="Id" khi bind.
    /// </summary>
    public class LookupItemDTO
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;

        public override string ToString() => Text;
    }
}