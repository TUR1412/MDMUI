using System;
using System.Drawing;
using System.Windows.Forms;
using MDMUI.Model;

namespace MDMUI
{
    public partial class FrmDataBackup : Form
    {
        private User CurrentUser;

        public FrmDataBackup(User user)
        {
            InitializeComponent();
            CurrentUser = user;
        }

        private void FrmDataBackup_Load(object sender, EventArgs e)
        {
            if (this.userLabel != null)
            {
                this.userLabel.Text = $"当前用户: {CurrentUser?.Username ?? "未知用户"}";
            }
            Console.WriteLine("数据备份窗体已加载");
        }
    }
} 