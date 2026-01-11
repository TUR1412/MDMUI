using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace MDMUI.Utility
{
    public static class UiThemingBootstrapper
    {
        private static int installed;
        private static readonly ConditionalWeakTable<Form, object> Applied = new ConditionalWeakTable<Form, object>();

        public static void Install()
        {
            if (Interlocked.Exchange(ref installed, 1) == 1) return;

            Application.Idle += (_, __) => ApplyToOpenForms();
        }

        private static void ApplyToOpenForms()
        {
            try
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form == null || form.IsDisposed) continue;
                    if (Applied.TryGetValue(form, out _)) continue;

                    Applied.Add(form, new object());

                    try { ThemeManager.ApplyTo(form); } catch { }
                    try { ModernTheme.EnableMicroInteractions(form); } catch { }
                }
            }
            catch
            {
                // ignore
            }
        }
    }
}

