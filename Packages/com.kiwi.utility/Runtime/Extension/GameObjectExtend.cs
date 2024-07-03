using UnityEngine;

namespace Kiwi.Utility
{
	public static partial class KiwiExtend
	{
		/// <summary>
		/// 获得 GameObject 的 RectTransform 组件
		/// </summary>
		/// <param name="go">被扩展的对象</param>
		/// <returns>获取成功返回 RectTransform 组件, 否则返回 null.</returns>
		public static RectTransform rectTransform(this GameObject go) => go.IsNull() ? null : go.transform as RectTransform;

		/// <summary>
		/// 强制获得对象组件,如果对象未挂载目标组件,则为该对象添加组件
		/// </summary>
		/// <param name="go">被扩展的对象</param>
		/// <param name="exist">想要获取的组件是否存在</param>
		/// <typeparam name="T">目标组件必须继承自 Component</typeparam>
		/// <returns>目标组件</returns>
		public static T ForceGetComponent< T >(this GameObject go , out bool exist) where T : UnityEngine.Component
		{
			if (go.IsNull())
			{
				exist = false;

				return null;
			}

			var result = go.GetComponent<T>();

			if (result != null)
			{
				exist = true;

				return result;
			}

			result = go.AddComponent<T>();
			exist  = false;

			return result;
		}

		/// <summary>
		/// 强制获得对象组件,如果对象未挂载目标组件,则为该对象添加组件
		/// </summary>
		/// <param name="go">被扩展的对象</param>
		/// <typeparam name="T">目标组件必须继承自 Component</typeparam>
		/// <returns>目标组件</returns>
		public static T ForceGetComponent< T >(this GameObject go) where T : UnityEngine.Component => go.ForceGetComponent<T>(out _);

		/// <summary>
		/// 强制获得对象组件,如果对象未挂载目标组件,则为该对象添加组件
		/// </summary>
		/// <param name="go">被扩展的对象</param>
		/// <param name="type">目标组件必须继承自Component,需要带命名空间的完整名称</param>
		/// <param name="exist">想要获取的组件是否存在</param>
		/// <returns>目标组件</returns>
		public static UnityEngine.Component ForceGetComponent(this GameObject go , string type , out bool exist)
		{
			if (go.IsNull() || string.IsNullOrEmpty(type))
			{
				exist = false;

				return null;
			}

			var componentType = System.Type.GetType(type);

			var result = go.GetComponent(componentType);

			if (result != null)
			{
				exist = true;

				return result;
			}

			result = go.AddComponent(componentType);
			exist  = false;

			return result;
		}

		/// <summary>
		/// 强制获得对象组件,如果对象未挂载目标组件,则为该对象添加组件
		/// </summary>
		/// <param name="go">被扩展的对象</param>
		/// <param name="type">目标组件必须继承自Component,需要带命名空间的完整名称</param>
		/// <returns>目标组件</returns>
		public static UnityEngine.Component ForceGetComponent(this GameObject go , string type) => go.ForceGetComponent(type , out _);

		/// <summary>
		/// 强制获得对象组件,如果对象未挂载目标组件,则为该对象添加组件
		/// </summary>
		/// <param name="go">被扩展的对象</param>
		/// <param name="type">目标组件必须继承自Component</param>
		/// <param name="exist">想要获取的组件是否存在</param>
		/// <returns>目标组件</returns>
		public static UnityEngine.Component ForceGetComponent(this GameObject go , System.Type type , out bool exist)
		{
			if (go.IsNull() || type == null)
			{
				exist = false;

				return null;
			}

			var result = go.GetComponent(type);

			if (result != null)
			{
				exist = true;

				return result;
			}

			result = go.AddComponent(type);
			exist  = false;

			return result;
		}

		/// <summary>
		/// 强制获得对象组件,如果对象未挂载目标组件,则为该对象添加组件
		/// </summary>
		/// <param name="go">被扩展的对象</param>
		/// <param name="type">目标组件必须继承自Component</param>
		/// <returns>目标组件</returns>
		public static UnityEngine.Component ForceGetComponent(this GameObject go , System.Type type) => go.ForceGetComponent(type , out _);

		/// <summary>
		/// 设置 GameObject 的 Layer
		/// </summary>
		/// <param name="go">被扩展的对象</param>
		/// <param name="layer">要设置的 Layer</param>
		/// <param name="syncChildren">是否同时设置子对象的 Layer</param>
		/// <param name="recursion">是否递归获得所有子对象</param>
		/// <param name="includeInactive">是否获取未激活的子对象</param>
		public static void SetLayer(this GameObject go , int layer , bool syncChildren = false , bool recursion = true , bool includeInactive = true)
		{
			if (go.IsNull()) return;

			go.layer = layer;

			if (!syncChildren) return;

			var children = recursion ? go.transform.GetAllChildren(includeInactive) : go.transform.GetSelfChildren(includeInactive);

			foreach (var child in children)
			{
				child.gameObject.layer = layer;
			}
		}

		/// <summary>
		/// 控制对象显隐
		/// </summary>
		/// <param name="go">被扩展的对象</param>
		/// <param name="val">是否要显示</param>
		public static void SetDisplay(this GameObject go , bool val)
		{
			if (go.IsNull()) return;

			var renderers = go.GetComponentsInChildren<Renderer>();

			foreach (var renderer in renderers)
			{
				renderer.enabled = val;
			}
		}

		/// <summary>
		/// 根据指定 tag 查找子对象
		/// </summary>
		/// <param name="go">被扩展的对象</param>
		/// <param name="tag">指定的 tag</param>
		/// <param name="targetChild">返回查找到的目标子对象</param>
		/// <returns>如果查找到指定对象返回 True,否则返回 false</returns>
		public static bool FindChildWithTag(this GameObject go , string tag , out GameObject targetChild)
		{
			if (go.IsNull() || string.IsNullOrEmpty(tag))
			{
				targetChild = null;

				return false;
			}

			var children = go.transform.GetAllChildren(true);

			foreach (var child in children)
			{
				if (!child.CompareTag(tag)) continue;

				targetChild = child.gameObject;

				return true;
			}

			targetChild = null;

			return false;
		}

		/// <summary>
		/// 查找子对象
		/// <para>若存在多个同名的子对象，则返回第一个</para>
		/// </summary>
		/// <param name="go">被扩展的对象</param>
		/// <param name="name">子对象名称</param>
		/// <returns>查找到的子对象</returns>
		public static GameObject FindChild(this GameObject go , string name)
		{
			if (go.IsNull() || string.IsNullOrEmpty(name))
				return null;

			var children = go.transform.GetAllChildren(true);

			if (children == null || children.Length == 0) return null;

			foreach (var t in children)
			{
				if (t.name == name)
					return t.gameObject;
			}

			return null;
		}

		/// <summary>
		/// 获取对象的中心位置坐标
		/// </summary>
		/// <param name="go">被扩展的对象</param>
		/// <returns>对象的中心位置坐标</returns>
		public static Vector3 GetCenterPosition(this GameObject go)
		{
			if (go.IsNull()) return Vector3.zero;

			var children = go.transform.GetAllChildren();

			if (children == null || children.Length == 0)
				return go.transform.position;

			var center = Vector3.zero;

			foreach (var child in children)
			{
				center += child.position;
			}

			return center / children.Length;
		}
	}
}
