using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace Kiwi.Utility.Editor
{
	/// <summary>
	/// 模型骨骼模块
	/// </summary>
	public class ModelBonesModule : IPreviewModule
	{
		/// <summary>
		/// 骨骼信息
		/// </summary>
		private readonly HashSet<Transform> _bones = new();

		/// <summary>
		/// 全部骨骼
		/// </summary>
		private readonly Dictionary<string , Transform> _allBones = new();

		/// <summary>
		/// 骨骼信息
		/// </summary>
		public HashSet<Transform> Bones => _bones;

		/// <summary>
		/// 全部骨骼
		/// </summary>
		public Dictionary<string , Transform> AllBones => _allBones;

		/// <summary>
		/// 是否显示面板
		/// </summary>
		private bool _isDisplay;

		private Vector2 _scrollPos;

		void IPreviewModule.OnTargetChanged(GameObject target)
		{
			if (target)
			{
				// 读取骨骼信息
				ClearBoneInfos();

				var skinnedMeshRenderers = target.GetComponentsInChildren<SkinnedMeshRenderer>();

				foreach (var renderer in skinnedMeshRenderers)
				{
					var bones = renderer.bones;
					var len   = bones.Length;

					for (var i = 0 ; i < len ; i++)
					{
						_bones.Add(bones[i]);
						_allBones.TryAdd(bones[i].name , bones[i]);
					}
				}
			}
			else
			{
				ClearBoneInfos();
			}
		}

		void IPreviewModule.OnOverlapGUI(Rect rect)
		{
			var btnGUIContent = new GUIContent("骨骼");
			var btnRect       = new Rect(rect.x , rect.y , 50 , EditorGUI.GetPropertyHeight(SerializedPropertyType.String , btnGUIContent));

			var curEvt = Event.current;

			if (curEvt.type == EventType.MouseDown && btnRect.Contains(curEvt.mousePosition))
				_isDisplay = !_isDisplay;
			else if (curEvt.type == EventType.Repaint)
				GUI.Toggle(btnRect , _isDisplay , btnGUIContent);

			if (_isDisplay)
			{
				var scrollViewRect = new Rect(rect.x , rect.y + btnRect.height , rect.width / 4 , rect.height - btnRect.height);

				using (var scrollScope = new GUI.ScrollViewScope(scrollViewRect , _scrollPos , new(0 , 0 , scrollViewRect.width , _bones.Count * 20 ) , false , true))
				{
					_scrollPos = scrollScope.scrollPosition;

					var index = 0;

					foreach (var bone in _bones)
					{
						EditorGUI.LabelField(new Rect(5 , index++ * 20 , scrollViewRect.width - 10 , 20) , bone.name , string.Empty , "box");
					}
				}
			}
		}

		public void Dispose()
		{
			ClearBoneInfos();
		}

		private void ClearBoneInfos()
		{
			_bones.Clear();
			_allBones.Clear();
		}
	}
}
