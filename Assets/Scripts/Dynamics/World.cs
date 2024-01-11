using System;
using System.Collections.Generic;
using DataStructures;
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
        }

        /// <summary>
        /// 创建body
        /// </summary>
        /// <returns></returns>
        public RigidBody RigidBodyCreate()
        {
            // 创建刚体
            RigidBody body = new RigidBody();



            return body;
        }

        /// <summary>
        /// 移除Body
        /// </summary>
        public void RigidBodyRemove(RigidBody rigidBody)
        {
            // 移除刚体

            // 清理刚体对应的Shape
        }

        /// <summary>
        /// 射线检测
        /// </summary>
        /// <returns></returns>
        public RayCastResult RayCast()
        {
            // 实现射线检测逻辑


            return new RayCastResult();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 碰撞检测
        /// </summary>
        private void CollisionDetection()
        {
            // 1.空间划分等方法进行粗筛选
            // 2.包围盒检测
            // 3.Shape对检测
            // 4.记录碰撞对并触发碰撞事件

            
        }

        /// <summary>
        /// 进行模拟
        /// </summary>
        private void SimulateStep(float deltaTime)
        {
            // 1.应用外力

            // 2.解决碰撞和约束
            
            // 3.积分求解，更新状态
        }

        /// <summary>
        /// 粗筛碰撞对
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private List<CollisionPair> CollectCollisionPairs()
        {
            // 用于存储碰撞对的列表
            var collisionPairs = new List<CollisionPair>();

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
                                var collisionPair = CollisionPairConstruct(gObjectA, gObjectB);
                                collisionPairs.Add(collisionPair);
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

            return collisionPairs;
        }

        private CollisionPair CollisionPairConstruct(Shape shapeA, Shape shapeB)
        {
            var collisionPair = new CollisionPair();

            return collisionPair;
        }

        #endregion

        #region 碰撞事件

        /// <summary>
        /// 碰撞事件
        /// </summary>
        public event Action<CollisionPair> EventOnCollisionEnter;
        public event Action<CollisionPair> EventOnCollisionStay;
        public event Action<CollisionPair> EventOnCollisionExit;

        #endregion

        #region 字段&属性

        /// <summary>
        /// 刚体列表
        /// </summary>
        private readonly Dictionary<ulong, RigidBody> m_bodiesDict = new();
        /// <summary>
        /// Shape列表
        /// </summary>
        private readonly Dictionary<ulong, Shape> m_shapesDict = new();

        /// <summary>
        /// 碰撞信息
        /// </summary>
        private Dictionary<(ulong, ulong), CollisionPair> m_collisionDataDict = new();

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

        /// <summary>
        /// idCounter
        /// </summary>
        private static ulong m_idCounter;

        #endregion
    }
}
