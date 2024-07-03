using System;
using System.Collections.Generic;

using UnityEngine;

namespace Kiwi.Utility
{
	/// <summary>
	/// Transform 类方法扩展
	/// </summary>
	public static partial class KiwiExtend
	{
		/// <summary>
		/// 获取 Transform 的世界坐标
		/// </summary>
		/// <param name="transform">被扩展的对象</param>
		/// <param name="x">x 轴坐标</param>
		/// <param name="y">y 轴坐标</param>
		/// <param name="z">z 轴坐标</param>
		public static void GetPosition(this Transform transform , out float x , out float y , out float z)
		{
			var pos = transform.position;

			x = pos.x;
			y = pos.y;
			z = pos.z;
		}

		/// <summary>
		/// 设置 Transform 的世界坐标
		/// </summary>
		/// <param name="transform">被扩展的对象</param>
		/// <param name="newPosition">要设置的坐标位置</param>
		public static void SetPosition(this Transform transform , Vector3 newPosition)
		{
			var pos = transform.position;
			if (SetPropertyUtility.SetVector3(ref pos , newPosition))
				transform.position = pos;
		}

		/// <summary>
		/// 设置 Transform 的世界坐标
		/// </summary>
		/// <param name="transform">被扩展的对象</param>
		/// <param name="x">X轴坐标</param>
		/// <param name="y">Y轴坐标</param>
		/// <param name="z">Z轴坐标</param>
		public static void SetPosition(this Transform transform , float x , float y , float z)
		{
			var pos = transform.position;

			if (SetPropertyUtility.SetVector3(ref pos , x , y , z))
				transform.position = pos;
		}

		/// <summary>
		/// 获取 Transform 的本地坐标
		/// </summary>
		/// <param name="transform">被扩展的对象</param>
		/// <param name="x">x 轴坐标</param>
		/// <param name="y">y 轴坐标</param>
		/// <param name="z">z 轴坐标</param>
		public static void GetLocalPosition(this Transform transform , out float x , out float y , out float z)
		{
			var pos = transform.localPosition;

			x = pos.x;
			y = pos.y;
			z = pos.z;
		}

		/// <summary>
		/// 设置 Transform 的本地坐标
		/// </summary>
		/// <param name="transform">被扩展的对象</param>
		/// <param name="newLocalPosition">要设置的本地坐标位置</param>
		public static void SetLocalPosition(this Transform transform , Vector3 newLocalPosition)
		{
			var localPos = transform.localPosition;
			if (SetPropertyUtility.SetVector3(ref localPos , newLocalPosition))
				transform.localPosition = localPos;
		}

		/// <summary>
		/// 设置 Transform 的本地坐标
		/// </summary>
		/// <param name="transform">被扩展的对象</param>
		/// <param name="x">X轴坐标</param>
		/// <param name="y">Y轴坐标</param>
		/// <param name="z">Z轴坐标</param>
		public static void SetLocalPosition(this Transform transform , float x , float y , float z)
		{
			var localPos = transform.localPosition;
			if (SetPropertyUtility.SetVector3(ref localPos , x , y , z))
				transform.localPosition = localPos;
		}

		/// <summary>
		/// 设置 Transform 的欧拉角
		/// </summary>
		/// <param name="transform">被扩展的对象</param>
		/// <param name="newLocalEuler">要设置的欧拉角</param>
		public static void SetEuler(this Transform transform , Vector3 newLocalEuler)
		{
			var euler = transform.eulerAngles;

			if (SetPropertyUtility.SetVector3(ref euler , newLocalEuler))
				transform.eulerAngles = euler;
		}

		/// <summary>
		/// 设置 Transform 的欧拉角
		/// </summary>
		/// <param name="transform">被扩展的对象</param>
		/// <param name="x">X轴角度</param>
		/// <param name="y">Y轴角度</param>
		/// <param name="z">Z轴角度</param>
		public static void SetEuler(this Transform transform , float x , float y , float z)
		{
			var euler = transform.eulerAngles;

			if (SetPropertyUtility.SetVector3(ref euler , x , y , z))
				transform.eulerAngles = euler;
		}

