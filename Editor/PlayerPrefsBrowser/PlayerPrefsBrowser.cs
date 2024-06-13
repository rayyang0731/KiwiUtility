using System;
using System.Linq;

using Microsoft.Win32;

using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

namespace Kiwi.Utility.Editor
{
	public class PlayerPrefsBrowser : EditorWindow
	{
		[ SerializeField ]
		private VisualTreeAsset m_VisualTreeAsset;

		[ SerializeField ]
		private VisualTreeAsset m_ItemVisualTreeAsset;

		private PlayerPrefPair[ ] allPlayerPrefs;

		private Type currentType;

		[ EditorToolbar(EditorToolbarAttribute.Anchor.Right , "工具" , "PrefabPrefs 浏览器") ]
		public static void ShowExample()
		{
			var window = GetWindow<PlayerPrefsBrowser>();
			window.titleContent = new("PrefabPrefs 浏览器");
		}

		public void CreateGUI()
		{
			allPlayerPrefs = GetAll(currentType);

			var root = rootVisualElement;
			m_VisualTreeAsset.CloneTree(root);

			var listView = root.Q<ListView>();

			listView.itemsSource = allPlayerPrefs;

			listView.makeItem += () =>
			{
				var newItem = new VisualElement();
				m_ItemVisualTreeAsset.CloneTree(newItem);

				var delButton = newItem.Q<Button>("Del");

				delButton.clicked += () =>
				{
					var index = (int) delButton.userData;
					PlayerPrefs.DeleteKey(((PlayerPrefPair) listView.itemsSource[index]).Key);
					allPlayerPrefs       = GetAll(currentType);
					listView.itemsSource = allPlayerPrefs;

					listView.Rebuild();
				};

				return newItem;
			};

			listView.bindItem += (element , i) =>
			{
				element.Q<Label>("Key").text = ((PlayerPrefPair) listView.itemsSource[i]).Key;
				element.Q<EnumField>().value = ((PlayerPrefPair) listView.itemsSource[i]).Type;
				element.Q<EnumField>().SetEnabled(false);
				element.Q<TextField>("Value").value = ((PlayerPrefPair) listView.itemsSource[i]).Value.ToString();

				var delButton = element.Q<Button>("Del");
				delButton.userData = i;

				if (element.Q<Label>("Key") is {text : "unity.cloud_userid" or "unity.player_sessionid" or "unity.player_session_count" or "UnityGraphicsQuality"})
				{
					element.SetEnabled(false);
				}
				else
				{
					element.SetEnabled(true);
				}
			};

			root.Q<ToolbarButton>("ClearAll").clicked += () =>
			{
				listView.Clear();

				PlayerPrefs.DeleteAll();
				allPlayerPrefs       = GetAll(currentType);
				listView.itemsSource = allPlayerPrefs;

				listView.Rebuild();
			};

			root.Q<ToolbarButton>("Refresh").clicked += () =>
			{
				listView.Clear();

				allPlayerPrefs       = GetAll(currentType);
				listView.itemsSource = allPlayerPrefs;

				listView.Rebuild();
			};

			var toolbarMenu = root.Q<ToolbarMenu>();

			toolbarMenu.menu.InsertAction((int) Type.Runtime , "Runtime" , (action =>
			{
				toolbarMenu.text = "Runtime";
				currentType      = Type.Runtime;

				listView.Clear();

				allPlayerPrefs       = GetAll(currentType);
				listView.itemsSource = allPlayerPrefs;

				listView.Rebuild();
			}));

			toolbarMenu.menu.InsertAction((int) Type.Editor , "Editor" , (action =>
			{
				toolbarMenu.text = "Editor";
				currentType      = Type.Editor;

				listView.Clear();

				allPlayerPrefs       = GetAll(currentType);
				listView.itemsSource = allPlayerPrefs;

				listView.Rebuild();
			}));

			var searchField = root.Q<ToolbarPopupSearchField>();


			searchField.menu.AppendAction("All" , _ => searchField.userData   = SearchRange.All);
			searchField.menu.AppendAction("Key" , _ => searchField.userData   = SearchRange.Key);
			searchField.menu.AppendAction("Value" , _ => searchField.userData = SearchRange.Value);

			searchField.RegisterCallback<ChangeEvent<string>>(evt =>
			{
				listView.Clear();

				if (string.IsNullOrEmpty(evt.newValue))
				{
					listView.itemsSource = allPlayerPrefs;

					return;
				}

				var filteredPlayerPrefs = (string) searchField.userData switch
				                          {
					                          "All"   => allPlayerPrefs.Where(pair => pair.Key.Contains(evt.newValue , StringComparison.OrdinalIgnoreCase) || pair.Value.ToString().Contains(evt.newValue , StringComparison.OrdinalIgnoreCase)).ToArray() ,
					                          "Key"   => allPlayerPrefs.Where(pair => pair.Key.Contains(evt.newValue , StringComparison.OrdinalIgnoreCase)).ToArray() ,
					                          "Value" => allPlayerPrefs.Where(pair => pair.Value.ToString().Contains(evt.newValue , StringComparison.OrdinalIgnoreCase)).ToArray() ,
					                          _       => null
				                          };

				listView.itemsSource = filteredPlayerPrefs;

				listView.Rebuild();
			});
		}

