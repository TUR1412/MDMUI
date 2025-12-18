using System;
using System.Windows.Forms;
using MDMUI.Model;

namespace MDMUI.Forms
{
    public partial class FrmLog : Form
    {
        private readonly User currentUser;

        public FrmLog(User user)
        {
            InitializeComponent();
            this.currentUser = user;
        }

        private void FrmLog_Load(object sender, EventArgs e)
        {
            // 初始化日志窗体
        }
    }
} 