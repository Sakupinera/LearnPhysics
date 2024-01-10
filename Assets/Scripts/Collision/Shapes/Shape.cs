using UnityEngine;

namespace PhysicsDemo
{
    /// <summary>
    /// Shape
    /// </summary>
    public abstract class Shape
    {
        #region 公共方法

        /// <summary>
        /// 关联到Body
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public void RigidBodyAttach(RigidBody body)
        {

        }

        /// <summary>
        /// 移除关联
        /// </summary>
        public void RigidBodyDetach()
        {
            
        }

        /// <summary>
        /// 更新惯性张量
        /// </summary>
        protected virtual void InertiaUpdate()
        {

        }

        /// <summary>
        /// 更新包围盒
        /// </summary>
        public virtual void BoundingBoxUpdate()
        {

        }

        #endregion

        #region 字段&属性

        /// <summary>
        /// id
        /// </summary>
        public readonly ulong m_shapeId;

        /// <summary>
        /// 关联Body
        /// </summary>
        public RigidBody RigidBody { get; private set; }

        /// <summary>
        /// 包围盒
        /// </summary>
        public Bounds WorldBoundingBox { get; protected set; }

        /// <summary>
        /// 惯性张量
        /// </summary>
        public Matrix4x4 Inertia { get; protected set; }

        /// <summary>
        /// 质量
        /// </summary>
        public float Mass { get; protected set; }

        /// <summary>
        /// 线速度
        /// </summary>
        public virtual Vector3 Velocity { get; protected set; }

        #endregion
    }
}
