using System;

namespace Kiwi.Utility.Editor
{
	[ AttributeUsage(AttributeTargets.Method) ]
	public class EditorToolbarAttribute : Attribute
	{
		/// <summary>
		/// 按钮名称
		/// </summary>
		public string Category;

		/// <summary>
		/// 位置
		/// </summary>
		public Anchor Position;

		/// <summary>
		/// 名称
		/// </summary>
		public string Path;

		/// <summary>
		/// 自定义
		/// </summary>
		public bool Custom;

		/// <summary>
		/// 排序
		/// </summary>
		public int Order;

		/// <summary>
		/// 自定义类型
		/// </summary>
		/// <param name="anchor">位置</param>
		/// <param name="custom">是否是自定义GUI</param>
		public EditorToolbarAttribute(Anchor anchor , bool custom)
		{
			Position = anchor;
			Custom   = custom;
		}

		/// <summary>
		/// 按钮类型
		/// </summary>
		/// <param name="anchor">位置</param>
		/// <param name="custom">是否是自定义GUI</param>
		/// <param name="order">排序</param>
		public EditorToolbarAttribute(Anchor anchor , bool custom , int order)
		{
			Position = anchor;
			Custom   = custom;
			Order    = order;
		}

		/// <summary>
		/// 按钮类型
		/// </summary>
		/// <param name="anchor">位置</param>
		/// <param name="category">按钮名称</param>
		public EditorToolbarAttribute(Anchor anchor , string category)
		{
			Position = anchor;
			Category = category;
		}

		/// <summary>
		/// 按钮类型
		/// </summary>
		/// <param name="anchor">位置</param>
		/// <param name="category">按钮名称</param>
		/// <param name="order">排序</param>
		public EditorToolbarAttribute(Anchor anchor , string category , int order)
		{
			Position = anchor;
			Category = category;
			Order    = order;
		}

		/// <summary>
		/// 下拉菜单类型
		/// </summary>
		/// <param name="anchor">位置</param>
		/// <param name="category">下拉菜单按钮名称</param>
		/// <param name="path">按钮路径</param>
		public EditorToolbarAttribute(Anchor anchor , string category , string path)
		{
			Position = anchor;
			Category = category;
			Path     = path;
		}

		/// <summary>
		/// 下拉菜单类型
		/// </summary>
		/// <param name="anchor">位置</param>
		/// <param name="category">下拉菜单按钮名称</param>
		/// <param name="path">按钮路径</param>
		/// <param name="order">排序</param>
		public EditorToolbarAttribute(Anchor anchor , string category , string path , int order)
		{
			Position = anchor;
			Category = category;
			Path     = path;
			Order    = order;
		}

		/// <summary>
		/// 位置
		/// </summary>
		public enum Anchor
		{
			/// <summary>
			/// 左侧
			/// </summary>
			Left ,

			/// <summary>
			/// 播放按钮左侧
			/// </summary>
			CenterLeft ,

			/// <summary>
			/// 播放按钮右侧
			/// </summary>
			CenterRight ,

			/// <summary>
			/// 右侧
			/// </summary>
			Right ,
		}
	}
}
