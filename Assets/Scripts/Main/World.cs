using System;
using System.Collections.Generic;
using UnityEngine;

namespace LearnPhysics
{
    /// <summary>
    /// World
    /// </summary>
    public class World
    {
        #region 公共方法

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            m_bvh = CreateBVH(new List<Shape>());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            // 1. 碰撞检测
            CollisionDetection();

            // 2. 进行模拟
            SimulateStep(deltaTime);

            // 3. 进行清理
            ContactsClear();
        }

        /// <summary>
        /// 创建BVH
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        BVH<Shape> CreateBVH(List<Shape> objects)
        {
            return new BVH<Shape>(new BVHShapeAdapter(), objects);
        }

        /// <summary>
        /// 创建body
        /// </summary>
        /// <returns></returns>
        public RigidBody RigidBodyCreate(bool isKinematic)
        {
            // 创建刚体
            RigidBody rigidBody = new RigidBody(this, isKinematic);
            rigidBody.IsActive = true;

            m_rigidBodyDict.Add(rigidBody.Id, rigidBody);
            return rigidBody;
        }

        /// <summary>
        /// 移除Body
        /// </summary>
        public void RigidBodyRemove(RigidBody rigidBody)
        {
            // 清理刚体对应的Shape
            foreach (var shape in rigidBody.m_shapes)
            {
                m_bvh.Remove(shape);
            }

            // 移除刚体
            m_rigidBodyDict.Remove(rigidBody.Id);
        }

        /// <summary>
        /// 射线检测
        /// </summary>
        /// <returns></returns>
        public bool RayCast(Ray ray, out RayCastResult result)
        {
            // 实现射线检测逻辑
            result = new RayCastResult();

            // 创建一个节点栈，用于遍历BVH
            var nodeStack = new Stack<BVHNode<Shape>>();
            nodeStack.Push(m_bvh.rootBVH);

            while (nodeStack.Count > 0)
            {
                BVHNode<Shape> node = nodeStack.Pop();

                if (node.IsLeaf)
                {
                    foreach (var nodeGObject in node.GObjects)
                    {
                        // 如果射线击中了叶子物体，记录射线射到的第一个物体
                        if (Hit(ray, nodeGObject.WorldBoundingBox, 0, Single.MaxValue))
                        {
                            result.m_shape = nodeGObject;
                            // todo 记录其他的信息

                            return true;
                        }
                    }
                }
                else
                {
                    nodeStack.Push(node.Left);
                    nodeStack.Push(node.Right);
                }
            }

            return false;
        }

        #region 私有方法

        /// <summary>
        /// 碰撞检测
        /// </summary>
        private void CollisionDetection()
        {
            // 收集碰撞对
            CollectCollisionPairs();
        }

        /// <summary>
        /// 进行模拟
        /// </summary>
        private void SimulateStep(float deltaTime)
        {
            // 1.应用外力
            ApplyForce();

            // 2.解决碰撞和约束
            Solve();

            // 3.积分求解，更新状态
            TransformUpdate();
        }

        /// <summary>
        /// 清空碰撞记录
        /// </summary>
        private void ContactsClear()
        {
            m_collisionPairDict.Clear();
            foreach (var rigidBody in m_rigidBodyDict.Values)
            {
                rigidBody.Force = Vector3.zero;
            }
        }

        /// <summary>
        /// 收集碰撞对
        /// </summary>
        private void CollectCollisionPairs()
        {
            // 1.空间划分等方法进行粗筛选
            // 2.包围盒检测
            // 3.Shape对检测
            // 4.记录碰撞对并触发碰撞事件

            // 创建一个节点栈，用于遍历BVH
            var nodeStack = new Stack<BVHNode<Shape>>();
            nodeStack.Push(m_bvh.rootBVH);

            while (nodeStack.Count > 0)
            {
                BVHNode<Shape> node = nodeStack.Pop();

                if (node == null)
                {
                    continue;
                }

                if (node.IsLeaf)
                {
                    foreach (var gObjectA in node.GObjects)
                    {
                        foreach (var gObjectB in node.GObjects)
                        {
                            if (gObjectA == gObjectB)
                            {
                                continue;
                            }

                            // 检测物体包围盒
                            if (gObjectA.WorldBoundingBox.Intersects(gObjectB.WorldBoundingBox))
                            {
                                var minId = gObjectA.Id;
                                var maxId = gObjectB.Id;
                                if (minId > maxId)
                                {
                                    var temp = minId;
                                    minId = maxId;
                                    maxId = temp;
                                }

                                // 跳过已经检测的碰撞对
                                if (m_collisionPairDict.ContainsKey((minId, maxId)))
                                {
                                    continue;
                                }

                                var collisionPair = CollisionPairConstruct(gObjectA, gObjectB);
                                m_collisionPairDict.Add((minId, maxId), collisionPair);

                                EventOnCollisionEnter?.Invoke(collisionPair);
                            }
                        }
                    }
                }
                else
                {
                    nodeStack.Push(node.Left);
                    nodeStack.Push(node.Right);
                }
            }
        }

