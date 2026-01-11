using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using MDMUI.BLL;
using MDMUI.Controls.Atoms;
using MDMUI.Controls.Molecules;
using MDMUI.Model;
using MDMUI.Utility;

namespace MDMUI
{
    public partial class FrmDataBackup : Form
    {
        private User CurrentUser;
        private readonly DatabaseBackupService backupService = new DatabaseBackupService();
        private readonly SystemParameterService parameterService = new SystemParameterService();

        private TextBox txtBackupPath;
        private AppButton btnBrowse;
        private AppButton btnBackup;
        private AppButton btnOpen;
        private ListView backupList;
        private Label statusLabel;
        private int retentionDays = 7;

        public FrmDataBackup(User user)
        {
            InitializeComponent();
            CurrentUser = user;
        }

        private void FrmDataBackup_Load(object sender, EventArgs e)
        {
            if (this.userLabel != null)
            {
                this.userLabel.Text = $"当前用户: {CurrentUser?.Username ?? "未知用户"}";
            }

            InitializeBackupPanel();
            LoadDefaults();
            RefreshBackupList();
            ThemeManager.ApplyTo(this);
            ModernTheme.EnableMicroInteractions(this);
        }

        private void InitializeBackupPanel()
        {
            if (demoPanel == null) return;

            titleLabel.Text = "数据备份中心";
            placeholderLabel.Visible = false;
            demoLabel.Visible = false;

            demoPanel.Controls.Clear();
            demoPanel.Dock = DockStyle.Fill;
            demoPanel.Padding = new Padding(16);
            demoPanel.BackColor = ThemeManager.Palette.Background;
            demoPanel.BorderStyle = BorderStyle.None;

            CardPanel contentCard = new CardPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                BackColor = ThemeManager.Palette.Surface
            };

            ActionToolbar toolbar = BuildToolbar();
            backupList = BuildBackupList();

            statusLabel = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 24,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = ThemeManager.Palette.TextSecondary
            };

