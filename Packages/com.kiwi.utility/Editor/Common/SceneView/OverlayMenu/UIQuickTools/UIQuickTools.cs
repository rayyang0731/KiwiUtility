using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;

using UnityEngine;
using UnityEngine.UIElements;

namespace Kiwi.Utility.Editor.SceneView.OverlayMenu
{
	[ Overlay(typeof(UnityEditor.SceneView) , "UI Quick Tools") ]
	public class UIQuickTools : ToolbarOverlay
	{
		private UIQuickTools() : base(AlignStartButton.id , AlignEndButton.id , AlignTopButton.id , AlignBottomButton.id ,
		                              AlignCenterButton.id , AlignMiddleButton.id , DistributeHorizontalButton.id , DistributeVerticalButton.id ,
		                              SizeMaxButton.id , SizeMinButton.id , MakeGroupButton.id , UnGroupButton.id , GraphicRaycastOutlineToggle.id ,
		                              UISelectorToggle.id)
		{
		}

		[ EditorToolbarElement(id , typeof(UnityEditor.SceneView)) ]
		internal class AlignStartButton : EditorToolbarButton
		{
			public const string id = "KiwiUIToolbar/AlignStart";

			public AlignStartButton()
			{
				tooltip = text = "左对齐";
				icon    = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.kiwi.utility/EditorAssets/UIQuickTools/align left.png");

				clicked += OnClick;
			}


			void OnClick() { RectTransformTools.AlignTool.Align(RectTransformTools.AlignType.Left); }
		}

		[ EditorToolbarElement(id , typeof(UnityEditor.SceneView)) ]
		internal class AlignEndButton : EditorToolbarButton
		{
			public const string id = "KiwiUIToolbar/AlignEnd";

			public AlignEndButton()
			{
				tooltip = text = "右对齐";
				icon    = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.kiwi.utility/EditorAssets/UIQuickTools/align right.png");

				clicked += OnClick;
			}

			void OnClick() { RectTransformTools.AlignTool.Align(RectTransformTools.AlignType.Right); }
		}

		[ EditorToolbarElement(id , typeof(UnityEditor.SceneView)) ]
		internal class AlignTopButton : EditorToolbarButton
		{
			public const string id = "KiwiUIToolbar/AlignTop";

			public AlignTopButton()
			{
				tooltip = text = "上对齐";
				icon    = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.kiwi.utility/EditorAssets/UIQuickTools/align top.png");

				clicked += OnClick;
			}

			void OnClick() { RectTransformTools.AlignTool.Align(RectTransformTools.AlignType.Top); }
		}

		[ EditorToolbarElement(id , typeof(UnityEditor.SceneView)) ]
		internal class AlignBottomButton : EditorToolbarButton
		{
			public const string id = "KiwiUIToolbar/AlignBottom";

			public AlignBottomButton()
			{
				tooltip = text = "下对齐";
				icon    = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.kiwi.utility/EditorAssets/UIQuickTools/align bottom.png");

				clicked += OnClick;
			}

			void OnClick() { RectTransformTools.AlignTool.Align(RectTransformTools.AlignType.Bottom); }
		}

		[ EditorToolbarElement(id , typeof(UnityEditor.SceneView)) ]
		internal class AlignCenterButton : EditorToolbarButton
		{
			public const string id = "KiwiUIToolbar/AlignCenter";

			public AlignCenterButton()
			{
				tooltip = text = "水平居中";
				icon    = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.kiwi.utility/EditorAssets/UIQuickTools/horizontal center.png");

				clicked += OnClick;
			}

			void OnClick() { RectTransformTools.AlignTool.Align(RectTransformTools.AlignType.HorizontalCenter); }
		}

		[ EditorToolbarElement(id , typeof(UnityEditor.SceneView)) ]
		internal class AlignMiddleButton : EditorToolbarButton
		{
			public const string id = "KiwiUIToolbar/AlignMiddle";

			public AlignMiddleButton()
			{
				tooltip = text = "垂直居中";
				icon    = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.kiwi.utility/EditorAssets/UIQuickTools/vertical center.png");

				clicked += OnClick;
			}

			void OnClick() { RectTransformTools.AlignTool.Align(RectTransformTools.AlignType.VerticalCenter); }
		}

