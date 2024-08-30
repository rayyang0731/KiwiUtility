using System;
using System.Text;

namespace Kiwi.Utility
{
	/// <summary>
	/// 时间工具集
	/// </summary>
	public static class TimeUtility
	{
		/// <summary>
		/// Unix 纪元时间
		/// <para>1970 年 01 月 01 日 0 时 0 分 0 秒</para>
		/// </summary>
		public static DateTime UnixTime { get; } = new(1970 , 1 , 1 , 0 , 0 , 0);


		#region 获取当前时间相关函数

		/// <summary>
		/// 获取当前 UTC 时间
		/// </summary>
		/// <returns>当前 UTC 时间</returns>
		public static DateTime GetCurrentUtcTime()
		{
			return DateTime.UtcNow;
		}

		/// <summary>
		/// 获取当前本地时间
		/// </summary>
		/// <returns>当前本地时间</returns>
		public static DateTime GetCurrentLocalTime()
		{
			return DateTime.Now;
		}

		/// <summary>
		/// 获取 Unix 时间戳
		/// <para>单位 : 秒</para>
		/// </summary>
		/// <returns>秒级的 Unix 时间戳</returns>
		public static long GetUnixTimestamp()
		{
			return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		}

		/// <summary>
		/// 获取 Unix 时间戳
		/// <para>单位 : 毫秒</para>
		/// </summary>
		/// <returns>毫秒级的 Unix 时间戳</returns>
		public static long GetUnixTimestampMilliseconds()
		{
			return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		}

		#endregion

		#region 时间转换相关函数

		/// <summary>
		/// 将 UTC 时间转换为本地时间
		/// </summary>
		/// <param name="utcTime">要转换的 UTC 时间</param>
		/// <returns>转换后的本地时间</returns>
		public static DateTime ConvertUtcToLocal(DateTime utcTime)
		{
			return utcTime.ToLocalTime();
		}

		/// <summary>
		/// 将本地时间转换为 UTC 时间
		/// </summary>
		/// <param name="localTime">要转换的本地时间</param>
		/// <returns>转换后的 UTC 时间</returns>
		public static DateTime ConvertLocalToUtc(DateTime localTime)
		{
			return localTime.ToUniversalTime();
		}

		/// <summary>
		/// 将 Unix 时间戳转换为 DateTime
		/// </summary>
		/// <param name="timestamp">秒级别的时间戳</param>
		/// <returns>转换后的时间</returns>
		public static DateTime ConvertUnixTimestampToDateTime(long timestamp)
		{
			return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
		}

		/// <summary>
		/// 将 Unix 时间戳转换为 DateTime
		/// </summary>
		/// <param name="timestamp">毫秒级别的时间戳</param>
		/// <returns>转换后的时间</returns>
		public static DateTime ConvertUnixTimestampMillisecondsToDateTime(long timestamp)
		{
			return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
		}

		/// <summary>
		/// 将 DateTime 转换为 Unix 时间戳
		/// <para>单位 : 秒</para>
		/// </summary>
		/// <param name="dateTime">要转换的时间</param>
		/// <returns>转换后的秒级 Unix 时间戳</returns>
		public static long ConvertDateTimeToUnixTimestamp(DateTime dateTime)
		{
			return ((DateTimeOffset) dateTime).ToUnixTimeSeconds();
		}

		/// <summary>
		/// 将 DateTime 转换为 Unix 时间戳
		/// <para>单位 : 秒</para>
		/// </summary>
		/// <param name="dateTime">要转换的时间</param>
		/// <returns>转换后的秒级 Unix 时间戳</returns>
		public static long ConvertDateTimeToUnixTimestampMilliseconds(DateTime dateTime)
		{
			return ((DateTimeOffset) dateTime).ToUnixTimeMilliseconds();
		}

		#endregion

		#region 时间计算相关函数

		/// <summary>
		/// 获取两个日期之间的天数差
		/// </summary>
		/// <param name="startDate">开始日期</param>
		/// <param name="endDate">结束日期</param>
		/// <returns>两个日期之间的天数差</returns>
		public static int GetDaysDifference(DateTime startDate , DateTime endDate)
		{
			return (endDate.Date - startDate.Date).Days;
		}

		/// <summary>
		/// 获取两个日期之间的小时差
		/// </summary>
		/// <param name="startDate">开始日期</param>
		/// <param name="endDate">结束日期</param>
		/// <returns>两个日期之间的小时差</returns>
		public static int GetHoursDifference(DateTime startDate , DateTime endDate)
		{
			return (endDate.Date - startDate.Date).Hours;
		}

		/// <summary>
		/// 获取两个日期之间的分钟差
		/// </summary>
		/// <param name="startDate">开始日期</param>
		/// <param name="endDate">结束日期</param>
		/// <returns>两个日期之间的分钟差</returns>
		public static int GetMinutesDifference(DateTime startDate , DateTime endDate)
		{
			return (endDate.Date - startDate.Date).Minutes;
		}