            contentCard.Controls.Add(backupList);
            contentCard.Controls.Add(statusLabel);
            contentCard.Controls.Add(toolbar);
            demoPanel.Controls.Add(contentCard);
        }

        private ActionToolbar BuildToolbar()
        {
            ActionToolbar toolbar = new ActionToolbar
            {
                Dock = DockStyle.Top,
                Height = 46,
                Padding = new Padding(0, 6, 0, 10)
            };

            Label pathLabel = new Label
            {
                AutoSize = true,
                Text = "目录：",
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = ThemeManager.Palette.TextSecondary,
                Padding = new Padding(0, 6, 0, 0),
                Margin = new Padding(0)
            };

            txtBackupPath = new TextBox
            {
                Width = 420,
                Height = 28,
                Margin = new Padding(8, 0, 0, 0),
                ReadOnly = true
            };

            btnBrowse = new AppButton
            {
                Text = "选择目录",
                Variant = AppButtonVariant.Secondary,
                MinimumSize = new Size(110, 32)
            };
            btnBrowse.Click += (s, e) => SelectBackupPath();

            btnOpen = new AppButton
            {
                Text = "打开目录",
                Variant = AppButtonVariant.Secondary,
                MinimumSize = new Size(110, 32)
            };
            btnOpen.Click += (s, e) => OpenBackupDirectory();

            btnBackup = new AppButton
            {
                Text = "立即备份",
                Variant = AppButtonVariant.Primary,
                MinimumSize = new Size(110, 32)
            };
            btnBackup.Click += async (s, e) => await RunBackupAsync();

            toolbar.LeftPanel.Controls.Add(pathLabel);
            toolbar.LeftPanel.Controls.Add(txtBackupPath);
            toolbar.RightPanel.Controls.Add(btnBrowse);
            toolbar.RightPanel.Controls.Add(btnOpen);
            toolbar.RightPanel.Controls.Add(btnBackup);

            return toolbar;
        }

        private ListView BuildBackupList()
        {
            ThemePalette palette = ThemeManager.Palette;

            ListView list = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                HideSelection = false,
                BorderStyle = BorderStyle.None,
                BackColor = palette.Surface,
                ForeColor = palette.TextPrimary
            };

            list.Columns.Add("文件名", 340);
            list.Columns.Add("大小", 110);
            list.Columns.Add("时间", 170);

            list.DoubleClick += (s, e) =>
            {
                try
                {
                    if (list.SelectedItems.Count != 1) return;
                    string fileName = list.SelectedItems[0].Text;
                    string path = Path.Combine(txtBackupPath.Text, fileName);
                    if (File.Exists(path))
                    {
                        System.Diagnostics.Process.Start(path);
                    }
                }
                catch
                {
                    // ignore
                }
            };

            return list;
        }

        private void LoadDefaults()
        {
            using (AppTelemetry.Measure("Backup.LoadDefaults"))
            {
                retentionDays = Math.Max(1, parameterService.GetInt("Backup.RetentionDays", 7));
                string configuredPath = parameterService.GetString("Backup.Directory", null);
                if (string.IsNullOrWhiteSpace(configuredPath))
                {
                    configuredPath = backupService.GetDefaultBackupDirectory();
                }

                txtBackupPath.Text = configuredPath;
            }
        }

        private void RefreshBackupList()
        {
            if (backupList == null) return;

            try
            {
                using (AppTelemetry.Measure("Backup.RefreshList"))
                {
                    backupList.BeginUpdate();
                    backupList.Items.Clear();

                    List<FileInfo> files = backupService.GetBackupFiles(txtBackupPath.Text);
                    foreach (FileInfo file in files)
                    {
                        ListViewItem item = new ListViewItem(file.Name);
                        item.SubItems.Add((file.Length / 1024.0 / 1024.0).ToString("F2") + " MB");
                        item.SubItems.Add(file.LastWriteTime.ToString("yyyy-MM-dd HH:mm"));
                        backupList.Items.Add(item);
                    }

                    backupList.EndUpdate();

                    UpdateStatus($"备份目录内共 {files.Count} 个文件，保留 {retentionDays} 天");
                }
            }
            catch (Exception ex)
            {
                try { backupList.EndUpdate(); } catch { }
                AppLog.Error(ex, "刷新备份列表失败");
                UpdateStatus("刷新失败: " + ex.Message);
            }
        }

        private void SelectBackupPath()
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择备份保存目录";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    txtBackupPath.Text = dialog.SelectedPath;
                    parameterService.SetValue("Backup.Directory", dialog.SelectedPath, "备份目录(为空则使用默认)");
                    RefreshBackupList();
                }
            }
        }

        private async Task RunBackupAsync()
        {
            if (btnBackup == null) return;

            btnBackup.Enabled = false;
            if (btnBrowse != null) btnBrowse.Enabled = false;
            if (btnOpen != null) btnOpen.Enabled = false;

            UpdateStatus("正在执行备份，请稍候...");

            BackupResult result;
            try
            {
                using (AppTelemetry.Measure("Backup.Run"))
                {
                    result = await Task.Run(() => backupService.CreateBackup(txtBackupPath.Text, retentionDays));
                }
            }
            catch (Exception ex)
            {
                AppLog.Error(ex, "备份执行异常");
                result = new BackupResult { Success = false, Message = ex.Message };
            }

            if (result.Success)
            {
                AuditTrail.Log(CurrentUser, "Backup", "System", $"备份完成: {Path.GetFileName(result.FilePath)}");
                UpdateStatus($"备份成功: {Path.GetFileName(result.FilePath)}");
            }
            else
            {
                AuditTrail.Log(CurrentUser, "BackupFailed", "System", result.Message ?? "备份失败");
                UpdateStatus(result.Message ?? "备份失败");
            }

            RefreshBackupList();
            btnBackup.Enabled = true;
            if (btnBrowse != null) btnBrowse.Enabled = true;
            if (btnOpen != null) btnOpen.Enabled = true;
        }

        private void OpenBackupDirectory()
        {
            try
            {
                string path = txtBackupPath.Text;
                if (!string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
                {
                    System.Diagnostics.Process.Start(path);
                }
            }
            catch
            {
                // ignore
            }
        }

        private void UpdateStatus(string message)
        {
            if (statusLabel == null) return;
            statusLabel.Text = message ?? string.Empty;
        }
    }
}