		/// <summary>
		/// 设置 Transform 的本地欧拉角
		/// </summary>
		/// <param name="transform">被扩展的对象</param>
		/// <param name="newLocalEuler">要设置的欧拉角</param>
		public static void SetLocalEuler(this Transform transform , Vector3 newLocalEuler)
		{
			var localEuler = transform.localEulerAngles;

			if (SetPropertyUtility.SetVector3(ref localEuler , newLocalEuler))
				transform.localEulerAngles = localEuler;
		}

		/// <summary>
		/// 设置 Transform 的本地欧拉角
		/// </summary>
		/// <param name="transform">被扩展的对象</param>
		/// <param name="x">X轴角度</param>
		/// <param name="y">Y轴角度</param>
		/// <param name="z">Z轴角度</param>
		public static void SetLocalEuler(this Transform transform , float x , float y , float z)
		{
			var localEuler = transform.localEulerAngles;

			if (SetPropertyUtility.SetVector3(ref localEuler , x , y , z))
				transform.localEulerAngles = localEuler;
		}

		/// <summary>
		/// 获取 Transform 的缩放比例
		/// </summary>
		/// <param name="transform">被扩展的对象</param>
		/// <param name="x">x 轴缩放</param>
		/// <param name="y">y 轴缩放</param>
		/// <param name="z">z 轴缩放</param>
		public static void GetLocalScale(this Transform transform , out float x , out float y , out float z)
		{
			var scale = transform.localScale;

			x = scale.x;
			y = scale.y;
			z = scale.z;
		}

		/// <summary>
		/// 设置缩放比例
		/// </summary>
		/// <param name="transform">被扩展的对象</param>
		/// <param name="scale">要设置的缩放比例</param>
		public static void SetLocalScale(this Transform transform , Vector3 scale)
		{
			var s = transform.localScale;

			if (SetPropertyUtility.SetVector3(ref s , scale))
				transform.localScale = s;
		}

		/// <summary>
		/// 设置缩放比例
		/// </summary>
		/// <param name="transform">被扩展的对象</param>
		/// <param name="x">X 轴缩放值</param>
		/// <param name="y">Y 轴缩放值</param>
		/// <param name="z">Z 轴缩放值</param>
		public static void SetLocalScale(this Transform transform , float x , float y , float z)
		{
			var s = transform.localScale;

			if (SetPropertyUtility.SetVector3(ref s , x , y , z))
				transform.localScale = s;
		}

		/// <summary>
		/// 设置Transform的父物体,并对相对坐标赋值
		/// </summary>
		/// <param name="transform">被扩展的对象</param>
		/// <param name="parent">父物体目标</param>
		/// <param name="localPosition">父物体下的相对坐标</param>
		public static void SetParentAndPos(this Transform transform , Transform parent , Vector3 localPosition)
		{
			transform.SetParent(parent , false);
			transform.SetLocalPosition(localPosition);
		}

		/// <summary>
		/// 查找子物体缓存
		/// </summary>
		private static readonly List<Transform> ChildrenCache = new();

		/// <summary>
		/// 获得 Transform 的子物体(仅 Transform 自己的子对象,不进行递归获取)
		/// </summary>
		/// <param name="transform">被扩展的对象</param>
		/// <param name="includeInactive">是否获取未激活的子对象</param>
		/// <returns></returns>
		public static Transform[ ] GetSelfChildren(this Transform transform , bool includeInactive = false)
		{
			if (transform.childCount == 0) return Array.Empty<Transform>();

			ChildrenCache.Clear();

			if (includeInactive)
				foreach (Transform t in transform)
					ChildrenCache.Add(t);
			else
				foreach (Transform t in transform)
					if (t.gameObject.activeSelf)
						ChildrenCache.Add(t);

			return ChildrenCache.ToArray();
		}

		/// <summary>
		/// 获得 Transform 下的所有子物体
		/// </summary>
		/// <param name="transform">被扩展的对象</param>
		/// <param name="includeInactive">是否获取未激活的子对象</param>
		/// <returns></returns>
		public static Transform[ ] GetAllChildren(this Transform transform , bool includeInactive = false)
			=> transform.GetComponentsInChildren<Transform>(includeInactive);

		/// <summary>
		/// 获取 Transform 的树结构
		/// </summary>
		/// <param name="transform">被扩展的对象</param>
		/// <returns></returns>
		public static string GetPath(this Transform transform)
			=> transform.parent == null ? $"/{transform.name}" : $"{transform.parent.GetPath()}/{transform.name}";
	}
}
