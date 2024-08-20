using System;
using System.Collections.Generic;
using Controller;
using UnityEngine;

namespace Managers
{
    public class ControllerManager : Singleton<ControllerManager>
    {
        //跨模块调用需要通过controllerManager来集中管理

        //有新的加在这

        #region 控制器合集

        public InventoryController inventoryController;
        public EquipmentController equipmentController;

        #endregion

        private Dictionary<Type, object> _managers = new Dictionary<Type, object>();

        public override void Awake()
        {
            base.Awake();
            InitializeManagers();
        }


        #region 注册入字典

        private void InitializeManagers()
        {
            _managers[typeof(InventoryController)] = Instance.inventoryController;
            _managers[typeof(EquipmentController)] = Instance.equipmentController;
        }

        #endregion

        /// <summary>
        /// 跨模块调用通过这儿
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool TryGetManager<T>(out T mgr) where T : class
        {
            if (_managers.TryGetValue(typeof(T), out var manager))
            {
                mgr = manager as T;
                return mgr != null;
            }

            mgr = null;
            return false;

            throw new Exception("Manager not found");
        }
    }
}