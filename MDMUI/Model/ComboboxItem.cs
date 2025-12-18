using System;

namespace MDMUI.Model
{
    /// <summary>
    /// 下拉框项辅助类，用于ComboBox项的值与显示文本分离
    /// </summary>
    public class ComboboxItem
    {
        /// <summary>
        /// 显示的文本
        /// </summary>
        public string Text { get; set; }
        
        /// <summary>
        /// 实际的值
        /// </summary>
        public object Value { get; set; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text">显示的文本</param>
        /// <param name="value">实际的值</param>
        public ComboboxItem(string text, object value)
        {
            Text = text;
            Value = value;
        }
        
        /// <summary>
        /// 重写ToString方法，使ComboBox显示Text而不是类名
        /// </summary>
        public override string ToString()
        {
            return Text;
        }
    }
} 