using System.Collections.Generic;
using UnityEngine;

namespace LearnPhysics
{
    /// <summary>
    /// 刚体
    /// </summary>
    public class RigidBody
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="world"></param>
        /// <param name="isStatic"></param>
        public RigidBody(World world, bool isStatic)
        {
            m_id = IdFactory.AcquireId();

            World = world;
            IsStatic = isStatic;
        }

        #region 公共方法

        public void TransformUpdate(Vector3 pos, Matrix4x4 ori)
        {
            Position = pos;
            Orientation = ori;

            // 设为Active
            IsActive = true;

            // 更新对应Shape
            foreach (var shape in m_shapes)
            {
                shape.UpdateBoundingBox();
                World.m_bvh.MarkForUpdate(shape);
            }
        }

        /// <summary>
        /// 施加力
        /// </summary>
        /// <param name="force"></param>
        public void ForceAdd(in Vector3 force)
        {
            Force += force;
        }

        /// <summary>
        /// 添加Shape
        /// </summary>
        /// <param name="shape"></param>
        public void ShapeAdd(Shape shape)
        {
            shape.AttachRigidBody(this);
            m_shapes.Add(shape);
        }

        /// <summary>
        /// 移除Shape
        /// </summary>
        /// <param name="shape"></param>
        public void ShapeRemove(Shape shape)
        {
            shape.DetachRigidBody();
            m_shapes.Remove(shape);
        }

        #endregion

        #region 字段&属性

        /// <summary>
        /// Id
        /// </summary>
        public ulong Id => m_id;
        private readonly ulong m_id;

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
        /// 是否受重力影响
        /// </summary>
        public bool UseGravity { get; set; } = true;

        /// <summary>
        /// 受力
        /// </summary>
        public Vector3 Force { get; set; }

        /// <summary>
        /// 力矩
        /// </summary>
        public Vector3 Torque { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position { get; set; } = Vector3.zero;

        /// <summary>
        /// 朝向
        /// </summary>
        public Matrix4x4 Orientation { get; set; } = Matrix4x4.identity;

        /// <summary>
        /// 速度
        /// </summary>
        public Vector3 LinearVelocity { get; set; } = Vector3.zero;
        public Vector3 AngularVelocity { get; set; } = Vector3.zero;

        /// <summary>
        /// 加速度
        /// </summary>
        public Vector3 LinearAcceleration { get; set; }
        public Vector3 AngularAcceleration { get; set; }

        /// <summary>
        /// 惯性矩阵
        /// </summary>
        public Matrix4x4 InverseInertia { get; set; }
        public Matrix4x4 InverseInertiaWorld { get; set; }

        /// <summary>
        /// 摩擦
        /// </summary>
        public float Friction { get; set; }

        /// <summary>
        /// 弹性系数
        /// </summary>
        public float Restitution { get; set; }

        /// <summary>
        /// 阻尼
        /// </summary>
        public const float LinearDampingMultiplier = 0.995f;
        public const float AngularDampingMultiplier = 0.995f;

        /// <summary>
        /// 质量
        /// </summary>
        public float Mass { get; set; }
        public float InverseMass => 1.0f / Mass;

        /// <summary>
        /// 世界
        /// </summary>
        public World World { get; }

        #endregion
    }
}
