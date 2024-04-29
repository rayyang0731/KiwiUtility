using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEditor;

using UnityEngine;

namespace Kiwi.Utility.Editor
{
	public class EditorIconsBrowser : EditorWindow
	{
		[ MenuItem("Window/Editor Icons" , priority = -1001) ]
		public static void EditorIconsOpen()
		{
			var window = CreateWindow<EditorIconsBrowser>("Editor Icons 浏览器");
			window.ShowUtility();
			window.minSize = new(320 , 450);
		}

		static bool viewBigIcons = true;

		static bool darkPreview = true;

		Vector2 scroll;

		int buttonSize = 70;

		string search = "";

		void SearchGUI()
		{
			using (new GUILayout.HorizontalScope())
			{
				if (isWide) GUILayout.Space(10);

#if UNITY_2018
            search = EditorGUILayout.TextField(search, EditorStyles.toolbarTextField);
#else
				search = EditorGUILayout.TextField(search , EditorStyles.toolbarSearchField);
#endif
				if (GUILayout.Button(EditorGUIUtility.IconContent("winbtn_mac_close_h") , //SVN_DeletedLocal
				                     EditorStyles.toolbarButton ,
				                     GUILayout.Width(22))
				) search = "";
			}
		}

		bool isWide => Screen.width > 550;

		bool doSearch => !string.IsNullOrWhiteSpace(search) && search != "";

		GUIContent GetIcon(string icon_name)
		{
			GUIContent valid = null;
			Debug.unityLogger.logEnabled = false;
			if (!string.IsNullOrEmpty(icon_name)) valid = EditorGUIUtility.IconContent(icon_name);
			Debug.unityLogger.logEnabled = true;

			return valid?.image == null ? null : valid;
		}

		void SaveIcon(string icon_name)
		{
			Texture2D tex = EditorGUIUtility.IconContent(icon_name).image as Texture2D;

			if (tex != null)
			{
				string path = EditorUtility.SaveFilePanel(
					"Save icon" , "" , icon_name , "png");

				if (path != null)
				{
					try
					{
#if UNITY_2018
                    Texture2D outTex = new Texture2D(
                        tex.width, tex.height,
                        tex.format, true);
#else
						Texture2D outTex = new Texture2D(
							tex.width , tex.height ,
							tex.format , tex.mipmapCount , true);
#endif

						Graphics.CopyTexture(tex , outTex);

						File.WriteAllBytes(path , outTex.EncodeToPNG());
					}
					catch (System.Exception e)
					{
						Debug.LogError("Cannot save the icon : " + e.Message);
					}
				}
			}
			else
			{
				Debug.LogError("Cannot save the icon : null texture error!");
			}
		}

		private void OnEnable()
		{
			//InitIcons();
			//var all_icons = iconContentListAll.Select(x => x.tooltip).ToArray();
			var all_icons = ico_list.Where(x => GetIcon(x) != null);

			//List<string> found = new List<string>();
			List<string> unique = new List<string>();

			//var skip_flag = HideFlags.HideInInspector | HideFlags.HideAndDontSave;
			//int unique_to_resources = 0, skipped_empty_str = 0, skipped_flags = 0, 
			//    skipped_not_persistent = 0, skipped_nulls = 0, unique_to_list = 0;

			foreach (Texture2D x in Resources.FindObjectsOfTypeAll<Texture2D>())
			{
				//if (string.IsNullOrEmpty(x.name)) skipped_empty_str++;                                      // skipped 10 empty
				//if (!EditorUtility.IsPersistent(x)) skipped_not_persistent++;                               // skipped 39 none persistent
				//if (x.hideFlags != HideFlags.HideAndDontSave && x.hideFlags != skip_flag) skipped_flags++;  // skipped 27 icons

				GUIContent icoContent = GetIcon(x.name);

				if (icoContent == null) continue; // skipped 14 icons 

				//{ 
				//    skipped_nulls++; 
				//    continue; 
				//}

				if (!all_icons.Contains(x.name))
				{
					//unique_to_resources++;
					unique.Add(x.name);
				}

				//found.Add( x.name );
			}

			//foreach (var ico in all_icons) if (!found.Contains(ico)) unique_to_list++;

			//Debug.Log( $"Resources skipped nulls={skipped_nulls} empty={skipped_empty_str} flags={skipped_flags}" );
			//Debug.Log("Resources skipped_not_persistent=" + skipped_not_persistent);
			//Debug.Log($"totals , list: {all_icons.Length} resource: {found.Count}");
			//Debug.Log($"Unique list={ unique_to_list } resources={unique_to_resources}") ;

			// Static list icons count : 1315 ( unique = 749 )
			// Found icons in resources : 1416 ( unique = 855 )

			Resources.UnloadUnusedAssets();
			System.GC.Collect();
		}

