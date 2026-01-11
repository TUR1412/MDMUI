using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MDMUI.Controls.Atoms;
using MDMUI.Utility;

namespace MDMUI
{
    public sealed class CrashReportForm : Form
    {
        private readonly string incidentId;
        private readonly Exception exception;
        private readonly string source;
        private readonly string logPath;
        private readonly bool terminating;

        public CrashReportForm(string incidentId, Exception exception, string source, string logPath, bool terminating)
        {
            this.incidentId = incidentId ?? "UNKNOWN";
            this.exception = exception ?? new Exception("Unknown exception");
            this.source = source ?? "Unknown";
            this.logPath = logPath;
            this.terminating = terminating;

            Text = terminating ? "应用程序错误（即将退出）" : "应用程序错误";
            MinimizeBox = false;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ClientSize = new Size(880, 580);

            try
            {
                Font = ThemeManager.CreateBodyFont(9F);
                BackColor = ThemeManager.Palette.Background;
                ForeColor = ThemeManager.Palette.TextPrimary;
            }
            catch
            {
                Font = new Font("Segoe UI", 9F);
                BackColor = Color.White;
                ForeColor = Color.Black;
            }

            BuildUi();

            // 尽量应用主题与微交互（不影响异常展示的可用性）
            try { ThemeManager.ApplyTo(this); } catch { }
            try { ModernTheme.EnableMicroInteractions(this); } catch { }
        }

        private void BuildUi()
        {
            ThemePalette palette = ThemeManager.Palette;

            TableLayoutPanel root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(16),
                BackColor = palette.Background
            };

            Label title = new Label
            {
                AutoSize = true,
                Text = terminating ? "发生严重错误，应用将退出。" : "发生错误，你可以选择继续或退出。",
                Font = ThemeManager.CreateTitleFont(11F, FontStyle.Bold),
                ForeColor = palette.TextPrimary
            };

            Label meta = new Label
            {
                AutoSize = true,
                ForeColor = palette.TextSecondary,
                Text = $"事件ID: {incidentId}    来源: {source}    时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}"
            };

            TextBox details = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                WordWrap = false,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9F),
                BackColor = palette.SurfaceAlt,
                ForeColor = palette.TextPrimary
            };
            details.Text = BuildDetailsText();

            CardPanel card = new CardPanel
            {
                Dock = DockStyle.Fill,
                CornerRadius = 14,
                BackColor = palette.Surface,
                Padding = new Padding(12)
            };

            TableLayoutPanel cardLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.Transparent
            };
            cardLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            cardLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            cardLayout.Controls.Add(meta, 0, 0);
            cardLayout.Controls.Add(details, 0, 1);
            card.Controls.Add(cardLayout);

            FlowLayoutPanel buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Padding = new Padding(0),
                AutoSize = true
            };

            AppButton btnClose = new AppButton
            {
                Text = terminating ? "退出" : "关闭",
                Variant = terminating ? AppButtonVariant.Danger : AppButtonVariant.Primary
            };
            btnClose.Click += (s, e) =>
            {
                try
                {
                    Close();
                }
                finally
                {
                    if (terminating)
                    {
                        try { Application.Exit(); } catch { }
                    }
                }
            };

            AppButton btnCopy = new AppButton
            {
                Text = "复制详情",
                Variant = AppButtonVariant.Secondary
            };
            btnCopy.Click += (s, e) =>
            {
                try
                {
                    Clipboard.SetText(details.Text);
                }
                catch
                {
                    MessageBox.Show("复制失败。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            AppButton btnOpenLogs = new AppButton
            {
                Text = "打开日志目录",
                Variant = AppButtonVariant.Secondary,
                Enabled = !string.IsNullOrWhiteSpace(logPath)
            };
            btnOpenLogs.Click += (s, e) => OpenLogLocation();

            AppButton btnContinue = new AppButton
            {
                Text = "继续运行",
                Variant = AppButtonVariant.Secondary,
                Visible = !terminating
            };
            btnContinue.Click += (s, e) => Close();

            buttons.Controls.Add(btnClose);
            buttons.Controls.Add(btnContinue);
            buttons.Controls.Add(btnCopy);
            buttons.Controls.Add(btnOpenLogs);

            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.Controls.Add(title, 0, 0);
            root.Controls.Add(card, 0, 1);
            root.Controls.Add(buttons, 0, 2);

            Controls.Add(root);
        }

        private string BuildDetailsText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== MDMUI Crash Report ===");
            sb.AppendLine($"Incident: {incidentId}");
            sb.AppendLine($"Source:   {source}");
            sb.AppendLine($"Time:     {DateTime.Now:O}");
            sb.AppendLine($"OS:       {Environment.OSVersion}");
            sb.AppendLine($"64-bit:   {Environment.Is64BitProcess}");
            sb.AppendLine($"CLR:      {Environment.Version}");
            sb.AppendLine();
            sb.AppendLine(exception.ToString());
            sb.AppendLine();

            if (!string.IsNullOrWhiteSpace(logPath))
            {
                sb.AppendLine($"LogFile: {logPath}");
            }

            return sb.ToString();
        }

        private void OpenLogLocation()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(logPath))
                {
                    return;
                }

                if (File.Exists(logPath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "explorer.exe",
                        Arguments = "/select,\"" + logPath + "\"",
                        UseShellExecute = true
                    });
                    return;
                }

                string dir = Path.GetDirectoryName(logPath);
                if (!string.IsNullOrWhiteSpace(dir) && Directory.Exists(dir))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "explorer.exe",
                        Arguments = "\"" + dir + "\"",
                        UseShellExecute = true
                    });
                }
            }
            catch
            {
                // ignore
            }
        }
    }
}

