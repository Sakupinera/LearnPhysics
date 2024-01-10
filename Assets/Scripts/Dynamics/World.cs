using System;

namespace PhysicsDemo
{
    /// <summary>
    /// World
    /// </summary>
    public class World
    {
        #region 公共方法

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {

        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            // 1. 碰撞检测
            CollisionDetection();

            // 2. 进行模拟
            SimulateStep(deltaTime);

            // 3. 进行清理
        }

        /// <summary>
        /// 创建body
        /// </summary>
        /// <returns></returns>
        public RigidBody RigidBodyCreate()
        {
            // 创建刚体
            RigidBody body = new RigidBody();



            return body;
        }

        /// <summary>
        /// 移除Body
        /// </summary>
        public void RigidBodyRemove(RigidBody rigidBody)
        {
            // 移除刚体

            // 清理刚体对应的Shape
        }

        /// <summary>
        /// 射线检测
        /// </summary>
        /// <returns></returns>
        public RayCastResult RayCast()
        {
            // 实现射线检测逻辑


            return new RayCastResult();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 碰撞检测
        /// </summary>
        private void CollisionDetection()
        {
            // 1.空间划分等方法进行粗筛选

            // 2.包围盒检测
            
            // 3.Shape对检测
            
            // 4.记录碰撞对并触发碰撞事件
        }

        /// <summary>
        /// 进行模拟
        /// </summary>
        private void SimulateStep(float deltaTime)
        {
            // 1.应用外力

            // 2.解决碰撞和约束
            
            // 3.积分求解，更新状态
        }

        #endregion

        #region 碰撞事件

        /// <summary>
        /// 碰撞事件
        /// </summary>
        public event Action<CollisionResult> EventOnCollisionEnter;
        public event Action<CollisionResult> EventOnCollisionStay;
        public event Action<CollisionResult> EventOnCollisionExit;

        #endregion

        #region 字段&属性


        #endregion
    }
}
