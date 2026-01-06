using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MDMUI.BLL;
using MDMUI.Model;
using MDMUI.Utility;
using MDMUI.Controls;
using System.Runtime.InteropServices; // 用于实现窗体圆角

namespace MDMUI
{
    public partial class Form1 : Form
    {
        private UserBLL userBLL;
        private User currentUser;
        private AppPreferences loginPreferences;

        // “玻璃拟态”登录卡片与加载遮罩
        private GlassPanel glassLoginPanel;
        private GlassPanel loginLoadingOverlay;
        private Label loginLoadingLabel;
        private ProgressBar loginLoadingBar;
        private bool loginInProgress;
        
        // 自定义圆角边框
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );

        // 拖动窗体
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        
        // 渐变背景色
        private Color gradientStart = Color.FromArgb(72, 52, 212);  // 深蓝紫色
        private Color gradientEnd = Color.FromArgb(130, 60, 229);   // 明亮紫色
        
        // 控件颜色
        private Color primaryColor = Color.FromArgb(72, 52, 212);   // 主色
        private Color secondaryColor = Color.FromArgb(110, 90, 220); // 次要色
        private Color accentColor = Color.FromArgb(255, 94, 98);     // 强调色
        private Color textDarkColor = Color.FromArgb(60, 60, 70);    // 深色文本
        private Color textLightColor = Color.FromArgb(240, 240, 250); // 浅色文本
        private Color shadowColor = Color.FromArgb(0, 0, 0, 60);     // 阴影色

        public Form1()
        {
            InitializeComponent();
            userBLL = new UserBLL();

            // 添加窗体加载事件
            this.Load += new EventHandler(Form1_Load);

            // 为登录按钮添加点击事件
            this.btnLogin.Click += new EventHandler(this.LoginButton_Click);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 应用现代样式
            ApplyModernStyle();

            // 玻璃背景（尽力而为：Windows 11 优先 Mica；不影响不支持的系统）
            try { WindowBackdrop.TryApply(this, WindowBackdrop.BackdropKind.Mica, useDarkMode: false); } catch { }

            // 微交互（hover/click 平滑过渡）
            ModernTheme.EnableMicroInteractions(this);

            // 设置默认文本和样式
            this.lblLoginHeader.Text = "用户登录";
            this.lblLoginHeader.ForeColor = textDarkColor;
            
            // 设置标签文本
            this.lblFactory.Text = "工厂";
            this.lblLanguage.Text = "语言";
            this.lblUsername.Text = "用户名";
            this.lblPassword.Text = "密码";
            
            // 设置输入框默认值
            this.txtNewUsername.Text = "admin";
            this.txtNewPassword.Text = "1";
            this.txtNewPassword.PasswordChar = '●';
            
            // 设置记住密码复选框
            this.chkRemember.Text = "记住密码";
            this.chkRemember.Checked = true;
            
            // 设置登录按钮样式
            this.btnLogin.Text = "登 录";
            
            // 设置下拉框样式
            this.cboNewLanguage.BackColor = Color.White;
            this.cboNewFactory.BackColor = Color.White;

            // 移除右侧白条（移除窗体右侧padding）
            this.Padding = new Padding(0, 0, 0, 0);
            
            // 设置关闭和最小化按钮事件
            this.btnClose.Click += (s, e2) => Application.Exit();
            this.btnMinimize.Click += (s, e2) => this.WindowState = FormWindowState.Minimized;
            
            // 初始化语言下拉框
            if (this.cboNewLanguage.Items.Count == 0)
            {
                this.cboNewLanguage.Items.Add("中文");
                this.cboNewLanguage.Items.Add("English");
            }
            this.cboNewLanguage.SelectedIndex = 0;
            
            // 设置版本标签的基本属性
            ConfigureVersionLabel();
            
            // 启动时做一次“非破坏性”数据库引导，避免首启缺库/缺表直接崩溃
            if (!EnsureDatabaseReadyForLogin())
            {
                // 失败时已给出提示并禁用登录按钮
                return;
            }

            // 从数据库加载工厂列表
            LoadFactoriesForNewUI();

            // 读取并应用“上次登录”偏好（用户名/工厂/语言/记住密码）
            ApplyLoginPreferences();

            // 预创建加载遮罩（避免首次点击时闪烁）
            EnsureLoginLoadingOverlay();
        }

