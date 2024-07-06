using System;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Kiwi.Utility.Editor
{
	/// <summary>
	/// 模型预览编辑窗体
	/// <para>fork from Graphi</para>
	/// </summary>
	public class AvatarEditor
	{
		/// <summary>
		/// 预览目标对象
		/// </summary>
		private GameObject _targetObj;

		/// <summary>
		/// 上一帧预览目标对象
		/// 用于检测模型是否更换
		/// </summary>
		private GameObject _preTargetObj;

		/// <summary>
		/// 模型预览窗体
		/// </summary>
		internal ModelPreviewEditor _modelPreviewEditor;

		/// <summary>
		/// 模型预览窗体模组
		/// </summary>
		private readonly List<IPreviewModule> _modules = new();

		/// <summary>
		/// 模型预览窗体模组
		/// </summary>
		private readonly Dictionary<Type , IPreviewModule> _modulesDic = new();

		/// <summary>
		/// 预览窗口位置
		/// </summary>
		private Rect _previewRect;

		/// <summary>
		/// 摄像机渲染清理标记
		/// </summary>
		private CameraClearFlags _cameraClearFlags = CameraClearFlags.Color;

		/// <summary>
		/// 预览窗口背景色
		/// </summary>
		private Color _backgroundColor = new(0.1698113f , 0.1698113f , 0.1698113f , 0f);

		/// <summary>
		/// 构造
		/// </summary>
		/// <param name="editor">父窗体</param>
		public AvatarEditor(EditorWindow editor)
		{
			_modelPreviewEditor = UnityEditor.Editor.CreateEditor(editor , typeof(ModelPreviewEditor)) as ModelPreviewEditor;
		}

		/// <summary>
		/// 添加模块
		/// </summary>
		public T AddModule< T >() where T : class , IPreviewModule
		{
			var type = typeof(T);

			if (!_modulesDic.ContainsKey(type))
			{
				var module = Activator.CreateInstance<T>();

				_modules.Add(module);
				_modulesDic.Add(type , module);
			}

			return _modulesDic[type] as T;
		}

		/// <summary>
		/// 获取模块
		/// </summary>
		/// <typeparam name="T">模块类型</typeparam>
		/// <returns>如果不存在,返回Null</returns>
		public T GetModule< T >() where T : class , IPreviewModule
		{
			if (_modulesDic.TryGetValue(typeof(T) , out var module))
			{
				return module as T;
			}

			return null;
		}

		/// <summary>
		/// 清除全部模块
		/// </summary>
		private void ClearModules()
		{
			foreach (var module in _modules)
			{
				module.Dispose();
			}

			_modules.Clear();
			_modulesDic.Clear();
		}

		/// <summary>
		/// 刷新（在父窗体的 Update 中调用）
		/// <para>包含动画等</para>
		/// </summary>
		public void Update()
		{
			foreach (var module in _modules)
			{
				if (_modelPreviewEditor != null && _modelPreviewEditor.previewInstance != null)
				{
					module.Update();
				}
			}
		}

		/// <summary>
		/// 释放（在父窗体被关闭时，必须在 OnDestroy 内调用）
		/// </summary>
		public void Dispose()
		{
			if (_modelPreviewEditor != null)
			{
				Object.DestroyImmediate(_modelPreviewEditor);
				_modelPreviewEditor = null;
			}

			ClearModules();

			_targetObj    = null;
			_preTargetObj = null;

			_cameraClearFlags = CameraClearFlags.Color;
		}


		/// <summary>
		/// 绘制（在父类的 OnGUI 中调用）
		/// </summary>
		/// <param name="rect">预览窗体所在的父窗体尺寸 <see cref="EditorWindow.position">(详情请查看 EditorWindow.position)</see></param>
		/// <param name="extraHeight">父窗体纵向排布的组件高度总和（也就是除此预览窗体外每一行的行高总和）<see cref="AvatarEditor.GetPropertyHeight">(可使用此类的 GetPropertyHeight 函数获取组件高度)</see></param>
		public void Draw(Rect rect , float extraHeight = 0)
		{
			EditorGUILayout.BeginVertical();
			{
				GUILayout.Space(3);

				#region 基础操作项

				EditorGUI.BeginChangeCheck();
				_targetObj = EditorGUILayout.ObjectField(new GUIContent("预览对象") , _targetObj , typeof(GameObject) , true) as GameObject;


				if (EditorGUI.EndChangeCheck())
				{
					if (_targetObj != null)
					{
						if (_preTargetObj != _targetObj)
						{
							_preTargetObj = _targetObj;
							ResetPreviewObj(_targetObj);
						}
					}
					else
					{
						_targetObj    = null;
						_preTargetObj = null;
						ResetPreviewObj(null);
					}

					foreach (var module in _modules)
					{
						if (_modelPreviewEditor != null && _modelPreviewEditor.previewInstance != null)
							module.OnTargetChanged(_modelPreviewEditor.previewInstance);
					}
				}

				EditorGUILayout.BeginHorizontal();
				{
					EditorGUI.BeginChangeCheck();
					_cameraClearFlags = (CameraClearFlags) EditorGUILayout.EnumPopup(new GUIContent("背景") , _cameraClearFlags);

					if (EditorGUI.EndChangeCheck())
					{
						if (_modelPreviewEditor != null) { _modelPreviewEditor.CamClearFlags(_cameraClearFlags); }
					}

					if (_cameraClearFlags is CameraClearFlags.Color or CameraClearFlags.SolidColor)
					{
						EditorGUI.BeginChangeCheck();
						_backgroundColor = EditorGUILayout.ColorField(GUIContent.none , _backgroundColor);

						if (EditorGUI.EndChangeCheck())
						{
							if (_modelPreviewEditor != null)
							{
								_modelPreviewEditor.BackgroundColor(_backgroundColor);
							}
						}
					}
				}

				EditorGUILayout.EndHorizontal();

				#endregion

				foreach (var module in _modules)
				{
					if (_modelPreviewEditor != null && _modelPreviewEditor.previewInstance != null)
						module.OnTopGUI();
				}

				if (_modelPreviewEditor != null)
				{
					// 绘制预览窗体
					GetPreviewPositionAndSize(rect , extraHeight);
					_modelPreviewEditor.Draw(_previewRect);

					foreach (var module in _modules)
					{
						if (_modelPreviewEditor != null && _modelPreviewEditor.previewInstance != null)
							module.OnOverlapGUI(_previewRect);
					}
				}

				foreach (var module in _modules)
				{
					if (_modelPreviewEditor != null && _modelPreviewEditor.previewInstance != null)
						module.OnBottomGUI();
				}
			}
			GUILayout.EndVertical();
		}


		/// <summary>
		/// 获取 EditorWindow 内组件的高度
		/// </summary>
		/// <param name="type">组件类型</param>
		/// <param name="label">标签（默认：空字符）</param>
		/// <returns></returns>
		public float GetPropertyHeight(SerializedPropertyType type , GUIContent label = null)
		{
			label ??= GUIContent.none;

			return EditorGUI.GetPropertyHeight(type , label) + 5; // 额外相加数值的原因是底层默认值为18，但 EditorWindow 组件默认高度实际要比18大。
		}


		/// <summary>
		/// 重置预览对象
		/// </summary>
		/// <param name="obj"></param>
		public void ResetPreviewObj(GameObject obj)
		{
			if (_modelPreviewEditor == null) { return; }

			_modelPreviewEditor.ResetPreviewObj(obj);
		}


		/// <summary>
		/// 获取预览对象实例（可对实例进行相关操作）
		/// </summary>
		public GameObject PreviewInstance
		{
			get
			{
				if (_modelPreviewEditor != null)
					return _modelPreviewEditor.previewInstance;

				return null;
			}
		}

		/// <summary>
		/// 获取预览窗位置和尺寸
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="extraHeight"></param>
		private void GetPreviewPositionAndSize(Rect rect , float extraHeight = 0)
		{
			var curRect = EditorGUILayout.GetControlRect();
			_previewRect.Set(curRect.x , curRect.y , curRect.width , rect.size.y - curRect.y - 5 - extraHeight);
		}
	}
}
