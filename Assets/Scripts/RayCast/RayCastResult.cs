using UnityEngine;

namespace LearnPhysics
{
    /// <summary>
    /// 射线检测结果
    /// </summary>
    public struct RayCastResult
    {
        /// <summary>
        /// 最近打中的形状
        /// </summary>
        public Shape m_shape;

        /// <summary>
        /// 击中点的位置
        /// </summary>
        public float m_hitPoint;

        /// <summary>
        /// 距离
        /// </summary>
        public float m_distance;
        
        /// <summary>
        /// 击中点的法线
        /// </summary>
        public Vector3 m_normal;
    }
}
