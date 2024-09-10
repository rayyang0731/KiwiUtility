using UnityEngine;

namespace Kiwi.Utility.Editor.OverrideNativeInspector
{
	public static class GUIContentPreset
	{
		public static readonly GUIContent OtherProperty = new("Other Property");
		public static readonly GUIContent WorldPosition = new("World Position");
		public static readonly GUIContent LocalPosition = new("Local Position");
		public static readonly GUIContent AnchoredPosition = new("Anchored Position");
		public static readonly GUIContent AnchoredPosition3D = new("Anchored Position 3D");
		public static readonly GUIContent WorldEuler = new("World Euler");
		public static readonly GUIContent Rotation = new("Rotation");
		public static readonly GUIContent Size = new("Size");

		public static readonly GUIContent Copy = new("Copy");
		public static readonly GUIContent Paste = new("Paste");
	}
}
