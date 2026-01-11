using System;
using System.Windows.Forms;

namespace MDMUI.Utility
{
    /// <summary>
    /// WinForms 的“错误边界”辅助：用于在事件处理/用户操作中捕获异常并记录日志，
    /// 避免异常直接冒泡到全局未处理异常处理器（影响用户体验）。
    /// </summary>
    public static class UiSafe
    {
        public static void Run(string operation, Action action, Action<Exception, string> onError = null)
        {
            if (action == null) return;
            string op = string.IsNullOrWhiteSpace(operation) ? "ui.operation" : operation.Trim();

            try
            {
                action();
            }
            catch (Exception ex)
            {
                string incidentId = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpperInvariant();
                try { AppLog.Error(ex, $"[UiSafe] incident={incidentId}, op={op}"); } catch { }

                if (onError != null)
                {
                    try { onError(ex, op); } catch { }
                    return;
                }

                // 无 UI 消息循环时不弹窗，避免测试/后台线程卡住
                if (!Application.MessageLoop) return;

                try
                {
                    MessageBox.Show(
                        $"操作失败。\n\n事件ID: {incidentId}\n操作: {op}\n\n{ex.Message}\n\n请查看日志以定位问题。",
                        "错误",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                catch
                {
                    // ignore
                }
            }
        }

        public static T Run<T>(string operation, Func<T> func, T fallback = default, Action<Exception, string> onError = null)
        {
            if (func == null) return fallback;

            T result = fallback;
            Run(operation, () => { result = func(); }, onError);
            return result;
        }

        public static EventHandler Wrap(string operation, EventHandler handler, Action<Exception, string> onError = null)
        {
            if (handler == null) return (s, e) => { };

            return (sender, args) => Run(operation, () => handler(sender, args), onError);
        }

        public static EventHandler<TEventArgs> Wrap<TEventArgs>(
            string operation,
            EventHandler<TEventArgs> handler,
            Action<Exception, string> onError = null)
            where TEventArgs : EventArgs
        {
            if (handler == null) return (s, e) => { };

            return (sender, args) => Run(operation, () => handler(sender, args), onError);
        }
    }
}

