using UnityEditor;

using UnityEngine;

namespace Kiwi.Utility.Editor.OverrideNativeInspector
{
	/// <summary>
	/// 扩展 Transform 编辑器面板
	/// </summary>
	[ CustomEditor(typeof(Transform) , true) ]
	public class TransformEditor : DecoratorEditor
	{
		public TransformEditor() : base("TransformInspector") { }

		private Transform _transform;

		private Vector4 _tempRotationV4;

		private Quaternion _tempRotation;

		private static bool _otherPropertyFoldOut;

		private void OnEnable()
		{
			_transform      = target as Transform;
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
				if (!ReferenceEquals(_transform , null))
				{
					Undo.RecordObject(target , "Transform Extra Property");

					using (new EditorGUI.IndentLevelScope())
					{
						_transform.position = EditorGUILayout.Vector3Field(GUIContentPreset.WorldPosition , _transform.position);
						ContextMenuUtility.HandleContextMenu(GUILayoutUtility.GetLastRect() ,
						                                     new ContextMenuData()
						                                     {
							                                     MenuName = GUIContentPreset.Copy ,
							                                     IsEnable = true ,
							                                     Callback = () =>
							                                     {
								                                     PropertyCopyValue.WorldPosition = _transform.position;
							                                     }
						                                     } ,
						                                     new ContextMenuData()
						                                     {
							                                     MenuName = GUIContentPreset.Paste ,
							                                     IsEnable = PropertyCopyValue.WorldPosition != null ,
							                                     Callback = () =>
							                                     {
								                                     Undo.RecordObject(target , "Transform Copy World Position");
								                                     _transform.position = PropertyCopyValue.WorldPosition.Value;
							                                     }
						                                     });

						_transform.localPosition = EditorGUILayout.Vector3Field(GUIContentPreset.LocalPosition , _transform.localPosition);
						ContextMenuUtility.HandleContextMenu(GUILayoutUtility.GetLastRect() ,
						                                     new ContextMenuData()
						                                     {
							                                     MenuName = GUIContentPreset.Copy ,
							                                     IsEnable = true ,
							                                     Callback = () =>
							                                     {
								                                     PropertyCopyValue.LocalPosition = _transform.localPosition;
							                                     }
						                                     } ,
						                                     new ContextMenuData()
						                                     {
							                                     MenuName = GUIContentPreset.Paste ,
							                                     IsEnable = PropertyCopyValue.LocalPosition != null ,
							                                     Callback = () =>
							                                     {
								                                     Undo.RecordObject(target , "Transform Copy Local Position");
								                                     _transform.localPosition = PropertyCopyValue.LocalPosition.Value;
							                                     }
						                                     });

						EditorGUILayout.Space();

						_transform.eulerAngles = EditorGUILayout.Vector3Field(GUIContentPreset.WorldEuler , _transform.eulerAngles);
						ContextMenuUtility.HandleContextMenu(GUILayoutUtility.GetLastRect() ,
						                                     new ContextMenuData()
						                                     {
							                                     MenuName = GUIContentPreset.Copy ,
							                                     IsEnable = true ,
							                                     Callback = () =>
							                                     {
								                                     PropertyCopyValue.Euler = _transform.eulerAngles;
							                                     }
						                                     } ,
						                                     new ContextMenuData()
						                                     {
							                                     MenuName = GUIContentPreset.Paste ,
							                                     IsEnable = PropertyCopyValue.Euler != null ,
							                                     Callback = () =>
							                                     {
								                                     Undo.RecordObject(target , "Transform Copy Euler");
								                                     _transform.eulerAngles = PropertyCopyValue.Euler.Value;
							                                     }
						                                     });

						_tempRotationV4.x = _transform.rotation.x;
						_tempRotationV4.y = _transform.rotation.y;
						_tempRotationV4.z = _transform.rotation.z;
						_tempRotationV4.w = _transform.rotation.w;
						_tempRotationV4   = EditorGUILayout.Vector4Field(GUIContentPreset.Rotation , _tempRotationV4);

						_tempRotation.x = _tempRotationV4.x;
						_tempRotation.y = _tempRotationV4.y;
						_tempRotation.z = _tempRotationV4.z;
						_tempRotation.w = _tempRotationV4.w;

						_transform.rotation = _tempRotation;
						ContextMenuUtility.HandleContextMenu(GUILayoutUtility.GetLastRect() ,
						                                     new ContextMenuData()
						                                     {
							                                     MenuName = GUIContentPreset.Copy ,
							                                     IsEnable = true ,
							                                     Callback = () =>
							                                     {
								                                     PropertyCopyValue.Rotation = _transform.rotation;
							                                     }
						                                     } ,
						                                     new ContextMenuData()
						                                     {
							                                     MenuName = GUIContentPreset.Paste ,
							                                     IsEnable = PropertyCopyValue.Rotation != null ,
							                                     Callback = () =>
							                                     {
								                                     Undo.RecordObject(target , "Transform Copy Rotation");
								                                     _transform.rotation = PropertyCopyValue.Rotation.Value;
							                                     }
						                                     });
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
			if (_transform != null) _transform.localPosition = Vector3.zero;
		}

		private void ResetRot()
		{
			GUI.color = Color.green;

			if (!GUILayout.Button("重置 Rotation")) return;
			if (_transform != null) _transform.localEulerAngles = Vector3.zero;
		}

		private void ResetScale()
		{
			GUI.color = Color.yellow;

			if (!GUILayout.Button("重置 Scale")) return;
			if (_transform != null) _transform.localScale = Vector3.one;
		}

		private void ResetAll()
		{
			GUI.color = Color.red;

			if (!GUILayout.Button("All")) return;

			if (_transform != null)
			{
				_transform.localPosition    = Vector3.zero;
				_transform.localEulerAngles = Vector3.zero;
				_transform.localScale       = Vector3.one;
			}
		}

		private void Rounding()
		{
			GUI.color = new(1 , 0.5f , 0f);

			if (!GUILayout.Button("全部取整")) return;

			if (_transform != null)
			{
				var pos = Vector3Int.RoundToInt(_transform.localPosition);
				_transform.localPosition = pos;
				var rot = Vector3Int.RoundToInt(_transform.localEulerAngles);
				var qua = Quaternion.Euler(rot);
				_transform.localRotation = qua;
				var scale = Vector3Int.RoundToInt(_transform.localScale);
				_transform.localScale = scale;
			}
		}
	}
}
