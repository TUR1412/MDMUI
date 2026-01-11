using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MDMUI.Controls.Atoms;
using MDMUI.Controls.Molecules;
using MDMUI.Utility;
using User = MDMUI.Model.User;

namespace MDMUI
{
    public sealed class FrmFileLogViewer : Form
    {
        private readonly User currentUser;

        private ListView fileList;
        private RichTextBox logText;
        private TextBox filterBox;
        private Label statusLabel;
        private Label emptyStateLabel;
        private AppButton btnRefresh;
        private AppButton btnOpenFolder;
        private AppButton btnOpenExternal;
        private AppButton btnCopy;

        private List<string> loadedLines = new List<string>();
        private string loadedFilePath;

        public FrmFileLogViewer(User user)
        {
            currentUser = user;

            Text = "日志查看器";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1100, 720);

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

            Load += (_, __) =>
            {
                try { RefreshFiles(); } catch { }
                try { ThemeManager.ApplyTo(this); } catch { }
                try { ModernTheme.EnableMicroInteractions(this); } catch { }
            };
        }

        private void BuildUi()
        {
            ThemePalette palette = ThemeManager.Palette;

            Panel root = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                BackColor = palette.Background
            };

            Label title = new Label
            {
                Dock = DockStyle.Top,
                AutoSize = false,
                Height = 38,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "诊断日志查看器",
                ForeColor = palette.TextPrimary
            };

            try { title.Font = ThemeManager.CreateTitleFont(11F, FontStyle.Bold); } catch { }

