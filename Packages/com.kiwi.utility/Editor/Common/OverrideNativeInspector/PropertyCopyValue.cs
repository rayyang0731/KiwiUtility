using UnityEngine;

namespace Kiwi.Utility.Editor.OverrideNativeInspector
{
	public static class PropertyCopyValue
	{
		private static Vector3? _worldPosition;

		public static Vector3? WorldPosition
		{
			get => _worldPosition;
			set
			{
				_worldPosition      = value;
				_localPosition      = null;
				_euler              = null;
				_rotation           = null;
				_anchoredPosition   = null;
				_anchoredPosition3D = null;
				_size               = null;
			}
		}

		private static Vector3? _localPosition;

		public static Vector3? LocalPosition
		{
			get => _localPosition;
			set
			{
				_worldPosition      = null;
				_localPosition      = value;
				_euler              = null;
				_rotation           = null;
				_anchoredPosition   = null;
				_anchoredPosition3D = null;
				_size               = null;
			}
		}

		private static Vector3? _euler;

		public static Vector3? Euler
		{
			get => _euler;
			set
			{
				_worldPosition      = null;
				_localPosition      = null;
				_euler              = value;
				_rotation           = null;
				_anchoredPosition   = null;
				_anchoredPosition3D = null;
				_size               = null;
			}
		}

		private static Quaternion? _rotation;

		public static Quaternion? Rotation
		{
			get => _rotation;
			set
			{
				_worldPosition      = null;
				_localPosition      = null;
				_euler              = null;
				_rotation           = value;
				_anchoredPosition   = null;
				_anchoredPosition3D = null;
				_size               = null;
			}
		}

		private static Vector2? _anchoredPosition;

		public static Vector2? AnchoredPosition
		{
			get => _anchoredPosition;
			set
			{
				_worldPosition      = null;
				_localPosition      = null;
				_euler              = null;
				_rotation           = null;
				_anchoredPosition   = value;
				_anchoredPosition3D = null;
				_size               = null;
			}
		}

		private static Vector3? _anchoredPosition3D;

		public static Vector3? AnchoredPosition3D
		{
			get => _anchoredPosition3D;
			set
			{
				_worldPosition      = null;
				_localPosition      = null;
				_euler              = null;
				_rotation           = null;
				_anchoredPosition   = null;
				_anchoredPosition3D = value;
				_size               = null;
			}
		}

		private static Vector2? _size;

		public static Vector2? Size
		{
			get => _size;
			set
			{
				_worldPosition      = null;
				_localPosition      = null;
				_euler              = null;
				_rotation           = null;
				_anchoredPosition   = null;
				_anchoredPosition3D = null;
				_size               = value;
			}
		}
	}
}
