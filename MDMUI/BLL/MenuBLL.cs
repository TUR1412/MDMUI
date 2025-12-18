using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MDMUI.DAL;
using MDMUI.Model;
using MDMUI.Utility;

namespace MDMUI.BLL
{
    public class MenuBLL
    {
        private MenuDAL menuDAL;
        private PermissionChecker permissionChecker;

        public MenuBLL()
        {
            menuDAL = new MenuDAL();
            permissionChecker = new PermissionChecker();
        }

        // 获取用户有权限访问的菜单
        public List<MenuItem> GetUserMenus(int userId, string factoryId = null)
        {
            try
            {
                // 获取用户菜单（考虑权限和工厂）
                List<Menu> menus = menuDAL.GetUserMenus(userId, factoryId);
                return ConvertToMenuItems(menus);
            }
            catch (Exception ex)
            {
                throw new Exception("获取用户菜单失败：" + ex.Message);
            }
        }

        // 获取所有菜单
        public List<MenuItem> GetAllMenus()
        {
            try
            {
                List<Menu> menus = menuDAL.GetAllMenus();
                return ConvertToMenuItems(menus);
            }
            catch (Exception ex)
            {
                throw new Exception("获取所有菜单失败：" + ex.Message);
            }
        }

        // 将菜单列表转换为树形结构的菜单项
        private List<MenuItem> ConvertToMenuItems(List<Menu> menus)
        {
            List<MenuItem> result = new List<MenuItem>();
            Dictionary<int, MenuItem> menuDict = new Dictionary<int, MenuItem>();

            // 第一步：创建所有MenuItem对象
            foreach (Menu menu in menus)
            {
                MenuItem item = new MenuItem
                {
                    MenuId = menu.MenuId,
                    MenuName = menu.MenuName,
                    ParentMenuId = menu.ParentMenuId,
                    MenuOrder = menu.MenuOrder,
                    MenuIcon = menu.MenuIcon,
                    Children = new List<MenuItem>()
                };

                menuDict.Add(menu.MenuId, item);
            }

            // 第二步：建立父子关系
            foreach (Menu menu in menus)
            {
                if (menu.ParentMenuId.HasValue && menuDict.ContainsKey(menu.ParentMenuId.Value))
                {
                    MenuItem parentItem = menuDict[menu.ParentMenuId.Value];
                    MenuItem currentItem = menuDict[menu.MenuId];
                    parentItem.Children.Add(currentItem);
                }
                else if (!menu.ParentMenuId.HasValue)
                {
                    // 顶级菜单
                    result.Add(menuDict[menu.MenuId]);
                }
            }

            // 对菜单项进行排序
            result.Sort((a, b) => a.MenuOrder.CompareTo(b.MenuOrder));
            foreach (MenuItem item in result)
            {
                SortMenuItems(item);
            }

            return result;
        }

        // 递归对菜单项进行排序
        private void SortMenuItems(MenuItem item)
        {
            if (item.Children.Count > 0)
            {
                item.Children.Sort((a, b) => a.MenuOrder.CompareTo(b.MenuOrder));
                foreach (MenuItem child in item.Children)
                {
                    SortMenuItems(child);
                }
            }
        }

        // 检查用户是否有权限访问指定模块
        public bool HasPermission(User user, string moduleName, string actionName)
        {
            if (user == null)
            {
                return false;
            }

            // 超级管理员拥有所有权限
            if (user.RoleName == "超级管理员")
            {
                return true;
            }

            return permissionChecker.HasPermission(user.Id, moduleName, actionName);
        }
    }

    // 菜单项模型
    public class MenuItem
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public int? ParentMenuId { get; set; }
        public int MenuOrder { get; set; }
        public string MenuIcon { get; set; }
        public List<MenuItem> Children { get; set; } = new List<MenuItem>();
    }
}