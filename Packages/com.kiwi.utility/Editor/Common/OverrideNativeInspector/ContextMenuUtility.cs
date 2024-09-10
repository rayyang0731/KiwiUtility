using UnityEditor;

using UnityEngine;

namespace Kiwi.Utility.Editor.OverrideNativeInspector
{
	/// <summary>
	/// 右键菜单工具
	/// </summary>
	public static class ContextMenuUtility
	{
		/// <summary>
		/// 处理右键菜单
		/// </summary>
		/// <param name="position">控件位置</param>
		/// <param name="menuData">菜单数据</param>
		public static void HandleContextMenu(Rect position , params ContextMenuData[ ] menuData)
		{
			if (Event.current.type != EventType.ContextClick || !position.Contains(Event.current.mousePosition))
				return;

			var menu = new GenericMenu();

			foreach (var data in menuData)
			{
				if (data.IsEnable)
					menu.AddItem(data.MenuName , false , data.Callback);
				else
					menu.AddDisabledItem(data.MenuName);
			}

			menu.ShowAsContext();
			Event.current.Use();
		}
	}

	/// <summary>
	/// 右键菜单数据
	/// </summary>
	public class ContextMenuData
	{
		/// <summary>
		/// 菜单名称
		/// </summary>
		public GUIContent MenuName;

		/// <summary>
		/// 菜单事件回调
		/// </summary>
		public GenericMenu.MenuFunction Callback;

		/// <summary>
		/// 是否启用
		/// </summary>
		public bool IsEnable;
	}
}
