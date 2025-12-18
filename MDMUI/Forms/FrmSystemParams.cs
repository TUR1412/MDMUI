using System;
using System.Windows.Forms;
using MDMUI.Model;

namespace MDMUI.Forms
{
    public partial class FrmSystemParams : Form
    {
        private readonly User currentUser;

        public FrmSystemParams(User user)
        {
            InitializeComponent();
            this.currentUser = user;
        }

        private void FrmSystemParams_Load(object sender, EventArgs e)
        {
            // 初始化系统参数窗体
        }
    }
} 