using System;
using MDMUI.DAL;
using MDMUI.Model;
using MDMUI.Utility;

namespace MDMUI.BLL
{
    public class UserBLL
    {
        private UserDAL userDAL;
        private UserSecurityService securityService;

        public UserBLL()
        {
            userDAL = new UserDAL();
            securityService = new UserSecurityService();
        }

        // 用户登录
        public User Login(string username, string password)
        {
            LoginResult result = TryLogin(username, password);
            return result?.User;
        }

        public LoginResult TryLogin(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("用户名和密码不能为空！");
            }

            LoginResult result = new LoginResult { Success = false };

            User candidate = userDAL.GetUserByUsername(username);
            if (candidate == null)
            {
                result.FailureReason = "用户名或密码错误";
                AuditTrail.Log(null, "LoginFailed", "Auth", "用户不存在或密码错误");
                return result;
            }

            if (securityService.IsLockedOut(candidate.Id, out DateTime? lockoutUntil))
            {
                result.LockoutUntil = lockoutUntil;
                result.FailureReason = "账号已锁定";
                AuditTrail.Log(candidate, "LoginBlocked", "Auth", $"账号锁定至 {lockoutUntil:yyyy-MM-dd HH:mm}");
                return result;
            }

            User user = userDAL.ValidateUser(username, password);
            if (user != null)
            {
                userDAL.UpdateLastLoginTime(user.Id);
                securityService.RecordSuccess(user.Id);
                user = userDAL.GetUserById(user.Id);

                result.Success = true;
                result.User = user;
                AuditTrail.Log(user, "Login", "Auth", "登录成功");
                return result;
            }

            UserSecurityStatus status = securityService.RecordFailure(candidate.Id);
            result.RemainingAttempts = securityService.GetRemainingAttempts(status);
            if (status?.LockoutUntil != null && status.LockoutUntil > DateTime.Now)
            {
                result.LockoutUntil = status.LockoutUntil;
                result.FailureReason = "账号已锁定";
                AuditTrail.Log(candidate, "LoginLocked", "Auth", $"登录失败触发锁定至 {status.LockoutUntil:yyyy-MM-dd HH:mm}");
            }
            else
            {
                result.FailureReason = "用户名或密码错误";
                AuditTrail.Log(candidate, "LoginFailed", "Auth", $"登录失败，剩余尝试 {result.RemainingAttempts}");
            }

            return result;
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
