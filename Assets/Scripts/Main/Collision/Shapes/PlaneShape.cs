using UnityEngine;

namespace LearnPhysics
{
    /// <summary>
    /// 平面形状
    /// </summary>
    public class PlaneShape : Shape
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="mass"></param>
        public PlaneShape( float mass) : base(mass)
        {
        }

        #region Overrides of Shape

        /// <summary>
        /// 更新包围盒
        /// </summary>
        public override void UpdateBoundingBox()
        {
            
        }

        /// <summary>
        /// 基础形状
        /// </summary>
        public override PrimitiveType PrimitiveType => PrimitiveType.Plane;

        #endregion
    }
}
