using System;
using System.Threading;
using System.Windows.Forms;

namespace MDMUI.Utility
{
    public static class CrashReporter
    {
        private static int handling;

        public static void Report(Exception exception, string source, bool isTerminating)
        {
            if (exception == null) exception = new Exception("Unknown exception");
            if (string.IsNullOrWhiteSpace(source)) source = "Unknown";

            // 避免重入导致无限弹窗/递归异常
            if (Interlocked.CompareExchange(ref handling, 1, 0) != 0)
            {
                try { AppLog.Critical(exception, $"[CrashReporter] 重入拦截: source={source}, terminating={isTerminating}"); } catch { }
                return;
            }

            string incidentId = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpperInvariant();
            try
            {
                AppLog.Critical(exception, $"[Unhandled] incident={incidentId}, source={source}, terminating={isTerminating}");
            }
            catch { }

            try
            {
                ShowDialogSafe(incidentId, exception, source, isTerminating);
            }
            catch
            {
                try
                {
                    MessageBox.Show(
                        $"应用程序遇到错误。\n\n事件ID: {incidentId}\n来源: {source}\n\n{exception.Message}",
                        "应用程序错误",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                catch { }
            }
            finally
            {
                Interlocked.Exchange(ref handling, 0);
            }
        }

        private static void ShowDialogSafe(string incidentId, Exception ex, string source, bool isTerminating)
        {
            string logPath = AppLog.CurrentLogFilePath;

            Action show = () =>
            {
                using (CrashReportForm form = new CrashReportForm(incidentId, ex, source, logPath, isTerminating))
                {
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.ShowDialog();
                }
            };

            if (Application.MessageLoop)
            {
                if (Application.OpenForms.Count > 0)
                {
                    Form owner = Application.OpenForms[0];
                    if (owner != null && owner.IsHandleCreated)
                    {
                        owner.BeginInvoke(show);
                        return;
                    }
                }

                // 无可用主窗体时直接展示
                show();
                return;
            }

            // 无消息循环：尽力展示对话框
            show();
        }
    }
}

