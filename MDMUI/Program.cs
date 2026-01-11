using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MDMUI.Utility;

namespace MDMUI
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            AppLog.Initialize();

            try
            {
                using (AppTelemetry.Measure("App.Startup"))
                {
                    // 设置编码为 UTF-8，解决中文显示问题（尽力而为）
                    try
                    {
                        Console.OutputEncoding = Encoding.UTF8;
                        Console.InputEncoding = Encoding.UTF8;
                    }
                    catch
                    {
                        // ignore
                    }

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    AppLog.Info($"应用程序启动... Version={Application.ProductVersion}");

                    // 设置应用程序异常处理
                    Application.ThreadException += Application_ThreadException;
                    Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                    AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                    TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

                    // 打开登录窗体 (使用Form1作为登录窗体)
                    Form1 loginForm = new Form1();
                    Application.Run(loginForm);

                    // 登录成功后在Form1中已经创建并显示MainForm
                    // 无需在这里创建MainForm实例

                    AppLog.Info("应用程序正常关闭");
                }
            }
            catch (Exception ex)
            {
                CrashReporter.Report(ex, "Main", isTerminating: true);
            }
            finally
            {
                AppLog.Flush();
                AppLog.Shutdown();
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            CrashReporter.Report(e.Exception, "UI Thread", isTerminating: false);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception ?? new Exception("Unhandled exception (non-Exception)");
            CrashReporter.Report(ex, "AppDomain", isTerminating: e.IsTerminating);
        }

        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            try
            {
                CrashReporter.Report(e.Exception, "TaskScheduler", isTerminating: false);
                e.SetObserved();
            }
            catch
            {
                // ignore
            }
        }
    }
}

