using System;
using System.Linq;

using UnityEngine;

namespace Kiwi.Utility
{
	/// <summary>
	/// 水平锚点枚举
	/// </summary>
	public enum AnchorHorizontal
	{
		Left,
		Center,
		Right,
		Stretch,
	}

	/// <summary>
	/// 垂直锚点枚举
	/// </summary>
	public enum AnchorVertical
	{
		Top,
		Middle,
		Bottom,
		Stretch
	}

	/// <summary>
	/// 锚点预设枚举
	/// </summary>
	public enum AnchorPresets
	{
		TopLeft = 1,
		TopCenter,
		TopRight,

		MiddleLeft,
		MiddleCenter,
		MiddleRight,

		BottomLeft,
		BottomCenter,
		BottomRight,

		VertStretchLeft,
		VertStretchRight,
		VertStretchCenter,

		HorStretchTop,
		HorStretchMiddle,
		HorStretchBottom,

		StretchAll,
	}

	/// <summary>
	/// 重点点预设枚举
	/// </summary>
	public enum PivotPresets
	{
		TopLeft = 1,
		TopCenter,
		TopRight,

		MiddleLeft,
		MiddleCenter,
		MiddleRight,

		BottomLeft,
		BottomCenter,
		BottomRight,
	}

	/// <summary>
	/// RectTransform 类方法扩展
	/// </summary>
	public static partial class KiwiExtend
	{
		/// <summary>
		/// 将位置,宽高,锚点,中心点全部重置
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="horizontal">水平锚点位置</param>
		/// <param name="vertical">垂直锚点位置</param>
		/// <param name="parent">父物体</param>
		public static void ResetZero(this RectTransform rectTransform, AnchorHorizontal horizontal,
		                             AnchorVertical vertical, Transform parent = null)
		{
			SetAnchorPresets(rectTransform, horizontal, vertical);
			if (parent != null) rectTransform.SetParent(parent);

			var anchoredPosition = rectTransform.anchoredPosition;
			anchoredPosition.Set(0, 0);
			rectTransform.anchoredPosition = anchoredPosition;

			var sizeDelta = rectTransform.sizeDelta;
			sizeDelta.Set(0, 0);
			rectTransform.sizeDelta = sizeDelta;

			var pivot = rectTransform.pivot;
			pivot.Set(0.5f, 0.5f);
			rectTransform.pivot = pivot;
		}

		/// <summary>
		/// 将位置,宽高,锚点,中心点全部重置
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="horizontal">水平锚点位置 <see cref="AnchorHorizontal"/></param>
		/// <param name="vertical">垂直锚点位置 <see cref="AnchorVertical"/></param>
		/// <param name="parent">父物体</param>
		public static void ResetZero(this RectTransform rectTransform, int horizontal, int vertical, Transform parent = null)
			=> rectTransform.ResetZero((AnchorHorizontal) horizontal, (AnchorVertical) vertical, parent);

		/// <summary>
		/// 设置预设锚点
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="horizontal">水平锚点位置 [0-Left,1-Center,2-Right,3-Stretch]</param>
		/// <param name="vertical">垂直锚点位置 [0-Top,1-Middle,2-Bottom,3-Stretch]</param>
		public static void SetAnchorPresets(this RectTransform rectTransform, int horizontal, int vertical)
			=> rectTransform.SetAnchorPresets((AnchorHorizontal) horizontal, (AnchorVertical) vertical);

		/// <summary>
		/// 设置预设锚点
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="horizontal">水平锚点位置</param>
		/// <param name="vertical">垂直锚点位置</param>
		public static void SetAnchorPresets(this RectTransform rectTransform, AnchorHorizontal horizontal,
		                                    AnchorVertical vertical)
		{
			var min = rectTransform.anchorMin;
			var max = rectTransform.anchorMax;

			var anchorHor = GetAnchorHorizontalValue(horizontal);
			var anchorVer = GetAnchorVerticalValue(vertical);

			min.Set(anchorHor.x, anchorVer.x);
			max.Set(anchorHor.y, anchorVer.y);

			rectTransform.anchorMin = min;
			rectTransform.anchorMax = max;

			var pivot = rectTransform.pivot;
			pivot.Set(min.x, max.y);
			rectTransform.pivot = pivot;
		}


		/// <summary>
		/// 设置水平预设锚点
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="horizontal">水平锚点位置 <see cref="AnchorHorizontal"/> [0-Left,1-Center,2-Right,3-Stretch]</param>
		public static void SetAnchorHorizontal(this RectTransform rectTransform, int horizontal)
			=> rectTransform.SetAnchorHorizontal((AnchorHorizontal) horizontal);

