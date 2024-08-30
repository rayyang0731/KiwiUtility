using UnityEditor;

namespace Kiwi.Utility.Editor
{
	/// <summary>
	/// 模型浏览器
	/// </summary>
	public class PreviewEditorWindow : EditorWindow
	{
		private AvatarEditor _avatarEditor;

		[ EditorToolbar(EditorToolbarAttribute.Anchor.Right , "Kiwi" , "浏览器/模型浏览器") ]
		private static void ShowWindow()
		{
			var window = GetWindow<PreviewEditorWindow>();
			window.titleContent = new("模型浏览器");
			window.Show();
		}

		private void OnEnable()
		{
			_avatarEditor = new(this);
			
			// 添加动画播放模块
			_avatarEditor.AddModule<PreviewAnimationModule>();
			// 添加骨骼列表模块
			_avatarEditor.AddModule<ModelBonesModule>();
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
