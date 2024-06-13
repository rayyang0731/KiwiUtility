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
		/// <param name="rootPath">根路径(UNITY中一般传入 Application.dataPath)</param>
		/// <param name="absolutePath">要转换的绝对路径</param>
		/// <returns>
		/// 将 <paramref name="absolutePath"/> 转换为
		/// 相对 <paramref name="rootPath"/> 的相对路径
		/// </returns>
		public static string ConvertToRelativePath(string rootPath, string absolutePath)
		{
			if (string.IsNullOrEmpty(rootPath) || string.IsNullOrEmpty(absolutePath))
			{
				Debug.LogError($"要转换的路径存在错误, rootPath : {rootPath}, absolutePath : {absolutePath}");

				return string.Empty;
			}

			var uri         = new Uri(rootPath);
			var absoluteUri = new Uri(absolutePath);
			var relativeUri = uri.MakeRelativeUri(absoluteUri);

			var relativePath = Uri.UnescapeDataString(relativeUri.ToString());

			return relativePath;
		}

		/// <summary>
		/// 转换为绝对路径
		/// </summary>
		/// <param name="rootPath">根路径(UNITY中一般传入 Application.dataPath)</param>
		/// <param name="relativePath">要转换的相对路径</param>
		/// <returns>
		/// 将 <paramref name="relativePath"/> 转换为
		/// 相对 <paramref name="rootPath"/> 的绝对路径
		/// </returns>
		public static string ConvertToAbsolutePath(string rootPath, string relativePath)
		{
			if (string.IsNullOrEmpty(rootPath) || string.IsNullOrEmpty(relativePath))
			{
				Debug.LogError($"要转换的路径存在错误, rootPath : {rootPath}, relativePath : {relativePath}");

				return string.Empty;
			}

			var projectPath = Path.GetFullPath($"{rootPath}/..");

			return Path.Combine(projectPath, relativePath);
		}
	}
}
