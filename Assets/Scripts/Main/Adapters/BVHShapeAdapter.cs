using System;
using System.Collections.Generic;
using UnityEngine;

namespace LearnPhysics
{
    /// <summary>
    /// BVH结点适配器
    /// </summary>
    /// <typeparam name="Shape"></typeparam>
    public class BVHShapeAdapter : IBVHNodeAdapter<Shape>
    {
        #region Implementation of IBVHNodeAdapter<Shape>

        /// <summary>
        /// 获取物体位置
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Vector3 GetObjectPos(Shape obj)
        {
            return obj.Position;
        }

        /// <summary>
        /// 获取包围盒半径
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public float GetRadius(Shape obj)
        {
            var bounds = obj.WorldBoundingBox;
            return Mathf.Max(Mathf.Max(bounds.extents.x, bounds.extents.y), bounds.extents.z);
        }

        /// <summary>
        /// 建立物体和BVH叶节点之间的映射
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="leaf"></param>
        public void MapObjectToBVHLeaf(Shape obj, BVHNode<Shape> leaf)
        {
            m_shapeToLeafMap[obj] = leaf;
        }

        /// <summary>
        /// 当位置或包围盒大小发生变化时的回调
        /// </summary>
        /// <param name="changed"></param>
        public void OnPositionOrSizeChanged(Shape changed)
        {
            m_shapeToLeafMap[changed].RefitObjectChanged(this, changed);
            EventOnPositionOrSizeChanged?.Invoke(changed);
        }

        /// <summary>
        /// 移除映射
        /// </summary>
        /// <param name="obj"></param>
        public void UnmapObject(Shape obj)
        {
            m_shapeToLeafMap.Remove(obj);
        }

        /// <summary>
        /// 获取对应物体的叶节点
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public BVHNode<Shape> GetLeaf(Shape obj)
        {
            return m_shapeToLeafMap[obj];
        }

        #endregion

        /// <summary>
        /// BVH树
        /// </summary>
        BVH<Shape> IBVHNodeAdapter<Shape>.BVH
        {
            get => m_bvh;
            set => m_bvh = value;
        }
        private BVH<Shape> m_bvh;
        
        /// <summary>
        /// 形状到叶节点的映射
        /// </summary>
        Dictionary<Shape, BVHNode<Shape>> m_shapeToLeafMap = new();
        
        /// <summary>
        /// 位置或形体包围盒大小发生改变事件
        /// </summary>
        private event Action<Shape> EventOnPositionOrSizeChanged;
    }
}
