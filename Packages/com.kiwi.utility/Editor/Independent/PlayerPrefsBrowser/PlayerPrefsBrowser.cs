using System;
using System.Linq;

using Microsoft.Win32;

using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

namespace Kiwi.Utility.Editor
{
	/// <summary>
	/// 注册表信息浏览器
	/// </summary>
	public class PlayerPrefsBrowser : EditorWindow
	{
		[ SerializeField ]
		private VisualTreeAsset m_VisualTreeAsset;

		[ SerializeField ]
		private VisualTreeAsset m_ItemVisualTreeAsset;

		/// <summary>
		/// 全部注册表键值对
		/// </summary>
		private PlayerPrefPair[ ] allPlayerPrefs;

		/// <summary>
		/// 当前模式
		/// </summary>
		private Mode _currentMode;

		[ EditorToolbar(EditorToolbarAttribute.Anchor.Right , "Kiwi" , "浏览器/注册表浏览器") ]
		public static void Open()
		{
			var window = CreateWindow<PlayerPrefsBrowser>("注册表浏览器");
			window.ShowUtility();
			window.minSize = new(550 , 450);
		}

		public void CreateGUI()
		{
			allPlayerPrefs = GetAll(_currentMode);

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
					allPlayerPrefs       = GetAll(_currentMode);
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
				allPlayerPrefs       = GetAll(_currentMode);
				listView.itemsSource = allPlayerPrefs;

				listView.Rebuild();
			};

			root.Q<ToolbarButton>("Refresh").clicked += () =>
			{
				Refresh(listView);
			};

			var toolbarMenu = root.Q<ToolbarMenu>();

			toolbarMenu.menu.InsertAction((int) Mode.Runtime , "Runtime" , _ =>
			{
				toolbarMenu.text = "Runtime";
				_currentMode     = Mode.Runtime;

				listView.Clear();

				allPlayerPrefs       = GetAll(_currentMode);
				listView.itemsSource = allPlayerPrefs;

				listView.Rebuild();
			} , _ => _currentMode == Mode.Runtime ? DropdownMenuAction.Status.Checked : DropdownMenuAction.Status.Normal);

			toolbarMenu.menu.InsertAction((int) Mode.Editor , "Editor" , _ =>
			{
				toolbarMenu.text = "Editor";
				_currentMode     = Mode.Editor;

				listView.Clear();

				allPlayerPrefs       = GetAll(_currentMode);
				listView.itemsSource = allPlayerPrefs;

				listView.Rebuild();
			} , _ => _currentMode == Mode.Editor ? DropdownMenuAction.Status.Checked : DropdownMenuAction.Status.Normal);

			var searchField = root.Q<ToolbarPopupSearchField>();

			searchField.menu.AppendAction("All" ,
			                              _ =>
			                              {
				                              searchField.userData = SearchRange.All;
				                              ExecuteSearch(searchField.value , (SearchRange) searchField.userData , listView);
			                              } ,
			                              _ => (SearchRange) searchField.userData == SearchRange.All
				                              ? DropdownMenuAction.Status.Checked
				                              : DropdownMenuAction.Status.Normal
			);

			searchField.menu.AppendAction("Key" ,
			                              _ =>
			                              {
				                              searchField.userData = SearchRange.Key;
				                              ExecuteSearch(searchField.value , (SearchRange) searchField.userData , listView);
			                              } ,
			                              _ => (SearchRange) searchField.userData == SearchRange.Key
				                              ? DropdownMenuAction.Status.Checked
				                              : DropdownMenuAction.Status.Normal
			);

			searchField.menu.AppendAction("Value" ,
			                              _ =>
			                              {
				                              searchField.userData = SearchRange.Value;
				                              ExecuteSearch(searchField.value , (SearchRange) searchField.userData , listView);
			                              } ,
			                              _ => (SearchRange) searchField.userData == SearchRange.Value
				                              ? DropdownMenuAction.Status.Checked
				                              : DropdownMenuAction.Status.Normal
			);

			searchField.RegisterCallback<ChangeEvent<string>>(evt =>
			{
				ExecuteSearch(evt.newValue , (SearchRange) searchField.userData , listView);
			});

