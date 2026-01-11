using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MDMUI.Utility;

namespace MDMUI
{
    public sealed class CommandPaletteItem
    {
        public CommandPaletteItem(string title, string group, string formName, string permissionTag)
        {
            Title = title ?? string.Empty;
            Group = group ?? string.Empty;
            FormName = formName ?? string.Empty;
            PermissionTag = permissionTag;
            SearchText = (Title + " " + Group + " " + FormName).Trim();
        }

        public string Title { get; }
        public string Group { get; }
        public string FormName { get; }
        public string PermissionTag { get; }
        public string SearchText { get; }
        public int UsageCount { get; set; }
        public DateTime LastUsedUtc { get; set; }
        public bool Pinned { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Group)) return Title;
            return Title + " — " + Group;
        }
    }

    public sealed class CommandPaletteForm : Form
    {
        private readonly IReadOnlyList<CommandPaletteItem> allCommands;
        private readonly List<CommandPaletteItem> filtered = new List<CommandPaletteItem>();

        private TextBox txtQuery;
        private ListBox lstCommands;
        private Label lblHint;

        private Font titleFont;
        private Font subtitleFont;

        public CommandPaletteItem SelectedCommand { get; private set; }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse);
        
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        public CommandPaletteForm(IReadOnlyList<CommandPaletteItem> commands)
        {
            allCommands = commands ?? Array.Empty<CommandPaletteItem>();
            InitializeComponent();

            // 玻璃背景（尽力而为，不影响不支持的系统）
            try { WindowBackdrop.TryApply(this, WindowBackdrop.BackdropKind.Mica, useDarkMode: false); } catch { }
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            ThemePalette palette = ThemeManager.Palette;
            titleFont = ThemeManager.CreateTitleFont(11.5f, FontStyle.Bold);
            subtitleFont = ThemeManager.CreateBodyFont(9f, FontStyle.Regular);

            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            TopMost = true;
            StartPosition = FormStartPosition.Manual;
            KeyPreview = true;
            DoubleBuffered = true;

            BackColor = palette.Background;
            Padding = new Padding(14);
            Size = new Size(720, 420);

            Panel container = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = palette.Surface,
                Padding = new Padding(12)
            };

            txtQuery = new TextBox
            {
                Dock = DockStyle.Top,
                Height = 36,
                Font = ThemeManager.CreateBodyFont(11f),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = palette.SurfaceAlt,
                ForeColor = palette.TextPrimary
            };
            txtQuery.TextChanged += (s, e) => RefreshFilter();
            txtQuery.KeyDown += TxtQuery_KeyDown;

            lblHint = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 22,
                Text = "Enter 打开 · Esc 关闭 · ↑↓ 选择 · Ctrl+P 固定",
                Font = ThemeManager.CreateBodyFont(8.5f),
                ForeColor = palette.TextSecondary,
                TextAlign = ContentAlignment.MiddleLeft
            };

            lstCommands = new ListBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                IntegralHeight = false,
                DrawMode = DrawMode.OwnerDrawFixed,
                ItemHeight = 48,
                BackColor = palette.Surface,
                ForeColor = palette.TextPrimary
            };
            lstCommands.DrawItem += LstCommands_DrawItem;
            lstCommands.DoubleClick += (s, e) => SubmitSelection();
            lstCommands.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    SubmitSelection();
                }
            };

            container.Controls.Add(lstCommands);
            container.Controls.Add(lblHint);
            container.Controls.Add(txtQuery);

            Controls.Add(container);

            Shown += (s, e) =>
            {
                ApplyRoundedCorners();
                CenterToOwner();
                txtQuery.Focus();
                RefreshFilter();
            };

            SizeChanged += (s, e) => ApplyRoundedCorners();

            KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    e.Handled = true;
                    Close();
                }

                if (e.Control && e.KeyCode == Keys.P)
                {
                    e.Handled = true;
                    TogglePinSelected();
                }
            };

            Deactivate += (s, e) =>
            {
                // 作为“命令面板”体验：失焦自动关闭，避免残留在最上层
                try { Close(); } catch { }
            };

            ResumeLayout(false);
        }

        private void ApplyRoundedCorners()
        {
            try
            {
                IntPtr rgn = CreateRoundRectRgn(0, 0, Width, Height, 16, 16);
                try
                {
                    Region = Region.FromHrgn(rgn);
                }
                finally
                {
                    DeleteObject(rgn);
                }
            }
            catch
            {
                // 忽略：圆角失败不影响功能
            }
        }

        private void CenterToOwner()
        {
            Rectangle bounds = Owner != null ? Owner.Bounds : Screen.FromPoint(Cursor.Position).WorkingArea;
            int x = bounds.Left + (bounds.Width - Width) / 2;
            int y = bounds.Top + (int)(bounds.Height * 0.18);
            Location = new Point(Math.Max(bounds.Left, x), Math.Max(bounds.Top, y));
        }

        private void TxtQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                if (lstCommands.Items.Count <= 0) return;
                lstCommands.SelectedIndex = Math.Min(lstCommands.Items.Count - 1, lstCommands.SelectedIndex + 1);
                return;
            }

            if (e.KeyCode == Keys.Up)
            {
                e.Handled = true;
                if (lstCommands.Items.Count <= 0) return;
                lstCommands.SelectedIndex = Math.Max(0, lstCommands.SelectedIndex - 1);
                return;
            }

            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                SubmitSelection();
            }
        }

        private void RefreshFilter()
        {
            string query = (txtQuery.Text ?? string.Empty).Trim();
            string[] terms = query
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .Where(t => t.Length > 0)
                .ToArray();

            filtered.Clear();

            IEnumerable<CommandPaletteItem> source = allCommands;
            List<(CommandPaletteItem item, int score)> scored = new List<(CommandPaletteItem, int)>();

            foreach (CommandPaletteItem item in source)
            {
                if (TryGetScore(item, terms, out int score))
                {
                    scored.Add((item, score));
                }
            }

            filtered.AddRange(scored
                .OrderByDescending(item => item.score)
                .ThenBy(item => item.item.Title, StringComparer.OrdinalIgnoreCase)
                .Select(item => item.item));

            lstCommands.BeginUpdate();
            lstCommands.Items.Clear();
            foreach (CommandPaletteItem item in filtered.Take(60))
            {
                lstCommands.Items.Add(item);
            }
            lstCommands.EndUpdate();

            if (lstCommands.Items.Count > 0)
            {
                lstCommands.SelectedIndex = 0;
            }
        }

        private static bool TryGetScore(CommandPaletteItem item, string[] terms, out int score)
        {
            score = 0;
            if (item == null) return false;

            string haystack = item.SearchText ?? string.Empty;
            if (string.IsNullOrWhiteSpace(haystack)) return false;

            int usageScore = GetUsageScore(item);

            if (terms == null || terms.Length == 0)
            {
                score = usageScore;
                return true;
            }

            int matchScore = 0;
            foreach (string term in terms)
            {
                if (string.IsNullOrWhiteSpace(term)) continue;
                int index = haystack.IndexOf(term, StringComparison.OrdinalIgnoreCase);
                if (index < 0)
                {
                    score = 0;
                    return false;
                }

                matchScore += index == 0 ? 120 : Math.Max(20, 80 - index);
            }

            score = usageScore + matchScore;
            return true;
        }

        private static int GetUsageScore(CommandPaletteItem item)
        {
            int score = 0;
            if (item.Pinned) score += 500;
            if (item.UsageCount > 0)
            {
                score += Math.Min(200, item.UsageCount * 12);
            }

            if (item.LastUsedUtc != DateTime.MinValue)
            {
                double days = (DateTime.UtcNow - item.LastUsedUtc).TotalDays;
                score += (int)Math.Max(0, 140 - days * 8);
            }

            return score;
        }

        private void SubmitSelection()
        {
            if (lstCommands.SelectedItem is CommandPaletteItem item)
            {
                SelectedCommand = item;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void TogglePinSelected()
        {
            if (!(lstCommands.SelectedItem is CommandPaletteItem item))
            {
                return;
            }

            CommandUsageStore.TogglePin(item.FormName);
            item.Pinned = !item.Pinned;
            RefreshFilter();
        }

        private void LstCommands_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index < 0 || e.Index >= lstCommands.Items.Count)
            {
                return;
            }

            CommandPaletteItem item = (CommandPaletteItem)lstCommands.Items[e.Index];
            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            ThemePalette palette = ThemeManager.Palette;
            Color bg = selected ? palette.AccentSoft : palette.Surface;
            Color border = selected ? palette.Accent : palette.Border;
            Color titleColor = palette.TextPrimary;
            Color subColor = palette.TextSecondary;

            Rectangle rect = e.Bounds;
            rect.Inflate(-6, -4);

            using (SolidBrush brush = new SolidBrush(bg))
            {
                e.Graphics.FillRectangle(brush, rect);
            }

            using (Pen pen = new Pen(border))
            {
                e.Graphics.DrawRectangle(pen, rect);
            }

            Rectangle titleRect = new Rectangle(rect.X + 10, rect.Y + 6, rect.Width - 20, 22);
            Rectangle subRect = new Rectangle(rect.X + 10, rect.Y + 26, rect.Width - 20, 18);

            string pin = item.Pinned ? "★ " : string.Empty;
            string usage = item.UsageCount > 0 ? $" · {item.UsageCount}次" : string.Empty;
            TextRenderer.DrawText(e.Graphics, pin + item.Title, titleFont, titleRect, titleColor,
                TextFormatFlags.EndEllipsis | TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

            TextRenderer.DrawText(e.Graphics, item.Group + usage, subtitleFont, subRect, subColor,
                TextFormatFlags.EndEllipsis | TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

            e.DrawFocusRectangle();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try { titleFont?.Dispose(); } catch { }
                try { subtitleFont?.Dispose(); } catch { }
            }
            base.Dispose(disposing);
        }
    }
}