		public static PlayerPrefPair[ ] GetAll(Type type)
		{
			EditorPrefs.SetBool("TestBool" , true);

			return GetAll(type , PlayerSettings.companyName , PlayerSettings.productName);
		}

		public static PlayerPrefPair[ ] GetAll(Type type , string companyName , string productName)
		{
			if (Application.platform == RuntimePlatform.WindowsEditor)
			{
				RegistryKey registryKey = type switch
				                          {
					                          Type.Runtime => Registry.CurrentUser.OpenSubKey(@$"Software\Unity\UnityEditor\{companyName}\{productName}") ,
					                          _            => Registry.CurrentUser.OpenSubKey(@$"Software\Unity Technologies\Unity Editor 5.x")
				                          };

				if (registryKey != null)
				{
					var valueNames      = registryKey.GetValueNames();
					var tempPlayerPrefs = new PlayerPrefPair[ valueNames.Length ];
					var i               = 0;

					foreach (var valueName in valueNames)
					{
						var key   = valueName;
						var index = key.LastIndexOf("_" , StringComparison.Ordinal);
						key = key.Remove(index , key.Length - index);
						var ambiguousValue = registryKey.GetValue(valueName);

						ValueType valueType = ValueType.String;

						if (ambiguousValue is int or long)
						{
							if (type == Type.Editor)
							{
								if (EditorPrefs.GetInt(key , int.MaxValue) != int.MaxValue)
								{
									valueType = ValueType.Int;
								}
								else
								{
									if (!Mathf.Approximately(EditorPrefs.GetFloat(key , float.MaxValue) , float.MaxValue))
									{
										valueType      = ValueType.Float;
										ambiguousValue = EditorPrefs.GetFloat(key);
									}
									else
									{
										valueType      = ValueType.Bool;
										ambiguousValue = EditorPrefs.GetBool(key);
									}
								}
							}
							else
							{
								if (PlayerPrefs.GetInt(key , int.MaxValue) != int.MaxValue)
								{
									valueType = ValueType.Int;
								}
								else
								{
									valueType      = ValueType.Float;
									ambiguousValue = PlayerPrefs.GetFloat(key);
								}
							}
						}
						else if (ambiguousValue is byte[ ] bytes)
						{
							valueType      = ValueType.String;
							ambiguousValue = System.Text.Encoding.Default.GetString(bytes);
						}

						tempPlayerPrefs[i] = new()
						                     {
							                     Key   = key ,
							                     Type  = valueType ,
							                     Value = ambiguousValue
						                     };

						i++;
					}

					return tempPlayerPrefs;
				}

				return Array.Empty<PlayerPrefPair>();
			}

			throw new NotSupportedException("PlayerPrefsEditor doesn't support this Unity Editor platform");
		}

		/// <summary>
		/// 类型
		/// </summary>
		public enum Type
		{
			Runtime ,
			Editor ,
		}

		public enum SearchRange
		{
			All ,
			Key ,
			Value ,
		}

		public enum ValueType
		{
			String ,
			Int ,
			Float ,
			Bool ,
		}

		[ Serializable ]
		public struct PlayerPrefPair
		{
			public string Key { get; set; }

			public ValueType Type { get; set; }

			public object Value { get; set; }
		}
	}
}
