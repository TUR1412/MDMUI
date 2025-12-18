using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MDMUI.Controls
{
    /// <summary>
    /// 可拖拽的ListView控件，支持项目的拖拽重排序和多选功能
    /// </summary>
    public class DraggableListView : ListView
    {
        // 拖拽操作相关变量
        private bool isDragging = false;
        private int draggedItemIndex = -1;
        private int dropTargetIndex = -1;
        private ListViewItem draggedItem = null;
        private Rectangle dragRect = Rectangle.Empty;
        
        // 拖拽视觉反馈相关变量
        private bool showDragImage = true;
        private Color dropIndicatorColor = Color.FromArgb(66, 139, 202); // 蓝色
        private int dropIndicatorHeight = 3;
        
        // 顺序变更事件
        public event EventHandler<ListViewItemOrderChangedEventArgs> ItemOrderChanged;

        /// <summary>
        /// 拖拽的视觉反馈颜色
        /// </summary>
        public Color DropIndicatorColor 
        { 
            get { return dropIndicatorColor; } 
            set { dropIndicatorColor = value; } 
        }

        /// <summary>
        /// 是否显示拖拽图像
        /// </summary>
        public bool ShowDragImage 
        { 
            get { return showDragImage; } 
            set { showDragImage = value; } 
        }

        public DraggableListView()
        {
            // 设置控件属性
            this.AllowDrop = true;
            this.FullRowSelect = true;
            this.HideSelection = false;
            this.View = View.Details;
            this.MultiSelect = true;
            this.LabelEdit = false;
            
            // 注册拖拽相关事件
            this.ItemDrag += DraggableListView_ItemDrag;
            this.DragEnter += DraggableListView_DragEnter;
            this.DragOver += DraggableListView_DragOver;
            this.DragDrop += DraggableListView_DragDrop;
            this.DragLeave += DraggableListView_DragLeave;
            
            // 注册绘制事件
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.OwnerDraw = true;
            this.DrawItem += DraggableListView_DrawItem;
            this.DrawColumnHeader += DraggableListView_DrawColumnHeader;
            this.DrawSubItem += DraggableListView_DrawSubItem;
        }

        private void DraggableListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // 开始拖拽
            if (e.Item != null)
            {
                isDragging = true;
                draggedItem = (ListViewItem)e.Item;
                draggedItemIndex = draggedItem.Index;
                
                if (showDragImage)
                {
                    // 创建拖拽图像
                    this.DoDragDrop(e.Item, DragDropEffects.Move);
                }
                else
                {
                    // 不创建拖拽图像，只是用简单指示器
                    this.DoDragDrop(draggedItemIndex, DragDropEffects.Move);
                }
            }
        }

        private void DraggableListView_DragEnter(object sender, DragEventArgs e)
        {
            if (isDragging)
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void DraggableListView_DragOver(object sender, DragEventArgs e)
        {
            if (!isDragging) return;
            
            // 获取鼠标下方的项目
            Point clientPoint = this.PointToClient(new Point(e.X, e.Y));
            ListViewItem itemUnderMouse = this.GetItemAt(clientPoint.X, clientPoint.Y);
            
            if (itemUnderMouse != null)
            {
                dropTargetIndex = itemUnderMouse.Index;
                
                // 计算拖放指示器的位置（项目上方或下方）
                Rectangle itemBounds = itemUnderMouse.Bounds;
                bool dropAbove = clientPoint.Y < itemBounds.Top + (itemBounds.Height / 2);
                
                if (dropAbove)
                {
                    // 拖放到项目上方
                    dragRect = new Rectangle(0, itemBounds.Top, this.Width, dropIndicatorHeight);
                }
                else
                {
                    // 拖放到项目下方
                    dragRect = new Rectangle(0, itemBounds.Bottom - dropIndicatorHeight, this.Width, dropIndicatorHeight);
                }
                
                // 触发重绘以显示拖拽指示器
                this.Invalidate(dragRect);
            }
            else if (this.Items.Count > 0)
            {
                // 如果鼠标不在任何项目上，但列表不为空，则考虑放在最后
                dropTargetIndex = this.Items.Count - 1;
                ListViewItem lastItem = this.Items[dropTargetIndex];
                dragRect = new Rectangle(0, lastItem.Bounds.Bottom, this.Width, dropIndicatorHeight);
                this.Invalidate(dragRect);
            }
        }

        private void DraggableListView_DragDrop(object sender, DragEventArgs e)
        {
            if (!isDragging || draggedItemIndex < 0 || dropTargetIndex < 0) return;
            
            // 重新排序并更新界面
            Point clientPoint = this.PointToClient(new Point(e.X, e.Y));
            ListViewItem targetItem = this.GetItemAt(clientPoint.X, clientPoint.Y);
            
            if (targetItem != null)
            {
                int targetIndex = targetItem.Index;
                
                // 调整放置位置（考虑是放在项目上方还是下方）
                Rectangle itemBounds = targetItem.Bounds;
                bool dropAbove = clientPoint.Y < itemBounds.Top + (itemBounds.Height / 2);
                
                // 确定最终的目标索引
                int finalTargetIndex = dropAbove ? targetIndex : targetIndex + 1;
                
                // 如果是拖放到自己或自己的后面一个位置，不处理
                if (finalTargetIndex == draggedItemIndex || finalTargetIndex == draggedItemIndex + 1)
                {
                    ResetDragState();
                    return;
                }
                
                // 如果目标索引大于拖拽项索引，需要调整（因为移除拖拽项后，索引会变化）
                if (finalTargetIndex > draggedItemIndex)
                {
                    finalTargetIndex--;
                }
                
                // 执行移动操作
                BeginUpdate();
                
                ListViewItem itemToMove = this.Items[draggedItemIndex];
                this.Items.RemoveAt(draggedItemIndex);
                
                // 确保索引在有效范围内
                if (finalTargetIndex >= this.Items.Count)
                {
                    this.Items.Add(itemToMove);
                }
                else
                {
                    this.Items.Insert(finalTargetIndex, itemToMove);
                }
                
                // 选中被移动的项目
                foreach (ListViewItem item in this.Items)
                {
                    item.Selected = false;
                }
                itemToMove.Selected = true;
                itemToMove.Focused = true;
                itemToMove.EnsureVisible();
                
                EndUpdate();
                
                // 触发顺序变更事件
                OnItemOrderChanged(new ListViewItemOrderChangedEventArgs(
                    draggedItemIndex, finalTargetIndex, itemToMove.Tag));
            }
            
            // 重置拖拽状态
            ResetDragState();
        }

        private void DraggableListView_DragLeave(object sender, EventArgs e)
        {
            // 清除拖拽状态和视觉指示
            ResetDragState();
        }

        private void ResetDragState()
        {
            isDragging = false;
            draggedItemIndex = -1;
            dropTargetIndex = -1;
            draggedItem = null;
            
            if (!dragRect.IsEmpty)
            {
                this.Invalidate(dragRect);
                dragRect = Rectangle.Empty;
            }
        }

        protected virtual void OnItemOrderChanged(ListViewItemOrderChangedEventArgs e)
        {
            ItemOrderChanged?.Invoke(this, e);
        }

        private void DraggableListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void DraggableListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void DraggableListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            // 绘制拖拽指示器
            if (isDragging && !dragRect.IsEmpty)
            {
                using (SolidBrush brush = new SolidBrush(dropIndicatorColor))
                {
                    e.Graphics.FillRectangle(brush, dragRect);
                }
            }
        }
    }

    /// <summary>
    /// 列表项顺序变更事件参数类
    /// </summary>
    public class ListViewItemOrderChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 原始索引
        /// </summary>
        public int OldIndex { get; private set; }
        
        /// <summary>
        /// 新索引
        /// </summary>
        public int NewIndex { get; private set; }
        
        /// <summary>
        /// 项目数据（存储在Tag属性中）
        /// </summary>
        public object ItemData { get; private set; }

        public ListViewItemOrderChangedEventArgs(int oldIndex, int newIndex, object itemData)
        {
            OldIndex = oldIndex;
            NewIndex = newIndex;
            ItemData = itemData;
        }
    }
} 