using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace MDMUI.Utility
{
    public static class ModernTheme
    {
        private sealed class BackColorAnimator
        {
            private readonly Control control;
            private readonly Timer timer;
            private Color from;
            private Color to;
            private int startTick;
            private int durationMs;

            public BackColorAnimator(Control control)
            {
                this.control = control;
                timer = new Timer { Interval = 15 };
                timer.Tick += (s, e) => Tick();

                control.Disposed += (s, e) =>
                {
                    try { timer.Stop(); } catch { }
                    timer.Dispose();
                };
            }

            public void AnimateTo(Color target, int durationMs)
            {
                if (control.IsDisposed) return;
                if (!control.Visible) { control.BackColor = target; return; }

                from = control.BackColor;
                to = target;
                startTick = Environment.TickCount;
                this.durationMs = Math.Max(1, durationMs);
                timer.Start();
            }

            private void Tick()
            {
                if (control.IsDisposed) { timer.Stop(); return; }

                int elapsed = unchecked(Environment.TickCount - startTick);
                float t = durationMs <= 0 ? 1f : Math.Min(1f, (float)elapsed / durationMs);

                // EaseOutCubic: 1 - (1 - t)^3
                float eased = 1f - (float)Math.Pow(1f - t, 3);

                control.BackColor = Lerp(from, to, eased);

                if (t >= 1f) timer.Stop();
            }
        }

        private sealed class ButtonInteractionState
        {
            public Color Normal { get; set; }
            public Color Hover { get; set; }
            public Color Pressed { get; set; }
            public Color Disabled { get; set; }
            public BackColorAnimator Animator { get; set; }
        }

        private static readonly ConditionalWeakTable<Button, ButtonInteractionState> ButtonStates =
            new ConditionalWeakTable<Button, ButtonInteractionState>();

        public static void EnableMicroInteractions(Control root)
        {
            if (root == null) return;

            foreach (Control control in EnumerateControlsBreadthFirst(root))
            {
                if (control is Button button)
                {
                    AttachAnimatedButton(button);
                }
            }
        }

        private static void AttachAnimatedButton(Button button)
        {
            if (button == null) return;
            if (ButtonStates.TryGetValue(button, out _)) return;

            Color normal = button.BackColor;
            if (normal == default(Color) || normal.ToArgb() == Color.Empty.ToArgb())
            {
                normal = SystemColors.Control;
            }

            ButtonInteractionState state = new ButtonInteractionState
            {
                Normal = normal,
                Hover = Mix(normal, Color.White, 0.08f),
                Pressed = Mix(normal, Color.Black, 0.12f),
                Disabled = Mix(normal, SystemColors.ControlDark, 0.35f),
                Animator = new BackColorAnimator(button)
            };

            ButtonStates.Add(button, state);

            // 不强制改造所有按钮的 FlatStyle，避免影响已有设计器样式
            if (button.Cursor == Cursors.Default) button.Cursor = Cursors.Hand;

            button.MouseEnter += (s, e) =>
            {
                if (!button.Enabled) return;
                state.Animator.AnimateTo(state.Hover, 160);
            };

            button.MouseLeave += (s, e) =>
            {
                if (!button.Enabled) return;
                state.Animator.AnimateTo(state.Normal, 220);
            };

            button.MouseDown += (s, e) =>
            {
                if (!button.Enabled) return;
                if (e.Button != MouseButtons.Left) return;
                state.Animator.AnimateTo(state.Pressed, 80);
            };

            button.MouseUp += (s, e) =>
            {
                if (!button.Enabled) return;

                Point client = button.PointToClient(Cursor.Position);
                bool inside = button.ClientRectangle.Contains(client);
                state.Animator.AnimateTo(inside ? state.Hover : state.Normal, 140);
            };

            button.EnabledChanged += (s, e) =>
            {
                state.Animator.AnimateTo(button.Enabled ? state.Normal : state.Disabled, 120);
            };
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
                    // 某些控件在句柄未创建时访问 Controls 可能抛异常，忽略即可
                }
            }
        }

        private static Color Lerp(Color a, Color b, float t)
        {
            t = Math.Max(0f, Math.Min(1f, t));

            int A = (int)(a.A + (b.A - a.A) * t);
            int R = (int)(a.R + (b.R - a.R) * t);
            int G = (int)(a.G + (b.G - a.G) * t);
            int B = (int)(a.B + (b.B - a.B) * t);

            return Color.FromArgb(A, R, G, B);
        }

        private static Color Mix(Color baseColor, Color overlay, float amount)
        {
            amount = Math.Max(0f, Math.Min(1f, amount));
            return Lerp(baseColor, overlay, amount);
        }
    }
}

