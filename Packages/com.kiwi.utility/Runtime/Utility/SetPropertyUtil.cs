using System.Collections.Generic;

using UnityEngine;

namespace Kiwi.Utility
{
   /// <summary>
	/// 属性设置工具集
	/// </summary>
	public static class SetPropertyUtility
	{
		#region Color

		/// <summary>
		/// 设置颜色
		/// </summary>
		/// <param name="currentValue">当前颜色值</param>
		/// <param name="newValue">要设置的颜色值</param>
		/// <returns>如果设置成功返回 true,否则返回 false</returns>
		public static bool SetColor(ref Color currentValue, Color newValue)
		{
			if (Math.Approximately(currentValue.r, newValue.r) &&
			    Math.Approximately(currentValue.g, newValue.g) &&
			    Math.Approximately(currentValue.b, newValue.b) &&
			    Math.Approximately(currentValue.a, newValue.a))
				return false;

			currentValue = newValue;
			return true;
		}

		/// <summary>
		/// 设置颜色
		/// </summary>
		/// <param name="currentValue">当前颜色值</param>
		/// <param name="r">要设置的红色色值</param>
		/// <param name="g">要设置的绿色色值</param>
		/// <param name="b">要设置的蓝色色值</param>
		/// <param name="a">要设置的透明度</param>
		/// <returns>如果设置成功返回 true,否则返回 false</returns>
		public static bool SetColor(ref Color currentValue, float r, float g, float b, float a)
		{
			if (Math.Approximately(currentValue.r, r) &&
			    Math.Approximately(currentValue.g, g) &&
			    Math.Approximately(currentValue.b, b) &&
			    Math.Approximately(currentValue.a, a))
				return false;

			currentValue.r = r;
			currentValue.g = g;
			currentValue.b = b;
			currentValue.a = a;
			return true;
		}

		#endregion

		#region Vector2

		/// <summary>
		/// 设置 Vector2
		/// </summary>
		/// <param name="currentValue">当前 Vector2 值</param>
		/// <param name="newValue">要设置的 Vector2 值</param>
		/// <returns>如果设置成功返回 true,否则返回 false</returns>
		public static bool SetVector2(ref Vector2 currentValue, Vector2 newValue)
		{
			if (Math.Approximately(currentValue.x, newValue.x) &&
			    Math.Approximately(currentValue.y, newValue.y))
				return false;

			currentValue = newValue;
			return true;
		}

		/// <summary>
		/// 设置 Vector2
		/// </summary>
		/// <param name="currentValue">当前 Vector2 值</param>
		/// <param name="x">要设置的 x 值</param>
		/// <param name="y">要设置的 y 值</param>
		/// <returns>如果设置成功返回 true,否则返回 false</returns>
		public static bool SetVector2(ref Vector2 currentValue, float x, float y)
		{
			if (Math.Approximately(currentValue.x, x) &&
			    Math.Approximately(currentValue.y, y))
				return false;

			currentValue.Set(x, y);
			return true;
		}

		#endregion

		#region Vector3

		/// <summary>
		/// 设置 Vector3
		/// </summary>
		/// <param name="currentValue">当前 Vector3 值</param>
		/// <param name="newValue">要设置的 Vector3 值</param>
		/// <returns>如果设置成功返回 true,否则返回 false</returns>
		public static bool SetVector3(ref Vector3 currentValue, Vector3 newValue)
		{
			if (Math.Approximately(currentValue.x, newValue.x) &&
			    Math.Approximately(currentValue.y, newValue.y) &&
			    Math.Approximately(currentValue.z, newValue.z))
				return false;

			currentValue = newValue;
			return true;
		}

		/// <summary>
		/// 设置 Vector3
		/// </summary>
		/// <param name="currentValue">当前 Vector3 值</param>
		/// <param name="x">要设置的 x 值</param>
		/// <param name="y">要设置的 y 值</param>
		/// <param name="z">要设置的 z 值</param>
		/// <returns>如果设置成功返回 true,否则返回 false</returns>
		public static bool SetVector3(ref Vector3 currentValue, float x, float y, float z)
		{
			if (Math.Approximately(currentValue.x, x) &&
			    Math.Approximately(currentValue.y, y) &&
			    Math.Approximately(currentValue.z, z))
				return false;

			currentValue.Set(x, y, z);
			return true;
		}

		#endregion
		
		/// <summary>
		/// 设置 Float
		/// </summary>
		/// <param name="currentValue">当前 Float 值</param>
		/// <param name="newValue">要设置的 Float 值</param>
		/// <returns>如果设置成功返回 true,否则返回 false</returns>
		public static bool SetFloat(ref float currentValue, float newValue)
		{
			if (Math.Approximately(currentValue, newValue))
				return false;

			currentValue = newValue;
			return true;
		}

		/// <summary>
		/// 设置结构体数据
		/// </summary>
		/// <param name="currentValue">当前结构体数据</param>
		/// <param name="newValue">要设置的结构体数据</param>
		/// <returns>如果设置成功返回 true,否则返回 false</returns>
		public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
		{
			if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
				return false;

			currentValue = newValue;
			return true;
		}

		/// <summary>
		/// 设置类数据
		/// </summary>
		/// <param name="currentValue">当前类数据</param>
		/// <param name="newValue">要设置的类数据</param>
		/// <returns>如果设置成功返回 true,否则返回 false</returns>
		public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
		{
			if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
				return false;

			currentValue = newValue;
			return true;
		}
	}
}