		/// <summary>
		/// 设置水平预设锚点
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="horizontal">水平锚点位置</param>
		public static void SetAnchorHorizontal(this RectTransform rectTransform, AnchorHorizontal horizontal)
		{
			var min    = rectTransform.anchorMin;
			var max    = rectTransform.anchorMax;
			var anchor = GetAnchorHorizontalValue(horizontal);

			min.x                   = anchor.x;
			max.x                   = anchor.y;
			rectTransform.anchorMin = min;
			rectTransform.anchorMax = max;

			rectTransform.pivot = anchor;
		}

		/// <summary>
		/// 设置垂直锚点
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="vertical">垂直锚点位置 <see cref="AnchorVertical"/> [0-Top,1-Middle,2-Bottom,3-Stretch]</param>
		public static void SetAnchorVertical(this RectTransform rectTransform, int vertical)
			=> rectTransform.SetAnchorVertical((AnchorVertical) vertical);

		/// <summary>
		/// 设置垂直锚点
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="vertical">垂直锚点位置</param>
		public static void SetAnchorVertical(this RectTransform rectTransform, AnchorVertical vertical)
		{
			var min    = rectTransform.anchorMin;
			var max    = rectTransform.anchorMax;
			var anchor = GetAnchorVerticalValue(vertical);

			min.x                   = anchor.x;
			max.x                   = anchor.y;
			rectTransform.anchorMin = min;
			rectTransform.anchorMax = max;

			rectTransform.pivot = anchor;
		}

		/// <summary>
		/// 设置锚点
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="anchorPresets">要设置的锚点预设 <see cref="AnchorPresets"/></param>
		/// <param name="posX">设置锚点后,要设置的 X 轴坐标</param>
		/// <param name="posY">设置锚点后,要设置的 Y 轴坐标</param>
		public static void SetAnchor(this RectTransform rectTransform, int anchorPresets, float posX = 0, float posY = 0)
			=> rectTransform.SetAnchor((AnchorPresets) anchorPresets, posX, posY);

		/// <summary>
		/// 设置锚点
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="anchorPresets">要设置的锚点预设</param>
		/// <param name="posX">设置锚点后,要设置的 X 轴坐标</param>
		/// <param name="posY">设置锚点后,要设置的 Y 轴坐标</param>
		public static void SetAnchor(this RectTransform rectTransform, AnchorPresets anchorPresets, float posX = 0, float posY = 0)
		{
			var anchorMin = rectTransform.anchorMin;
			var anchorMax = rectTransform.anchorMax;

			switch (anchorPresets)
			{
				case (AnchorPresets.TopLeft):
					anchorMin.Set(0, 1);
					anchorMax.Set(0, 1);
					break;

				case (AnchorPresets.TopCenter):
					anchorMin.Set(0.5f, 1);
					anchorMax.Set(0.5f, 1);
					break;

				case (AnchorPresets.TopRight):
					anchorMin.Set(1, 1);
					anchorMax.Set(1, 1);
					break;

				case (AnchorPresets.MiddleLeft):
					anchorMin.Set(0, 0.5f);
					anchorMax.Set(0, 0.5f);
					break;

				case (AnchorPresets.MiddleCenter):
					anchorMin.Set(0.5f, 0.5f);
					anchorMax.Set(0.5f, 0.5f);
					break;

				case (AnchorPresets.MiddleRight):
					anchorMin.Set(1, 0.5f);
					anchorMax.Set(1, 0.5f);
					break;

				case (AnchorPresets.BottomLeft):
					anchorMin.Set(0, 0);
					anchorMax.Set(0, 0);
					break;

				case (AnchorPresets.BottomCenter):
					anchorMin.Set(0.5f, 0);
					anchorMax.Set(0.5f, 0);
					break;

				case (AnchorPresets.BottomRight):
					anchorMin.Set(1, 0);
					anchorMax.Set(1, 0);
					break;

				case (AnchorPresets.HorStretchTop):
					anchorMin.Set(0, 1);
					anchorMax.Set(1, 1);
					break;

				case (AnchorPresets.HorStretchMiddle):
					anchorMin.Set(0, 0.5f);
					anchorMax.Set(1, 0.5f);
					break;

				case (AnchorPresets.HorStretchBottom):
					anchorMin.Set(0, 0);
					anchorMax.Set(1, 0);
					break;

				case (AnchorPresets.VertStretchLeft):
					anchorMin.Set(0, 0);
					anchorMax.Set(0, 1);
					break;

				case (AnchorPresets.VertStretchCenter):
					anchorMin.Set(0.5f, 0);
					anchorMax.Set(0.5f, 1);
					break;

				case (AnchorPresets.VertStretchRight):
					anchorMin.Set(1, 0);
					anchorMax.Set(1, 1);
					break;

				case (AnchorPresets.StretchAll):
					anchorMin.Set(0, 0);
					anchorMax.Set(1, 1);
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(anchorPresets), anchorPresets, null);
			}

