using System;
using System.Collections.Generic;
using System.Linq;
using MDMUI.DAL;
using MDMUI.Model;
using MDMUI.Utility;

namespace MDMUI.BLL
{
    /// <summary>
    /// 业务逻辑服务类 - 部门管理
    /// </summary>
    public class DepartmentService
    {
        private DepartmentDAL deptDal = new DepartmentDAL();
        private FactoryRepository factoryRepo = new FactoryRepository(); // 改用FactoryRepository
        private EmployeeDAL employeeDal = new EmployeeDAL(); // 可能需要获取负责人信息
        // private SystemLogBLL systemLogBll = new SystemLogBLL(); // 可选用于日志

        /// <summary>
        /// 获取所有部门信息 (通常不直接使用，可能按工厂过滤)
        /// </summary>
        public List<Department> GetAllDepartments()
        {
            try
            {
                return deptDal.GetAllDepartments();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BLL Error getting all departments: " + ex.Message);
                throw;
            }
        }
        
        /// <summary>
        /// 根据工厂ID获取部门信息
        /// </summary>
        public List<Department> GetDepartmentsByFactoryId(string factoryId)
        {
             if (string.IsNullOrEmpty(factoryId)){
                 // 或者返回空列表，取决于UI如何处理
                 throw new ArgumentNullException(nameof(factoryId), "必须提供工厂ID");
             }
            try
            {
                return deptDal.GetDepartmentsByFactoryId(factoryId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BLL Error getting departments for factory {factoryId}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 根据ID获取单个部门信息
        /// </summary>
        public Department GetDepartmentById(string deptId)
        {
            if (string.IsNullOrEmpty(deptId))
            {
                return null;
            }
            try
            {
                return deptDal.GetDepartmentById(deptId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BLL Error getting department by ID {deptId}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 添加新部门
        /// </summary>
        public bool AddDepartment(Department department, User currentUser = null)
        {
            if (department == null) { throw new ArgumentNullException(nameof(department)); }
            if (string.IsNullOrWhiteSpace(department.DeptId)) { throw new ArgumentException("部门ID不能为空", nameof(department.DeptId)); }
            if (string.IsNullOrWhiteSpace(department.DeptName)) { throw new ArgumentException("部门名称不能为空", nameof(department.DeptName)); }
            if (string.IsNullOrWhiteSpace(department.FactoryId)) { throw new ArgumentException("必须指定所属工厂", nameof(department.FactoryId)); }
            // 可选：验证 ParentDeptId 是否存在于同一 FactoryId 下
            // 可选：验证 ManagerEmployeeId 是否存在

            try
            {
                 department.CreateTime = DateTime.Now; // 在 BLL 层设置创建时间
                 bool success = deptDal.AddDepartment(department);
                 // Log if needed
                 return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BLL Error adding department: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 更新部门信息
        /// </summary>
        public bool UpdateDepartment(Department department, User currentUser = null)
        {
            if (department == null) { throw new ArgumentNullException(nameof(department)); }
            if (string.IsNullOrWhiteSpace(department.DeptId)) { throw new ArgumentException("部门ID不能为空", nameof(department.DeptId)); }
            if (string.IsNullOrWhiteSpace(department.DeptName)) { throw new ArgumentException("部门名称不能为空", nameof(department.DeptName)); }
             if (string.IsNullOrWhiteSpace(department.FactoryId)) { throw new ArgumentException("必须指定所属工厂", nameof(department.FactoryId)); }

            // 防止循环引用：ParentDeptId 不能是自身或其子部门
            ValidateNoCircularReference(department);

            try
            {
                bool success = deptDal.UpdateDepartment(department);
                 // Log if needed
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BLL Error updating department: " + ex.Message);
                throw;
            }
        }

        private void ValidateNoCircularReference(Department department)
        {
            if (department == null) { throw new ArgumentNullException(nameof(department)); }

            string deptId = department.DeptId?.Trim();
            string newParentDeptId = string.IsNullOrWhiteSpace(department.ParentDeptId) ? null : department.ParentDeptId.Trim();

            if (string.IsNullOrWhiteSpace(deptId))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(newParentDeptId))
            {
                return; // 未设置上级部门，不存在循环问题
            }

            if (string.Equals(deptId, newParentDeptId, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("上级部门不能设置为自身。");
            }

            // 上级部门必须存在
            Department parent = deptDal.GetDepartmentById(newParentDeptId);
            if (parent == null)
            {
                throw new InvalidOperationException("上级部门不存在，请重新选择。");
            }

            // 上级部门必须在同一工厂内（避免跨工厂挂靠导致的层级混乱）
            if (!string.Equals(parent.FactoryId, department.FactoryId, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("上级部门必须属于同一工厂。");
            }

            // 判断 newParentDeptId 是否在 deptId 的子孙节点集合里
            List<Department> all = deptDal.GetAllDepartments();
            Dictionary<string, List<string>> childrenByParentId = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            foreach (Department d in all)
            {
                if (d == null) { continue; }

                string parentId = string.IsNullOrWhiteSpace(d.ParentDeptId) ? null : d.ParentDeptId.Trim();
                string childId = string.IsNullOrWhiteSpace(d.DeptId) ? null : d.DeptId.Trim();
                if (string.IsNullOrWhiteSpace(childId) || string.IsNullOrWhiteSpace(parentId))
                {
                    continue;
                }

                if (!childrenByParentId.TryGetValue(parentId, out List<string> children))
                {
                    children = new List<string>();
                    childrenByParentId[parentId] = children;
                }
                children.Add(childId);
            }

            HashSet<string> descendants = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(deptId);

            while (queue.Count > 0)
            {
                string current = queue.Dequeue();
                if (!childrenByParentId.TryGetValue(current, out List<string> children))
                {
                    continue;
                }

                foreach (string child in children)
                {
                    if (descendants.Add(child))
                    {
                        queue.Enqueue(child);
                    }
                }
            }

            if (descendants.Contains(newParentDeptId))
            {
                throw new InvalidOperationException("上级部门不能设置为当前部门的子部门（会导致循环引用）。");
            }
        }

        /// <summary>
        /// 删除部门 (增加子部门和用户关联检查)
        /// </summary>
        public bool DeleteDepartment(string deptId, User currentUser = null)
        {
            if (string.IsNullOrEmpty(deptId)) { throw new ArgumentNullException(nameof(deptId)); }

            try
            {
                // 检查是否有子部门
                if (deptDal.HasChildDepartments(deptId))
                {
                    throw new InvalidOperationException("删除失败：该部门下存在子部门。");
                }
                // 检查是否有关联用户 (Employee 表)
                 if (deptDal.IsDepartmentAssociatedWithUser(deptId))
                 {
                     throw new InvalidOperationException("删除失败：该部门下存在关联的员工。请先处理员工归属。");
                 }
                 
                 // 获取部门名称用于日志 (可选)
                 string deptName = deptDal.GetDepartmentById(deptId)?.DeptName ?? deptId;

                bool success = deptDal.DeleteDepartment(deptId);
                 // Log if needed
                return success;
            }
             catch (InvalidOperationException bizEx) // 捕获业务逻辑异常
            {
                 Console.WriteLine("BLL Error deleting department: " + bizEx.Message);
                 throw;
            }
            catch (Exception ex) // 捕获 DAL 异常
            {
                Console.WriteLine("BLL Error deleting department: " + ex.Message);
                throw;
            }
        }
        
        /// <summary>
        /// 获取所有工厂列表，用于下拉框
        /// </summary>
        public List<ComboboxItem> GetFactoriesForComboBox(User currentUser)
        {
             try
            {
                List<Factory> factories;
                if (currentUser.RoleName == "超级管理员")
                {
                     factories = factoryRepo.GetFiltered(null); // 使用GetFiltered替代GetAllFactories
                }
                else if (!string.IsNullOrEmpty(currentUser.FactoryId))
                {
                     // 如果非管理员且有关联工厂，只获取他所属的工厂
                     Factory userFactory = factoryRepo.GetById(currentUser.FactoryId); // 使用GetById
                     factories = userFactory != null ? new List<Factory> { userFactory } : new List<Factory>();
                }
                else
                {
                    // 非管理员且无关联工厂（理论上不应发生，或按需处理）
                    factories = new List<Factory>();
                }
               
                return factories.Select(f => new ComboboxItem(f.FactoryName, f.FactoryId)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BLL Error getting factories for combobox: " + ex.Message);
                throw;
            }
        }
        
        /// <summary>
        /// 根据工厂ID获取部门列表，用于下拉框 (包含"无"选项)
        /// </summary>
        public List<ComboboxItem> GetDepartmentsForComboBox(string factoryId, string excludeDeptId = null)
        {
             List<ComboboxItem> items = new List<ComboboxItem>();
             items.Add(new ComboboxItem("(无)", "")); // 添加空选项
             if (string.IsNullOrEmpty(factoryId)) return items;
             
             try
             {
                var departments = deptDal.GetDepartmentsByFactoryId(factoryId);
                 foreach (var dept in departments)
                 {
                      // 排除自身（用于编辑时的上级下拉框）
                     if (dept.DeptId == excludeDeptId) continue;
                     // TODO: 排除子孙节点防止循环引用
                     items.Add(new ComboboxItem(dept.DeptName, dept.DeptId));
                 }
             }
             catch (Exception ex)
             {
                  Console.WriteLine($"BLL Error getting depts for combobox (Factory: {factoryId}): {ex.Message}");
                  // 即使出错，也返回包含"(无)"的列表
             }
             return items;
        }
        
        /// <summary>
        /// 获取所有员工列表，用于下拉框 (负责人)
        /// </summary>
        public List<ComboboxItem> GetEmployeesForComboBox()
        {
             List<ComboboxItem> items = new List<ComboboxItem>();
             items.Add(new ComboboxItem("(无)", "")); // 添加空选项
             try
             {
                items.AddRange(employeeDal.GetAllEmployeesForComboBox()); // 使用 EmployeeDAL 的方法
             }
             catch (Exception ex)
             {
                  Console.WriteLine("BLL Error getting employees for combobox: " + ex.Message);
                  // 出错时返回包含"(无)"的列表
             }
             return items;
        }
    }
} 
