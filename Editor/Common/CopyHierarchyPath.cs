using UnityEditor;

using UnityEngine;

namespace Kiwi.Utility.Editor
{
	/// <summary>
	/// 复制 GameObject 在 Hierarchy 面板中的的父子级路径
	/// </summary>
	public static class CopyHierarchyPath
	{
		[ MenuItem("GameObject/Kiwi/Copy Hierarchy Path" , false , -10) ]
		public static void Copy()
		{
			var path = EditorUtil.GetPath(Selection.activeGameObject);
			EditorGUIUtility.systemCopyBuffer = path;

			Debug.Log($"复制 {Selection.activeGameObject.name} 路径到剪贴板: {path}");
		}

		[ MenuItem("GameObject/Kiwi/Copy Hierarchy Path" , true) ]
		public static bool CopyValidate() => Selection.activeGameObject != null;
	}
}
