using System;
using System.Windows.Forms;
using MDMUI.Model;

namespace MDMUI.Forms
{
    public partial class FrmUserInfo : Form
    {
        private readonly User currentUser;

        public FrmUserInfo(User user)
        {
            InitializeComponent();
            this.currentUser = user;
        }

        private void FrmUserInfo_Load(object sender, EventArgs e)
        {
            // 初始化用户信息窗体
        }
    }
} 