        /// <summary>
        /// 构建碰撞对
        /// </summary>
        /// <param name="shapeA"></param>
        /// <param name="shapeB"></param>
        /// <returns></returns>
        private CollisionResult CollisionPairConstruct(Shape shapeA, Shape shapeB)
        {
            CollisionResult collisionPair = new CollisionResult();

            // todo：目前仅支持球体和球体之间的碰撞
            if (shapeA.PrimitiveType == PrimitiveType.Sphere && shapeB.PrimitiveType == PrimitiveType.Sphere)
            {
                CollisionHelper.ContactSphereSphere((SphereShape)shapeA, (SphereShape)shapeB, out collisionPair);
            }

            return collisionPair;
        }

        /// <summary>
        /// 应用外力
        /// </summary>
        private void ApplyForce()
        {
            foreach (var body in m_rigidBodyDict.Values)
            {
                if (!body.IsActive || body.IsStatic) continue;

                if (body.UseGravity)
                {
                    body.LinearVelocity += Gravity * StepDeltaTime;
                }

                // 速度
                body.LinearVelocity += body.InverseMass * body.Force * StepDeltaTime;

                // 阻尼
                body.LinearVelocity *= RigidBody.LinearDampingMultiplier;
                body.AngularAcceleration *= RigidBody.AngularDampingMultiplier;
            }
        }

        /// <summary>
        /// 碰撞求解
        /// </summary>
        private void Solve()
        {
            foreach (var collisionResult in m_collisionPairDict.Values)
            {
                CollisionSolver.SolveCollision(collisionResult);
            }
        }

        /// <summary>
        /// 更新刚体Transform
        /// </summary>
        private void TransformUpdate()
        {
            foreach (var body in m_rigidBodyDict.Values)
            {
                if (body.IsStatic) continue;

                Vector3 linearVelocity = body.LinearVelocity;

                if (linearVelocity == Vector3.zero)
                {
                    continue;
                }

                var newPos = body.Position + linearVelocity * StepDeltaTime;
                var newRot = body.Orientation;

                // 更新位置和旋转
                body.TransformUpdate(newPos, newRot);
            }
        }

        /// <summary>
        /// 判断射线是否在t区间
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="bounds"></param>
        /// <param name="tmin">射线起点到边界框的最小距离</param>
        /// <param name="tmax">射线起点到边界框的最大距离</param>
        /// <returns></returns>
        private bool Hit(Ray ray, Bounds bounds, float tmin, float tmax)
        {
            // xyz三个方向分别判断
            for (int a = 0; a < 3; a++)
            {
                float invD = 1.0f / ray.m_direction[a];
                float t0 = (bounds.min[a] - ray.m_origin[a]) * invD;
                float t1 = (bounds.max[a] - ray.m_origin[a]) * invD;

                if (invD < 0.0f)
                {
                    (t0, t1) = (t1, t0);
                }

                tmax = t1 < tmax ? t1 : tmax;
                tmin = t0 > tmin ? t0 : tmin;
                if (tmax <= tmin)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #endregion

        #region 碰撞事件

        /// <summary>
        /// 碰撞事件
        /// </summary>
        public event Action<CollisionResult> EventOnCollisionEnter;
        public event Action<CollisionResult> EventOnCollisionStay;
        public event Action<CollisionResult> EventOnCollisionExit;

        #endregion

        #region 字段&属性

        /// <summary>
        /// 刚体列表
        /// </summary>
        private readonly Dictionary<ulong, RigidBody> m_rigidBodyDict = new();

        /// <summary>
        /// 碰撞信息
        /// </summary>
        private Dictionary<(ulong, ulong), CollisionResult> m_collisionPairDict = new();

        /// <summary>
        /// 重力
        /// </summary>
        public Vector3 Gravity => m_gravity;
        private Vector3 m_gravity = new(0, -9.81f * 0.1f, 0);

        /// <summary>
        /// 更新步长
        /// </summary>
        public const float StepDeltaTime = 1.0f / 100.0f;

        /// <summary>
        /// BVH
        /// </summary>
        public BVH<Shape> m_bvh;

        #endregion
    }
}
