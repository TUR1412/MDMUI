using System;

namespace MDMUI.Model
{
    public class UserSecurityStatus
    {
        public int UserId { get; set; }
        public int FailedCount { get; set; }
        public DateTime? LastFailedAt { get; set; }
        public DateTime? LockoutUntil { get; set; }
        public DateTime? LastSuccessAt { get; set; }
    }
}
