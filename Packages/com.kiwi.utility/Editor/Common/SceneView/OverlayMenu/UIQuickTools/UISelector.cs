using System.Collections.Generic;
using System.Linq;

using UnityEditor;
using UnityEditor.SceneManagement;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kiwi.Utility.Editor.SceneView.OverlayMenu
{
	public static class UISelector
	{
		private static bool _enabled = false;

		public static bool Enabled
		{
			get => _enabled;
			set
			{
				if (_enabled == value)
					return;

				_enabled = value;

				if (_enabled)
					UnityEditor.SceneView.duringSceneGui += OnSceneGUI;
				else
					UnityEditor.SceneView.duringSceneGui -= OnSceneGUI;
			}
		}

		private static void OnSceneGUI(UnityEditor.SceneView sceneView)
		{
			var current = Event.current;

			if (current is {button: 1 , type: EventType.MouseDown})
			{
				// 当前屏幕坐标，左上角是（0，0）右下角（camera.pixelWidth，camera.pixelHeight）
				var mousePosition = Event.current.mousePosition;

				// Retina 屏幕需要拉伸值
				var mult = EditorGUIUtility.pixelsPerPoint;

				// 转换成摄像机可接受的屏幕坐标，左下角是（0，0，0）右上角是（camera.pixelWidth，camera.pixelHeight，0）
				mousePosition.y =  sceneView.camera.pixelHeight - mousePosition.y * mult;
				mousePosition.x *= mult;
				var scenes = GetAllScenes();
				var groups = scenes
				             .Where(m => m.isLoaded)
				             .SelectMany(m => m.GetRootGameObjects())
				             .Where(m => m.activeInHierarchy)
				             .SelectMany(m => m.GetComponentsInChildren<RectTransform>())
				             .Where(m => RectTransformUtility.RectangleContainsScreenPoint(m , mousePosition , sceneView.camera))
				             .GroupBy(m => m.gameObject.scene.name)
				             .ToArray();
				var sceneCount = scenes.Count(m => m.isLoaded);

				var menu = new GenericMenu();
				var dic  = new Dictionary<string , int>();

				foreach (var group in groups)
				{
					foreach (var rt in group)
					{
						var name              = rt.name;
						var sceneName         = rt.gameObject.scene.name;
						var nameWithSceneName = sceneName + "/" + name;
						var isContains        = dic.ContainsKey(nameWithSceneName);
						var text              = sceneCount <= 1 ? name : nameWithSceneName;

						if (isContains)
						{
							var count = dic[nameWithSceneName]++;
							text += " [" + count + "]";
						}

						var content = new GUIContent(text);
						menu.AddItem(content , false , () =>
						{
							Selection.activeTransform = rt;
							EditorGUIUtility.PingObject(rt.gameObject);
						});

						if (!isContains)
						{
							dic.Add(nameWithSceneName , 1);
						}
					}
				}

				menu.ShowAsContext();
				current.Use();
			}
		}

		private static IEnumerable<Scene> GetAllScenes()
		{
			var scenes = new HashSet<Scene>();

			for (var i = 0 ; i < SceneManager.sceneCount ; i++)
				scenes.Add(SceneManager.GetSceneAt(i));

			var temp = new GameObject("uiselector");
			if (EditorApplication.isPlaying)
				Object.DontDestroyOnLoad(temp);

			scenes.Add(temp.scene);

			if (EditorApplication.isPlaying)
				Object.Destroy(temp);
			else
				Object.DestroyImmediate(temp);

			var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

			if (prefabStage != null)
				scenes.Add(prefabStage.scene);

			return scenes;
		}
	}
}
