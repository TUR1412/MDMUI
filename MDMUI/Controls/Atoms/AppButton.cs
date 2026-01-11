using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MDMUI.Utility;

namespace MDMUI.Controls.Atoms
{
    public enum AppButtonVariant
    {
        Primary = 0,
        Secondary = 1,
        Danger = 2
    }

    public class AppButton : Button
    {
        private AppButtonVariant variant = AppButtonVariant.Primary;

        [DefaultValue(AppButtonVariant.Primary)]
        public AppButtonVariant Variant
        {
            get => variant;
            set
            {
                variant = value;
                ApplyVariant();
            }
        }

        public AppButton()
        {
            FlatStyle = FlatStyle.Flat;
            UseVisualStyleBackColor = false;
            AutoSize = true;
            Padding = new Padding(10, 6, 10, 6);
            Margin = new Padding(8, 0, 0, 0);

            FlatAppearance.BorderSize = 1;
            FlatAppearance.BorderColor = ThemeManager.Palette.Border;

            ApplyVariant();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            try { Font = ThemeManager.CreateBodyFont(Font?.Size ?? 9f); } catch { }
            ApplyVariant();
        }

        private void ApplyVariant()
        {
            ThemePalette palette = ThemeManager.Palette;

            switch (variant)
            {
                case AppButtonVariant.Secondary:
                    BackColor = palette.Surface;
                    ForeColor = palette.TextPrimary;
                    FlatAppearance.BorderColor = palette.Border;
                    break;
                case AppButtonVariant.Danger:
                    BackColor = Color.FromArgb(220, 68, 68);
                    ForeColor = Color.White;
                    FlatAppearance.BorderColor = Color.FromArgb(200, 60, 60);
                    break;
                default:
                    BackColor = palette.Accent;
                    ForeColor = Color.White;
                    FlatAppearance.BorderColor = palette.Accent;
                    break;
            }
        }
    }
}

