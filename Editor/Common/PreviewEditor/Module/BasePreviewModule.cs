using System;

using UnityEngine;

namespace Kiwi.Utility.Editor
{
	/// <summary>
	/// 预览窗体模组接口
	/// </summary>
	public class BasePreviewModule : IDisposable
	{
		/// <summary>
		/// 模型预览窗体
		/// </summary>
		protected ModelPreviewEditor _modelPreviewEditor;

		public BasePreviewModule(AvatarEditor avatarEditor)
		{
 			_modelPreviewEditor = avatarEditor._modelPreviewEditor;
		}

		public virtual void Update() { }

		public virtual void OnTargetChanged(GameObject target) { }

		public virtual void OnTopGUI() { }

		public virtual void OnBottomGUI() { }

		public virtual void Dispose() { }
	}
}
