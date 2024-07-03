using UnityEditor;

using UnityEngine;

namespace Kiwi.Utility.Editor
{
	/// <summary>
	/// 预览窗体事件
	/// <para>fork from Graphi</para>
	/// </summary>
	public static class PreviewEvt
	{
		private static int _hotControlHash = "hotControl".GetHashCode();

		private static MouseCursor _cursor = MouseCursor.Arrow;

		/// <summary>
		/// 执行
		/// </summary>
		/// <param name="rect">可处理拖拽区域范围</param>
		/// <param name="previewRenderUtils">预览渲染工具对象</param>
		/// <param name="bodyPosition">模型包围盒的中心位置</param>
		/// <param name="rot">旋转变化量</param>
		/// <param name="zoom">缩放变化量</param>
		/// <param name="pivotPositionOffset">平移变化量</param>
		/// <returns></returns>
		public static void Execute(Rect rect , PreviewRenderUtility previewRenderUtils , Vector3 bodyPosition , ref Vector2 rot , ref float zoom , ref Vector3 pivotPositionOffset)
		{
			var controlID = GUIUtility.GetControlID(_hotControlHash , FocusType.Passive);
			var curEvt    = Event.current;

			switch (curEvt.GetTypeForControl(controlID))
			{
				case EventType.MouseDown:
				{
					var flag = rect.Contains(curEvt.mousePosition) && rect.width > 50f;

					if (flag)
					{
						GUIUtility.hotControl = controlID; // 鼠标在屏幕外依然可以继续事件
						curEvt.Use();
						EditorGUIUtility.SetWantsMouseJumping(1); // 跳屏开启
					}

					break;
				}

				case EventType.MouseUp:
				{
					var flag = GUIUtility.hotControl == controlID;

					if (flag)
						GUIUtility.hotControl = 0;

					EditorGUIUtility.SetWantsMouseJumping(0);
					_cursor = MouseCursor.Arrow;
					curEvt.Use();

					break;
				}

				case EventType.MouseDrag:
				{
					var flag = GUIUtility.hotControl == controlID;

					if (flag)
					{
						switch (curEvt.button)
						{
							case 0:
							case 2:
								// 鼠标左键或中键 - 平移
								Pan(curEvt , previewRenderUtils , bodyPosition , zoom , ref pivotPositionOffset);
								_cursor = MouseCursor.Pan;

								break;

							case 1:
								// 鼠标右键 - 旋转
								Orbit(curEvt , ref rot , rect);
								_cursor = MouseCursor.Orbit;

								break;
						}
					}

					break;
				}

				case EventType.KeyDown:
				{
					if (curEvt.keyCode == KeyCode.F)
					{
						pivotPositionOffset = Vector3.zero;
						rot                 = new(180f , -10f);
						zoom                = 1.0f;

						curEvt.Use();
					}
				}

					break;

				case EventType.ScrollWheel:
				{
					// 鼠标滚轮 - 缩放
					Zoom(curEvt , ref zoom , 1);
				}

					break;

				case EventType.Repaint:
				{
					// 重绘时修改当前鼠标样式
					EditorGUIUtility.AddCursorRect(rect , _cursor);
				}

					break;
			}
		}


		/// <summary>
		/// 旋转
		/// </summary>
		/// <param name="curEvt">当前事件数据</param>
		/// <param name="rot">旋转值</param>
		/// <param name="rect">可处理拖拽区域范围</param>
		private static void Orbit(Event curEvt , ref Vector2 rot , Rect rect)
		{
			rot   -= curEvt.delta * (!curEvt.shift ? 1 : 3) / Mathf.Min(rect.width , rect.height) * 140f;
			rot.y =  Mathf.Clamp(rot.y , -90f , 90f);
			curEvt.Use();
		}

		/// <summary>
		/// 缩放
		/// </summary>
		/// <param name="curEvt">当前事件数据</param>
		/// <param name="zoom">缩放值</param>
		/// <param name="dir">滚轮方向</param>
		private static void Zoom(Event curEvt , ref float zoom , int dir)
		{
			var delta = (0f + HandleUtility.niceMouseDeltaZoom * dir) * (curEvt.shift ? 2f : 0.5f);
			var num   = (0f - delta) * 0.05f;
			zoom += zoom * num;
			zoom =  Mathf.Max(zoom , 1.0f / 15f);
			curEvt.Use();
		}

		/// <summary>
		/// 平移
		/// </summary>
		/// <param name="curEvt">当前事件数据</param>
		/// <param name="previewRenderUtils">预览渲染工具对象</param>
		/// <param name="bodyPosition">模型位置</param>
		/// <param name="zoom">当前缩放值</param>
		/// <param name="pivotPositionOffset">平移值</param>
		private static void Pan(Event curEvt , PreviewRenderUtility previewRenderUtils , Vector3 bodyPosition , float zoom , ref Vector3 pivotPositionOffset)
		{
			var camera = previewRenderUtils.camera;
			var pos    = camera.WorldToScreenPoint(bodyPosition + pivotPositionOffset);
			var vector = new Vector3(0f - curEvt.delta.x , curEvt.delta.y , 0f);
			pos += vector * Mathf.Lerp(1f , 2f , zoom * 0.5f); // zoom值越小，平移偏移量越小
			var vector2 = camera.ScreenToWorldPoint(pos) - (bodyPosition + pivotPositionOffset);
			pivotPositionOffset += vector2;
			curEvt.Use();
		}
	}
}
