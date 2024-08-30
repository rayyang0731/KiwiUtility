using System;

namespace Kiwi.Utility
{
	/// <summary>
	/// 时间工具
	/// </summary>
	public static class TimeUtil
	{
		private static readonly DateTime DataStart = new(1970, 1, 1);

		/// <summary>
		/// 当前时间总毫秒数
		/// </summary>
		/// <returns></returns>
		public static long CurrentTotalMS_Long()
		{
			return (long)CurrentTotalMS_Double();
		}

		/// <summary>
		/// 当前时间总毫秒数
		/// </summary>
		/// <returns></returns>
		public static double CurrentTotalMS_Double()
		{
			return (DateTime.UtcNow - DataStart).TotalMilliseconds;
		}
	}
}
