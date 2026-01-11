using System;

namespace MDMUI.Model
{
    public sealed class BackupResult
    {
        public bool Success { get; set; }
        public string FilePath { get; set; }
        public string Message { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
