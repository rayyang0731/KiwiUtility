using System;
using System.Diagnostics;

using Debug = UnityEngine.Debug;

namespace Kiwi.Utility.Editor
{
	/// <summary>
	/// 程序执行辅助器
	/// </summary>
	public static class ProcessHelper
	{
		/// <summary>
		/// 执行命令行
		/// </summary>
		/// <param name="folderPath">要执行文件的路径</param>
		/// <param name="fileName">要执行的文件名称(带扩展名)</param>
		/// <param name="args">参数</param>
		/// <param name="useShellExecute">是否打开程序或者文件或者其他任何能够打开的东西（如网址）</param>
		/// <param name="noWindow">是否显示窗口</param>
		/// <param name="printException">是否打印错误</param>
		/// <returns>CMD output 信息</returns>
		public static string ExecuteCommand(string folderPath , string fileName , string args = null , bool useShellExecute = true , bool noWindow = false , bool printException = false)
		{
			var output    = string.Empty;
			var errorInfo = string.Empty;

			if (string.IsNullOrEmpty(folderPath) || string.IsNullOrEmpty(fileName))
			{
				output = "要执行文件的路径或文件名称为空.";
				Debug.LogError(output);

				return output;
			}

			var process = new System.Diagnostics.Process()
			              {
				              StartInfo = new ProcessStartInfo
				                          {
					                          WorkingDirectory       = folderPath ,
					                          FileName               = fileName ,
					                          Arguments              = !string.IsNullOrEmpty(args) ? args : string.Empty ,
					                          UseShellExecute        = useShellExecute ,
					                          RedirectStandardInput  = !useShellExecute ,
					                          RedirectStandardOutput = !useShellExecute ,
					                          RedirectStandardError  = !useShellExecute ,
					                          CreateNoWindow         = noWindow ,
				                          }
			              };

			try
			{
				if (process.Start()) //开始进程
				{
					if (!useShellExecute)
					{
						//读取输出流释放缓冲
						output    = process.StandardOutput.ReadToEnd();
						errorInfo = process.StandardError.ReadToEnd();
					}

					process.WaitForExit();

					Debug.Log("命令执行完成.");
				}
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
			finally
			{
				process.Close();
			}

			if (string.IsNullOrEmpty(errorInfo)) return output;

			if (printException)
				Debug.LogError(errorInfo);

			return output;
		}
	}
}
