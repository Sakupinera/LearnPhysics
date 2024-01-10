using UnityEngine;

namespace PhysicsDemo
{
    /// <summary>
    /// 射线检测结果
    /// </summary>
    public struct RayCastResult
    {
        public Shape m_entity;
        public float m_fraction;
        public Vector3 m_normal;
        public bool m_hit;
    }
}
