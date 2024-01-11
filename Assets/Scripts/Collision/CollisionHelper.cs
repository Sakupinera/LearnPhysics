using System;
using UnityEngine;

namespace LearnPhysics
{
    /// <summary>
    /// 碰撞帮助类
    /// </summary>
    public static class CollisionHelper
    {
        /// <summary>
        /// 球体和球体之间的碰撞信息构建
        /// </summary>
        /// <param name="shapeA"></param>
        /// <param name="shapeB"></param>
        /// <param name="collisionResult"></param>
        public static void ContactSphereSphere(SphereShape shapeA, SphereShape shapeB, out CollisionResult collisionResult)
        {
            collisionResult = new CollisionResult();

            // 计算两球的半径之和
            float r = shapeA.Raidus + shapeB.Raidus;

            // 计算两球心间距离的平方
            float d2 = Mathf.Pow((shapeA.Position.x - shapeB.Position.x), 2) +
                       Mathf.Pow((shapeA.Position.y - shapeB.Position.y), 2) +
                       Mathf.Pow((shapeA.Position.z - shapeB.Position.z), 2);

            // 通过比较d平方和r平方判断是否发生碰撞
            bool isTouching = d2 <= r * r ? true : false;

            // 计算球A质心指向球2质心的矢量，即法线矢量
            Vector3 normal = Vector3.zero;
            normal.x = shapeB.Position.x - shapeA.Position.x;
            normal.y = shapeB.Position.y - shapeA.Position.y;
            normal.z = shapeB.Position.z - shapeA.Position.z;

            // 对法线进行归一化
            float factor = 1f / (float)Math.Sqrt(normal.x * normal.x + normal.y * normal.y + normal.z * normal.z);
            normal.x *= factor;
            normal.y *= factor;
            normal.z *= factor;

            // 计算相对速度的法线分量
            // 这个值为正表示相对法线速度方向与法线方向的夹角小于90度，即两球正在相互靠近
            var vrn = normal.x * (shapeA.RigidBody.LinearVelocity.x - shapeB.RigidBody.LinearVelocity.x) +
                      normal.y * (shapeA.RigidBody.LinearVelocity.y - shapeB.RigidBody.LinearVelocity.y) +
                      normal.z * (shapeA.RigidBody.LinearVelocity.z - shapeB.RigidBody.LinearVelocity.z);

            collisionResult.m_isTouching = isTouching;
            collisionResult.m_normal = normal;
            collisionResult.m_vrn = vrn;

        }
    }
}