			rectTransform.anchorMin = anchorMin;
			rectTransform.anchorMax = anchorMax;

			var anchoredPos = rectTransform.anchoredPosition;
			anchoredPos.Set(posX, posY);
			rectTransform.anchoredPosition = anchoredPos;
		}

		/// <summary>
		/// 设置锚点最小值
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="x">锚点 X 轴坐标</param>
		/// <param name="y">锚点 Y 轴坐标</param>
		public static void SetAnchorMin(this RectTransform rectTransform, float x, float y)
		{
			var anchorMin = rectTransform.anchorMin;
			anchorMin.Set(x, y);
			rectTransform.anchorMin = anchorMin;
		}

		/// <summary>
		/// 设置锚点 X 轴最小值
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="x">锚点 X 轴坐标</param>
		public static void SetAnchorMinX(this RectTransform rectTransform, float x)
		{
			var anchorMin = rectTransform.anchorMin;
			anchorMin.Set(x, anchorMin.y);
			rectTransform.anchorMin = anchorMin;
		}

		/// <summary>
		/// 设置锚点 Y 轴最小值
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="y">锚点 Y 轴坐标</param>
		public static void SetAnchorMinY(this RectTransform rectTransform, float y)
		{
			var anchorMin = rectTransform.anchorMin;
			anchorMin.Set(anchorMin.x, y);
			rectTransform.anchorMin = anchorMin;
		}

		/// <summary>
		/// 设置锚点最大值
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="x">锚点 X 轴坐标</param>
		/// <param name="y">锚点 Y 轴坐标</param>
		public static void SetAnchorMax(this RectTransform rectTransform, float x, float y)
		{
			var anchorMax = rectTransform.anchorMax;
			anchorMax.Set(x, y);
			rectTransform.anchorMax = anchorMax;
		}

		/// <summary>
		/// 设置锚点 X 轴最大值
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="x">锚点 X 轴坐标</param>
		public static void SetAnchorMaxX(this RectTransform rectTransform, float x)
		{
			var anchorMax = rectTransform.anchorMax;
			anchorMax.Set(x, anchorMax.y);
			rectTransform.anchorMax = anchorMax;
		}

		/// <summary>
		/// 设置锚点 Y 轴最大值
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="y">锚点 Y 轴坐标</param>
		public static void SetAnchorMaxY(this RectTransform rectTransform, float y)
		{
			var anchorMax = rectTransform.anchorMax;
			anchorMax.Set(anchorMax.x, y);
			rectTransform.anchorMax = anchorMax;
		}

		/// <summary>
		/// 设置中心点
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="x">X 轴坐标</param>
		/// <param name="y">Y 轴坐标</param>
		public static void SetPivot(this RectTransform rectTransform, float x, float y)
		{
			var pivot = rectTransform.pivot;
			pivot.Set(x, y);
			rectTransform.pivot = pivot;
		}

		/// <summary>
		/// 设置中心点
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="preset">预设中心点 <see cref="PivotPresets"/></param>
		public static void SetPivot(this RectTransform rectTransform, int preset) => rectTransform.SetPivot((PivotPresets) preset);

		/// <summary>
		/// 设置中心点
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="preset">预设中心点</param>
		public static void SetPivot(this RectTransform rectTransform, PivotPresets preset)
		{
			var pivot = rectTransform.pivot;

			switch (preset)
			{
				case (PivotPresets.TopLeft):
					pivot.Set(0, 1);
					break;
				case (PivotPresets.TopCenter):
					pivot.Set(0.5f, 1);
					break;
				case (PivotPresets.TopRight):
					pivot.Set(1, 1);
					break;
				case (PivotPresets.MiddleLeft):
					pivot.Set(0, 0.5f);
					break;
				case (PivotPresets.MiddleCenter):
					pivot.Set(0.5f, 0.5f);
					break;
				case (PivotPresets.MiddleRight):
					pivot.Set(1, 0.5f);
					break;
				case (PivotPresets.BottomLeft):
					pivot.Set(0, 0);
					break;
				case (PivotPresets.BottomCenter):
					pivot.Set(0.5f, 0);
					break;
				case (PivotPresets.BottomRight):
					pivot.Set(1, 0);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(preset), preset, null);
			}