            CardPanel card = new CardPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                BackColor = palette.Surface
            };

            statusLabel = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 24,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = palette.TextSecondary
            };

            ActionToolbar toolbar = BuildToolbar();

            SplitContainer split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 330,
                BackColor = palette.Surface
            };

            fileList = BuildFileList();
            split.Panel1.Padding = new Padding(0, 6, 10, 0);
            split.Panel1.Controls.Add(fileList);

            Panel logHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = palette.Surface
            };

            logText = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                DetectUrls = true,
                BorderStyle = BorderStyle.None,
                BackColor = palette.Surface,
                ForeColor = palette.TextPrimary,
                HideSelection = false,
                WordWrap = false
            };

            try
            {
                logText.Font = new Font("Consolas", 9F);
            }
            catch
            {
                // ignore
            }

            emptyStateLabel = new Label
            {
                Dock = DockStyle.Fill,
                Text = "请选择左侧日志文件",
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = palette.TextSecondary,
                BackColor = palette.Surface,
                Visible = true
            };

            logHost.Controls.Add(logText);
            logHost.Controls.Add(emptyStateLabel);
            emptyStateLabel.BringToFront();

            split.Panel2.Padding = new Padding(10, 6, 0, 0);
            split.Panel2.Controls.Add(logHost);

            card.Controls.Add(split);
            card.Controls.Add(statusLabel);
            card.Controls.Add(toolbar);

            root.Controls.Add(card);
            root.Controls.Add(title);
            Controls.Add(root);

            UpdateStatus("就绪");
        }

        private ActionToolbar BuildToolbar()
        {
            ThemePalette palette = ThemeManager.Palette;

            ActionToolbar toolbar = new ActionToolbar
            {
                Dock = DockStyle.Top,
                Height = 46,
                Padding = new Padding(0, 6, 0, 10)
            };

            Label filterLabel = new Label
            {
                AutoSize = true,
                Text = "过滤：",
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = palette.TextSecondary,
                Padding = new Padding(0, 6, 0, 0),
                Margin = new Padding(0)
            };

            filterBox = new TextBox
            {
                Width = 260,
                Height = 28,
                Margin = new Padding(8, 0, 0, 0)
            };
            filterBox.TextChanged += (s, e) => RenderCurrentLines();
            filterBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    filterBox.Text = string.Empty;
                    e.Handled = true;
                }
            };

            btnRefresh = new AppButton
            {
                Text = "刷新",
                Variant = AppButtonVariant.Secondary,
                MinimumSize = new Size(90, 32)
            };
            btnRefresh.Click += (s, e) => RefreshFiles();

            btnOpenFolder = new AppButton
            {
                Text = "打开目录",
                Variant = AppButtonVariant.Secondary,
                MinimumSize = new Size(110, 32)
            };
            btnOpenFolder.Click += (s, e) => OpenLogFolder();

            btnOpenExternal = new AppButton
            {
                Text = "外部打开",
                Variant = AppButtonVariant.Secondary,
                MinimumSize = new Size(110, 32)
            };
            btnOpenExternal.Click += (s, e) => OpenSelectedFileExternally();

            btnCopy = new AppButton
            {
                Text = "复制",
                Variant = AppButtonVariant.Primary,
                MinimumSize = new Size(90, 32)
            };
            btnCopy.Click += (s, e) => CopyVisibleText();

            toolbar.LeftPanel.Controls.Add(filterLabel);
            toolbar.LeftPanel.Controls.Add(filterBox);
            toolbar.RightPanel.Controls.Add(btnRefresh);
            toolbar.RightPanel.Controls.Add(btnOpenFolder);
            toolbar.RightPanel.Controls.Add(btnOpenExternal);
            toolbar.RightPanel.Controls.Add(btnCopy);

            return toolbar;
        }

        private ListView BuildFileList()
        {
            ThemePalette palette = ThemeManager.Palette;

            ListView list = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                HideSelection = false,
                MultiSelect = false,
                BorderStyle = BorderStyle.None,
                BackColor = palette.Surface,
                ForeColor = palette.TextPrimary
            };

            list.Columns.Add("文件", 170);
            list.Columns.Add("大小", 80);
            list.Columns.Add("时间", 140);

            list.SelectedIndexChanged += async (s, e) =>
            {
                if (list.SelectedItems.Count != 1) return;
                string path = list.SelectedItems[0].Tag as string;
                if (string.IsNullOrWhiteSpace(path)) return;
                await LoadFileAsync(path);
            };

            return list;
        }

        private void RefreshFiles()
        {
            try
            {
                using (AppTelemetry.Measure("FileLogViewer.RefreshFiles"))
                {
                    string dir = LogFileService.GetEffectiveLogDirectory();
                    IReadOnlyList<FileInfo> files = LogFileService.GetLogFiles(dir);

                    string keepSelected = loadedFilePath;

                    fileList.BeginUpdate();
                    try
                    {
                        fileList.Items.Clear();
                        foreach (FileInfo file in files)
                        {
                            ListViewItem item = new ListViewItem(file.Name);
                            item.SubItems.Add(LogFileService.FormatFileSize(file.Length));
                            item.SubItems.Add(file.LastWriteTime.ToString("yyyy-MM-dd HH:mm"));
                            item.Tag = file.FullName;
                            fileList.Items.Add(item);

                            if (!string.IsNullOrWhiteSpace(keepSelected) &&
                                string.Equals(keepSelected, file.FullName, StringComparison.OrdinalIgnoreCase))
                            {
                                item.Selected = true;
                            }
                        }
                    }
                    finally
                    {
                        fileList.EndUpdate();
                    }

                    if (files.Count == 0)
                    {
                        UpdateStatus("未找到日志文件（目录不存在或为空）");
                    }
                    else if (string.IsNullOrWhiteSpace(keepSelected))
                    {
                        UpdateStatus($"发现 {files.Count} 个日志文件");
                    }
                }
            }
            catch (Exception ex)
            {
                AppLog.Error(ex, "刷新日志文件列表失败");
                UpdateStatus("刷新失败: " + ex.Message);
            }
        }

        private async Task LoadFileAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) return;

            try
            {
                string fileName = Path.GetFileName(filePath);
                UpdateStatus($"正在加载: {fileName}");

                IReadOnlyList<string> lines = await Task.Run(() =>
                {
                    using (AppTelemetry.Measure("FileLogViewer.ReadTail"))
                    {
                        return LogFileService.ReadTailLines(filePath, maxLines: 2500, maxChars: 280_000);
                    }
                });

                loadedFilePath = filePath;
                loadedLines = lines.ToList();

                SetEmptyStateVisible(false);
                RenderCurrentLines();
            }
            catch (Exception ex)
            {
                AppLog.Error(ex, "加载日志文件失败");
                loadedFilePath = filePath;
                loadedLines = new List<string>();
                logText.Text = string.Empty;
                SetEmptyStateVisible(true);
                UpdateStatus("加载失败: " + ex.Message);
            }
        }

        private void RenderCurrentLines()
        {
            string filter = filterBox?.Text?.Trim();
            IEnumerable<string> view = loadedLines ?? Enumerable.Empty<string>();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                view = view.Where(l => l != null && l.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            List<string> finalLines = view.ToList();
            StringBuilder sb = new StringBuilder();
            foreach (string line in finalLines)
            {
                sb.AppendLine(line);
            }

            logText.Text = sb.ToString();

            string name = string.IsNullOrWhiteSpace(loadedFilePath) ? "-" : Path.GetFileName(loadedFilePath);
            string status = $"文件: {name} · 行数: {finalLines.Count}";
            if (!string.IsNullOrWhiteSpace(filter)) status += $" · 过滤: {filter}";
            UpdateStatus(status);
        }

        private void OpenLogFolder()
        {
            try
            {
                string dir = LogFileService.GetEffectiveLogDirectory();
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                System.Diagnostics.Process.Start(dir);
            }
            catch (Exception ex)
            {
                AppLog.Error(ex, "打开日志目录失败");
                MessageBox.Show("打开日志目录失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenSelectedFileExternally()
        {
            try
            {
                string path = GetSelectedFilePath();
                if (string.IsNullOrWhiteSpace(path)) return;
                if (!File.Exists(path)) return;

                System.Diagnostics.Process.Start(path);
            }
            catch (Exception ex)
            {
                AppLog.Error(ex, "外部打开日志文件失败");
                MessageBox.Show("外部打开失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CopyVisibleText()
        {
            try
            {
                string text = logText?.SelectedText;
                if (string.IsNullOrWhiteSpace(text))
                {
                    text = logText?.Text;
                }

                if (string.IsNullOrWhiteSpace(text)) return;
                Clipboard.SetText(text);
                UpdateStatus("已复制到剪贴板");
            }
            catch (Exception ex)
            {
                AppLog.Error(ex, "复制日志内容失败");
            }
        }

        private string GetSelectedFilePath()
        {
            try
            {
                if (fileList == null) return null;
                if (fileList.SelectedItems.Count != 1) return null;
                return fileList.SelectedItems[0].Tag as string;
            }
            catch
            {
                return null;
            }
        }

        private void SetEmptyStateVisible(bool visible)
        {
            if (emptyStateLabel == null) return;
            emptyStateLabel.Visible = visible;
        }

        private void UpdateStatus(string message)
        {
            if (statusLabel == null) return;
            statusLabel.Text = message ?? string.Empty;
        }
    }
}
