using UnityEditor;

using UnityEngine;

namespace Kiwi.Utility.Editor
{
	/// <summary>
	/// 模型预览窗体
	/// <para>fork from Graphi</para>
	/// </summary>
	public class ModelPreviewEditor : UnityEditor.Editor
	{
		/// <summary>
		/// 环境光颜色
		/// </summary>
		private static readonly Color AmColor = new(0.1f , 0.1f , 0.1f , 1f);

		/// <summary>
		/// 预览目标对象
		/// </summary>
		private GameObject _targetObj = null;

		/// <summary>
		/// 预览实例
		/// </summary>
		private GameObject _previewInstance = null;

		/// <summary>
		/// Unity 预览工具类
		/// </summary>
		private PreviewRenderUtility _previewRenderUtility = null;

		/// <summary>
		/// 预览模型的平移
		/// </summary>
		private Vector3 _pivotPositionOffset;

		/// <summary>
		/// 预览模型的旋转
		/// </summary>
		private Vector2 _rot;

		/// <summary>
		/// 预览模型的缩放
		/// </summary>
		private float _zoom;

		/// <summary>
		/// 预览窗口背景色
		/// </summary>
		private Color _previewWinBackground = new(0.1698113f , 0.1698113f , 0.1698113f , 0f);

		/// <summary>
		/// 摄像机渲染清理标记
		/// </summary>
		private CameraClearFlags _camClearFlags = CameraClearFlags.Color;

		/// <summary>
		/// 是否重置摄像机
		/// </summary>
		private bool _resetCamera = false;

		/// <summary>
		/// 摄像机初始坐标
		/// </summary>
		private Vector3 _cameraOriginPos;

		/// <summary>
		/// 初始距离
		/// </summary>
		private float _originDistance;

		private void OnEnable()
		{
			_previewRenderUtility ??= new();

			ResetTransform();
		}

		private void OnDisable()
		{
			if (_previewRenderUtility != null)
			{
				_previewRenderUtility.Cleanup();
				_previewRenderUtility = null;
			}

			_previewInstance = null;
			_targetObj       = null;

			ResetTransform();
		}

		/// <summary>
		/// 重置模型操作相关数值
		/// </summary>
		private void ResetTransform()
		{
			_pivotPositionOffset = Vector3.zero;
			_rot                 = new(180f , -10f);
			_zoom                = 1.0f;

			_resetCamera = true;
		}

		/// <summary>
		/// 渲染预览窗体
		/// </summary>
		/// <param name="rect">预览 GUI 渲染位置</param>
		/// <param name="background">预览 GUI 渲染背景</param>
		public override void OnPreviewGUI(Rect rect , GUIStyle background)
		{
			if (_previewInstance != null)
			{
				PreviewEvt.Execute(rect , _previewRenderUtility , _previewInstance.GetCenterPosition() , ref _rot , ref _zoom , ref _pivotPositionOffset);

				if (Event.current.type == EventType.Repaint)
				{
					// 重绘
					_previewRenderUtility.BeginPreview(rect , background);

					// 设置摄像机
					var cam      = _previewRenderUtility.camera;
					var camTrans = cam.transform;

					if (_resetCamera)
					{
						ResetCameraPosAndRot(camTrans , cam);

						_resetCamera = false;
					}

					SyncCameraState(camTrans , cam);
					
					// 设置光源信息
					_previewRenderUtility.lights[0].intensity          = 1.4f;
					_previewRenderUtility.lights[0].transform.rotation = Quaternion.Euler(40f , 40f , 0f);
					_previewRenderUtility.lights[1].intensity          = 1.4f;
					_previewRenderUtility.ambientColor                 = AmColor;

					// 渲染
					cam.Render();

					_previewRenderUtility.EndAndDrawPreview(rect);
				}
			}
		}

