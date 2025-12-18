using System;
using System.Collections.Generic;

namespace MDMUI.Model
{
    public class Menu
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public int? ParentMenuId { get; set; }
        public int MenuOrder { get; set; }
        public string MenuIcon { get; set; }
        public List<Menu> Children { get; set; } = new List<Menu>();

        // 导航属性 - 父菜单
        public Menu ParentMenu { get; set; }

        // 导航属性 - 子菜单
        public List<Menu> ChildMenus { get; set; } = new List<Menu>();

        // 与此菜单关联的权限
        public List<Permission> Permissions { get; set; } = new List<Permission>();

        public override string ToString()
        {
            return MenuName;
        }
    }
}