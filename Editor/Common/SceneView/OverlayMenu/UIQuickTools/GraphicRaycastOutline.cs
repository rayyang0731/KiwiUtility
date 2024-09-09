using UnityEditor;
using UnityEditor.SceneManagement;

using UnityEngine;
using UnityEngine.UI;

namespace Kiwi.Utility.Editor.SceneView.OverlayMenu
{
	/// <summary>
	/// Graphic 射线检测轮廓线显示
	/// </summary>
	[ InitializeOnLoad ]
	public static class GraphicRaycastOutline
	{
		static GraphicRaycastOutline() { UnityEditor.SceneView.duringSceneGui += OnSceneGUI; }

		/// <summary>
		/// 是否显示
		/// </summary>
		public static bool Display = false;

		/// <summary>
		/// 拐点(临时字段)
		/// </summary>
		private static readonly Vector3[ ] Cornets = new Vector3[ 4 ];

		/// <summary>
		/// 轮廓线颜色
		/// </summary>
		internal static Color Color = Color.cyan;

		/// <summary>
		/// 当前场景中的全部 Graphic
		/// </summary>
		private static Graphic[ ] _graphics;

		/// <summary>
		/// 轮廓线宽度
		/// </summary>
		private const float CONST_THICKNESS = 2;

		private static void OnSceneGUI(UnityEditor.SceneView obj)
		{
			if (!Display) return;

			Handles.color = Color;

			_graphics = StageUtility.GetCurrentStageHandle().FindComponentsOfType<Graphic>();

			foreach (var graphic in _graphics)
			{
				if (!graphic.raycastTarget) continue;

				var graphicRT = graphic.rectTransform;
				graphicRT.GetWorldCorners(Cornets);

				Handles.DrawLine(Cornets[0] , Cornets[1] , CONST_THICKNESS);
				Handles.DrawLine(Cornets[1] , Cornets[2] , CONST_THICKNESS);
				Handles.DrawLine(Cornets[2] , Cornets[3] , CONST_THICKNESS);
				Handles.DrawLine(Cornets[3] , Cornets[0] , CONST_THICKNESS);
			}
		}
	}
}
