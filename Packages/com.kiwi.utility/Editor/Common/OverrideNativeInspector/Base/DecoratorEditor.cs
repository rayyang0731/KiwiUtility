using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEditor;

using UnityEngine;

using Object = UnityEngine.Object;

/*
 * 原地址 : https://gist.github.com/liortal53/352fda2d01d339306e03
 * 因为 Unity 内部组件的面板有 internal 修饰符,只能通过反射获取回来,
 * 所有用这个类将 Unity 内部面板的类继承出来.
 */

namespace Kiwi.Utility.Editor.OverrideNativeInspector
{
	/// <summary>
	/// Unity 面板装饰基类
	/// </summary>
	public abstract class DecoratorEditor : UnityEditor.Editor
	{
		// empty array for invoking methods using reflection
		private static readonly object[ ] EMPTY_ARRAY = Array.Empty<object>();

		#region Editor Fields

		/// <summary>
		/// Type object for the internally used (decorated) editor.
		/// </summary>
		private Type decoratedEditorType;

		/// <summary>
		/// Type object for the object that is edited by this editor.
		/// </summary>
		private Type _editedObjectType;

		private UnityEditor.Editor _editorInstance;

		#endregion

		private static readonly Dictionary<string , MethodInfo> DecoratedMethods = new();

		private static readonly Assembly EditorAssembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));

		protected UnityEditor.Editor EditorInstance
		{
			get
			{
				if (_editorInstance == null && targets is {Length: > 0})
				{
					_editorInstance = CreateEditor(targets , decoratedEditorType);
				}

				if (_editorInstance == null)
				{
					Debug.LogError("Could not create editor !");
				}

				return _editorInstance;
			}
		}

		protected DecoratorEditor(string editorTypeName)
		{
			decoratedEditorType = EditorAssembly.GetTypes().FirstOrDefault(t => t.Name == editorTypeName);

			Init();

			// Check CustomEditor types.
			var originalEditedType = GetCustomEditorType(decoratedEditorType);

			if (originalEditedType != _editedObjectType)
			{
				throw new ArgumentException($"Type {_editedObjectType} does not match the editor {editorTypeName} type {originalEditedType}");
			}
		}

		private Type GetCustomEditorType(Type type)
		{
			const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;

			var attributes = type.GetCustomAttributes(typeof(CustomEditor) , true) as CustomEditor[ ];

			if (attributes == null)
				return null;
			var field = attributes.Select(editor => editor.GetType().GetField("m_InspectedType" , flags)).First();

			if (field != null)
				return field.GetValue(attributes[0]) as Type;

			return null;
		}

		private void Init()
		{
			const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;

			if (GetType().GetCustomAttributes(typeof(CustomEditor) , true) is not CustomEditor[ ] attributes)
				return;

			var field = attributes.Select(editor => editor.GetType().GetField("m_InspectedType" , flags)).First();

			if (field != null)
				_editedObjectType = field.GetValue(attributes[0]) as Type;
		}

		private void OnDisable()
		{
			if (_editorInstance != null)
				DestroyImmediate(_editorInstance);
		}

		/// <summary>
		/// Delegates a method call with the given name to the decorated editor instance.
		/// </summary>
		protected void CallInspectorMethod(string methodName)
		{
			MethodInfo method;

			// Add MethodInfo to cache
			if (!DecoratedMethods.TryGetValue(methodName , out var decoratedMethod))
			{
				const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;

				method = decoratedEditorType.GetMethod(methodName , flags);

				if (method != null)
					DecoratedMethods[methodName] = method;
			}
			else
			{
				method = decoratedMethod;
			}

			if (method != null)
				method.Invoke(EditorInstance , EMPTY_ARRAY);
		}

		// public void OnSceneGUI()
		// {
		// 	CallInspectorMethod("OnSceneGUI");
		// }

		protected override void OnHeaderGUI()
		{
			CallInspectorMethod("OnHeaderGUI");
		}

		public override void OnInspectorGUI()
		{
			EditorInstance.OnInspectorGUI();
		}

		public override void DrawPreview(Rect previewArea)
		{
			EditorInstance.DrawPreview(previewArea);
		}

		public override string GetInfoString()
		{
			return EditorInstance.GetInfoString();
		}

		public override GUIContent GetPreviewTitle()
		{
			return EditorInstance.GetPreviewTitle();
		}

		public override bool HasPreviewGUI()
		{
			return EditorInstance.HasPreviewGUI();
		}

		public override void OnInteractivePreviewGUI(Rect r , GUIStyle background)
		{
			EditorInstance.OnInteractivePreviewGUI(r , background);
		}

		public override void OnPreviewGUI(Rect r , GUIStyle background)
		{
			EditorInstance.OnPreviewGUI(r , background);
		}

		public override void OnPreviewSettings()
		{
			EditorInstance.OnPreviewSettings();
		}

		public override void ReloadPreviewInstances()
		{
			EditorInstance.ReloadPreviewInstances();
		}

		public override Texture2D RenderStaticPreview(string assetPath , Object[ ] subAssets , int width , int height)
		{
			return EditorInstance.RenderStaticPreview(assetPath , subAssets , width , height);
		}

		public override bool RequiresConstantRepaint()
		{
			return EditorInstance.RequiresConstantRepaint();
		}

		public override bool UseDefaultMargins()
		{
			return EditorInstance.UseDefaultMargins();
		}
	}
}
