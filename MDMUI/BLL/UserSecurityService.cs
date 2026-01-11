using System;
using System.Linq;
using MDMUI.DAL;
using MDMUI.Model;

namespace MDMUI.BLL
{
    public class UserSecurityService
    {
        private readonly UserSecurityDAL securityDal = new UserSecurityDAL();
        private readonly SystemParameterService parameterService = new SystemParameterService();

        public int MaxFailedAttempts => Math.Max(1, parameterService.GetInt("Security.MaxFailedLogin", 5));
        public TimeSpan LockoutDuration => parameterService.GetTimeSpanMinutes("Security.LockoutMinutes", 15);

        public UserSecurityStatus GetStatus(int userId)
        {
            return securityDal.GetStatus(userId);
        }

        public bool IsLockedOut(int userId, out DateTime? lockoutUntil)
        {
            lockoutUntil = null;
            if (userId <= 0) return false;

            UserSecurityStatus status = securityDal.GetStatus(userId);
            if (status == null || status.LockoutUntil == null) return false;

            if (status.LockoutUntil.Value > DateTime.Now)
            {
                lockoutUntil = status.LockoutUntil;
                return true;
            }

            return false;
        }

        public UserSecurityStatus RecordFailure(int userId)
        {
            if (userId <= 0) return null;
            return securityDal.RecordFailure(userId, MaxFailedAttempts, LockoutDuration);
        }

        public UserSecurityStatus RecordSuccess(int userId)
        {
            if (userId <= 0) return null;
            return securityDal.RecordSuccess(userId);
        }

        public int GetRemainingAttempts(UserSecurityStatus status)
        {
            if (status == null) return MaxFailedAttempts;
            int remaining = MaxFailedAttempts - status.FailedCount;
            return Math.Max(0, remaining);
        }

        public bool ValidatePassword(string password, out string reason)
        {
            reason = null;

            if (string.IsNullOrWhiteSpace(password))
            {
                reason = "密码不能为空";
                return false;
            }

            int minLength = Math.Max(6, parameterService.GetInt("Security.PasswordMinLength", 8));
            bool requireNumber = parameterService.GetBool("Security.PasswordRequireNumber", true);
            bool requireUpper = parameterService.GetBool("Security.PasswordRequireUpper", false);
            bool requireLower = parameterService.GetBool("Security.PasswordRequireLower", false);
            bool requireSpecial = parameterService.GetBool("Security.PasswordRequireSpecial", false);

            if (password.Length < minLength)
            {
                reason = $"密码长度至少 {minLength} 位";
                return false;
            }

            if (requireNumber && !password.Any(char.IsDigit))
            {
                reason = "密码需包含数字";
                return false;
            }

            if (requireUpper && !password.Any(char.IsUpper))
            {
                reason = "密码需包含大写字母";
                return false;
            }

            if (requireLower && !password.Any(char.IsLower))
            {
                reason = "密码需包含小写字母";
                return false;
            }

            if (requireSpecial && password.All(char.IsLetterOrDigit))
            {
                reason = "密码需包含特殊字符";
                return false;
            }

            return true;
        }
    }
}