		/// <summary>
		/// 获取两个日期之间的秒数差
		/// </summary>
		/// <param name="startDate">开始日期</param>
		/// <param name="endDate">结束日期</param>
		/// <returns>两个日期之间的秒数差</returns>
		public static int GetSecondsDifference(DateTime startDate , DateTime endDate)
		{
			return (endDate.Date - startDate.Date).Seconds;
		}

		/// <summary>
		/// 获取两个日期之间的毫秒差
		/// </summary>
		/// <param name="startDate">开始日期</param>
		/// <param name="endDate">结束日期</param>
		/// <returns>两个日期之间的毫秒差</returns>
		public static int GetMillisecondsDifference(DateTime startDate , DateTime endDate)
		{
			return (endDate.Date - startDate.Date).Milliseconds;
		}

		#endregion

		// /// <summary>
		// /// 纪元起始时
		// /// </summary>
		// public static DateTime UnixTime = new(1970 , 1 , 1 , 0 , 0 , 0);
		//
		// /// <summary>
		// /// 当前时区(默认 : 北京时间 [ +8 时区 ])
		// /// </summary>
		// public static int CurrentTimeZone { get; set; } = 8;
		//
		// /// <summary>
		// /// 时区偏移(秒)
		// /// </summary>
		// public static int GetTimeZoneOffset(int timeZone)
		// {
		// 	return timeZone * 3600;
		// }
		//
		// /// <summary>
		// /// 获取本地时间戳 10位(单位:秒)
		// /// </summary>
		// public static long GetLocalTimestampSec()
		// {
		// 	var ts = DateTime.UtcNow - UnixTime;
		//
		// 	return Convert.ToInt64(ts.TotalSeconds);
		// }
		//
		// /// <summary>
		// /// 获取本地时间戳 13位(单位:毫秒)
		// /// </summary>
		// public static long GetLocalTimestampMS()
		// {
		// 	var ts = DateTime.UtcNow - UnixTime;
		//
		// 	return Convert.ToInt64(ts.TotalMilliseconds);
		// }
		//
		// /// <summary>
		// /// 时间戳转换
		// /// </summary>
		// /// <param name="timestamp">要转换的时间戳</param>
		// /// <param name="timeZone">时区</param>
		// public static DateTime ConvertTimestamp(double timestamp , int timeZone)
		// {
		// 	var timeZoneOffset = GetTimeZoneOffset(timeZone);
		//
		// 	var dt = UnixTime;
		// 	dt = dt.Add(new(0 , 0 , timeZoneOffset));
		//
		// 	return timestamp > 9999999999D
		// 		? dt.AddMilliseconds(timestamp) //13位
		// 		: dt.AddSeconds(timestamp);     //10位
		// }
		//
		// /// <summary>
		// /// 时间戳转换(按默认时区转换)
		// /// </summary>
		// public static DateTime ConvertTimestamp(double timestamp) { return ConvertTimestamp(timestamp , CurrentTimeZone); }
		//
		// /// <summary>
		// /// 时间戳转换字符串格式
		// /// </summary>
		// public static string ConvertTimestampToString(double timestamp , int timeZone , string format = "yyyy/MM/dd HH:mm:ss") { return ConvertTimestamp(timestamp , timeZone).ToString(format); }
		//
		// /// <summary>
		// /// 时间戳转换字符串格式(按默认时区转换)
		// /// </summary>
		// public static string ConvertTimestampToString(double timestamp , string format = "yyyy/MM/dd HH:mm:ss") { return ConvertTimestamp(timestamp).ToString(format); }
		//
		// /// <summary>
		// /// 根据时间戳(单位:秒)获取时间跨度信息
		// /// </summary>
		// public static TimeSpan GetTimeSpanByTimestampSec(double oldTimestamp , double newTimestamp) { return TimeSpan.FromSeconds(newTimestamp - oldTimestamp); }
		//
		// /// <summary>
		// /// 根据时间戳(单位:毫秒)获取时间跨度信息
		// /// </summary>
		// public static TimeSpan GetTimeSpanByTimestampMS(double oldTimestamp , double newTimestamp) { return TimeSpan.FromMilliseconds(newTimestamp - oldTimestamp); }
		//
		// /// <summary>
		// /// 时间戳按天数比较
		// /// </summary>
		// public static int DayOfDiff(double timestamp1 , double timestamp2) { return ConvertTimestamp(timestamp1).DayOfYear - ConvertTimestamp(timestamp2).DayOfYear; }
		//
		// /// <summary>
		// /// 时间戳相减
		// /// </summary>
		// public static TimeSpan SubtractTimestamp(double timestamp1 , double timestamp2) { return ConvertTimestamp(timestamp1) - ConvertTimestamp(timestamp2); }
		//
		// /// <summary>
		// /// 判断一个新日期是否超过旧日期几天以上
		// /// </summary>
		// public static bool MoreThanDay(DateTime oldTime , DateTime newTime , int day) { return (newTime.DayOfYear - oldTime.DayOfYear) >= day; }
		//
		// /// <summary>
		// /// 检测新时间时间戳是否超过旧时间指定天数
		// /// </summary>
		// public static bool MoreThanDay(double oldTimestamp , double newTimestamp , int day)
		// {
		// 	return MoreThanDay(ConvertTimestamp(oldTimestamp) , ConvertTimestamp(newTimestamp) , day);
		// }
		//
		// /// <summary>
		// /// 检测新时间时间戳是否超过旧时间戳一天
		// /// </summary>
		// public static bool MoreThanOneDay(double oldTimestamp , double newTimestamp) { return MoreThanOneDay(ConvertTimestamp(oldTimestamp) , ConvertTimestamp(newTimestamp)); }
		//
		// /// <summary>
		// /// 检测新时间是否超过旧时间一天
		// /// </summary>
		// public static bool MoreThanOneDay(DateTime oldTime , DateTime newTime) { return MoreThanDay(oldTime , newTime , 1); }
		//
		// /// <summary>
		// /// 检测两个时间时间戳是同一天
		// /// </summary>
		// public static bool IsSameDay(double oldTimestamp , double newTimestamp) { return IsSameDay(ConvertTimestamp(oldTimestamp) , ConvertTimestamp(newTimestamp)); }
		//
		// /// <summary>
		// /// 检测两个时间时间戳是同一天
		// /// </summary>
		// public static bool IsSameDay(DateTime oldTime , DateTime newTime)
		// {
		// 	return ((oldTime.Year == newTime.Year) && (oldTime.Month == newTime.Month) && (oldTime.Day == newTime.Day));
		// }
		//
		// /// <summary>
		// /// 检测现在的时间戳是否超过旧时间戳一周
		// /// </summary>
		// public static bool MoreThanOneWeek(double oldTimestamp , double newTimestamp) { return MoreThanOneWeek(ConvertTimestamp(oldTimestamp) , ConvertTimestamp(newTimestamp)); }
		//
		// /// <summary>
		// /// 检测现在的时间是否超过旧时间一周
		// /// </summary>
		// public static bool MoreThanOneWeek(DateTime oldTime , DateTime nowTime)
		// {
		// 	if ((oldTime.Year < nowTime.Year) || (oldTime.Year <= nowTime.Year && oldTime.Month < nowTime.Month))
		// 	{
		// 		return true;
		// 	}
		//
		// 	int oldWeek = GetWeekOfYear(oldTime);
		// 	int nowWeek = GetWeekOfYear(nowTime);
		//
		// 	return nowWeek > oldWeek;
		// }
		//
		// /// <summary>
		// /// 获取时间戳为一年中的第几周
		// /// </summary>
		// public static int GetWeekOfYear(double timestamp) { return GetWeekOfYear(ConvertTimestamp(timestamp)); }
		//
		// /// <summary>
		// /// 获取时间为一年中的第几周
		// /// </summary>
		// public static int GetWeekOfYear(DateTime curTime)
		// {
		// 	System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
		//
		// 	return gc.GetWeekOfYear(curTime , System.Globalization.CalendarWeekRule.FirstDay , DayOfWeek.Monday);
		// }
		//
		/// <summary>
		/// 格式化显示毫秒时间
		/// </summary>
		public static string MillisecondsFormatToString(long totalMilliseconds , string hourUnit = ":" , string minuteUnit = ":" , string secondUnit = null)
		{
			return SecondsFormatToString((int) totalMilliseconds / 1000 , hourUnit , minuteUnit , secondUnit);
		}

