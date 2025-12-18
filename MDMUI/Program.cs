using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MDMUI
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                // 设置编码为UTF-8，解决中文显示问题 
                // 在.NET Framework 4.0+中，Encoding.RegisterProvider不可用，直接设置控制台编码
                try
                {
                    Console.OutputEncoding = Encoding.UTF8;
                    Console.InputEncoding = Encoding.UTF8;
                }
                catch
                {
                    // 如果设置编码失败，忽略错误继续执行
                }
                
                // 配置调试日志
                Debug.AutoFlush = true;
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app_debug.log");
                TextWriterTraceListener textListener = new TextWriterTraceListener(logPath);
                Debug.Listeners.Add(textListener);
                
                // 记录应用程序启动时间
                Debug.WriteLine($"[{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}] 应用程序启动...");
                
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                // 设置应用程序异常处理
                Application.ThreadException += Application_ThreadException;
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                
                // 打开登录窗体 (使用Form1作为登录窗体)
                Form1 loginForm = new Form1();
                Application.Run(loginForm);
                
                // 登录成功后在Form1中已经创建并显示MainForm
                // 无需在这里创建MainForm实例
                
                Debug.WriteLine($"[{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}] 应用程序正常关闭");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}] 应用程序主线程异常: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                MessageBox.Show($"应用程序遇到严重错误，需要关闭。\n\n错误详情: {ex.Message}", 
                    "应用程序错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 确保日志写入
                Debug.Flush();
                foreach (TraceListener listener in Debug.Listeners)
                {
                    listener.Flush();
                    if (listener is TextWriterTraceListener textListener)
                    {
                        textListener.Close();
                    }
                }
            }
        }
        
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Debug.WriteLine($"[{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}] UI线程异常: {e.Exception.Message}");
            Debug.WriteLine(e.Exception.StackTrace);
            MessageBox.Show($"应用程序遇到错误，请重试或联系管理员。\n\n错误详情: {e.Exception.Message}", 
                "操作错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            Debug.WriteLine($"[{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}] 未处理的应用程序域异常: {ex?.Message}");
            Debug.WriteLine(ex?.StackTrace);
            MessageBox.Show($"应用程序遇到严重错误，需要关闭。\n\n错误详情: {ex?.Message}", 
                "应用程序错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
