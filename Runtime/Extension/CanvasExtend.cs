using UnityEngine;

namespace Kiwi.Utility
{
	/// <summary>
	/// Canvas 类方法扩展
	/// </summary>
	public static partial class KiwiExtend
	{
		/// <summary>
		/// 启用 CanvasGroup
		/// </summary>
		/// <param name="canvas">被扩展对象</param>
		public static void EnableCanvasGroup(this Canvas canvas)
		{
			var group = canvas.ForceGetComponent<CanvasGroup>();

			group.alpha          = 1;
			group.blocksRaycasts = true;
			group.interactable   = true;
		}

		/// <summary>
		/// 隐藏 CanvasGroup
		/// </summary>
		/// <param name="canvas">被扩展对象</param>
		public static void DisableCanvasGroup(this GameObject canvas)
		{
			var group = canvas.ForceGetComponent<CanvasGroup>();

			group.alpha          = 0;
			group.blocksRaycasts = false;
			group.interactable   = false;
		}
	}
}