			rectTransform.pivot = pivot;
		}

		/// <summary>
		/// 是否与指定的预设锚点相等
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="horizontal">水平锚点位置 <see cref="AnchorHorizontal"/> [0-Left,1-Center,2-Right,3-Stretch]</param>
		/// <param name="vertical">垂直锚点位置 <see cref="AnchorVertical"/> [0-Top,1-Middle,2-Bottom,3-Stretch]</param>
		/// <returns></returns>
		public static bool IsAnchorPresets(this RectTransform rectTransform, int horizontal, int vertical)
			=> rectTransform.IsAnchorPresets((AnchorHorizontal) horizontal, (AnchorVertical) vertical);

		/// <summary>
		/// 是否与指定的预设锚点相等
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="horizontal">水平锚点位置</param>
		/// <param name="vertical">垂直锚点位置</param>
		/// <returns></returns>
		public static bool IsAnchorPresets(this RectTransform rectTransform, AnchorHorizontal horizontal, AnchorVertical vertical)
			=> IsAnchorHorizontal(rectTransform, horizontal) && IsAnchorVertical(rectTransform, vertical);

		/// <summary>
		/// 是否与指定的水平预设锚点相等
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="horizontal">水平锚点位置 <see cref="AnchorHorizontal"/> [0-Left,1-Center,2-Right,3-Stretch]</param>
		/// <returns></returns>
		public static bool IsAnchorHorizontal(this RectTransform rectTransform, int horizontal)
			=> rectTransform.IsAnchorHorizontal((AnchorHorizontal) horizontal);

		/// <summary>
		/// 是否与指定的水平预设锚点相等
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="horizontal">水平锚点位置</param>
		/// <returns></returns>
		public static bool IsAnchorHorizontal(this RectTransform rectTransform, AnchorHorizontal horizontal)
		{
			var anchor = GetAnchorHorizontalValue(horizontal);
			return Math.Approximately(rectTransform.anchorMin.x, anchor.x) && Mathf.Approximately(rectTransform.anchorMax.x, anchor.y);
		}

		/// <summary>
		/// 是否与指定的垂直预设锚点相等
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="vertical">垂直预设锚点位置 <see cref="AnchorVertical"/> [0-Top,1-Middle,2-Bottom,3-Stretch]</param>
		/// <returns></returns>
		public static bool IsAnchorVertical(this RectTransform rectTransform, int vertical)
			=> rectTransform.IsAnchorVertical((AnchorVertical) vertical);

		/// <summary>
		/// 是否与指定的垂直预设锚点值相等
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="vertical">垂直锚点位置</param>
		/// <returns></returns>
		public static bool IsAnchorVertical(this RectTransform rectTransform, AnchorVertical vertical)
		{
			var anchor = GetAnchorVerticalValue(vertical);
			return Mathf.Approximately(rectTransform.anchorMin.y, anchor.x) && Mathf.Approximately(rectTransform.anchorMax.y, anchor.y);
		}

		/// <summary>
		/// 获得锚点坐标
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="x">x 轴坐标</param>
		/// <param name="y">y 轴坐标</param>
		public static void GetAnchoredPosition(this RectTransform rectTransform, out float x, out float y)
		{
			var pos = rectTransform.anchoredPosition;

			x = pos.x;
			y = pos.y;
		}

		/// <summary>
		/// 设置锚点坐标
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="anchoredPosition">要设置的锚点坐标</param>
		public static void SetAnchoredPosition(this RectTransform rectTransform, Vector2 anchoredPosition)
		{
			var pos = rectTransform.anchoredPosition;

			if (SetPropertyUtility.SetVector2(ref pos, anchoredPosition))
				rectTransform.anchoredPosition = pos;
		}

		/// <summary>
		/// 设置锚点坐标
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="x">X 轴坐标</param>
		/// <param name="y">Y 轴坐标</param>
		public static void SetAnchoredPosition(this RectTransform rectTransform, float x, float y)
		{
			var pos = rectTransform.anchoredPosition;

			if (SetPropertyUtility.SetVector2(ref pos, x, y))
				rectTransform.anchoredPosition = pos;
		}

		/// <summary>
		/// 设置锚点 X 轴坐标
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="x">要设置的 X 轴坐标</param>
		public static void SetAnchoredPositionX(this RectTransform rectTransform, float x)
		{
			var pos = rectTransform.anchoredPosition;

			if (!Math.Approximately(pos.x, x))
				pos.x = x;

			rectTransform.anchoredPosition = pos;
		}

		/// <summary>
		/// 设置锚点 Y 轴坐标
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="y">要设置的 Y 轴坐标</param>
		public static void SetAnchoredPositionY(this RectTransform rectTransform, float y)
		{
			var pos = rectTransform.anchoredPosition;

			if (!Math.Approximately(pos.y, y))
				pos.y = y;

			rectTransform.anchoredPosition = pos;
		}

		/// <summary>
		/// 获得3D锚点坐标
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="x">x 轴坐标</param>
		/// <param name="y">y 轴坐标</param>
		/// <param name="z">z 轴坐标</param>
		public static void GetAnchoredPosition3D(this RectTransform rectTransform, out float x, out float y, out float z)
		{
			var pos = rectTransform.anchoredPosition3D;

			x = pos.x;
			y = pos.y;
			z = pos.z;
		}

		/// <summary>
		/// 设置锚点 3D 坐标
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="anchoredPosition3D">要设置的锚点 3D 坐标</param>
		public static void SetAnchoredPosition3D(this RectTransform rectTransform, Vector3 anchoredPosition3D)
		{
			var pos = rectTransform.anchoredPosition3D;

			if (SetPropertyUtility.SetVector3(ref pos, anchoredPosition3D))
				rectTransform.anchoredPosition3D = pos;
		}

		/// <summary>
		/// 设置锚点 3D 坐标
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="x">要设置的 X 轴坐标</param>
		/// <param name="y">要设置的 Y 轴坐标</param>
		/// <param name="z">要设置的 Z 轴坐标</param>
		public static void SetAnchoredPosition3D(this RectTransform rectTransform, float x, float y, float z)
		{
			var pos = rectTransform.anchoredPosition3D;

			if (SetPropertyUtility.SetVector3(ref pos, x, y, z))
				rectTransform.anchoredPosition3D = pos;
		}

		/// <summary>
		/// 设置锚点 X 轴 3D 坐标
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="x">要设置的 X 轴坐标</param>
		public static void SetAnchoredPosition3DX(this RectTransform rectTransform, float x)
		{
			var pos = rectTransform.anchoredPosition3D;

			if (!Math.Approximately(pos.x, x))
				pos.x = x;

			rectTransform.anchoredPosition3D = pos;
		}

		/// <summary>
		/// 设置锚点 Y 轴 3D 坐标
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="y">要设置的 Y 轴坐标</param>
		public static void SetAnchoredPosition3DY(this RectTransform rectTransform, float y)
		{
			var pos = rectTransform.anchoredPosition3D;

			if (!Math.Approximately(pos.y, y))
				pos.y = y;

			rectTransform.anchoredPosition3D = pos;
		}

		/// <summary>
		/// 设置锚点 Z 轴 3D 坐标
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="z">要设置的 Z 轴坐标</param>
		public static void SetAnchoredPosition3DZ(this RectTransform rectTransform, float z)
		{
			var pos = rectTransform.anchoredPosition3D;

			if (!Math.Approximately(pos.z, z))
				pos.z = z;

			rectTransform.anchoredPosition3D = pos;
		}

		/// <summary>
		/// 获取宽度
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <returns></returns>
		public static float GetWidth(this RectTransform rectTransform) => rectTransform.rect.width;

		/// <summary>
		/// 获取高度
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <returns></returns>
		public static float GetHeight(this RectTransform rectTransform) => rectTransform.rect.height;

		/// <summary>
		/// 获取尺寸
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <returns></returns>
		public static Vector2 GetSize(this RectTransform rectTransform) => rectTransform.rect.size;

		/// <summary>
		/// 获取尺寸
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="width">返回的宽度</param>
		/// <param name="height">返回的高度</param>
		public static void GetSize(this RectTransform rectTransform, out float width, out float height)
		{
			var size = rectTransform.rect.size;
			width  = size.x;
			height = size.y;
		}

		/// <summary>
		/// 设置宽度
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="width">要设置的宽度</param>
		public static void SetWidth(this RectTransform rectTransform, float width)
			=> rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);

		/// <summary>
		/// 设置高度
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="height">要设置的高度</param>
		public static void SetHeight(this RectTransform rectTransform, float height)
			=> rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

		/// <summary>
		/// 设置尺寸
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="width">要设置的宽度</param>
		/// <param name="height">要设置的高度</param>
		public static void SetSize(this RectTransform rectTransform, float width, float height)
		{
			rectTransform.SetWidth(width);
			rectTransform.SetHeight(height);
		}

		/// <summary>
		/// 设置尺寸
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="size">要设置的尺寸</param>
		public static void SetSize(this RectTransform rectTransform, Vector2 size)
			=> rectTransform.SetSize(size.x, size.y);

		/// <summary>
		/// 获取 Transform 的 SizeDelta
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public static void GetSizeDelta(this RectTransform rectTransform, out float x, out float y)
		{
			var delta = rectTransform.sizeDelta;

			x = delta.x;
			y = delta.y;
		}

		/// <summary>
		/// 设置 SizeDelta
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="sizeDelta"></param>
		public static void SetSizeDelta(this RectTransform rectTransform, Vector2 sizeDelta)
		{
			var delta = rectTransform.sizeDelta;

			if (SetPropertyUtility.SetVector2(ref delta, sizeDelta))
				rectTransform.sizeDelta = delta;
		}

		/// <summary>
		/// 设置 SizeDelta
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public static void SetSizeDelta(this RectTransform rectTransform, float x, float y)
		{
			var delta = rectTransform.sizeDelta;

			if (SetPropertyUtility.SetVector2(ref delta, x, y))
				rectTransform.sizeDelta = delta;
		}

		/// <summary>
		/// 全延展并轴点居中
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		public static void SetFullStretch(this RectTransform rectTransform)
		{
			rectTransform.anchoredPosition = Vector2.zero;
			rectTransform.anchorMin        = Vector2.zero;
			rectTransform.anchorMax        = Vector2.one;
			rectTransform.pivot            = Vector2.one * 0.5f;
			rectTransform.sizeDelta        = Vector2.zero;
		}

		/// <summary>
		/// 判断两个 RectTransform 是否相交(矩形相交)
		/// <para>主要就是检测 RectTransform 的拐点是否在另一个 RectTransform 中.</para>
		/// </summary>
		/// <param name="rectTransform">被扩展的对象</param>
		/// <param name="target">要判断的目标对象</param>
		/// <returns></returns>
		public static bool Overlaps(this RectTransform rectTransform, RectTransform target)
		{
			var corner1 = new Vector3[4];
			rectTransform.GetWorldCorners(corner1);

			var corner2 = new Vector3[4];
			target.GetWorldCorners(corner2);

			bool IsPointInRect(Vector3 lb, Vector3 rt, Vector3 point)
				=> point.x <= rt.x &&
				   point.x >= lb.x &&
				   point.y <= rt.y &&
				   point.y >= lb.y;

			return corner1.Any(point => IsPointInRect(corner2[0], corner2[2], point)) ||
			       corner2.Any(point => IsPointInRect(corner1[0], corner1[2], point));
		}

		/// <summary>
		/// 获得指定水平锚点位置的值
		/// </summary>
		/// <param name="horizontal">水平锚点位置</param>
		/// <returns></returns>
		private static Vector2 GetAnchorHorizontalValue(AnchorHorizontal horizontal)
		{
			return horizontal switch
			       {
				       AnchorHorizontal.Center  => new Vector2(0.5f, 0.5f),
				       AnchorHorizontal.Left    => Vector2.zero,
				       AnchorHorizontal.Right   => Vector2.one,
				       AnchorHorizontal.Stretch => Vector2.up,
				       _                        => new Vector2(0.5f, 0.5f),
			       };
		}

		/// <summary>
		/// 获得指定垂直锚点位置的值
		/// </summary>
		/// <param name="vertical">垂直锚点位置</param>
		/// <returns></returns>
		private static Vector2 GetAnchorVerticalValue(AnchorVertical vertical)
		{
			return vertical switch
			       {
				       AnchorVertical.Middle  => new Vector2(0.5f, 0.5f),
				       AnchorVertical.Top     => Vector2.one,
				       AnchorVertical.Bottom  => Vector2.zero,
				       AnchorVertical.Stretch => Vector2.up,
				       _                      => new Vector2(0.5f, 0.5f),
			       };
		}
	}
}