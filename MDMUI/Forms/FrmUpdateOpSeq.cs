using MDMUI.BLL;
using MDMUI.Controls;
using MDMUI.Model;
using MDMUI.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace MDMUI
{
    public partial class FrmUpdateOpSeq : Form
    {
        private string selectedFid; // 选中的工艺流程ID
        private ProcessRouteBLL routeBLL;
        private DataTable leftTable; // 左侧数据表（不在当前工艺流程中的工艺站）
        private DataTable rightTable; // 右侧数据表（当前工艺流程中的工艺站）
        
        // 拖拽控件
        private DraggableListView lstRightOpers;

        public FrmUpdateOpSeq(string fid)
        {
            InitializeComponent();
            selectedFid = fid;
            routeBLL = new ProcessRouteBLL();
            
            // 初始化拖拽控件
            InitializeDraggableList();
            
            // 绑定窗体加载事件
            this.Load += FrmUpdateOpSeq_Load;
            
            // 绑定按钮事件
            btnAdd.Click += BtnAdd_Click;
            btnRemove.Click += BtnRemove_Click;
            btnUp.Click += BtnUp_Click;
            btnDown.Click += BtnDown_Click;
            btnClose.Click += BtnClose_Click;
            
            // 绑定新增按钮事件
            btnAutoSort.Click += BtnAutoSort_Click;
            btnBatchAdd.Click += BtnBatchAdd_Click;
            btnBatchRemove.Click += BtnBatchRemove_Click;
            chkEnableDrag.CheckedChanged += ChkEnableDrag_CheckedChanged;
        }
        
        private void InitializeDraggableList()
        {
            // 创建拖拽控件
            lstRightOpers = new DraggableListView();
            lstRightOpers.Dock = DockStyle.Fill;
            lstRightOpers.View = View.Details;
            lstRightOpers.FullRowSelect = true;
            lstRightOpers.GridLines = true;
            lstRightOpers.HideSelection = false;
            lstRightOpers.MultiSelect = true;
            lstRightOpers.BackColor = Color.White;
            lstRightOpers.Font = new Font("Microsoft YaHei UI", 9F);
            
            // 添加列
            lstRightOpers.Columns.Add("StationId", "工站号", 100);
            lstRightOpers.Columns.Add("Version", "版本", 80);
            lstRightOpers.Columns.Add("Description", "工站说明", 200);
            lstRightOpers.Columns.Add("Sequence", "顺序", 80);
            lstRightOpers.Columns.Add("StationType", "工站类型", 120);
            lstRightOpers.Columns.Add("RouteId", "路由ID", 0); // 隐藏列
            
            // 绑定事件
            lstRightOpers.ItemOrderChanged += LstRightOpers_ItemOrderChanged;
            
            // 添加到界面
            splitContainer.Panel2.Controls.Add(lstRightOpers);
            
            // 默认不显示，等加载完数据再显示
            lstRightOpers.Visible = false;
        }

        private void FrmUpdateOpSeq_Load(object sender, EventArgs e)
        {
            // 设置窗体标题
            this.Text = "修改工艺路线";
            
            // 设置标题标签
            lblTitle.Text = "工艺路线顺序调整";
            
            // 初始化界面
            InitializeUI();
            
            // 加载数据
            LoadData();
        }

        private void InitializeUI()
        {
            // 设置窗体整体样式
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular);
            this.Icon = null; // 移除默认图标
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // 固定窗体大小
            this.MaximizeBox = false; // 禁用最大化按钮
            
            // 设置分割面板样式
            splitContainer.BackColor = Color.FromArgb(230, 230, 245);
            splitContainer.Panel1.BackColor = Color.White;
            splitContainer.Panel2.BackColor = Color.White;
            splitContainer.SplitterWidth = 5;
            splitContainer.SplitterDistance = splitContainer.Width / 2 - 10;
            
            // 设置标签样式
            lblLeftCount.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            lblLeftCount.ForeColor = Color.FromArgb(80, 80, 100);
            lblLeftCount.BackColor = Color.FromArgb(240, 240, 250);
            lblLeftCount.Padding = new Padding(10, 5, 10, 5);
            lblLeftCount.Dock = DockStyle.Top;
            lblLeftCount.TextAlign = ContentAlignment.MiddleLeft;
            
            lblRightCount.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            lblRightCount.ForeColor = Color.FromArgb(80, 80, 100);
            lblRightCount.BackColor = Color.FromArgb(240, 240, 250);
            lblRightCount.Padding = new Padding(10, 5, 10, 5);
            lblRightCount.Dock = DockStyle.Top;
            lblRightCount.TextAlign = ContentAlignment.MiddleLeft;
            
            // 设置按钮面板样式
            panelButtons.BackColor = Color.FromArgb(240, 240, 250);
            panelButtons.Height = 60;
            
            // 美化按钮
            StyleButton(btnAdd, Color.FromArgb(92, 184, 92)); // 绿色按钮
            StyleButton(btnRemove, Color.FromArgb(217, 83, 79)); // 红色按钮
            StyleButton(btnUp, Color.FromArgb(66, 139, 202)); // 蓝色按钮
            StyleButton(btnDown, Color.FromArgb(66, 139, 202)); // 蓝色按钮
            StyleButton(btnClose, Color.FromArgb(153, 153, 153)); // 灰色按钮
            
            // 添加按钮悬停效果
            AddButtonHoverEffects();
            
            // 调整按钮位置
            btnAdd.Left = 20;
            btnBatchAdd.Left = btnAdd.Right + 20;
            btnRemove.Left = btnBatchAdd.Right + 20;
            btnBatchRemove.Left = btnRemove.Right + 20;
            btnUp.Left = btnBatchRemove.Right + 20;
            btnDown.Left = btnUp.Right + 20;
            btnAutoSort.Left = btnDown.Right + 20;
            chkEnableDrag.Left = btnAutoSort.Right + 20;
            btnClose.Left = panelButtons.Width - btnClose.Width - 20;
            
            // 设置数据网格样式
            ConfigureDataGridView(dgvLeftOpers);
            ConfigureDataGridView(dgvRightOpers);
            
            // 隐藏原始的右侧数据网格
            dgvRightOpers.Visible = false;
        }

        private void LoadData()
        {
            try
            {
                // 加载左侧数据（不在当前工艺流程中的工艺站）
                leftTable = routeBLL.GetNonOperListByFid(selectedFid);
                dgvLeftOpers.DataSource = leftTable;
                
                // 加载右侧数据（当前工艺流程中的工艺站）
                rightTable = routeBLL.GetRoutesByProcessId(selectedFid);
                
                // 配置列
                ConfigureLeftGridColumns();
                
                // 填充拖拽列表
                FillDraggableList();
                
                // 更新标签显示
                lblLeftCount.Text = $"不在当前工艺流程中的工艺站 ({leftTable.Rows.Count})";
                lblRightCount.Text = $"当前工艺流程中的工艺站 ({rightTable.Rows.Count})";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void FillDraggableList()
        {
            // 清空列表
            lstRightOpers.Items.Clear();
            
            // 添加项目
            foreach (DataRow row in rightTable.Rows)
            {
                ListViewItem item = new ListViewItem(row["StationId"].ToString());
                item.SubItems.Add(row["Version"].ToString());
                item.SubItems.Add(row["Description"].ToString());
                item.SubItems.Add(row["Sequence"].ToString());
                item.SubItems.Add(row["StationType"].ToString());
                item.SubItems.Add(row["RouteId"].ToString());
                
                // 在Tag中存储RouteId，用于后续更新
                item.Tag = row["RouteId"];
                
                lstRightOpers.Items.Add(item);
            }
            
            // 调整列宽
            lstRightOpers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            lstRightOpers.Columns["RouteId"].Width = 0;
            
            // 显示拖拽列表
            lstRightOpers.Visible = true;
        }

        private void ConfigureLeftGridColumns()
        {
            // 清除现有列
            if (dgvLeftOpers.Columns.Count > 0) dgvLeftOpers.Columns.Clear();
            dgvLeftOpers.AutoGenerateColumns = false;

            // 添加列
            dgvLeftOpers.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", DataPropertyName = "id", Width = 50, Visible = false });
            dgvLeftOpers.Columns.Add(new DataGridViewTextBoxColumn { Name = "OperId", HeaderText = "工站号", DataPropertyName = "oper_id", Width = 100 });
            dgvLeftOpers.Columns.Add(new DataGridViewTextBoxColumn { Name = "OperVersion", HeaderText = "工站版本", DataPropertyName = "oper_version", Width = 80 });
            dgvLeftOpers.Columns.Add(new DataGridViewTextBoxColumn { Name = "OperDescription", HeaderText = "工站说明", DataPropertyName = "oper_description", Width = 200 });
            dgvLeftOpers.Columns.Add(new DataGridViewTextBoxColumn { Name = "OperType", HeaderText = "工站类型", DataPropertyName = "oper_type", Width = 100 });
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否选中左侧行
                if (dgvLeftOpers.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请先选择要添加的工艺站", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 获取选中行
                DataRowView selectedRow = dgvLeftOpers.SelectedRows[0].DataBoundItem as DataRowView;
                if (selectedRow == null) return;

                string operId = selectedRow["oper_id"].ToString();

                // 确定新工站的序号
                int sequence = 1;
                if (rightTable.Rows.Count > 0)
                {
                    // 找到最大序号并加1
                    sequence = rightTable.AsEnumerable()
                                        .Select(r => Convert.ToInt32(r["Sequence"]))
                                        .Max() + 1;
                }

                // 将工站添加到工艺流程
                bool success = routeBLL.AddOperToProcess(selectedFid, operId, sequence);
                if (success)
                {
                    // 重新加载数据
                    LoadData();
                }
                else
                {
                    MessageBox.Show("添加工艺站失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"添加工艺站时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnBatchAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否选中左侧行
                if (dgvLeftOpers.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请选择要批量添加的工艺站", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 确认是否批量添加
                DialogResult result = MessageBox.Show(
                    $"确定要批量添加 {dgvLeftOpers.SelectedRows.Count} 个工艺站到工艺流程吗？",
                    "确认批量操作",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    int successCount = 0;
                    int failCount = 0;
                    
                    // 确定起始序号
                    int sequence = 1;
                    if (rightTable.Rows.Count > 0)
                    {
                        // 找到最大序号并加1
                        sequence = rightTable.AsEnumerable()
                                            .Select(r => Convert.ToInt32(r["Sequence"]))
                                            .Max() + 1;
                    }

                    // 批量添加
                    foreach (DataGridViewRow row in dgvLeftOpers.SelectedRows)
                    {
                        DataRowView selectedRow = row.DataBoundItem as DataRowView;
                        if (selectedRow == null) continue;

                        string operId = selectedRow["oper_id"].ToString();

                        // 将工站添加到工艺流程
                        bool success = routeBLL.AddOperToProcess(selectedFid, operId, sequence++);
                        if (success)
                        {
                            successCount++;
                        }
                        else
                        {
                            failCount++;
                        }
                    }

                    // 显示结果
                    if (failCount == 0)
                    {
                        MessageBox.Show($"成功批量添加 {successCount} 个工艺站", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"批量添加结果：\n成功：{successCount} 个\n失败：{failCount} 个", "操作部分成功", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    // 重新加载数据
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"批量添加工艺站时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否选中右侧行（从拖拽列表中获取选择）
                if (lstRightOpers.SelectedItems.Count == 0)
                {
                    MessageBox.Show("请先选择要移除的工艺站", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 获取选中的工艺路线ID
                ListViewItem selectedItem = lstRightOpers.SelectedItems[0];
                string routeId = selectedItem.SubItems[5].Text; // RouteId在第6列

                // 确认是否移除
                DialogResult result = MessageBox.Show("确定要移除选中的工艺站吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // 从工艺流程中移除工艺站
                    bool success = routeBLL.RemoveOperFromProcess(routeId);
                    if (success)
                    {
                        // 重新加载数据
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("移除工艺站失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"移除工艺站时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnBatchRemove_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否选中右侧行
                if (lstRightOpers.SelectedItems.Count == 0)
                {
                    MessageBox.Show("请选择要批量移除的工艺站", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 确认是否批量移除
                DialogResult result = MessageBox.Show(
                    $"确定要批量移除 {lstRightOpers.SelectedItems.Count} 个工艺站吗？",
                    "确认批量操作",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    int successCount = 0;
                    int failCount = 0;
                    
                    // 收集所有要删除的RouteId
                    List<string> routeIds = new List<string>();
                    foreach (ListViewItem item in lstRightOpers.SelectedItems)
                    {
                        routeIds.Add(item.SubItems[5].Text); // RouteId在第6列
                    }

                    // 批量移除
                    foreach (string routeId in routeIds)
                    {
                        bool success = routeBLL.RemoveOperFromProcess(routeId);
                        if (success)
                        {
                            successCount++;
                        }
                        else
                        {
                            failCount++;
                        }
                    }

                    // 显示结果
                    if (failCount == 0)
                    {
                        MessageBox.Show($"成功批量移除 {successCount} 个工艺站", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"批量移除结果：\n成功：{successCount} 个\n失败：{failCount} 个", "操作部分成功", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    // 重新加载数据
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"批量移除工艺站时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否选中右侧行
                if (lstRightOpers.SelectedItems.Count == 0)
                {
                    MessageBox.Show("请先选择要上移的工艺站", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 获取选中行的索引
                int selectedIndex = lstRightOpers.SelectedIndices[0];
                
                // 检查是否已经是第一行
                if (selectedIndex == 0)
                {
                    MessageBox.Show("已经是第一个工艺站，无法上移", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 获取选中的项和上一项
                ListViewItem selectedItem = lstRightOpers.Items[selectedIndex];
                ListViewItem prevItem = lstRightOpers.Items[selectedIndex - 1];
                
                // 获取RouteId和序号
                string routeId = selectedItem.SubItems[5].Text;
                string prevRouteId = prevItem.SubItems[5].Text;
                
                int currSeq = int.Parse(selectedItem.SubItems[3].Text);
                int prevSeq = int.Parse(prevItem.SubItems[3].Text);

                // 更新工艺站顺序（互换序号）
                bool success1 = routeBLL.UpdateRouteSequence(routeId, prevSeq);
                bool success2 = routeBLL.UpdateRouteSequence(prevRouteId, currSeq);

                if (success1 && success2)
                {
                    // 更新UI而不重新加载数据
                    selectedItem.SubItems[3].Text = prevSeq.ToString();
                    prevItem.SubItems[3].Text = currSeq.ToString();
                    
                    // 交换位置
                    lstRightOpers.Items.RemoveAt(selectedIndex);
                    lstRightOpers.Items.Insert(selectedIndex - 1, selectedItem);
                    lstRightOpers.Items[selectedIndex - 1].Selected = true;
                    lstRightOpers.Focus();
                }
                else
                {
                    MessageBox.Show("上移工艺站失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // 重新加载数据以确保UI与数据库同步
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"上移工艺站时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // 重新加载数据以确保UI与数据库同步
                LoadData();
            }
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否选中右侧行
                if (lstRightOpers.SelectedItems.Count == 0)
                {
                    MessageBox.Show("请先选择要下移的工艺站", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 获取选中行的索引
                int selectedIndex = lstRightOpers.SelectedIndices[0];
                
                // 检查是否已经是最后一行
                if (selectedIndex >= lstRightOpers.Items.Count - 1)
                {
                    MessageBox.Show("已经是最后一个工艺站，无法下移", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 获取选中的项和下一项
                ListViewItem selectedItem = lstRightOpers.Items[selectedIndex];
                ListViewItem nextItem = lstRightOpers.Items[selectedIndex + 1];
                
                // 获取RouteId和序号
                string routeId = selectedItem.SubItems[5].Text;
                string nextRouteId = nextItem.SubItems[5].Text;
                
                int currSeq = int.Parse(selectedItem.SubItems[3].Text);
                int nextSeq = int.Parse(nextItem.SubItems[3].Text);

                // 更新工艺站顺序（互换序号）
                bool success1 = routeBLL.UpdateRouteSequence(routeId, nextSeq);
                bool success2 = routeBLL.UpdateRouteSequence(nextRouteId, currSeq);

                if (success1 && success2)
                {
                    // 更新UI而不重新加载数据
                    selectedItem.SubItems[3].Text = nextSeq.ToString();
                    nextItem.SubItems[3].Text = currSeq.ToString();
                    
                    // 交换位置
                    lstRightOpers.Items.RemoveAt(selectedIndex);
                    lstRightOpers.Items.Insert(selectedIndex + 1, selectedItem);
                    lstRightOpers.Items[selectedIndex + 1].Selected = true;
                    lstRightOpers.Focus();
                }
                else
                {
                    MessageBox.Show("下移工艺站失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // 重新加载数据以确保UI与数据库同步
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"下移工艺站时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // 重新加载数据以确保UI与数据库同步
                LoadData();
            }
        }
        
        private void BtnAutoSort_Click(object sender, EventArgs e)
        {
            try
            {
                // 首先验证当前顺序是否有问题
                SequenceValidationResult validationResult = routeBLL.ValidateRouteSequence(selectedFid);
                
                if (!validationResult.HasIssues)
                {
                    DialogResult prompt = MessageBox.Show(
                        "当前工艺流程顺序没有检测到明显问题。是否仍要执行自动排序？",
                        "确认排序",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );
                    
                    if (prompt == DialogResult.No)
                    {
                        return;
                    }
                }
                else
                {
                    // 显示检测到的问题
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("检测到以下顺序问题:");
                    
                    foreach (var issue in validationResult.Issues)
                    {
                        sb.AppendLine($"• {issue.Description}");
                    }
                    
                    sb.AppendLine("\n是否执行自动排序来解决这些问题？");
                    
                    DialogResult prompt = MessageBox.Show(
                        sb.ToString(),
                        "确认自动排序",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );
                    
                    if (prompt == DialogResult.No)
                    {
                        return;
                    }
                }
                
                // 执行自动排序
                bool success = routeBLL.OptimizeRouteSequence(selectedFid);
                
                if (success)
                {
                    MessageBox.Show("工艺流程顺序已自动优化", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData(); // 重新加载数据
                }
                else
                {
                    MessageBox.Show("自动优化工艺流程顺序失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"自动排序时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void ChkEnableDrag_CheckedChanged(object sender, EventArgs e)
        {
            // 启用或禁用拖拽功能
            lstRightOpers.AllowDrop = chkEnableDrag.Checked;
        }
        
        private void LstRightOpers_ItemOrderChanged(object sender, ListViewItemOrderChangedEventArgs e)
        {
            try
            {
                // 获取移动的项
                ListViewItem movedItem = lstRightOpers.Items[e.NewIndex];
                string routeId = movedItem.SubItems[5].Text;
                
                // 更新所有项的序号
                Dictionary<string, int> updates = new Dictionary<string, int>();
                
                for (int i = 0; i < lstRightOpers.Items.Count; i++)
                {
                    string itemRouteId = lstRightOpers.Items[i].SubItems[5].Text;
                    int newSequence = i + 1;
                    
                    // 更新显示的序号
                    lstRightOpers.Items[i].SubItems[3].Text = newSequence.ToString();
                    
                    // 添加到更新字典
                    updates[itemRouteId] = newSequence;
                }
                
                // 批量更新数据库
                bool success = routeBLL.BatchUpdateRouteSequence(updates);
                
                if (!success)
                {
                    MessageBox.Show("更新工艺站顺序失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // 重新加载数据以确保UI与数据库同步
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新工艺站顺序时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // 重新加载数据以确保UI与数据库同步
                LoadData();
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // 新增的按钮样式方法
        private void StyleButton(Button btn, Color baseColor)
        {
            btn.BackColor = baseColor;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            btn.Height = 36;
            btn.Width = 120;
            btn.Cursor = Cursors.Hand;
            
            // 添加圆角效果 (通过按钮的Region属性)
            int radius = 5;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            btn.Region = new Region(path);
        }

        private void AddButtonHoverEffects()
        {
            // 常规按钮悬停效果
            AddButtonHoverEffect(btnAdd, Color.FromArgb(92, 184, 92), Color.FromArgb(76, 156, 76));
            AddButtonHoverEffect(btnRemove, Color.FromArgb(217, 83, 79), Color.FromArgb(187, 72, 68));
            AddButtonHoverEffect(btnUp, Color.FromArgb(66, 139, 202), Color.FromArgb(51, 119, 182));
            AddButtonHoverEffect(btnDown, Color.FromArgb(66, 139, 202), Color.FromArgb(51, 119, 182));
            AddButtonHoverEffect(btnClose, Color.FromArgb(153, 153, 153), Color.FromArgb(133, 133, 133));
            
            // 新增按钮悬停效果
            AddButtonHoverEffect(btnAutoSort, Color.FromArgb(0, 150, 136), Color.FromArgb(0, 130, 116));
            AddButtonHoverEffect(btnBatchAdd, Color.FromArgb(156, 39, 176), Color.FromArgb(136, 34, 156));
            AddButtonHoverEffect(btnBatchRemove, Color.FromArgb(255, 87, 34), Color.FromArgb(230, 77, 29));
        }

        private void AddButtonHoverEffect(Button btn, Color normalColor, Color hoverColor)
        {
            btn.FlatAppearance.MouseOverBackColor = hoverColor;
            btn.FlatAppearance.MouseDownBackColor = normalColor;
            
            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = hoverColor;
            };
            
            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = normalColor;
            };
        }

        // 数据网格格式化方法保持不变
        private void ConfigureDataGridView(DataGridView dgv)
        {
            // 设置基本属性
            dgv.BorderStyle = BorderStyle.None;
            dgv.BackgroundColor = Color.White;
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowTemplate.Height = 30;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.GridColor = Color.FromArgb(230, 230, 235);
            
            // 设置单元格样式
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(66, 139, 202);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Font = new Font("Microsoft YaHei UI", 9F);
            dgv.DefaultCellStyle.Padding = new Padding(5);
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            
            // 设置表头样式
            dgv.ColumnHeadersHeight = 40;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 120, 180);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(5);
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            
            // 设置交替行颜色
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 252);
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            
            // 添加行高亮效果
            dgv.CellMouseEnter += (s, e) => {
                if (e.RowIndex >= 0 && !dgv.Rows[e.RowIndex].Selected) {
                    dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(240, 245, 250);
                }
            };
            dgv.CellMouseLeave += (s, e) => {
                if (e.RowIndex >= 0 && !dgv.Rows[e.RowIndex].Selected) {
                    dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = e.RowIndex % 2 == 0 ? 
                        Color.White : Color.FromArgb(248, 248, 252);
                }
            };
        }
    }
} 