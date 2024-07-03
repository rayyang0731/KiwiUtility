using System;

using UnityEditor;

using UnityEngine;

namespace Kiwi.Utility.Editor
{
	/// <summary>
	/// 预览动画模组
	/// </summary>
	public class PreviewAnimationModule : BasePreviewModule
	{
		/// <summary>
		/// 动画对象
		/// </summary>
		private GameObject _animatorObj = null;

		/// <summary>
		/// 动画片段列表
		/// </summary>
		private AnimationClip[ ] _clips = null;

		/// <summary>
		/// 动画片段名称列表
		/// </summary>
		private string[ ] _clipNames = null;

		/// <summary>
		/// 当前动画片段索引
		/// </summary>
		private int _clipIndex = 0;

		/// <summary>
		/// 当前动画进度
		/// </summary>
		[ Range(0f , 1f) ]
		private float _clipProgress = 0.0f;

		/// <summary>
		/// 当前动画时间
		/// </summary>
		private float _animationTime = 0f;

		/// <summary>
		/// 当前动画片段长度
		/// </summary>
		private float _clipLen = 0;

		/// <summary>
		/// 是否自动播放动画片段
		/// </summary>
		private bool _autoClip = false;

		/// <summary>
		/// 播放倍速枚举
		/// </summary>
		private enum MultipleSpeed
		{
			x1 ,
			x2 ,
			x3 ,
		}

		/// <summary>
		/// 播放倍速名称列表
		/// </summary>
		private readonly string[ ] _multipleSpeedNames =
		{
			"1.0x" ,
			"2.0x" ,
			"3.0x"
		};

		/// <summary>
		/// 当前播放倍速
		/// </summary>
		private MultipleSpeed _multipleSpeed = MultipleSpeed.x1;

		public override void Update()
		{
			if (_clips == null || _clips.Length == 0) { return; }

			if (_clipIndex < 0 || _clipIndex > _clips.Length - 1) { return; }

			if (_animatorObj == null) { return; }

			if (_animatorObj != null)
			{
				AnimationClip clip = _clips[_clipIndex];
				float         len  = clip.length;

				if (_autoClip)
				{
					float add     = Time.deltaTime * ((int) _multipleSpeed + 1); // 每帧增量
					float surplus = Mathf.Abs(len - _animationTime);             // 剩余动画时间
					_animationTime += (surplus > add) ? add : surplus;
					_clipProgress  =  _animationTime / len;

					// 采样动画
					AnimationMode.StartAnimationMode();
					AnimationMode.BeginSampling();
					clip.SampleAnimation(_animatorObj , _animationTime);
					AnimationMode.EndSampling();
					AnimationMode.StopAnimationMode();

					if (_animationTime >= len)
						_animationTime = 0;
				}
				else
				{
					_animationTime = _clipProgress * len;
					clip.SampleAnimation(_animatorObj , _animationTime);
				}
			}
		}

		public override void OnTargetChanged(GameObject target)
		{
			if (target)
			{
				_clipProgress  = 0;
				_animationTime = 0;

				// 检测是否包含动画控制器
				Animator animator = target.GetComponentInChildren<Animator>();

				if (animator != null)
				{
					_animatorObj = animator.gameObject;
					_clips       = animator.runtimeAnimatorController?.animationClips;

					if (_clips != null)
					{
						_clipNames = new string[ _clips.Length ];
						for (int i = 0 ; i < _clips.Length ; i++)
							_clipNames[i] = _clips[i].name;
					}
					else { ClearClip(); }
				}
				else { ClearClip(); }
			}
			else
			{
				ClearClip();
			}
		}

		public override void Dispose()
		{
			ClearClip();
		}

		public override void OnTopGUI()
		{
			if (_clipNames != null && _clipNames.Length != 0)
			{
				EditorGUILayout.Space(1);
				EditorGUILayout.BeginHorizontal();
				{
					_clipIndex = EditorGUILayout.Popup(_clipIndex , _clipNames , GUILayout.Width(240));
					CreateAnimationControlButton("\u25b6" , Color.cyan , _autoClip , true);
					CreateAnimationControlButton("\u25a0" , Color.red , !_autoClip , false);
					_clipProgress = EditorGUILayout.Slider(_clipProgress , 0 , 1);
					_clipLen      = _clips[_clipIndex].length;
					EditorGUILayout.LabelField($"{TimeFormat(_clipLen * _clipProgress)}/{TimeFormat(_clipLen)}" , GUILayout.Width(100));
					_multipleSpeed = (MultipleSpeed) EditorGUILayout.Popup((int) _multipleSpeed , _multipleSpeedNames , GUILayout.Width(66));
				}
				EditorGUILayout.EndHorizontal();
			}
		}

		private void ClearClip()
		{
			if (_clipNames != null)
			{
				Array.Clear(_clipNames , 0 , _clipNames.Length);
				_clipNames = null;
			}

			if (_clips != null)
			{
				Array.Clear(_clips , 0 , _clips.Length);
				_clips = null;
			}

			_clipIndex     = 0;
			_clipProgress  = 0.0f;
			_animationTime = 0.0f;
			_animatorObj   = null;
			_clipLen       = 0;
			_autoClip      = false;
			_multipleSpeed = MultipleSpeed.x1;
		}

		private void CreateAnimationControlButton(string label , Color backgroundColor , bool b , bool flag)
		{
			EditorGUI.BeginDisabledGroup(b);

			Color c = GUI.backgroundColor;

			if (!b) { GUI.backgroundColor = backgroundColor; }

			if (GUILayout.Button(label , GUILayout.Width(20) , GUILayout.Height(18)))
			{
				_autoClip = flag;
			}

			if (!b) { GUI.backgroundColor = c; }

			EditorGUI.EndDisabledGroup();
		}

		private string TimeFormat(float len)
		{
			int v = (int) len;
			int m = v / 60;
			int s = v % 60;

			return string.Format("{0:D2}:{1:D2}" , m , s);
		}

		public PreviewAnimationModule(AvatarEditor avatarEditor) : base(avatarEditor)
		{
		}
	}
}
