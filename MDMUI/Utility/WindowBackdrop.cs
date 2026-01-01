using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MDMUI.Utility
{
    /// <summary>
    /// WinForms 的轻量“玻璃”窗口背景支持：
    /// - Windows 11: Mica/Acrylic（DwmSetWindowAttribute）
    /// - Windows 10: 尝试 Blur Behind（SetWindowCompositionAttribute）
    /// 
    /// 所有方法都遵循“尽力而为”，失败时静默返回 false，不影响主流程。
    /// </summary>
    public static class WindowBackdrop
    {
        public enum BackdropKind
        {
            Mica = 2,
            Acrylic = 3,
            Tabbed = 4
        }

        public static bool TryApply(Form form, BackdropKind kind, bool useDarkMode = false)
        {
            if (form == null || form.IsDisposed) return false;

            try
            {
                // Windows 11: System Backdrop (Mica/Acrylic/Tabbed)
                int backdrop = (int)kind;
                if (TryDwmSetWindowAttribute(form.Handle, DWMWA_SYSTEMBACKDROP_TYPE, ref backdrop))
                {
                    TrySetImmersiveDarkMode(form.Handle, useDarkMode);
                    return true;
                }

                // Windows 10: fallback blur
                if (TryEnableBlurBehind(form.Handle))
                {
                    TrySetImmersiveDarkMode(form.Handle, useDarkMode);
                    return true;
                }
            }
            catch
            {
                // ignored
            }

            return false;
        }

        private static bool TrySetImmersiveDarkMode(IntPtr hwnd, bool enable)
        {
            try
            {
                int value = enable ? 1 : 0;

                // 20 (Win11) / 19 (older) 任选其一成功即可
                if (TryDwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE, ref value)) return true;
                const int DWMWA_USE_IMMERSIVE_DARK_MODE_OLD = 19;
                return TryDwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE_OLD, ref value);
            }
            catch
            {
                return false;
            }
        }

        private static bool TryDwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int value)
        {
            int hr = DwmSetWindowAttribute(hwnd, attribute, ref value, Marshal.SizeOf(typeof(int)));
            return hr >= 0;
        }

        private static bool TryEnableBlurBehind(IntPtr hwnd)
        {
            try
            {
                AccentPolicy accent = new AccentPolicy
                {
                    AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND,
                    AccentFlags = 2,
                    GradientColor = 0
                };

                int size = Marshal.SizeOf(accent);
                IntPtr accentPtr = Marshal.AllocHGlobal(size);
                try
                {
                    Marshal.StructureToPtr(accent, accentPtr, false);
                    WindowCompositionAttributeData data = new WindowCompositionAttributeData
                    {
                        Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                        SizeOfData = size,
                        Data = accentPtr
                    };

                    int result = SetWindowCompositionAttribute(hwnd, ref data);
                    return result != 0;
                }
                finally
                {
                    Marshal.FreeHGlobal(accentPtr);
                }
            }
            catch
            {
                return false;
            }
        }

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_SYSTEMBACKDROP_TYPE = 38;

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int dwAttribute, ref int pvAttribute, int cbAttribute);

        private enum AccentState
        {
            ACCENT_DISABLED = 0,
            ACCENT_ENABLE_GRADIENT = 1,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_ENABLE_ACRYLICBLURBEHIND = 4
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct AccentPolicy
        {
            public AccentState AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }

        private enum WindowCompositionAttribute
        {
            WCA_ACCENT_POLICY = 19
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        [DllImport("user32.dll")]
        private static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);
    }
}

