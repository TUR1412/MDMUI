using System;
using System.Diagnostics;

namespace MDMUI.Utility
{
    public static class AppTelemetry
    {
        public static IDisposable Measure(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) name = "Unnamed";
            return new TelemetryScope(name.Trim());
        }

        private sealed class TelemetryScope : IDisposable
        {
            private readonly string name;
            private readonly Stopwatch stopwatch;
            private bool disposed;

            public TelemetryScope(string name)
            {
                this.name = name;
                stopwatch = Stopwatch.StartNew();
            }

            public void Dispose()
            {
                if (disposed) return;
                disposed = true;

                stopwatch.Stop();
                AppLog.Info($"[perf] {name} took {stopwatch.ElapsedMilliseconds}ms");
            }
        }
    }
}

