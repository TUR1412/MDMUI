using System;

namespace MDMUI.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RealName { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public string FactoryId { get; set; }
    }
}