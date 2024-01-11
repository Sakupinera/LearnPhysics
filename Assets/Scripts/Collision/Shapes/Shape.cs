using UnityEngine;

namespace LearnPhysics
{
    /// <summary>
    /// 形状基类
    /// </summary>
    public abstract class Shape
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="mass"></param>
        protected Shape(float mass)
        {
            m_id = IdFactory.AcquireId();
            Mass = mass;
        }

        #region 对外方法

        /// <summary>
        /// 关联到Body
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public void AttachRigidBody(RigidBody body)
        {
            RigidBody = body;
        }

        /// <summary>
        /// 移除关联
        /// </summary>
        public void DetachRigidBody()
        {
            RigidBody = null;
        }

        /// <summary>
        /// 更新惯性张量
        /// </summary>
        public virtual void UpdateInertia()
        {
            Inertia = Matrix4x4.zero;
        }

        /// <summary>
        /// 更新包围盒
        /// </summary>
        public abstract void UpdateBoundingBox();

        #endregion

        #region 内部方法

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Initialize()
        {
            UpdateBoundingBox();
        }

        #endregion

        #region 字段&属性

        /// <summary>
        /// id
        /// </summary>
        public ulong Id => m_id;
        private readonly ulong m_id;

        /// <summary>
        /// 相对于刚体位置的局部坐标
        /// </summary>
        public Vector3 m_localPosition;

        /// <summary>
        /// 获取形状对应的位置
        /// </summary>
        public Vector3 Position => RigidBody.Position + m_localPosition;

        /// <summary>
        /// 基础形状
        /// </summary>
        public abstract PrimitiveType PrimitiveType { get; }

        /// <summary>
        /// 关联Body
        /// </summary>
        public RigidBody RigidBody { get; protected set; }

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

        #endregion
    }
}
