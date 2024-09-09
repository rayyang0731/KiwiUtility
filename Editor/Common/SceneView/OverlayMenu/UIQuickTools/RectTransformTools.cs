using System;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;

using UnityEngine;

namespace Kiwi.Utility.Editor.SceneView.OverlayMenu
{
	internal static class RectTransformTools
	{
		public enum AlignType
		{
			/// <summary>
			/// 上对齐
			/// </summary>
			Top = 1 ,

			/// <summary>
			/// 左对齐
			/// </summary>
			Left = 2 ,

			/// <summary>
			/// 右对齐
			/// </summary>
			Right = 3 ,

			/// <summary>
			/// 下对齐
			/// </summary>
			Bottom = 4 ,

			/// <summary>
			/// 水平居中
			/// </summary>
			HorizontalCenter = 5 ,

			/// <summary>
			/// 垂直居中
			/// </summary>
			VerticalCenter = 6 ,

			/// <summary>
			/// 横向分布
			/// </summary>
			Horizontal = 7 ,

			/// <summary>
			/// 纵向分布
			/// </summary>
			Vertical = 8 ,

			/// <summary>
			/// 尺寸等小
			/// </summary>
			SizeMin = 9 ,

			/// <summary>
			/// 尺寸等大
			/// </summary>
			SizeMax = 10 ,
		}

		/// <summary>
		/// 对齐工具
		/// </summary>
		public static class AlignTool
		{
			public static void Align(AlignType type)
			{
				var objects = Selection.GetFiltered<RectTransform>(SelectionMode.TopLevel);
				if (objects.Length > 1) Align(type , new List<RectTransform>(objects));
			}

			public static void Align(AlignType type , List<RectTransform> rects)
			{
				var template  = rects[0];
				var sizeDelta = template.sizeDelta;
				var position  = template.position;
				var pivot     = template.pivot;

				var w = sizeDelta.x * template.lossyScale.x;
				var h = sizeDelta.y * template.localScale.y;
				var x = position.x - pivot.x * w;
				var y = position.y - pivot.y * h;

				Undo.RecordObjects(rects.ToArray() , $"Align {Enum.GetName(typeof(AlignType) , type)}");

				switch (type)
				{
					case AlignType.Top:
						for (var i = 1 ; i < rects.Count ; i++)
						{
							var trans  = rects[i];
							var height = trans.sizeDelta.y * trans.localScale.y;
							var pos    = trans.position;
							pos.y          = y + h - height + trans.pivot.y * height;
							trans.position = pos;
						}

						break;

					case AlignType.Left:
						for (var i = 1 ; i < rects.Count ; i++)
						{
							var trans = rects[i];
							var width = trans.sizeDelta.x * trans.lossyScale.x;
							var pos   = trans.position;
							pos.x          = x + width * trans.pivot.x;
							trans.position = pos;
						}

						break;

					case AlignType.Right:
						for (var i = 1 ; i < rects.Count ; i++)
						{
							var trans = rects[i];
							var width = trans.sizeDelta.x * trans.lossyScale.x;
							var pos   = trans.position;
							pos.x          = x + w - width + width * trans.pivot.x;
							trans.position = pos;
						}

						break;

					case AlignType.Bottom:
						for (var i = 1 ; i < rects.Count ; i++)
						{
							var trans  = rects[i];
							var height = trans.sizeDelta.y * trans.localScale.y;
							var pos    = trans.position;
							pos.y          = y + height * trans.pivot.y;
							trans.position = pos;
						}

						break;

					case AlignType.HorizontalCenter:
						for (var i = 1 ; i < rects.Count ; i++)
						{
							var trans = rects[i];
							var width = trans.sizeDelta.x * trans.lossyScale.x;
							var pos   = trans.position;
							pos.x          = x + 0.5f * (w - width) + width * trans.pivot.x;
							trans.position = pos;
						}

						break;

					case AlignType.VerticalCenter:
						for (var i = 1 ; i < rects.Count ; i++)
						{
							var trans  = rects[i];
							var height = trans.sizeDelta.y * trans.localScale.y;
							var pos    = trans.position;
							pos.y          = y + 0.5f * (h - height) + height * trans.pivot.y;
							trans.position = pos;
						}

						break;

					case AlignType.Horizontal:
						var minX = GetMinX(rects);
						var maxX = GetMaxX(rects);
						rects.Sort(SortListRectTransformByX);
						var distance = (maxX - minX) / (rects.Count - 1);

						for (var i = 1 ; i < rects.Count - 1 ; i++)
						{
							var trans = rects[i];
							var pos   = trans.position;
							pos.x          = minX + i * distance;
							trans.position = pos;
						}

						break;

					case AlignType.Vertical:
						var minY = GetMinY(rects);
						var maxY = GetMaxY(rects);
						rects.Sort(SortListRectTransformByY);
						var distanceY = (maxY - minY) / (rects.Count - 1);

						for (var i = 1 ; i < rects.Count - 1 ; i++)
						{
							var trans = rects[i];
							var pos   = trans.position;
							pos.y          = minY + i * distanceY;
							trans.position = pos;
						}

						break;

					case AlignType.SizeMin:
					{
						var height = Mathf.Min(rects.Select(obj => obj.GetHeight()).ToArray());
						var width  = Mathf.Min(rects.Select(obj => obj.GetWidth()).ToArray());

						foreach (var rt in rects)
						{
							rt.SetSize(width , height);
						}
					}

						break;

					case AlignType.SizeMax:
					{
						var height = Mathf.Max(rects.Select(obj => obj.GetHeight()).ToArray());
						var width  = Mathf.Max(rects.Select(obj => obj.GetWidth()).ToArray());

						foreach (var rt in rects)
						{
							rt.SetSize(width , height);
						}
					}

						break;
				}
			}

