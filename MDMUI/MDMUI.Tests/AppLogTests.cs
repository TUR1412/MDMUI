using System.Globalization;
using MDMUI.Utility;

namespace MDMUI.Tests;

[TestClass]
[DoNotParallelize]
public sealed class AppLogTests
{
    [TestMethod]
    public void Initialize_AndWrite_CreatesLogFile()
    {
        string? originalDir = Environment.GetEnvironmentVariable("MDMUI_LOG_DIR");
        string? originalDisabled = Environment.GetEnvironmentVariable("MDMUI_LOG_DISABLED");

        string tempDir = Path.Combine(Path.GetTempPath(), "MDMUI-AppLogTests-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            Environment.SetEnvironmentVariable("MDMUI_LOG_DIR", tempDir);
            Environment.SetEnvironmentVariable("MDMUI_LOG_DISABLED", "0");

            AppLog.Initialize();

            string marker = "test-log-" + Guid.NewGuid().ToString("N");
            AppLog.Info(marker);
            AppLog.Flush();

            string expectedPath = Path.Combine(tempDir, "mdmui-" + DateTime.Today.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + ".log");
            Assert.IsTrue(File.Exists(expectedPath), "Expected log file was not created.");

            string text;
            using (FileStream stream = new FileStream(expectedPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
            Assert.IsTrue(text.Contains(marker), "Expected log content marker was not found.");
        }
        finally
        {
            try { AppLog.Shutdown(); } catch { }

            Environment.SetEnvironmentVariable("MDMUI_LOG_DIR", originalDir);
            Environment.SetEnvironmentVariable("MDMUI_LOG_DISABLED", originalDisabled);

            try { Directory.Delete(tempDir, recursive: true); } catch { }
        }
    }
}