        private bool EnsureDatabaseReadyForLogin()
        {
            try
            {
                DatabaseBootstrapper.EnsureDatabaseReady();
                return true;
            }
            catch (Exception ex)
            {
                string message =
                    "数据库初始化失败，登录功能已被禁用。\n\n" +
                    "原因: " + CommonHelper.GetFullExceptionMessage(ex) + "\n\n" +
                    "建议：\n" +
                    "1) 确认本机已安装 SQL Server LocalDB（MSSQLLocalDB）。\n" +
                    "2) 检查 App.config 中 DefaultConnection 是否正确。\n" +
                    "3) 若是首次使用，可执行 dbo.sql 初始化数据（项目根目录提供）。";

                MessageBox.Show(message, "数据库初始化失败", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // 禁用登录，避免后续操作继续触发更多异常
                if (this.btnLogin != null) this.btnLogin.Enabled = false;
                if (this.cboNewFactory != null) this.cboNewFactory.Enabled = false;

                return false;
            }
        }

        // 应用现代UI样式
        private void ApplyModernStyle()
        {
            // 应用圆角
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            
            // 设置窗体背景色
            this.BackColor = Color.FromArgb(245, 245, 250);
            
            // 设置头部面板
            this.pnlHeader.BackColor = gradientStart;
            
            // 设置顶部面板可拖动
            this.pnlHeader.MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) { ReleaseCapture(); SendMessage(Handle, 0xA1, 0x2, 0); } };
            
            // 设置左侧面板背景
            this.pnlLeft.BackColor = gradientStart;
            