		/// <summary>
		/// 同步摄像机状态
		/// </summary>
		/// <param name="camTrans"></param>
		/// <param name="cam"></param>
		private void SyncCameraState(Transform camTrans , Camera cam)
		{
			camTrans.rotation = Quaternion.Euler(-_rot.y , -_rot.x , 0);
			camTrans.position = _cameraOriginPos - camTrans.forward * (_originDistance * _zoom) + _pivotPositionOffset;

			EditorUtility.SetCameraAnimateMaterials(cam , true);

			cam.cameraType      = CameraType.Preview;
			cam.enabled         = false;
			cam.clearFlags      = _camClearFlags;
			cam.backgroundColor = _previewWinBackground;
			cam.fieldOfView     = 30;
			cam.farClipPlane    = 1000.0f;
			cam.nearClipPlane   = 0.01f;
		}

		/// <summary>
		/// 重置摄像机位置和朝向
		/// </summary>
		/// <param name="camTrans"></param>
		/// <param name="cam"></param>
		private void ResetCameraPosAndRot(Transform camTrans , Camera cam)
		{
			camTrans.transform.position    = Vector3.zero;
			camTrans.transform.eulerAngles = Vector3.zero;
			cam.fieldOfView                = 15;

			var bounds            = new Bounds(_previewInstance.transform.position , Vector3.zero);
			var childrenRenderers = _previewInstance.GetComponentsInChildren<Renderer>();

			foreach (var renderer in childrenRenderers)
			{
				bounds.Encapsulate(renderer.bounds);
			}

			var maxExtent = Mathf.Max(bounds.extents.x , bounds.extents.y , bounds.extents.z);
			_originDistance  = maxExtent / Mathf.Sin(cam.fieldOfView * Mathf.Deg2Rad / 2f);
			_cameraOriginPos = bounds.center;
		}


		/// <summary>
		/// 绘制背景的ClearFlags
		/// </summary>
		/// <param name="flags"></param>
		public void CamClearFlags(CameraClearFlags flags)
		{
			_camClearFlags = flags;
		}


		/// <summary>
		/// 绘制背景色
		/// <para>当绘制背景类型不是颜色背景类型时，则设置无效</para>
		/// </summary>
		/// <param name="color"></param>
		public void BackgroundColor(Color color)
		{
			_previewWinBackground = color;
		}
		
		/// <summary>
		/// 绘制
		/// </summary>
		/// <param name="w">宽度</param>
		/// <param name="h">高度</param>
		public void Draw(float w = 256 , float h = 256)
		{
			DrawPreview(GUILayoutUtility.GetRect(w , h));
		}

		/// <summary>
		/// 绘制
		/// </summary>
		/// <param name="x">x坐标 </param>
		/// <param name="y">y坐标 </param>
		/// <param name="w">宽度</param>
		/// <param name="h">高度</param>
		public void Draw(float x , float y , float w = 256 , float h = 256)
		{
			DrawPreview(new Rect(x , y , w , h));
		}

		/// <summary>
		/// 绘制
		/// </summary>
		/// <param name="rect">位置 </param>
		public void Draw(Rect rect)
		{
			DrawPreview(rect);
		}

		/// <summary>
		/// 绘制
		/// </summary>
		/// <param name="size">尺寸（x：宽度，y：高度）</param>
		public void Draw(Vector2 size)
		{
			Draw(size.x , size.y);
		}

		/// <summary>
		/// 绘制
		/// </summary>
		/// <param name="pos">位置 </param>
		/// <param name="size">尺寸（x：宽度，y：高度）</param>
		public void Draw(Vector2 pos , Vector2 size)
		{
			Draw(pos.x , pos.y , size.x , size.y);
		}


		/// <summary>
		/// 重置预览对象
		/// </summary>
		/// <param name="obj">预览对象</param>
		public void ResetPreviewObj(GameObject obj)
		{
			_targetObj = obj;

			if (_previewInstance != null)
			{
				DestroyImmediate(_previewInstance);
				_previewInstance = null;
			}

			ResetTransform();

			if (_targetObj == null) return;

			// 添加物体
			_previewInstance = Instantiate(_targetObj , Vector3.zero , Quaternion.identity);
			_previewRenderUtility.AddSingleGO(_previewInstance);
		}

		/// <summary>
		/// 获取当前的预览实例
		/// </summary>
		public GameObject previewInstance => _previewInstance;
	}
}
