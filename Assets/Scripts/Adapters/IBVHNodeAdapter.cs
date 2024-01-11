using UnityEngine;

namespace LearnPhysics
{
    /// <summary>
    /// BVH结点适配器
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public interface IBVHNodeAdapter<T>
	{
        /// <summary>
        /// BVH树
        /// </summary>
		BVH<T> BVH { get; set; }
		
        /// <summary>
        /// 获取物体位置
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Vector3 GetObjectPos(T obj);
		
        /// <summary>
        /// 获取包围盒半径
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        float GetRadius(T obj);
		
        /// <summary>
        /// 建立物体和BVH叶节点之间的映射
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="leaf"></param>
        void MapObjectToBVHLeaf(T obj, BVHNode<T> leaf);

        /// <summary>
        /// 当位置或包围盒大小发生变化时的回调
        /// </summary>
        /// <param name="changed"></param>
        void OnPositionOrSizeChanged(T changed);

        /// <summary>
        /// 移除映射
        /// </summary>
        /// <param name="obj"></param>
        void UnmapObject(T obj);

        /// <summary>
        /// 获取对应物体的叶节点
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
		BVHNode<T> GetLeaf(T obj);
	}
}