		private void OnGUI()
		{
			var ppp = EditorGUIUtility.pixelsPerPoint;

			InitIcons();

			if (!isWide) SearchGUI();

			using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
			{
				GUILayout.Label("Select what icons to show" , GUILayout.Width(160));

				viewBigIcons = GUILayout.SelectionGrid(
						viewBigIcons ? 1 : 0 , new string[ ]
						                       {
							                       "Small" ,
							                       "Big"
						                       } ,
						2 , EditorStyles.toolbarButton)
				 == 1;

				if (isWide) SearchGUI();
			}

			if (isWide) GUILayout.Space(3);

			using (var scope = new GUILayout.ScrollViewScope(scroll))
			{
				GUILayout.Space(10);

				scroll = scope.scrollPosition;

				buttonSize = viewBigIcons ? 70 : 40;

				// scrollbar_width = ~ 12.5
				var render_width = (Screen.width / ppp - 13f);
				var gridW        = Mathf.FloorToInt(render_width / buttonSize);
				var margin_left  = (render_width - buttonSize * gridW) / 2;

				int row = 0 , index = 0;

				List<GUIContent> iconList;

				if (doSearch)
					iconList = iconContentListAll.Where(x => x.tooltip.ToLower()
					                                          .Contains(search.ToLower()))
					                             .ToList();
				else iconList = viewBigIcons ? iconContentListBig : iconContentListSmall;

				while (index < iconList.Count)
				{
					using (new GUILayout.HorizontalScope())
					{
						GUILayout.Space(margin_left);

						for (var i = 0 ; i < gridW ; ++i)
						{
							int k = i + row * gridW;

							var icon = iconList[k];

							if (GUILayout.Button(icon ,
							                     iconButtonStyle ,
							                     GUILayout.Width(buttonSize) ,
							                     GUILayout.Height(buttonSize)))
							{
								EditorGUI.FocusTextInControl("");
								iconSelected = icon;
							}

							index++;

							if (index == iconList.Count) break;
						}
					}

					row++;
				}

				GUILayout.Space(10);
			}


			if (iconSelected == null) return;

			GUILayout.FlexibleSpace();

			using (new GUILayout.HorizontalScope(EditorStyles.helpBox , GUILayout.MaxHeight(viewBigIcons ? 140 : 120)))
			{
				using (new GUILayout.VerticalScope(GUILayout.Width(130)))
				{
					GUILayout.Space(2);

					GUILayout.Button(iconSelected ,
					                 darkPreview ? iconPreviewBlack : iconPreviewWhite ,
					                 GUILayout.Width(128) , GUILayout.Height(viewBigIcons ? 128 : 40));

					GUILayout.Space(5);

					darkPreview = GUILayout.SelectionGrid(
							darkPreview ? 1 : 0 , new string[ ]
							                      {
								                      "Light" ,
								                      "Dark"
							                      } ,
							2 , EditorStyles.miniButton)
					 == 1;

					GUILayout.FlexibleSpace();
				}

				GUILayout.Space(10);

				using (new GUILayout.VerticalScope())
				{
					var s = $"Size: {iconSelected.image.width}x{iconSelected.image.height}";
					s += "\nIs Pro Skin Icon: " + (iconSelected.tooltip.IndexOf("d_") == 0 ? "Yes" : "No");
					s += $"\nTotal {iconContentListAll.Count} icons";
					GUILayout.Space(5);
					EditorGUILayout.HelpBox(s , MessageType.None);
					GUILayout.Space(5);
					EditorGUILayout.TextField("EditorGUIUtility.IconContent(\"" + iconSelected.tooltip + "\")");
					GUILayout.Space(5);

					if (GUILayout.Button("Copy to clipboard" , EditorStyles.miniButton))
						EditorGUIUtility.systemCopyBuffer = iconSelected.tooltip;

					if (GUILayout.Button("Save icon to file ..." , EditorStyles.miniButton))
						SaveIcon(iconSelected.tooltip);
				}

				GUILayout.Space(10);

				if (GUILayout.Button("X" , GUILayout.ExpandHeight(true)))
				{
					iconSelected = null;
				}
			}
		}

