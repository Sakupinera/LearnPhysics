using System.Collections.Generic;
using UnityEngine;

namespace PhysicsDemo
{
    /// <summary>
    /// RigidBody
    /// </summary>
    public class RigidBody
    {
        #region 公共方法

        /// <summary>
        /// 施加力
        /// </summary>
        /// <param name="force"></param>
        public void ForceAdd(in Vector3 force)
        {

        }

        public void ForceAdd(in Vector3 force, in Vector3 position)
        {

        }

        /// <summary>
        /// 添加Shape
        /// </summary>
        /// <param name="shape"></param>
        public void ShapeAdd(Shape shape)
        {

        }

        /// <summary>
        /// 移除Shape
        /// </summary>
        /// <param name="shape"></param>
        public void ShapeRemove(Shape shape)
        {

        }

        #endregion

        #region 私有方法

        #endregion

        #region 字段&属性

        /// <summary>
        /// 刚体对应shape
        /// </summary>
        public readonly List<Shape> m_shapes = new(1);

        /// <summary>
        /// 是否Active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 是否为静态
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// 世界
        /// </summary>
        public World World { get; }

        #endregion
    }
}
