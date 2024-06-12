using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEditor;
using UnityEditor.Toolbars;

using UnityEngine;
using UnityEngine.UIElements;

namespace Kiwi.Utility.Editor
{
	/// <summary>
	/// 编辑器工具栏扩展
	/// </summary>
	[ InitializeOnLoad ]
	public static class EditorToolbarExtender
	{
		private static readonly Type UnityEditorToolbarType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.Toolbar");

		private static VisualElement mRoot;

		private static readonly MenuGroup LeftMenuGroup = new();
		private static readonly MenuGroup CenterLeftMenuGroup = new();
		private static readonly MenuGroup CenterRightMenuGroup = new();
		private static readonly MenuGroup RightMenuGroup = new();

		static EditorToolbarExtender()
		{
			EditorApplication.delayCall += BuildToolbarExtendGUI;
		}

		/// <summary>
		/// 构建工具栏的扩展UI
		/// </summary>
		private static void BuildToolbarExtendGUI()
		{
			if (mRoot != null) return;
			var toolbars = Resources.FindObjectsOfTypeAll(UnityEditorToolbarType);

			if (toolbars.Length <= 0) return;
			var rootProperty = toolbars[0].GetType().GetField("m_Root" , BindingFlags.NonPublic | BindingFlags.Instance);

			if (rootProperty == null) return;

			var rootPropertyValue = rootProperty.GetValue(toolbars[0]);

			if (rootPropertyValue is VisualElement root)
			{
				mRoot = root;

				GetAllToolbarElement();

				CreateButton(EditorToolbarMenuAttribute.Anchor.Left);
				CreateButton(EditorToolbarMenuAttribute.Anchor.CenterLeft);
				CreateButton(EditorToolbarMenuAttribute.Anchor.CenterRight);
				CreateButton(EditorToolbarMenuAttribute.Anchor.Right);
			}
		}

		/// <summary>
		/// 创建指定位置的Toolbar GUI
		/// </summary>
		/// <param name="anchor">位置</param>
		private static void CreateButton(EditorToolbarMenuAttribute.Anchor anchor)
		{
			VisualElement root;
			MenuGroup     menuGroup;

			switch (anchor)
			{
				case EditorToolbarMenuAttribute.Anchor.Left:
					root      = mRoot.Q<VisualElement>("ToolbarZoneLeftAlign");
					menuGroup = LeftMenuGroup;

					break;

				case EditorToolbarMenuAttribute.Anchor.CenterLeft:
					root      = mRoot.Q<VisualElement>("ToolbarZonePlayMode");
					menuGroup = CenterLeftMenuGroup;

					break;

				case EditorToolbarMenuAttribute.Anchor.CenterRight:
					root      = mRoot.Q<VisualElement>("ToolbarZonePlayMode");
					menuGroup = CenterRightMenuGroup;

					break;

				case EditorToolbarMenuAttribute.Anchor.Right:
					root      = mRoot.Q<VisualElement>("ToolbarZoneRightAlign");
					menuGroup = RightMenuGroup;

					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(anchor) , anchor , null);
			}

			if (root == null || menuGroup == null) return;

			foreach (var (category , genericMenu) in menuGroup.GenericMenus)
			{
				var button = new EditorToolbarDropdown
				             {
					             text = category ,
				             };
				button.clicked += () =>
				{
					genericMenu.DropDown(button.worldBound);
				};

				if (anchor == EditorToolbarMenuAttribute.Anchor.CenterLeft)
					root.Insert(0 , button);
				else
					root.Add(button);
			}

			foreach (var (category , action) in menuGroup.Buttons)
			{
				var button = new EditorToolbarButton()
				             {
					             text = category ,
				             };
				button.clicked += action.Invoke;

				if (anchor == EditorToolbarMenuAttribute.Anchor.CenterLeft)
					root.Insert(0 , button);
				else
					root.Add(button);
			}

			foreach (var action in menuGroup.Actions)
			{
				var imguiContainer = new IMGUIContainer();
				imguiContainer.onGUIHandler = action;

				if (anchor == EditorToolbarMenuAttribute.Anchor.CenterLeft)
					root.Insert(0 , imguiContainer);
				else
					root.Add(imguiContainer);
			}
		}

		/// <summary>
		/// 获取全部工具栏元素
		/// </summary>
		private static void GetAllToolbarElement()
		{
			var methodInfos = TypeCache.GetMethodsWithAttribute<EditorToolbarMenuAttribute>()
			                           .Where(methodInfo => methodInfo.IsStatic)
			                           .OrderBy(methodInfo => methodInfo.GetCustomAttribute<EditorToolbarMenuAttribute>().Order)
			                           .ToArray();
			var orderPrev = -1;

			foreach (var methodInfo in methodInfos)
			{
				var attr = methodInfo.GetCustomAttribute<EditorToolbarMenuAttribute>();

				var menuGroup = attr.Position switch
				                {
					                EditorToolbarMenuAttribute.Anchor.Left        => LeftMenuGroup ,
					                EditorToolbarMenuAttribute.Anchor.CenterLeft  => CenterLeftMenuGroup ,
					                EditorToolbarMenuAttribute.Anchor.CenterRight => CenterRightMenuGroup ,
					                EditorToolbarMenuAttribute.Anchor.Right       => RightMenuGroup ,
					                _                                             => throw new ArgumentOutOfRangeException()
				                };

				if (!string.IsNullOrEmpty(attr.Path))
				{
					// 下拉菜单形式
					if (!menuGroup.GenericMenus.ContainsKey(attr.Category))
						menuGroup.GenericMenus.Add(attr.Category , new());

					if (orderPrev != -1 && attr.Order - orderPrev > 10)
						menuGroup.GenericMenus[attr.Category].AddSeparator("");

					var info = methodInfo;
					menuGroup.GenericMenus[attr.Category].AddItem(new(attr.Path) , false , () => { info.Invoke(null , null); });

					orderPrev = attr.Order;
				}
				else
				{
					if (attr.Custom)
					{
						// 自定义形式
						menuGroup.Actions.Add(() => { methodInfo.Invoke(null , null); });
					}
					else
					{
						// 按钮形式
						if (!menuGroup.Buttons.ContainsKey(attr.Category))
							menuGroup.Buttons.Add(attr.Category , () => { methodInfo.Invoke(null , null); });
					}
				}
			}
		}

		private class MenuGroup
		{
			public readonly Dictionary<string , GenericMenu> GenericMenus = new();
			public readonly Dictionary<string , Action> Buttons = new();
			public readonly List<Action> Actions = new();
		}
	}
}