			searchField.userData = SearchRange.All;

			var addPopup = rootVisualElement.Q<VisualElement>("AddPopup");

			var newButton    = root.Q<Button>("New");
			var newTypeField = addPopup.Q<DropdownField>("NewType");
			newTypeField.RegisterCallback<ChangeEvent<string>>(evt =>
			{
				switch (evt.newValue)
				{
					case "String":
						addPopup.Q<TextField>("NewValue_String").style.display = DisplayStyle.Flex;
						addPopup.Q<IntegerField>("NewValue_Int").style.display = DisplayStyle.None;
						addPopup.Q<FloatField>("NewValue_Float").style.display = DisplayStyle.None;
						addPopup.Q<Toggle>("NewValue_Bool").style.display      = DisplayStyle.None;

						break;

					case "Int":
						addPopup.Q<TextField>("NewValue_String").style.display = DisplayStyle.None;
						addPopup.Q<IntegerField>("NewValue_Int").style.display = DisplayStyle.Flex;
						addPopup.Q<FloatField>("NewValue_Float").style.display = DisplayStyle.None;
						addPopup.Q<Toggle>("NewValue_Bool").style.display      = DisplayStyle.None;

						break;

					case "Float":
						addPopup.Q<TextField>("NewValue_String").style.display = DisplayStyle.None;
						addPopup.Q<IntegerField>("NewValue_Int").style.display = DisplayStyle.None;
						addPopup.Q<FloatField>("NewValue_Float").style.display = DisplayStyle.Flex;
						addPopup.Q<Toggle>("NewValue_Bool").style.display      = DisplayStyle.None;

						break;

					case "Bool":
						addPopup.Q<TextField>("NewValue_String").style.display = DisplayStyle.None;
						addPopup.Q<IntegerField>("NewValue_Int").style.display = DisplayStyle.None;
						addPopup.Q<FloatField>("NewValue_Float").style.display = DisplayStyle.None;
						addPopup.Q<Toggle>("NewValue_Bool").style.display      = DisplayStyle.Flex;

						break;
				}
			});
			newButton.clicked += () =>
			{
				addPopup.style.display = DisplayStyle.Flex;

				var newKeyField = addPopup.Q<TextField>("NewKey");
				newKeyField.value = string.Empty;

				newTypeField.choices.Clear();

				if (_currentMode == Mode.Runtime)
				{
					newTypeField.choices.AddRange(new[ ]
					                              {
						                              "String" ,
						                              "Int" ,
						                              "Float"
					                              });
				}
				else
				{
					newTypeField.choices.AddRange(new[ ]
					                              {
						                              "String" ,
						                              "Int" ,
						                              "Float" ,
						                              "Bool"
					                              });
				}

				newTypeField.value = "String";
			};
			var okButton = addPopup.Q<Button>("OK");
			okButton.clicked += () =>
			{
				var newKeyField = addPopup.Q<TextField>("NewKey");

				if (string.IsNullOrEmpty(newKeyField.value))
					return;

				var type = addPopup.Q<DropdownField>().value;

				if (_currentMode == Mode.Runtime)
				{
					switch (type)
					{
						case "String":
							PlayerPrefs.SetString(newKeyField.value , addPopup.Q<TextField>("NewValue_String").value);

							break;

						case "Int":
							PlayerPrefs.SetInt(newKeyField.value , addPopup.Q<IntegerField>("NewValue_Int").value);

							break;

						case "Float":
							PlayerPrefs.SetFloat(newKeyField.value , addPopup.Q<FloatField>("NewValue_Float").value);

							break;
					}

					PlayerPrefs.Save();
				}
				else
				{
					switch (type)
					{
						case "String":
							EditorPrefs.SetString(newKeyField.value , addPopup.Q<TextField>("NewValue_String").value);

							break;

						case "Int":
							EditorPrefs.SetInt(newKeyField.value , addPopup.Q<IntegerField>("NewValue_Int").value);

							break;

						case "Float":
							EditorPrefs.SetFloat(newKeyField.value , addPopup.Q<FloatField>("NewValue_Float").value);

							break;

						case "Bool":
							EditorPrefs.SetBool(newKeyField.value , addPopup.Q<Toggle>("NewValue_Bool").value);

							break;
					}
				}

				addPopup.style.display = DisplayStyle.None;
				Refresh(listView);
			};

