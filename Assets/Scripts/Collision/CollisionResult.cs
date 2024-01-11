using UnityEngine;

namespace LearnPhysics
{
    /// <summary>
    /// 碰撞对
    /// </summary>
    public class CollisionResult
    {
        public RigidBody m_bodyA;
        public RigidBody m_bodyB;

        // TODO: 碰撞信息
        // 碰撞点、法线、切线等

        /// <summary>
        /// 是否发生了碰撞
        /// </summary>
        public bool m_isTouching;

        /// <summary>
        /// 碰撞法线
        /// </summary>
        public Vector3 m_normal;

        /// <summary>
        /// 相对速度的法线分量
        /// </summary>
        public float m_vrn;
    }

}
