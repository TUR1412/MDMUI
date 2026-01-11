using System.Drawing;
using System.Windows.Forms;
using MDMUI.Utility;

namespace MDMUI.Controls.Molecules
{
    public sealed class ActionToolbar : Panel
    {
        public FlowLayoutPanel LeftPanel { get; }
        public FlowLayoutPanel RightPanel { get; }

        public ActionToolbar()
        {
            Dock = DockStyle.Top;
            Height = 46;
            Padding = new Padding(0, 6, 0, 6);
            BackColor = Color.Transparent;

            RightPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0),
                Padding = new Padding(0),
                BackColor = Color.Transparent
            };

            LeftPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = false,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0),
                Padding = new Padding(0),
                BackColor = Color.Transparent
            };

            Controls.Add(LeftPanel);
            Controls.Add(RightPanel);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            try { BackColor = ThemeManager.Palette.Surface; } catch { }
        }
    }
}
