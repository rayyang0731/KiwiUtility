using UnityEditor;

using UnityEngine;

namespace Kiwi.Utility.Editor.OverrideNativeInspector
{
	/// <summary>
	/// 扩展 RectTransform 编辑器面板
	/// </summary>
	[ CustomEditor(typeof(RectTransform) , true) ]
	public class RectTransformEditor : DecoratorEditor
	{
		public RectTransformEditor() : base("RectTransformEditor") { }

		private RectTransform _rectTransform;

		private Vector4 _tempRotationV4;

		private Quaternion _tempRotation;

		private static bool _otherPropertyFoldOut;



		private void OnEnable()
		{
			_rectTransform  = target as RectTransform;
			_tempRotationV4 = new();
			_tempRotation   = new();
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			serializedObject.Update();

			EditorGUILayout.Space();

			_otherPropertyFoldOut = EditorGUILayout.Foldout(_otherPropertyFoldOut , GUIContentPreset.OtherProperty);

			if (EditorGUILayout.BeginFadeGroup(_otherPropertyFoldOut ? 1 : 0))
			{
				if (!ReferenceEquals(_rectTransform , null))
				{
					Undo.RecordObject(target , "RectTransform Extra Property");

					using (new EditorGUI.IndentLevelScope())
					{
						_rectTransform.position = EditorGUILayout.Vector3Field(GUIContentPreset.WorldPosition , _rectTransform.position);
						ContextMenuUtility.HandleContextMenu(GUILayoutUtility.GetLastRect() ,
						                                     new ContextMenuData()
						                                     {
							                                     MenuName = GUIContentPreset.Copy ,
							                                     IsEnable = true ,
							                                     Callback = () =>
							                                     {
								                                     PropertyCopyValue.WorldPosition = _rectTransform.position;
							                                     }
						                                     } ,
						                                     new ContextMenuData()
						                                     {
							                                     MenuName = GUIContentPreset.Paste ,
							                                     IsEnable = PropertyCopyValue.WorldPosition != null ,
							                                     Callback = () =>
							                                     {
								                                     Undo.RecordObject(target , "RectTransform Copy World Position");
								                                     _rectTransform.position = PropertyCopyValue.WorldPosition.Value;
							                                     }
						                                     });

						_rectTransform.localPosition = EditorGUILayout.Vector3Field(GUIContentPreset.LocalPosition , _rectTransform.localPosition);
						ContextMenuUtility.HandleContextMenu(GUILayoutUtility.GetLastRect() ,
						                                     new ContextMenuData()
						                                     {
							                                     MenuName = GUIContentPreset.Copy ,
							                                     IsEnable = true ,
							                                     Callback = () =>
							                                     {
								                                     PropertyCopyValue.LocalPosition = _rectTransform.localPosition;
							                                     }
						                                     } ,
						                                     new ContextMenuData()
						                                     {
							                                     MenuName = GUIContentPreset.Paste ,
							                                     IsEnable = PropertyCopyValue.LocalPosition != null ,
							                                     Callback = () =>
							                                     {
								                                     Undo.RecordObject(target , "RectTransform Copy Local Position");
								                                     _rectTransform.localPosition = PropertyCopyValue.LocalPosition.Value;
							                                     }
						                                     });

						EditorGUILayout.Space();

						_rectTransform.eulerAngles = EditorGUILayout.Vector3Field(GUIContentPreset.WorldEuler , _rectTransform.eulerAngles);
						ContextMenuUtility.HandleContextMenu(GUILayoutUtility.GetLastRect() ,
						                                     new ContextMenuData()
						                                     {
							                                     MenuName = GUIContentPreset.Copy ,
							                                     IsEnable = true ,
							                                     Callback = () =>
							                                     {
								                                     PropertyCopyValue.Euler = _rectTransform.eulerAngles;
							                                     }
						                                     } ,
						                                     new ContextMenuData()
						                                     {
							                                     MenuName = GUIContentPreset.Paste ,
							                                     IsEnable = PropertyCopyValue.Euler != null ,
							                                     Callback = () =>
							                                     {
								                                     Undo.RecordObject(target , "RectTransform Copy Euler");
								                                     _rectTransform.eulerAngles = PropertyCopyValue.Euler.Value;
							                                     }
						                                     });

						_tempRotationV4.x = _rectTransform.rotation.x;
						_tempRotationV4.y = _rectTransform.rotation.y;
						_tempRotationV4.z = _rectTransform.rotation.z;
						_tempRotationV4.w = _rectTransform.rotation.w;
						_tempRotationV4   = EditorGUILayout.Vector4Field(GUIContentPreset.Rotation , _tempRotationV4);

						_tempRotation.x = _tempRotationV4.x;
						_tempRotation.y = _tempRotationV4.y;
						_tempRotation.z = _tempRotationV4.z;
						_tempRotation.w = _tempRotationV4.w;

						_rectTransform.rotation = _tempRotation;
						ContextMenuUtility.HandleContextMenu(GUILayoutUtility.GetLastRect() ,
						                                     new ContextMenuData()
						                                     {
							                                     MenuName = GUIContentPreset.Copy ,
							                                     IsEnable = true ,
							                                     Callback = () =>
							                                     {
								                                     PropertyCopyValue.Rotation = _rectTransform.rotation;
							                                     }
						                                     } ,
						                                     new ContextMenuData()
						                                     {
							                                     MenuName = GUIContentPreset.Paste ,
							                                     IsEnable = PropertyCopyValue.Rotation != null ,
							                                     Callback = () =>
							                                     {
								                                     Undo.RecordObject(target , "RectTransform Copy Rotation");
								                                     _rectTransform.rotation = PropertyCopyValue.Rotation.Value;
							                                     }
						                                     });

						EditorGUILayout.Space();

						_rectTransform.anchoredPosition = EditorGUILayout.Vector2Field(GUIContentPreset.AnchoredPosition , _rectTransform.anchoredPosition);
						ContextMenuUtility.HandleContextMenu(GUILayoutUtility.GetLastRect() ,
						                                     new ContextMenuData()
						                                     {
							                                     MenuName = GUIContentPreset.Copy ,
							                                     IsEnable = true ,
							                                     Callback = () =>
							                                     {
								                                     PropertyCopyValue.AnchoredPosition = _rectTransform.anchoredPosition;
							                                     }
						                                     } ,
						                                     new ContextMenuData()
						                                     {
							                                     MenuName = GUIContentPreset.Paste ,
							                                     IsEnable = PropertyCopyValue.AnchoredPosition != null ,
							                                     Callback = () =>
							                                     {
								                                     Undo.RecordObject(target , "RectTransform Copy AnchoredPosition");
								                                     _rectTransform.anchoredPosition = PropertyCopyValue.AnchoredPosition.Value;
							                                     }
						                                     });

						_rectTransform.anchoredPosition3D = EditorGUILayout.Vector3Field(GUIContentPreset.AnchoredPosition3D , _rectTransform.anchoredPosition3D);
						ContextMenuUtility.HandleContextMenu(GUILayoutUtility.GetLastRect() ,
						                                     new ContextMenuData()
						                                     {
							                                     MenuName = GUIContentPreset.Copy ,
							                                     IsEnable = true ,
							                                     Callback = () =>
							                                     {
								                                     PropertyCopyValue.AnchoredPosition3D = _rectTransform.anchoredPosition3D;
							                                     }
						                                     } ,
						                                     new ContextMenuData()
						                                     {
							                                     MenuName = GUIContentPreset.Paste ,
							                                     IsEnable = PropertyCopyValue.AnchoredPosition3D != null ,
							                                     Callback = () =>
							                                     {
								                                     Undo.RecordObject(target , "RectTransform Copy AnchoredPosition3D");
								                                     _rectTransform.anchoredPosition3D = PropertyCopyValue.AnchoredPosition3D.Value;
							                                     }
						                                     });

						EditorGUILayout.Space();

						_rectTransform.SetSize(EditorGUILayout.Vector2Field(GUIContentPreset.Size , _rectTransform.GetSize()));
					}
				}
			}

			EditorGUILayout.EndFadeGroup();

			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();
			{
				ResetPos();
				ResetRot();
				ResetScale();
				ResetAll();
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();

			Rounding();

			GUI.color = Color.white;

			serializedObject.ApplyModifiedProperties();
		}

		private void ResetPos()
		{
			GUI.color = Color.cyan;

			if (!GUILayout.Button("重置 Pos")) return;
			if (_rectTransform != null) _rectTransform.anchoredPosition3D = Vector3.zero;
		}

		private void ResetRot()
		{
			GUI.color = Color.green;

			if (!GUILayout.Button("重置 Rotation")) return;
			if (_rectTransform != null) _rectTransform.localEulerAngles = Vector3.zero;
		}

		private void ResetScale()
		{
			GUI.color = Color.yellow;

			if (!GUILayout.Button("重置 Scale")) return;
			if (_rectTransform != null) _rectTransform.localScale = Vector3.one;
		}

		private void ResetAll()
		{
			GUI.color = Color.red;

			if (!GUILayout.Button("All")) return;

			if (_rectTransform != null)
			{
				_rectTransform.anchoredPosition3D = Vector3.zero;
				_rectTransform.localEulerAngles   = Vector3.zero;
				_rectTransform.localScale         = Vector3.one;
			}
		}

		private void Rounding()
		{
			GUI.color = new(1 , 0.5f , 0f);

			if (!GUILayout.Button("全部取整")) return;

			if (_rectTransform != null)
			{
				var pos = Vector3Int.RoundToInt(_rectTransform.anchoredPosition3D);
				_rectTransform.anchoredPosition3D = pos;
				var size = Vector2Int.RoundToInt(_rectTransform.rect.size);
				_rectTransform.SetSize(size);
			}
		}
	}
}
