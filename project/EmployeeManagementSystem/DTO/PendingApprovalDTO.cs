using System;

namespace DTO
{
    public class PendingApprovalDTO
    {
        public int ApprovalId { get; set; }
        public string ActionType { get; set; }
        public string TargetTable { get; set; }
        public int? TargetId { get; set; }
        public string RequestedBy { get; set; }
        public DateTime RequestTime { get; set; }
        public string Payload { get; set; }
        
        public string ApprovedBy1 { get; set; }
        public DateTime? ApprovalTime1 { get; set; }
        public string ApprovedBy2 { get; set; }
        public DateTime? ApprovalTime2 { get; set; }
        
        public string Status { get; set; }
        public DateTime? FinalActionTime { get; set; }
        public string Notes { get; set; }
    }
}