		[ EditorToolbarElement(id , typeof(UnityEditor.SceneView)) ]
		internal class DistributeHorizontalButton : EditorToolbarButton
		{
			public const string id = "KiwiUIToolbar/DistributeHorizontal";

			public DistributeHorizontalButton()
			{
				tooltip = text = "水平平均";
				icon    = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.kiwi.utility/EditorAssets/UIQuickTools/horizontal equal.png");

				clicked += OnClick;
			}

			void OnClick() { RectTransformTools.AlignTool.Align(RectTransformTools.AlignType.Horizontal); }
		}

		[ EditorToolbarElement(id , typeof(UnityEditor.SceneView)) ]
		internal class DistributeVerticalButton : EditorToolbarButton
		{
			public const string id = "KiwiUIToolbar/DistributeVertical";

			public DistributeVerticalButton()
			{
				tooltip = text = "垂直平均";
				icon    = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.kiwi.utility/EditorAssets/UIQuickTools/vertical equal.png");

				clicked += OnClick;
			}

			void OnClick() { RectTransformTools.AlignTool.Align(RectTransformTools.AlignType.Vertical); }
		}

		[ EditorToolbarElement(id , typeof(UnityEditor.SceneView)) ]
		internal class SizeMaxButton : EditorToolbarButton
		{
			public const string id = "KiwiUIToolbar/SizeMax";

			public SizeMaxButton()
			{
				tooltip = text = "等大";
				icon    = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.kiwi.utility/EditorAssets/UIQuickTools/isometric.png");

				clicked += OnClick;
			}

			void OnClick() { RectTransformTools.AlignTool.Align(RectTransformTools.AlignType.SizeMax); }
		}

		[ EditorToolbarElement(id , typeof(UnityEditor.SceneView)) ]
		internal class SizeMinButton : EditorToolbarButton
		{
			public const string id = "KiwiUIToolbar/SizeMin";

			public SizeMinButton()
			{
				tooltip = text = "等小";
				icon    = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.kiwi.utility/EditorAssets/UIQuickTools/isosmall.png");

				clicked += OnClick;
			}

			void OnClick() { RectTransformTools.AlignTool.Align(RectTransformTools.AlignType.SizeMin); }
		}

		[ EditorToolbarElement(id , typeof(UnityEditor.SceneView)) ]
		internal class MakeGroupButton : EditorToolbarButton
		{
			public const string id = "KiwiUIToolbar/MakeGroup";

			public MakeGroupButton()
			{
				tooltip = text = "组合";
				icon    = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.kiwi.utility/EditorAssets/UIQuickTools/group.png");

				clicked += OnClick;
			}

			void OnClick() { RectTransformTools.GroupTool.MakeGroup(); }
		}

		[ EditorToolbarElement(id , typeof(UnityEditor.SceneView)) ]
		internal class UnGroupButton : EditorToolbarButton
		{
			public const string id = "KiwiUIToolbar/UnGroupButton";

			public UnGroupButton()
			{
				tooltip = text = "解组";
				icon    = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.kiwi.utility/EditorAssets/UIQuickTools/ungroup.png");

				clicked += OnClick;
			}

			void OnClick() { RectTransformTools.GroupTool.UnGroup(); }
		}

		[ EditorToolbarElement(id , typeof(UnityEditor.SceneView)) ]
		internal class GraphicRaycastOutlineToggle : EditorToolbarToggle
		{
			public const string id = "KiwiUIToolbar/GraphicRaycastOutlineToggle";

			private GraphicRaycastOutlineToggle()
			{
				tooltip = text = "Raycast 检测器";
				icon    = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.kiwi.utility/EditorAssets/UIQuickTools/raycast check.png");

				this.RegisterValueChangedCallback(ValueChanged);
			}

			private void ValueChanged(ChangeEvent<bool> evt) { GraphicRaycastOutline.Display = evt.newValue; }
		}

		[ EditorToolbarElement(id , typeof(UnityEditor.SceneView)) ]
		internal class UISelectorToggle : EditorToolbarToggle
		{
			public const string id = "UIQuickTools/UISelectorToggle";

			private UISelectorToggle()
			{
				text = "UI选择器";
				icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.kiwi.utility/EditorAssets/UIQuickTools/ui selector.png");

				this.RegisterValueChangedCallback(ValueChanged);
			}

			private void ValueChanged(ChangeEvent<bool> evt) { UISelector.Enabled = evt.newValue; }
		}
	}
}