            // 为左侧面板添加渐变和图案
            this.pnlLeft.Paint += (s, e) => 
            {
                // 创建高质量绘图对象
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            
                // 创建圆润渐变背景
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    this.pnlLeft.ClientRectangle,
                    gradientStart,
                    gradientEnd,
                    LinearGradientMode.Vertical))
                {
                    ColorBlend blend = new ColorBlend(3);
                    blend.Colors = new Color[] { gradientStart, Color.FromArgb(100, 56, 220), gradientEnd };
                    blend.Positions = new float[] { 0.0f, 0.5f, 1.0f };
                    brush.InterpolationColors = blend;
                    e.Graphics.FillRectangle(brush, this.pnlLeft.ClientRectangle);
                }
                
                // 绘制MDM大字
                string companyName = "MDM";
                using (Font logoFont = new Font("微软雅黑", 50, FontStyle.Bold))
                {
                    SizeF textSize = e.Graphics.MeasureString(companyName, logoFont);
                    
                    // 阴影效果
                    using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(30, 0, 0, 0)))
                    {
                        e.Graphics.DrawString(companyName, logoFont, shadowBrush, 
                            (this.pnlLeft.Width - textSize.Width) / 2 + 2, 
                            (this.pnlLeft.Height - textSize.Height) / 2 - 60 + 2);
                    }
                    
                    // 主文字
                    using (SolidBrush textBrush = new SolidBrush(Color.White))
                    {
                        e.Graphics.DrawString(companyName, logoFont, textBrush, 
                            (this.pnlLeft.Width - textSize.Width) / 2, 
                            (this.pnlLeft.Height - textSize.Height) / 2 - 60);
                    }
                }
                
                // 添加标语
                string slogan = "智能制造·数字化管理";
                using (Font sloganFont = new Font("微软雅黑", 15, FontStyle.Regular))
                {
                    SizeF sloganSize = e.Graphics.MeasureString(slogan, sloganFont);
                    
                    // 标语阴影
                    using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(30, 0, 0, 0)))
                    {
                        e.Graphics.DrawString(slogan, sloganFont, shadowBrush, 
                            (this.pnlLeft.Width - sloganSize.Width) / 2 + 1, 
                            (this.pnlLeft.Height / 2) + 50 + 1);
                    }
                    
                    // 主标语
                    using (SolidBrush textBrush = new SolidBrush(Color.White))
                    {
                        e.Graphics.DrawString(slogan, sloganFont, textBrush, 
                            (this.pnlLeft.Width - sloganSize.Width) / 2, 
                            (this.pnlLeft.Height / 2) + 50);
                    }
                }
                
                // 绘制简约装饰元素
                int centerX = this.pnlLeft.Width / 2;
                int centerY = this.pnlLeft.Height / 2;
                
                // 外圆
                using (Pen pen = new Pen(Color.FromArgb(40, 255, 255, 255), 2))
                {
                    e.Graphics.DrawEllipse(pen, centerX - 100, centerY - 100, 200, 200);
                }
                
                // 点缀小圆点
                using (SolidBrush dotBrush = new SolidBrush(Color.FromArgb(80, 255, 255, 255)))
                {
                    // 四周点缀小圆点
                    e.Graphics.FillEllipse(dotBrush, centerX - 115, centerY, 6, 6);
                    e.Graphics.FillEllipse(dotBrush, centerX + 110, centerY, 6, 6);
                    e.Graphics.FillEllipse(dotBrush, centerX, centerY - 115, 6, 6);
                    e.Graphics.FillEllipse(dotBrush, centerX, centerY + 110, 6, 6);
                }
            };
            
            // 设置登录面板样式
            EnsureGlassLoginPanel();

            // 为登录面板添加柔和的阴影效果
            this.Paint += (s, e) =>
            {
                if (!DesignMode)
                {
                    // 创建更自然的阴影效果
                    for (int i = 1; i <= 10; i++)
                    {
                        int alpha = 15 - i;
                        if (alpha < 0) alpha = 0;
                        
                        using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(alpha, 0, 0, 0)))
                        {
                            Control card = (Control)(glassLoginPanel ?? this.pnlLogin);
                            // 使用GraphicsExtensions类中的方法
                            GraphicsExtensions.FillRoundedRectangle(e.Graphics, shadowBrush,
                                new Rectangle(card.Left + i / 2, card.Top + i / 2,
                                card.Width + i, card.Height + i), 15);
                        }
                    }
                }
            };
            
            // 创建并设置图标
            this.picFactory.Image = CreateIconImage("🏭", 20);
            this.picLanguage.Image = CreateIconImage("🌐", 20);
            this.picUsername.Image = CreateIconImage("👤", 20);
            this.picPassword.Image = CreateIconImage("🔒", 20);
            
            // 设置控件颜色和样式
            this.cboNewLanguage.BackColor = Color.White;
            this.cboNewFactory.BackColor = Color.White;
            
            // 登录按钮样式
            this.btnLogin.BackColor = primaryColor;
            this.btnLogin.ForeColor = Color.White;
            this.btnLogin.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.btnLogin.Width, this.btnLogin.Height, 10, 10));
        }

        private void EnsureGlassLoginPanel()
        {
            if (glassLoginPanel != null) return;
            if (this.pnlLogin == null) return;
            if (this.pnlLogin.Parent == null) return;

            glassLoginPanel = new GlassPanel
            {
                Name = "glassLoginPanel",
                Bounds = this.pnlLogin.Bounds,
                Anchor = this.pnlLogin.Anchor,
                CornerRadius = 18,
                GlassColor = Color.FromArgb(210, 255, 255, 255),
                BorderColor = Color.FromArgb(90, 255, 255, 255),
                BorderThickness = 1f
            };

            Control parent = this.pnlLogin.Parent;
            parent.Controls.Add(glassLoginPanel);
            glassLoginPanel.BringToFront();

            // 将登录控件从旧面板迁移到玻璃面板（不改设计器文件）
            while (this.pnlLogin.Controls.Count > 0)
            {
                Control child = this.pnlLogin.Controls[0];
                this.pnlLogin.Controls.RemoveAt(0);
                glassLoginPanel.Controls.Add(child);
            }

            this.pnlLogin.Visible = false;
        }

        private void EnsureLoginLoadingOverlay()
        {
            EnsureGlassLoginPanel();
            Control container = (Control)(glassLoginPanel ?? this.pnlLogin);
            if (container == null) return;

            if (loginLoadingOverlay != null) return;

            loginLoadingOverlay = new GlassPanel
            {
                Name = "loginLoadingOverlay",
                Dock = DockStyle.Fill,
                CornerRadius = 18,
                GlassColor = Color.FromArgb(200, 245, 245, 250),
                BorderColor = Color.FromArgb(80, 255, 255, 255),
                BorderThickness = 1f,
                Visible = false
            };

            loginLoadingLabel = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 46,
                Font = new Font("微软雅黑", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 70),
                Text = "正在处理…"
            };

            loginLoadingBar = new ProgressBar
            {
                Dock = DockStyle.Top,
                Height = 10,
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 30
            };

            Panel content = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 24, 20, 24),
                BackColor = Color.Transparent
            };

            // 简单的垂直居中布局
            Panel center = new Panel
            {
                Width = Math.Max(260, container.Width - 80),
                Height = 120,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.None
            };
            center.Controls.Add(loginLoadingBar);
            center.Controls.Add(loginLoadingLabel);
            loginLoadingBar.Top = 56;
            loginLoadingLabel.Top = 8;

            content.Controls.Add(center);
            content.Resize += (s, e) =>
            {
                center.Left = Math.Max(0, (content.ClientSize.Width - center.Width) / 2);
                center.Top = Math.Max(0, (content.ClientSize.Height - center.Height) / 2);
            };

            loginLoadingOverlay.Controls.Add(content);
            container.Controls.Add(loginLoadingOverlay);
            loginLoadingOverlay.BringToFront();
        }

        private void SetLoginLoading(bool loading, string message)
        {
            EnsureLoginLoadingOverlay();
            if (loginLoadingOverlay == null) return;

            loginLoadingLabel.Text = string.IsNullOrWhiteSpace(message) ? "正在处理…" : message;
            loginLoadingOverlay.Visible = loading;
            if (loading) loginLoadingOverlay.BringToFront();

            // 只禁用登录卡片里的交互，避免影响窗体拖拽/关闭
            if (glassLoginPanel != null)
            {
                foreach (Control child in glassLoginPanel.Controls)
                {
                    if (child == loginLoadingOverlay) continue;
                    child.Enabled = !loading;
                }
            }
        }
        
        private Image CreateIconImage(string text, int fontSize)
        {
            // 创建更大的图标图像，确保完全显示
            Bitmap bmp = new Bitmap(32, 32);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                
                // 使用更清晰的字体和颜色
                using (Font font = new Font("Segoe UI Emoji", fontSize, FontStyle.Regular))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(72, 52, 212)))
                {
                    // 居中绘制图标
                    SizeF size = g.MeasureString(text, font);
                    float x = (bmp.Width - size.Width) / 2;
                    float y = (bmp.Height - size.Height) / 2;
                    g.DrawString(text, font, brush, x, y);
                }
            }
            return bmp;
        }
        
        private string GetFactoryIdByName(string factoryName)
        {
            string factoryId = "";
            string connectionString = DbConnectionHelper.GetConnectionString();
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT FactoryId FROM Factory WHERE FactoryName = @FactoryName";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FactoryName", factoryName);
                        var result = command.ExecuteScalar();
                        if (result != null)
                        {
                            factoryId = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取工厂ID失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return factoryId; // 确保总是返回值
        }

        private void LoadFactoriesForNewUI()
        {
            string connectionString = DbConnectionHelper.GetConnectionString();
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    string query = "SELECT FactoryName FROM Factory ORDER BY FactoryName";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            this.cboNewFactory.Items.Clear();
                            
                            while (reader.Read())
                            {
                                this.cboNewFactory.Items.Add(reader["FactoryName"].ToString());
                            }
                            
                            if (this.cboNewFactory.Items.Count > 0)
                            {
                                this.cboNewFactory.SelectedIndex = 0;
                            }
                            else
                            {
                                // 如果没有从数据库加载到工厂，添加默认值
                                AddDefaultFactories();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载工厂列表失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddDefaultFactories();
            }
        }
        
        private void AddDefaultFactories()
        {
            // 添加默认工厂项
            this.cboNewFactory.Items.Clear();
            this.cboNewFactory.Items.Add("第一机械厂");
            this.cboNewFactory.Items.Add("第二电子厂");
            this.cboNewFactory.SelectedIndex = 0;
        }

        private void ApplyLoginPreferences()
        {
            try
            {
                loginPreferences = AppPreferencesStore.Load();

                if (!string.IsNullOrWhiteSpace(loginPreferences.LastUsername))
                {
                    txtNewUsername.Text = loginPreferences.LastUsername;
                }

                if (!string.IsNullOrWhiteSpace(loginPreferences.LastLanguage))
                {
                    SelectComboItem(cboNewLanguage, loginPreferences.LastLanguage);
                }

                if (!string.IsNullOrWhiteSpace(loginPreferences.LastFactoryName))
                {
                    SelectComboItem(cboNewFactory, loginPreferences.LastFactoryName);
                }

                chkRemember.Checked = loginPreferences.RememberPassword;

                if (loginPreferences.RememberPassword && !string.IsNullOrWhiteSpace(loginPreferences.ProtectedPassword))
                {
                    string password = AppPreferencesStore.UnprotectString(loginPreferences.ProtectedPassword);
                    if (!string.IsNullOrEmpty(password))
                    {
                        txtNewPassword.Text = password;
                    }
                }
                else
                {
                    // 不记住密码时不覆盖默认体验（首次启动仍显示默认演示账号），但如果用户之前明确取消记住，则清空更符合预期
                    if (loginPreferences.LastUpdatedUtc != default(DateTime))
                    {
                        txtNewPassword.Text = string.Empty;
                    }
                }
            }
            catch
            {
                // 偏好设置读取失败不影响登录主流程
            }
        }

        private void SaveLoginPreferences(string username, string password, string factoryName, string language)
        {
            try
            {
                AppPreferences prefs = loginPreferences ?? new AppPreferences();
                prefs.LastUsername = username;
                prefs.LastFactoryName = factoryName;
                prefs.LastLanguage = language;

                bool rememberPassword = chkRemember.Checked;
                prefs.RememberPassword = rememberPassword;
                prefs.ProtectedPassword = rememberPassword ? AppPreferencesStore.ProtectString(password) : string.Empty;

                AppPreferencesStore.Save(prefs);
                loginPreferences = prefs;
            }
            catch
            {
                // 保存失败不阻塞登录
            }
        }

        private static void SelectComboItem(ComboBox comboBox, string text)
        {
            if (comboBox == null || string.IsNullOrEmpty(text)) return;

            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if (string.Equals(comboBox.Items[i]?.ToString(), text, StringComparison.OrdinalIgnoreCase))
                {
                    comboBox.SelectedIndex = i;
                    return;
                }
            }
        }
        
        // 重命名按钮点击事件，避免使用旧的命名
        private async void LoginButton_Click(object sender, EventArgs e)
        {
            if (loginInProgress) return;

            // 获取用户名和密码
            string username = this.txtNewUsername.Text.Trim();
            string password = this.txtNewPassword.Text.Trim();
            string factoryName = this.cboNewFactory.SelectedItem?.ToString();
            string language = this.cboNewLanguage.SelectedItem?.ToString();
            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("用户名和密码不能为空", "登录错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (string.IsNullOrEmpty(factoryName))
            {
                MessageBox.Show("请选择工厂", "登录错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                loginInProgress = true;
                SetLoginLoading(true, "正在验证账号…");
                this.btnLogin.Enabled = false;

                string factoryId = GetFactoryIdByName(factoryName);
                // 修改为使用Login方法而不是ValidateUser
                currentUser = await Task.Run(() => userBLL.Login(username, password));

                if (currentUser != null)
                {
                    SaveLoginPreferences(username, password, factoryName, language);

                    // 设置用户的工厂ID
                    currentUser.FactoryId = factoryId;
                    
                    // 登录成功的动画效果
                    this.btnLogin.Text = "登录成功";
                    this.btnLogin.BackColor = Color.FromArgb(76, 175, 80);

                    // 延迟显示主窗体，修改为只传递currentUser
                    System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                    timer.Interval = 1000;
                    timer.Tick += (s, args) =>
                    {
                        timer.Stop();
                        MainForm mainForm = new MainForm(currentUser);
                        mainForm.Show();
                        this.Hide();
                    };
                    timer.Start();
                }
                else
                {
                    MessageBox.Show("用户名或密码错误", "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("登录过程中发生错误: " + ex.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetLoginLoading(false, null);
                if (!this.IsDisposed && this.btnLogin != null && this.Visible)
                {
                    // 成功后会 Hide 本窗体，此时不必再恢复按钮
                    if (this.Visible) this.btnLogin.Enabled = true;
                }

                loginInProgress = false;
            }
        }
        
        // 设置版本标签的基本属性
        private void ConfigureVersionLabel()
        {
            // 使用设计器中的版本标签设置
            lblVersion.AutoSize = false;
            lblVersion.Font = new Font("微软雅黑", 9F, FontStyle.Regular);
            lblVersion.ForeColor = Color.Gray;
            string version = "0.0.0";
            try
            {
                version = Application.ProductVersion;
            }
            catch
            {
                // 忽略版本读取失败，使用默认值
            }
            lblVersion.Text = $"版本 v{version} © {DateTime.Now:yyyy} MDMUI";
            lblVersion.BackColor = Color.Transparent;
            lblVersion.TextAlign = ContentAlignment.MiddleRight;
            lblVersion.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            
            // 立即更新位置
            UpdateVersionLabelPosition();
            
            // 监听窗体大小变化
            this.Resize += (sender, e) => UpdateVersionLabelPosition();
        }

        // 更新版本标签位置
        private void UpdateVersionLabelPosition()
        {
            // 确保标签尺寸已计算
            lblVersion.Size = new Size(220, 25);
            
            // 计算位置，确保在右下角且不会被截断
            lblVersion.Location = new Point(
                this.ClientSize.Width - lblVersion.Width - 15,
                this.ClientSize.Height - lblVersion.Height - 10
            );
            
            // 确保可见性
            lblVersion.Visible = true;
            lblVersion.BringToFront();
        }

        private void lblVersion_Click(object sender, EventArgs e)
        {

        }
    }

    // 图形扩展工具类
    public static class GraphicsExtensions
    {
        // 创建圆角矩形路径
        public static GraphicsPath CreateRoundedRectangle(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            // 左上角
            path.AddArc(arc, 180, 90);

            // 右上角
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // 右下角
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // 左下角
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        // 扩展方法：填充圆角矩形
        public static void FillRoundedRectangle(this Graphics graphics, Brush brush, Rectangle bounds, int cornerRadius)
        {
            using (GraphicsPath path = CreateRoundedRectangle(bounds, cornerRadius))
            {
                graphics.FillPath(brush, path);
            }
        }
    }
}
