using UnityEngine;

namespace Kiwi.Utility
{
	/// <summary>
	/// 码表工具
	/// </summary>
	public class Stopwatch
	{
		private readonly System.Diagnostics.Stopwatch _stopwatch;

		/// <summary>
		/// 用时(单位:毫秒)
		/// </summary>
		public long ElapseMilliseconds => _stopwatch.ElapsedMilliseconds;

		public Stopwatch() { _stopwatch = System.Diagnostics.Stopwatch.StartNew(); }

		/// <summary>
		/// 刷新
		/// </summary>
		public void Refresh() { _stopwatch?.Restart(); }

		/// <summary>
		/// 停止
		/// </summary>
		/// <param name="showElapsedTime">是否打印耗时</param>
		/// <param name="userData">自定义数据</param>
		public void Stop(bool showElapsedTime = false, object userData = null)
		{
			_stopwatch.Stop();
			if (showElapsedTime)
				Debug.Log($"load [{userData}] : {ElapseMilliseconds}");
		}
	}
}