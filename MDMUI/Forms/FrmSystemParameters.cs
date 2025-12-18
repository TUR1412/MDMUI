using System;
using System.Drawing;
using System.Windows.Forms;
using MDMUI.Model;

namespace MDMUI
{
    // Note: This class is now partial to allow separation of Designer code
    public partial class FrmSystemParameters : Form 
    {
        // Keep CurrentUser as a field accessible by the form logic
        private User CurrentUser; 

        public FrmSystemParameters(User user)
        {
            InitializeComponent(); // Keep this call
            CurrentUser = user;
            // Remove call to InitializePlaceholderContent - logic moved to Designer
            // InitializePlaceholderContent();
            // this.Text is set in Designer
            // this.Text = "系统参数设置";
        }

        private void FrmSystemParameters_Load(object sender, EventArgs e)
        {
            // Update user label text after controls are initialized by the designer
            if (this.userLabel != null) 
            {
                 this.userLabel.Text = $"当前用户: {CurrentUser?.Username ?? "未知用户"}";
            }
            Console.WriteLine("系统参数设置窗体已加载");
        }
    }
} 