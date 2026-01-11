using MDMUI.Utility;

namespace MDMUI.Tests;

[TestClass]
public sealed class LogFileServiceTests
{
    [TestMethod]
    public void ReadTailLines_ReturnsLastLines()
    {
        string tempDir = Path.Combine(Path.GetTempPath(), "MDMUI-LogFileServiceTests-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        string filePath = Path.Combine(tempDir, "mdmui-20260111.log");
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, append: false, encoding: new System.Text.UTF8Encoding(false)))
            {
                for (int i = 1; i <= 100; i++)
                {
                    writer.WriteLine("line-" + i);
                }
            }

            IReadOnlyList<string> tail = LogFileService.ReadTailLines(filePath, maxLines: 10, maxChars: 200_000);
            Assert.AreEqual(10, tail.Count);
            Assert.AreEqual("line-91", tail[0]);
            Assert.AreEqual("line-100", tail[9]);
        }
        finally
        {
            try { Directory.Delete(tempDir, recursive: true); } catch { }
        }
    }

    [TestMethod]
    public void GetLogFiles_OrdersByLastWriteTimeDesc()
    {
        string tempDir = Path.Combine(Path.GetTempPath(), "MDMUI-LogFileServiceTests-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        try
        {
            string oldFile = Path.Combine(tempDir, "mdmui-20260101.log");
            string newFile = Path.Combine(tempDir, "mdmui-20260102.log");

            File.WriteAllText(oldFile, "old");
            File.WriteAllText(newFile, "new");

            File.SetLastWriteTimeUtc(oldFile, new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            File.SetLastWriteTimeUtc(newFile, new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc));

            IReadOnlyList<FileInfo> files = LogFileService.GetLogFiles(tempDir);
            Assert.AreEqual(2, files.Count);
            Assert.AreEqual("mdmui-20260102.log", files[0].Name);
            Assert.AreEqual("mdmui-20260101.log", files[1].Name);
        }
        finally
        {
            try { Directory.Delete(tempDir, recursive: true); } catch { }
        }
    }
}

