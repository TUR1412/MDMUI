using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using MDMUI.BLL;
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
        private Button btnBrowse;
        private Button btnBackup;
        private Button btnOpen;
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
            demoPanel.BackColor = ThemeManager.Palette.Surface;

            Panel toolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50
            };

            txtBackupPath = new TextBox
            {
                Width = 360,
                Height = 28,
                Location = new Point(0, 10),
                ReadOnly = true
            };

            btnBrowse = new Button
            {
                Text = "选择目录",
                Width = 90,
                Height = 30,
                Location = new Point(txtBackupPath.Right + 10, 8)
            };
            btnBrowse.Click += (s, e) => SelectBackupPath();

            btnBackup = new Button
            {
                Text = "立即备份",
                Width = 90,
                Height = 30,
                Location = new Point(btnBrowse.Right + 10, 8)
            };
            btnBackup.Click += async (s, e) => await RunBackupAsync();

            btnOpen = new Button
            {
                Text = "打开目录",
                Width = 90,
                Height = 30,
                Location = new Point(btnBackup.Right + 10, 8)
            };
            btnOpen.Click += (s, e) => OpenBackupDirectory();

            toolbar.Controls.Add(txtBackupPath);
            toolbar.Controls.Add(btnBrowse);
            toolbar.Controls.Add(btnBackup);
            toolbar.Controls.Add(btnOpen);

            backupList = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true
            };
            backupList.Columns.Add("文件名", 320);
            backupList.Columns.Add("大小", 100);
            backupList.Columns.Add("时间", 160);

            statusLabel = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 24,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = ThemeManager.Palette.TextSecondary
            };

            demoPanel.Controls.Add(backupList);
            demoPanel.Controls.Add(statusLabel);
            demoPanel.Controls.Add(toolbar);
        }

        private void LoadDefaults()
        {
            retentionDays = Math.Max(1, parameterService.GetInt("Backup.RetentionDays", 7));
            string configuredPath = parameterService.GetString("Backup.Directory", null);
            if (string.IsNullOrWhiteSpace(configuredPath))
            {
                configuredPath = backupService.GetDefaultBackupDirectory();
            }

            txtBackupPath.Text = configuredPath;
        }

        private void RefreshBackupList()
        {
            if (backupList == null) return;
            backupList.Items.Clear();

            List<FileInfo> files = backupService.GetBackupFiles(txtBackupPath.Text);
            foreach (FileInfo file in files)
            {
                ListViewItem item = new ListViewItem(file.Name);
                item.SubItems.Add((file.Length / 1024.0 / 1024.0).ToString("F2") + " MB");
                item.SubItems.Add(file.LastWriteTime.ToString("yyyy-MM-dd HH:mm"));
                backupList.Items.Add(item);
            }

            UpdateStatus($"备份目录内共 {files.Count} 个文件，保留 {retentionDays} 天");
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
            UpdateStatus("正在执行备份，请稍候...");

            BackupResult result = await Task.Run(() => backupService.CreateBackup(txtBackupPath.Text, retentionDays));
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
