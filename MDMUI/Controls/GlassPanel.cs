using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MDMUI.Controls
{
    /// <summary>
    /// 轻量的“玻璃拟态”面板：圆角 + 半透明填充 + 细边框。
    /// WinForms 无原生实时模糊，本控件侧重于视觉语言与一致性（支持子控件布局）。
    /// </summary>
    public class GlassPanel : Panel
    {
        private int cornerRadius = 16;
        private Color glassColor = Color.FromArgb(210, 255, 255, 255);
        private Color borderColor = Color.FromArgb(80, 255, 255, 255);
        private float borderThickness = 1f;

        public GlassPanel()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor,
                true);

            BackColor = Color.Transparent;
        }

        [DefaultValue(16)]
        public int CornerRadius
        {
            get { return cornerRadius; }
            set
            {
                int next = Math.Max(0, value);
                if (cornerRadius == next) return;
                cornerRadius = next;
                UpdateRegion();
                Invalidate();
            }
        }

        public Color GlassColor
        {
            get { return glassColor; }
            set
            {
                if (glassColor.ToArgb() == value.ToArgb()) return;
                glassColor = value;
                Invalidate();
            }
        }

        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                if (borderColor.ToArgb() == value.ToArgb()) return;
                borderColor = value;
                Invalidate();
            }
        }

        [DefaultValue(1f)]
        public float BorderThickness
        {
            get { return borderThickness; }
            set
            {
                float next = Math.Max(0f, value);
                if (Math.Abs(borderThickness - next) < 0.0001f) return;
                borderThickness = next;
                Invalidate();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateRegion();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // 由 OnPaint 统一绘制背景（避免默认背景擦除导致闪烁）
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle rect = ClientRectangle;
            if (rect.Width <= 0 || rect.Height <= 0) return;

            using (GraphicsPath path = CreateRoundedRectanglePath(rect, cornerRadius))
            {
                using (SolidBrush brush = new SolidBrush(glassColor))
                {
                    e.Graphics.FillPath(brush, path);
                }

                if (borderThickness > 0f)
                {
                    using (Pen pen = new Pen(borderColor, borderThickness))
                    {
                        pen.Alignment = PenAlignment.Inset;
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            }

            // 子控件绘制
            base.OnPaint(e);
        }

        private void UpdateRegion()
        {
            if (IsDisposed) return;
            if (cornerRadius <= 0)
            {
                Region = null;
                return;
            }

            Rectangle rect = ClientRectangle;
            if (rect.Width <= 0 || rect.Height <= 0) return;

            using (GraphicsPath path = CreateRoundedRectanglePath(rect, cornerRadius))
            {
                Region = new Region(path);
            }
        }

        private static GraphicsPath CreateRoundedRectanglePath(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            if (radius <= 0)
            {
                path.AddRectangle(bounds);
                path.CloseFigure();
                return path;
            }

            int diameter = radius * 2;
            Rectangle arc = new Rectangle(bounds.Location, new Size(diameter, diameter));

            // 左上
            path.AddArc(arc, 180, 90);

            // 右上
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // 右下
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // 左下
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }
    }
}