			private static int SortListRectTransformByX(RectTransform r1 , RectTransform r2)
			{
				var w  = r1.sizeDelta.x * r1.lossyScale.x;
				var x1 = r1.position.x - r1.pivot.x * w;
				w = r2.sizeDelta.x * r2.lossyScale.x;
				var x2 = r2.position.x - r2.pivot.x * w;

				if (x1 >= x2)
					return 1;

				return -1;
			}

			private static int SortListRectTransformByY(RectTransform r1 , RectTransform r2)
			{
				var w  = r1.sizeDelta.y * r1.lossyScale.y;
				var y1 = r1.position.y - r1.pivot.y * w;
				w = r2.sizeDelta.y * r2.lossyScale.y;
				var y2 = r2.position.y - r2.pivot.y * w;

				if (y1 >= y2)
					return 1;

				return -1;
			}

			private static float GetMinX(List<RectTransform> rects)
			{
				if (rects == null || rects.Count == 0)
					return 0;

				return Mathf.Min(rects.Select(obj => obj.position.x).ToArray());
			}

			private static float GetMaxX(List<RectTransform> rects)
			{
				if (null == rects || rects.Count == 0)
					return 0;

				return Mathf.Max(rects.Select(obj => obj.position.x).ToArray());
			}

			private static float GetMinY(List<RectTransform> rects)
			{
				if (null == rects || rects.Count == 0)
					return 0;

				return Mathf.Min(rects.Select(obj => obj.position.y).ToArray());
			}

			private static float GetMaxY(List<RectTransform> rects)
			{
				if (null == rects || rects.Count == 0)
					return 0;

				return Mathf.Max(rects.Select(obj => obj.position.y).ToArray());
			}
		}

		/// <summary>
		/// 组合工具
		/// </summary>
		public static class GroupTool
		{
			/// <summary>
			/// 解组
			/// </summary>
			public static void UnGroup()
			{
				if (Selection.gameObjects == null || Selection.gameObjects.Length <= 0)
				{
					UnityEditor.SceneView.lastActiveSceneView.ShowNotification(new GUIContent("请选中一个节点") , 1f);

					return;
				}

				if (Selection.gameObjects.Length > 1)
				{
					UnityEditor.SceneView.lastActiveSceneView.ShowNotification(new GUIContent("只能选择一个对象") , 1f);

					return;
				}

				var target    = Selection.activeTransform;
				var newParent = target.parent;

				if (target.childCount > 0)
				{
					var children = target.GetComponentsInChildren<Transform>(true);

					foreach (var child in children)
					{
						if (child.parent != target || child == target) // 不是自己的子节点或是自己的话就跳过
							continue;

						Undo.SetTransformParent(child , newParent , "UnGroup");
					}

					Undo.DestroyObjectImmediate(target.gameObject);
				}
				else
				{
					UnityEditor.SceneView.lastActiveSceneView.ShowNotification(new GUIContent("没有子节点对象") , 1f);
				}
			}

			public static void MakeGroup()
			{
				if (Selection.gameObjects == null || Selection.gameObjects.Length <= 0)
				{
					UnityEditor.SceneView.lastActiveSceneView.ShowNotification(new GUIContent("请选中一个节点") , 1f);

					return;
				}

				// 先判断选中的节点是不是挂在同个父节点上的
				var parent = Selection.transforms[0].parent;

				if (Selection.transforms.Any(item => item.parent != parent))
				{
					UnityEditor.SceneView.lastActiveSceneView.ShowNotification(new GUIContent("不能跨父节点组合") , 1f);

					return;
				}

				var container = new GameObject("container" , typeof(RectTransform));

				Undo.IncrementCurrentGroup();

				var groupIndex = Undo.GetCurrentGroup();

				Undo.SetCurrentGroupName("Make Group");
				Undo.RegisterCreatedObjectUndo(container , "Create Group Object");
				var rectTrans = container.rectTransform();

				if (rectTrans != null)
				{
					var leftTopPos     = new Vector2(float.MaxValue , float.MinValue);
					var rightBottomPos = new Vector2(float.MinValue , float.MaxValue);

					foreach (var item in Selection.transforms)
					{
						var bound      = EditorUtil.GetBounds(item.gameObject);
						var itemParent = item.parent;

						var boundMin = itemParent.InverseTransformPoint(bound.min);
						var boundMax = itemParent.InverseTransformPoint(bound.max);

						if (boundMin.x < leftTopPos.x)
							leftTopPos.x = boundMin.x;
						if (boundMax.y > leftTopPos.y)
							leftTopPos.y = boundMax.y;
						if (boundMax.x > rightBottomPos.x)
							rightBottomPos.x = boundMax.x;
						if (boundMin.y < rightBottomPos.y)
							rightBottomPos.y = boundMin.y;
					}

					rectTrans.SetParent(parent);

					var sizeDelta = new Vector2(rightBottomPos.x - leftTopPos.x , leftTopPos.y - rightBottomPos.y);
					rectTrans.sizeDelta =  sizeDelta;
					leftTopPos.x        += sizeDelta.x / 2;
					leftTopPos.y        -= sizeDelta.y / 2;

					rectTrans.localPosition = leftTopPos;
					rectTrans.localScale    = Vector3.one;

					// 需要先生成好Box和设置好它的坐标和大小才可以把选中的节点挂进来，注意要先排好序，不然层次就乱了
					var sortedObjs = Selection.transforms.OrderBy(x => x.GetSiblingIndex()).ToArray();

					foreach (var obj in sortedObjs)
					{
						Undo.SetTransformParent(obj , rectTrans , "move item to group");
					}
				}

				Selection.activeGameObject = container;

				Undo.CollapseUndoOperations(groupIndex);
			}
		}
	}
}