			root.RegisterCallback<MouseDownEvent>(evt =>
			{
				// 检查点击是否在窗口外部
				var clickPosition = evt.mousePosition;
				if (addPopup.style.display == DisplayStyle.Flex && !addPopup.worldBound.Contains(clickPosition))
					addPopup.style.display = DisplayStyle.None;
			});
		}

		/// <summary>
		/// 刷新显示
		/// </summary>
		private void Refresh(ListView listView)
		{
			listView.Clear();

			allPlayerPrefs       = GetAll(_currentMode);
			listView.itemsSource = allPlayerPrefs;

			listView.Rebuild();
		}

		/// <summary>
		/// 执行搜索
		/// </summary>
		/// <param name="value">搜索内容</param>
		/// <param name="searchRange">搜索范围</param>
		/// <param name="listView">搜索结果列表</param>
		private void ExecuteSearch(string value , SearchRange searchRange , ListView listView)
		{
			listView.Clear();

			if (string.IsNullOrEmpty(value))
			{
				listView.itemsSource = allPlayerPrefs;

				return;
			}

			var filteredPlayerPrefs = searchRange switch
			                          {
				                          SearchRange.All   => allPlayerPrefs.Where(pair => pair.Key.Contains(value , StringComparison.OrdinalIgnoreCase) || pair.Value.ToString().Contains(value , StringComparison.OrdinalIgnoreCase)).ToArray() ,
				                          SearchRange.Key   => allPlayerPrefs.Where(pair => pair.Key.Contains(value , StringComparison.OrdinalIgnoreCase)).ToArray() ,
				                          SearchRange.Value => allPlayerPrefs.Where(pair => pair.Value.ToString().Contains(value , StringComparison.OrdinalIgnoreCase)).ToArray() ,
				                          _                 => null
			                          };

			listView.itemsSource = filteredPlayerPrefs;

			listView.Rebuild();
		}

		/// <summary>
		/// 获得全部指定模式的注册表键值对
		/// </summary>
		/// <param name="mode">模式(运行时还是仅编辑器)</param>
		/// <returns></returns>
		public static PlayerPrefPair[ ] GetAll(Mode mode)
		{
			return GetAll(mode , PlayerSettings.companyName , PlayerSettings.productName);
		}

		/// <summary>
		/// 获得全部指定模式的注册表键值对
		/// </summary>
		/// <param name="mode">模式(运行时还是仅编辑器)</param>
		/// <param name="companyName">公司名称 </param>
		/// <param name="productName">项目名称 </param>
		/// <returns></returns>
		public static PlayerPrefPair[ ] GetAll(Mode mode , string companyName , string productName)
		{
			if (Application.platform == RuntimePlatform.WindowsEditor)
			{
				RegistryKey registryKey = mode switch
				                          {
					                          Mode.Runtime => Registry.CurrentUser.OpenSubKey(@$"Software\Unity\UnityEditor\{companyName}\{productName}") ,
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
							if (mode == Mode.Editor)
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
		/// 模式
		/// </summary>
		public enum Mode
		{
			/// <summary>
			/// 运行时
			/// </summary>
			Runtime ,

			/// <summary>
			/// 仅编辑器
			/// </summary>
			Editor ,
		}

		/// <summary>
		/// 搜索范围
		/// </summary>
		public enum SearchRange
		{
			/// <summary>
			/// 全部
			/// </summary>
			All ,

			/// <summary>
			/// 仅针对 Key
			/// </summary>
			Key ,

			/// <summary>
			/// 仅针对 Value
			/// </summary>
			Value ,
		}

		/// <summary>
		/// Value 类型
		/// </summary>
		public enum ValueType
		{
			String ,
			Int ,
			Float ,
			Bool ,
		}

		/// <summary>
		/// 键值对数据
		/// </summary>
		[ Serializable ]
		public struct PlayerPrefPair
		{
			public string Key { get; set; }

			public ValueType Type { get; set; }

			public object Value { get; set; }
		}
	}
}