		static GUIContent iconSelected;
		static GUIStyle iconBtnStyle = null;
		static List<GUIContent> iconContentListAll;
		static List<GUIContent> iconContentListSmall;
		static List<GUIContent> iconContentListBig;
		static List<string> iconMissingNames;
		static GUIStyle iconButtonStyle = null;
		static GUIStyle iconPreviewBlack = null;
		static GUIStyle iconPreviewWhite = null;

		void AllTheTEXTURES(ref GUIStyle s , Texture2D t)
		{
			s.hover.background = s.onHover.background = s.focused.background = s.onFocused.background = s.active.background = s.onActive.background = s.normal.background = s.onNormal.background = t;

			s.hover.scaledBackgrounds = s.onHover.scaledBackgrounds = s.focused.scaledBackgrounds = s.onFocused.scaledBackgrounds = s.active.scaledBackgrounds = s.onActive.scaledBackgrounds = s.normal.scaledBackgrounds = s.onNormal.scaledBackgrounds = new Texture2D[ ]
			                                                                                                                                                                                                                                                {
				                                                                                                                                                                                                                                                t
			                                                                                                                                                                                                                                                };
		}

		Texture2D Texture2DPixel(Color c)
		{
			Texture2D t = new Texture2D(1 , 1);
			t.SetPixel(0 , 0 , c);
			t.Apply();

			return t;
		}

		void InitIcons()
		{
			if (iconContentListSmall != null) return;

			iconButtonStyle             = new GUIStyle(EditorStyles.miniButton);
			iconButtonStyle.margin      = new RectOffset(0 , 0 , 0 , 0);
			iconButtonStyle.fixedHeight = 0;

			iconPreviewBlack = new GUIStyle(iconButtonStyle);
			AllTheTEXTURES(ref iconPreviewBlack , Texture2DPixel(new Color(0.15f , 0.15f , 0.15f)));

			iconPreviewWhite = new GUIStyle(iconButtonStyle);
			AllTheTEXTURES(ref iconPreviewWhite , Texture2DPixel(new Color(0.85f , 0.85f , 0.85f)));

			iconMissingNames     = new List<string>();
			iconContentListSmall = new List<GUIContent>();
			iconContentListBig   = new List<GUIContent>();
			iconContentListAll   = new List<GUIContent>();

			for (var i = 0 ; i < ico_list.Length ; ++i)
			{
				GUIContent ico = GetIcon(ico_list[i]);

				if (ico == null)
				{
					iconMissingNames.Add(ico_list[i]);

					continue;
				}

				ico.tooltip = ico_list[i];

				iconContentListAll.Add(ico);

				if (!(ico.image.width <= 36 || ico.image.height <= 36))
					iconContentListBig.Add(ico);
				else iconContentListSmall.Add(ico);
			}
		}

		// https://gist.github.com/MattRix/c1f7840ae2419d8eb2ec0695448d4321
		// https://unitylist.com/p/5c3/Unity-editor-icons

		#region ICONS

		[ MenuItem("") ]
		public static void AllIcon()
		{
			Texture2D[ ] t = Resources.FindObjectsOfTypeAll<Texture2D>();

			foreach (Texture2D x in t)
			{
				Debug.Log(x.name);
			}
		}

		private static string[ ] _iconNames;

		public static string[ ] ico_list
		{
			get
			{
				if (_iconNames == null)
				{
					var texture2Ds = Resources.FindObjectsOfTypeAll<Texture2D>();
					var result     = new List<string>();

					for (var i = 0 ; i < texture2Ds.Length ; i++)
					{
						result.Add(texture2Ds[i].name);
					}

					result.Sort();
					_iconNames = result.ToArray();
					Resources.UnloadUnusedAssets();
				}

				return _iconNames;
			}
		}

		#endregion
	}
}
