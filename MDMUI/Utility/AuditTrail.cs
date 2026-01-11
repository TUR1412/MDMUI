using MDMUI.BLL;
using MDMUI.Model;

namespace MDMUI.Utility
{
    public static class AuditTrail
    {
        public static void Log(User user, string operationType, string module, string description, string ipAddress = null)
        {
            try
            {
                int userId = user?.Id ?? -1;
                string userName = user?.Username ?? "未知";
                new SystemLogBLL().AddLog(userId, userName, operationType, module, description, ipAddress);
            }
            catch
            {
                // 审计失败不影响主流程
            }
        }
    }
}
