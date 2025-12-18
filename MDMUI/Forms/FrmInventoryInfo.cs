using System;
using System.Drawing;
using System.Windows.Forms;
using MDMUI.Model;

namespace MDMUI
{
    public partial class FrmInventoryInfo : Form
    {
        private User CurrentUser;

        public FrmInventoryInfo(User user)
        {
            InitializeComponent();
            CurrentUser = user;
        }

        private void FrmInventoryInfo_Load(object sender, EventArgs e)
        {
            if (this.userLabel != null)
            {
                this.userLabel.Text = $"当前用户: {CurrentUser?.Username ?? "未知用户"}";
            }
            Console.WriteLine("库存信息窗体已加载");
        }
    }
} 