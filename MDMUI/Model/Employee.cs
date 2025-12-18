using System;

namespace MDMUI.Model
{
    /// <summary>
    /// 员工模型
    /// </summary>
    public class Employee
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Gender { get; set; } // 可以考虑使用枚举类型
        public DateTime? BirthDate { get; set; } // 可空日期
        public string IdCard { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string DeptId { get; set; }
        public string Position { get; set; }
        public DateTime HireDate { get; set; }
        public string Status { get; set; } // 可以考虑使用枚举类型 (如 在职, 离职, 休假)
        public int? UserId { get; set; } // 外键关联 Users 表，设为可空 int
        public DateTime CreateTime { get; set; }

        // 可以添加 DepartmentName 等用于显示的属性，在DAL中填充
        // public string DepartmentName { get; set; }
    }
} 