		/// <summary>
		/// 格式化显示秒时间
		/// </summary>
		public static string SecondsFormatToString(int totalSeconds , string hourUnit = ":" , string minuteUnit = ":" , string secondUnit = null)
		{
			var sb = new StringBuilder();

			if (totalSeconds < 0)
			{
				totalSeconds = System.Math.Abs(totalSeconds);
				sb.Append('-');
			}

			int seconds = totalSeconds % 60;
			int minutes = (totalSeconds / 60) % 60;
			int hours   = totalSeconds / 3600;

			if (hours > 0)
			{
				sb.Append(hours.ToString().PadLeft(2 , '0'));
				sb.Append(hourUnit);
			}

			if (hours > 0 || minutes > 0)
			{
				sb.Append(minutes.ToString().PadLeft(2 , '0'));
				sb.Append(minuteUnit);
			}

			sb.Append(seconds.ToString().PadLeft(2 , '0'));

			if (!string.IsNullOrEmpty(secondUnit))
			{
				sb.Append(secondUnit);
			}

			return sb.ToString();
		}

		/// <summary>
		/// 格式化秒时间为电子钟格式
		/// </summary>
		/// <param name="totalSeconds"></param>
		/// <returns></returns>
		public static string SecondsFormatToClock(double totalSeconds)
		{
			var total   = (int) totalSeconds;
			var minutes = total / 60;
			var seconds = total % 60;

			return $"{minutes:D2}:{seconds:D2}";
		}
	}
}
