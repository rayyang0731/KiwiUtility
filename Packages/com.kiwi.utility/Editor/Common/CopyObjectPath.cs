using UnityEditor;

using UnityEngine;

namespace Kiwi.Utility.Editor
{
	/// <summary>
	/// 复制 GameObject 的父子级路径
	/// </summary>
	public static class CopyObjectPath
	{
		[ MenuItem("GameObject/Kiwi/Copy Path" , false , -10) ]
		public static void Copy()
		{
			var path = GetPath(Selection.activeGameObject);
			EditorGUIUtility.systemCopyBuffer = path;

			Debug.Log($"复制 {Selection.activeGameObject.name} 路径到剪贴板: {path}");
		}

		[ MenuItem("GameObject/Kiwi/Copy Path" , true) ]
		public static bool CopyValidate() => Selection.activeGameObject != null;

		/// <summary>
		/// 获取路径
		/// </summary>
		private static string GetPath(GameObject gameObject)
		{
			var path   = gameObject.name;
			var parent = gameObject.transform.parent;

			while (parent != null)
			{
				path   = parent.name + "/" + path;
				parent = parent.parent;
			}

			return path;
		}
	}
}
