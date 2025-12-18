using System;

namespace MDMUI.Model
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public string ModuleName { get; set; }
        public string ActionName { get; set; }
        public string Description { get; set; }
    }

    public class UserPermission
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PermissionId { get; set; }
    }
}