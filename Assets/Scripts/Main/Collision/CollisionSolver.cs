﻿using UnityEngine;

namespace LearnPhysics
{
    /// <summary>
    /// 碰撞解算器
    /// </summary>
    public static class CollisionSolver
    {
        /// <summary>
        /// 碰撞解算
        /// </summary>
        /// <param name="collisionResult"></param>
        public static void SolveCollision(CollisionResult collisionResult)
        {
            // todo 目前仅支持球体之间的碰撞

            // 碰撞时受到的冲量的公式为：
            // I＝－vr(1+e)/(1/m1＋1/m2) 
            // 可将1/(1/m1＋1/m2)看成一个等效质量
            float normalMass = 1.0f / (collisionResult.m_bodyA.InverseMass + collisionResult.m_bodyB.InverseMass);
            // 恢复系数取为两个球体恢复系数的平均值
            float restitution = (collisionResult.m_bodyA.Restitution + collisionResult.m_bodyA.Restitution) / 2;

            // 根据公式计算法线冲量
            float normalImpulse = -normalMass * collisionResult.m_vrn * (1 + restitution);
            // 分别求出x,y,z方向上的冲量分量 
            float Ix = normalImpulse * collisionResult.m_normal.x;
            float Iy = normalImpulse * collisionResult.m_normal.y;
            float Iz = normalImpulse * collisionResult.m_normal.z;

            // 在两个刚体上施加冲量
            collisionResult.m_bodyA.LinearVelocity += new Vector3(collisionResult.m_bodyA.InverseMass * Ix,
                collisionResult.m_bodyA.InverseMass * Iy, collisionResult.m_bodyA.InverseMass * Iz);
            collisionResult.m_bodyB.LinearVelocity += new Vector3(collisionResult.m_bodyB.InverseMass * Ix,
                collisionResult.m_bodyB.InverseMass * Iy, collisionResult.m_bodyB.InverseMass * Iz);
        }

        /// <summary>
        /// 碰撞解算
        /// </summary>
        /// <param name="collisionResult"></param>
        public static void SolveCollision2(CollisionResult collisionResult)
        {
            // 碰撞时受到的冲量的公式为：
            // I＝－vr(1+e)/(1/m1＋1/m2) 
            // 可将1/(1/m1＋1/m2)看成一个等效质量
            float normalMass = 1.0f / (collisionResult.m_bodyA.InverseMass + collisionResult.m_bodyB.InverseMass);
            // 恢复系数取为两个球体恢复系数的平均值
            float restitution = (collisionResult.m_bodyA.Restitution + collisionResult.m_bodyA.Restitution) / 2;

            // 根据公式计算法线冲量
            float normalImpulse = -normalMass * collisionResult.m_vrn * (1 + restitution);
            // 分别求出x,y,z方向上的冲量分量 
            float Ix = normalImpulse * collisionResult.m_normal.x;
            float Iy = normalImpulse * collisionResult.m_normal.y;
            float Iz = normalImpulse * collisionResult.m_normal.z;

            // 在两个刚体上施加冲量
            collisionResult.m_bodyA.LinearVelocity += new Vector3(collisionResult.m_bodyA.InverseMass * Ix,
                collisionResult.m_bodyA.InverseMass * Iy, collisionResult.m_bodyA.InverseMass * Iz);
            collisionResult.m_bodyB.LinearVelocity += new Vector3(collisionResult.m_bodyB.InverseMass * Ix,
                collisionResult.m_bodyB.InverseMass * Iy, collisionResult.m_bodyB.InverseMass * Iz);

            // 计算角冲量
            float Jx = collisionResult.m_normal.x * normalImpulse;
            float Jy = collisionResult.m_normal.y * normalImpulse;
            float Jz = collisionResult.m_normal.z * normalImpulse;

            // 计算碰撞点相对于小球质心的位置向量
            Vector3 r1 = collisionResult.m_contactPoint - collisionResult.m_bodyA.Position;
            Vector3 r2 = collisionResult.m_contactPoint - collisionResult.m_bodyB.Position;

            // 在两个刚体上施加角冲量
            collisionResult.m_bodyA.AngularVelocity += collisionResult.m_bodyA.InverseInertiaWorld.MultiplyVector(Vector3.Cross(r1, new Vector3(Jx, Jy, Jz)));
            collisionResult.m_bodyB.AngularVelocity += collisionResult.m_bodyB.InverseInertiaWorld.MultiplyVector(Vector3.Cross(r2, new Vector3(Jx, Jy, Jz)));
        }
    }
}
