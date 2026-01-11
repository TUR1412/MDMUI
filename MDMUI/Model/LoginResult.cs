using System;

namespace MDMUI.Model
{
    public sealed class LoginResult
    {
        public bool Success { get; set; }
        public User User { get; set; }
        public string FailureReason { get; set; }
        public int? RemainingAttempts { get; set; }
        public DateTime? LockoutUntil { get; set; }
    }
}
