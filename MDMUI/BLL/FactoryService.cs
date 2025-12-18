using System;
using System.Collections.Generic;
using MDMUI.DAL;
using MDMUI.Model;

namespace MDMUI.BLL
{
    /// <summary>
    /// 处理工厂相关的业务逻辑
    /// </summary>
    public class FactoryService
    {
        private FactoryRepository factoryRepository = new FactoryRepository();

        /// <summary>
        /// 获取工厂列表 (根据用户权限过滤)
        /// </summary>
        public List<Factory> GetFactories(User user, string searchTerm = null)
        {
            // 如果是超级管理员，获取所有工厂，否则按用户工厂过滤
            if (user.RoleName == "超级管理员")
            {
                // 超级管理员也可能需要搜索
                return factoryRepository.GetFiltered(null, searchTerm); // 传递 null user 表示获取所有
            }
            else
            {
                return factoryRepository.GetFiltered(user, searchTerm);
            }
        }

        /// <summary>
        /// 根据ID获取工厂
        /// </summary>
        public Factory GetFactoryById(string factoryId)
        {
            return factoryRepository.GetById(factoryId);
        }

        /// <summary>
        /// 添加工厂
        /// </summary>
        public bool AddFactory(Factory factory)
        {
            // TODO: 添加业务逻辑验证 (例如，检查 FactoryId 是否唯一等)
            return factoryRepository.Add(factory);
        }

        /// <summary>
        /// 更新工厂
        /// </summary>
        public bool UpdateFactory(Factory factory)
        {
            // TODO: 添加业务逻辑验证
            return factoryRepository.Update(factory);
        }

        /// <summary>
        /// 删除工厂
        /// </summary>
        public bool DeleteFactory(string factoryId)
        {
            // 业务逻辑：删除前必须检查关联
            if (factoryRepository.IsFactoryAssociated(factoryId))
            {
                throw new InvalidOperationException("无法删除该工厂，因为它已关联了用户或部门。");
            }
            return factoryRepository.Delete(factoryId);
        }
    }
} 