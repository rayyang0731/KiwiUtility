using System;

using UnityEditor;

using UnityEngine;

namespace Kiwi.Utility.Editor
{
	public class PreviewEditorWindow : EditorWindow
	{
		private AvatarEditor _avatarEditor;

		[ EditorToolbar(EditorToolbarAttribute.Anchor.Right , "工具" , "模型动画预览") ]
		private static void ShowWindow()
		{
			var window = GetWindow<PreviewEditorWindow>();
			window.titleContent = new("模型动画预览");
			window.Show();
		}

		private void OnEnable()
		{
			_avatarEditor = new AvatarEditor(this);
			_avatarEditor.AddModule(new PreviewAnimationModule(_avatarEditor));
		}

		private void Update()
		{
			_avatarEditor?.Update();
		}

		private void OnGUI()
		{
			
			_avatarEditor.Draw(position);
		}

		private void OnDestroy()
		{
			_avatarEditor.Dispose();
		}
	}
}
