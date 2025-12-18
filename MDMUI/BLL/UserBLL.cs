using System;
using MDMUI.DAL;
using MDMUI.Model;

namespace MDMUI.BLL
{
    public class UserBLL
    {
        private UserDAL userDAL;

        public UserBLL()
        {
            userDAL = new UserDAL();
        }

        // 用户登录
        public User Login(string username, string password)
        {
            // 验证参数
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("用户名和密码不能为空！");
            }

            // 验证用户
            User user = userDAL.ValidateUser(username, password);

            if (user != null)
            {
                // 更新最后登录时间
                userDAL.UpdateLastLoginTime(user.Id);

                // 重新获取用户信息（包含更新后的登录时间）
                user = userDAL.GetUserById(user.Id);
            }

            return user;
        }

        // 获取用户
        public User GetUserById(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("无效的用户ID");
            }

            return userDAL.GetUserById(userId);
        }
    }
}