using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace MDMUI.Utility
{
    public sealed class ThemePalette
    {
        public ThemePalette(
            Color background,
            Color surface,
            Color surfaceAlt,
            Color accent,
            Color accentSoft,
            Color border,
            Color textPrimary,
            Color textSecondary)
        {
            Background = background;
            Surface = surface;
            SurfaceAlt = surfaceAlt;
            Accent = accent;
            AccentSoft = accentSoft;
            Border = border;
            TextPrimary = textPrimary;
            TextSecondary = textSecondary;
        }

        public Color Background { get; }
        public Color Surface { get; }
        public Color SurfaceAlt { get; }
        public Color Accent { get; }
        public Color AccentSoft { get; }
        public Color Border { get; }
        public Color TextPrimary { get; }
        public Color TextSecondary { get; }

        public ThemePalette WithAccent(Color accent)
        {
            Color soft = Color.FromArgb(220, accent.R, accent.G, accent.B);
            return new ThemePalette(Background, Surface, SurfaceAlt, accent, soft, Border, TextPrimary, TextSecondary);
        }
    }

    public static class ThemeManager
    {
        private static readonly FontFamily PreferredFamily = ResolveFontFamily(
            "Bahnschrift",
            "Segoe UI Variable Display",
            "Segoe UI",
            "Microsoft YaHei UI");

        private static readonly ThemePalette DefaultPalette = new ThemePalette(
            background: Color.FromArgb(243, 246, 251),
            surface: Color.FromArgb(255, 255, 255),
            surfaceAlt: Color.FromArgb(247, 250, 255),
            accent: Color.FromArgb(0, 163, 255),
            accentSoft: Color.FromArgb(210, 232, 255),
            border: Color.FromArgb(220, 228, 240),
            textPrimary: Color.FromArgb(24, 32, 46),
            textSecondary: Color.FromArgb(90, 98, 112));

        private static ThemePalette palette = DefaultPalette;

        public static ThemePalette Palette => palette;

        public static void SetAccent(Color accent)
        {
            palette = DefaultPalette.WithAccent(accent);
        }

        public static Font CreateBodyFont(float size, FontStyle style = FontStyle.Regular)
        {
            return new Font(PreferredFamily, size, style, GraphicsUnit.Point);
        }

        public static Font CreateTitleFont(float size, FontStyle style = FontStyle.Bold)
        {
            return new Font(PreferredFamily, size, style, GraphicsUnit.Point);
        }

        public static void ApplyTo(Control root)
        {
            if (root == null) return;

            foreach (Control control in EnumerateControlsBreadthFirst(root))
            {
                ApplyControlStyle(control);
            }
        }

        private static void ApplyControlStyle(Control control)
        {
            if (control is Form form)
            {
                if (IsDefaultBackColor(form.BackColor))
                {
                    form.BackColor = palette.Background;
                }

                if (IsDefaultForeColor(form.ForeColor))
                {
                    form.ForeColor = palette.TextPrimary;
                }

                if (IsDefaultFont(form.Font))
                {
                    form.Font = CreateBodyFont(form.Font?.Size ?? 9f);
                }

                return;
            }

            if (control is Panel panel)
            {
                if (IsDefaultBackColor(panel.BackColor))
                {
                    panel.BackColor = palette.Surface;
                }
            }

            if (control is GroupBox groupBox)
            {
                if (IsDefaultBackColor(groupBox.BackColor))
                {
                    groupBox.BackColor = palette.Surface;
                }

                if (IsDefaultForeColor(groupBox.ForeColor))
                {
                    groupBox.ForeColor = palette.TextPrimary;
                }
            }

            if (control is Label label)
            {
                if (IsDefaultForeColor(label.ForeColor))
                {
                    label.ForeColor = palette.TextSecondary;
                }
            }

            if (control is Button button)
            {
                if (button is IThemeSelfStyled) return;

                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderColor = palette.Border;
                button.FlatAppearance.BorderSize = 1;

                Control parent = null;
                bool isInheritedBack = false;
                bool isInheritedFore = false;
                try
                {
                    parent = button.Parent;
                    if (parent != null)
                    {
                        isInheritedBack = button.BackColor.ToArgb() == parent.BackColor.ToArgb();
                        isInheritedFore = button.ForeColor.ToArgb() == parent.ForeColor.ToArgb();
                    }
                }
                catch
                {
                    // ignore
                }

                bool isDefaultBack = IsDefaultBackColor(button.BackColor) || isInheritedBack;
                bool isDefaultFore = IsDefaultForeColor(button.ForeColor) || isInheritedFore;

                Form hostForm = null;
                try { hostForm = button.FindForm(); } catch { }

                bool isPrimaryAction = false;
                bool isCancelAction = false;

                try
                {
                    if (hostForm != null)
                    {
                        isPrimaryAction = ReferenceEquals(hostForm.AcceptButton, button);
                        isCancelAction = ReferenceEquals(hostForm.CancelButton, button);
                    }
                }
                catch
                {
                    // ignore
                }

                // 约定：默认按钮为“次要”风格；仅将窗体 AcceptButton 视为主按钮并上色
                if (isDefaultBack)
                {
                    if (isPrimaryAction)
                    {
                        button.BackColor = palette.Accent;
                        button.FlatAppearance.BorderColor = palette.Accent;
                        if (isDefaultFore) button.ForeColor = Color.White;
                    }
                    else
                    {
                        button.BackColor = palette.Surface;
                        button.FlatAppearance.BorderColor = palette.Border;
                        if (isDefaultFore) button.ForeColor = palette.TextPrimary;

                        // CancelButton 一般是次要动作，保持 Secondary 即可
                        _ = isCancelAction;
                    }
                }
                else
                {
                    // 按钮已自定义背景色（例如危险按钮/图标按钮），仅在前景色仍为默认时套用主题文本色
                    if (isDefaultFore)
                    {
                        button.ForeColor = palette.TextPrimary;
                    }
                }
            }

            if (control is TextBox textBox)
            {
                if (IsDefaultBackColor(textBox.BackColor))
                {
                    textBox.BackColor = palette.Surface;
                }

                if (IsDefaultForeColor(textBox.ForeColor))
                {
                    textBox.ForeColor = palette.TextPrimary;
                }
            }

            if (control is ComboBox comboBox)
            {
                if (IsDefaultBackColor(comboBox.BackColor))
                {
                    comboBox.BackColor = palette.Surface;
                }

                if (IsDefaultForeColor(comboBox.ForeColor))
                {
                    comboBox.ForeColor = palette.TextPrimary;
                }
            }

            if (control is DataGridView grid)
            {
                grid.BackgroundColor = palette.Surface;
                grid.GridColor = palette.Border;
                grid.EnableHeadersVisualStyles = false;
                grid.ColumnHeadersDefaultCellStyle.BackColor = palette.SurfaceAlt;
                grid.ColumnHeadersDefaultCellStyle.ForeColor = palette.TextPrimary;
                grid.RowHeadersDefaultCellStyle.BackColor = palette.SurfaceAlt;
                grid.RowsDefaultCellStyle.BackColor = palette.Surface;
                grid.RowsDefaultCellStyle.ForeColor = palette.TextPrimary;
                grid.DefaultCellStyle.SelectionBackColor = palette.AccentSoft;
                grid.DefaultCellStyle.SelectionForeColor = palette.TextPrimary;

                GridStyler.Apply(grid);
                TryEnableDoubleBuffer(grid);
            }
        }

        private static bool IsDefaultBackColor(Color color)
        {
            return color == default(Color) || color == SystemColors.Control || color.ToArgb() == Color.White.ToArgb();
        }

        private static bool IsDefaultForeColor(Color color)
        {
            return color == default(Color) || color == SystemColors.ControlText || color == Color.Black;
        }

        private static bool IsDefaultFont(Font font)
        {
            if (font == null) return true;
            return string.Equals(font.Name, "Microsoft Sans Serif", StringComparison.OrdinalIgnoreCase)
                || string.Equals(font.Name, "Segoe UI", StringComparison.OrdinalIgnoreCase)
                || string.Equals(font.Name, "Microsoft YaHei UI", StringComparison.OrdinalIgnoreCase);
        }

        private static FontFamily ResolveFontFamily(params string[] names)
        {
            InstalledFontCollection fonts = new InstalledFontCollection();
            foreach (string name in names)
            {
                FontFamily family = fonts.Families.FirstOrDefault(f => string.Equals(f.Name, name, StringComparison.OrdinalIgnoreCase));
                if (family != null)
                {
                    return family;
                }
            }

            return SystemFonts.DefaultFont.FontFamily;
        }

        private static void TryEnableDoubleBuffer(DataGridView grid)
        {
            try
            {
                typeof(DataGridView).InvokeMember("DoubleBuffered",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                    null, grid, new object[] { true });
            }
            catch
            {
                // ignore
            }
        }

        private static IEnumerable<Control> EnumerateControlsBreadthFirst(Control root)
        {
            Queue<Control> queue = new Queue<Control>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                Control current = queue.Dequeue();
                yield return current;

                try
                {
                    foreach (Control child in current.Controls)
                    {
                        queue.Enqueue(child);
                    }
                }
                catch
                {
                    // ignore
                }
            }
        }
    }
}
