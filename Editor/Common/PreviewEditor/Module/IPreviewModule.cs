using System;

using UnityEngine;

namespace Kiwi.Utility.Editor
{
	/// <summary>
	/// 预览窗体模组接口
	/// </summary>
	public interface IPreviewModule : IDisposable
	{
		void Update() { }

		/// <summary>
		/// 当预览对象发生改变
		/// </summary>
		/// <param name="target">改变后的对象</param>
		void OnTargetChanged(GameObject target) { }

		/// <summary>
		/// 预览窗口上方
		/// </summary>
		void OnTopGUI() { }

		/// <summary>
		/// 覆盖预览窗口
		/// </summary>
		void OnOverlapGUI(Rect rect) { }

		/// <summary>
		/// 预览窗口下方
		/// </summary>
		void OnBottomGUI() { }
	}
}
