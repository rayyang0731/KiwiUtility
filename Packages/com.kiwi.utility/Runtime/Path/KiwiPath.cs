using System;
using System.IO;

using UnityEngine;

namespace Kiwi.Utility
{
	/// <summary>
	/// 路径工具
	/// </summary>
	public static class KiwiPath
	{
		/// <summary>
		/// 转换为相对路径
		/// </summary>
		/// <param name="absolutePath">要转换的绝对路径</param>
		/// <param name="rootPath">根路径(默认为 Application.dataPath)</param>
		/// <returns>
		/// 将 <paramref name="absolutePath"/> 转换为
		/// 相对 <paramref name="rootPath"/> 的相对路径
		/// </returns>
		public static string ConvertToRelativePath(string absolutePath , string rootPath = null)
		{
			if (string.IsNullOrEmpty(absolutePath))
			{
				Debug.LogError($"要转换的路径存在错误 absolutePath : {absolutePath}");

				return string.Empty;
			}

			if (string.IsNullOrEmpty(rootPath)) rootPath = Application.dataPath;

			var uri         = new Uri(rootPath);
			var absoluteUri = new Uri(absolutePath);
			var relativeUri = uri.MakeRelativeUri(absoluteUri);

			var relativePath = Uri.UnescapeDataString(relativeUri.ToString());

			return relativePath;
		}

		/// <summary>
		/// 转换为绝对路径
		/// </summary>
		/// <param name="relativePath">要转换的相对路径</param>
		/// <param name="rootPath">根路径(默认为 Application.dataPath)</param>
		/// <returns>
		/// 将 <paramref name="relativePath"/> 转换为
		/// 相对 <paramref name="rootPath"/> 的绝对路径
		/// </returns>
		public static string ConvertToAbsolutePath(string relativePath , string rootPath = null)
		{
			if (string.IsNullOrEmpty(relativePath))
			{
				Debug.LogError($"要转换的路径存在错误, rootPath : {rootPath}, relativePath : {relativePath}");

				return string.Empty;
			}

			if (string.IsNullOrEmpty(rootPath)) rootPath = Application.dataPath;

			var projectPath = Path.GetFullPath($"{rootPath}/..");

			return Path.Combine(projectPath , relativePath);
		}
	}
}
