using UnityEngine;

namespace LearnPhysics
{
    /// <summary>
    /// 球体形状
    /// </summary>
    public class SphereShape : Shape
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="mass"></param>
        public SphereShape(float radius, float mass) : base(mass)
        {
            Raidus = radius;
            Mass = mass;
        }

        #region Overrides of Shape

        /// <summary>
        /// 更新包围盒
        /// </summary>
        public override void UpdateBoundingBox()
        {
            WorldBoundingBox = new Bounds(RigidBody?.Position ?? Vector3.zero,
                new Vector3(Raidus * 2, Raidus * 2, Raidus * 2));
        }

        #endregion

        /// <summary>
        /// 半径
        /// </summary>
        public float Raidus { get; }

        /// <summary>
        /// 基础形状
        /// </summary>
        public override PrimitiveType PrimitiveType => PrimitiveType.Sphere;
    }
}
