using System;
using System.Text;

namespace Kiwi.Utility
{
	/// <summary>
	/// 时间转换工具集
	/// </summary>
	public static class TimeConvert
	{
		/// <summary>
		/// 纪元起始时
		/// </summary>
		public static DateTime UnixTime = new(1970, 1, 1, 0, 0, 0);

		/// <summary>
		/// 时区偏移(秒) 默认 +8时区(北京时区)
		/// </summary>
		public static int TimeZoneOffset { get; set; } = 8 * 3600;

		/// <summary>
		/// 获取本地时间戳 10位(秒级)
		/// </summary>
		public static long GetLocalTimestampSec()
		{
			var ts = DateTime.UtcNow - UnixTime;
			return Convert.ToInt64(ts.TotalSeconds);
		}

		/// <summary>
		/// 获取本地时间戳 13位(毫秒级)
		/// </summary>
		public static long GetLocalTimestampMS()
		{
			var ts = DateTime.UtcNow - UnixTime;
			return Convert.ToInt64(ts.TotalMilliseconds);
		}

		/// <summary>
		/// 时间戳转换
		/// </summary>
		public static DateTime ConvertTimestamp(double timestamp, int timeZoneOffset)
		{
			var dt = UnixTime;
			dt = dt.Add(new TimeSpan(0, 0, timeZoneOffset));
			return timestamp > 9999999999D
				? dt.AddMilliseconds(timestamp) //13位
				: dt.AddSeconds(timestamp);     //10位
		}

		/// <summary>
		/// 时间戳转换(按默认时区转换)
		/// </summary>
		public static DateTime ConvertTimestamp(double timestamp) { return ConvertTimestamp(timestamp, TimeZoneOffset); }

		/// <summary>
		/// 时间戳转换字符串格式
		/// </summary>
		public static string ConvertTimestampToString(double timestamp, string format = "yyyy/MM/dd HH:mm:ss") { return ConvertTimestamp(timestamp).ToString(format); }

		/// <summary>
		/// 时间戳间隔
		/// </summary>
		public static TimeSpan TimestampInterval(double oldTimestamp, double newTimestamp) { return new TimeSpan(0, 0, (int) (newTimestamp - oldTimestamp)); }

		/// <summary>
		/// 时间戳按天数比较
		/// </summary>
		public static int DayOfDiff(double timestamp1, double timestamp2) { return ConvertTimestamp(timestamp1).DayOfYear - ConvertTimestamp(timestamp2).DayOfYear; }

		/// <summary>
		/// 时间戳相减
		/// </summary>
		public static TimeSpan SubtractTimestamp(double timestamp1, double timestamp2) { return ConvertTimestamp(timestamp1) - ConvertTimestamp(timestamp2); }

		/// <summary>
		/// 判断一个新日期是否超过旧日期几天以上
		/// </summary>
		public static bool MoreThanDay(DateTime oldTime, DateTime newTime, int day) { return (newTime.DayOfYear - oldTime.DayOfYear) >= day; }

		/// <summary>
		/// 检测新时间时间戳是否超过旧时间指定天数
		/// </summary>
		public static bool MoreThanDay(double oldTimestamp, double newTimestamp, int day)
		{
			return MoreThanDay(ConvertTimestamp(oldTimestamp), ConvertTimestamp(newTimestamp), day);
		}

		/// <summary>
		/// 检测新时间时间戳是否超过旧时间戳一天
		/// </summary>
		public static bool MoreThanOneDay(double oldTimestamp, double newTimestamp) { return MoreThanOneDay(ConvertTimestamp(oldTimestamp), ConvertTimestamp(newTimestamp)); }

		/// <summary>
		/// 检测新时间是否超过旧时间一天
		/// </summary>
		public static bool MoreThanOneDay(DateTime oldTime, DateTime newTime) { return MoreThanDay(oldTime, newTime, 1); }

		/// <summary>
		/// 检测两个时间时间戳是同一天
		/// </summary>
		public static bool IsSameDay(double oldTimestamp, double newTimestamp) { return IsSameDay(ConvertTimestamp(oldTimestamp), ConvertTimestamp(newTimestamp)); }

		/// <summary>
		/// 检测两个时间时间戳是同一天
		/// </summary>
		public static bool IsSameDay(DateTime oldTime, DateTime newTime)
		{
			return ((oldTime.Year == newTime.Year) && (oldTime.Month == newTime.Month) && (oldTime.Day == newTime.Day));
		}

		/// <summary>
		/// 检测现在的时间戳是否超过旧时间戳一周
		/// </summary>
		public static bool MoreThanOneWeek(double oldTimestamp, double newTimestamp) { return MoreThanOneWeek(ConvertTimestamp(oldTimestamp), ConvertTimestamp(newTimestamp)); }

		/// <summary>
		/// 检测现在的时间是否超过旧时间一周
		/// </summary>
		public static bool MoreThanOneWeek(DateTime oldTime, DateTime nowTime)
		{
			if ((oldTime.Year < nowTime.Year) || (oldTime.Year <= nowTime.Year && oldTime.Month < nowTime.Month))
			{
				return true;
			}

			int oldWeek = GetWeekOfYear(oldTime);
			int nowWeek = GetWeekOfYear(nowTime);
			return nowWeek > oldWeek;
		}

		/// <summary>
		/// 获取时间戳为一年中的第几周
		/// </summary>
		public static int GetWeekOfYear(double timestamp) { return GetWeekOfYear(ConvertTimestamp(timestamp)); }

		/// <summary>
		/// 获取时间为一年中的第几周
		/// </summary>
		public static int GetWeekOfYear(DateTime curTime)
		{
			System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
			return gc.GetWeekOfYear(curTime, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
		}

		/// <summary>
		/// 格式化显示毫秒时间
		/// </summary>
		public static string MillisecondsFormatToString(long totalMilliseconds, string hourUnit = ":", string minuteUnit = ":", string secondUnit = null)
		{
			return SecondsFormatToString((int) totalMilliseconds / 1000, hourUnit, minuteUnit, secondUnit);
		}

		/// <summary>
		/// 格式化显示秒时间
		/// </summary>
		public static string SecondsFormatToString(int totalSeconds, string hourUnit = ":", string minuteUnit = ":", string secondUnit = null)
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
				sb.Append(hours.ToString().PadLeft(2, '0'));
				sb.Append(hourUnit);
			}

			if (hours > 0 || minutes > 0)
			{
				sb.Append(minutes.ToString().PadLeft(2, '0'));
				sb.Append(minuteUnit);
			}

			sb.Append(seconds.ToString().PadLeft(2, '0'));
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
			var sb      = new StringBuilder();
			var seconds = UnityEngine.Mathf.Max((int) (totalSeconds % 60), 0);
			var minutes = UnityEngine.Mathf.Max((int) ((totalSeconds / 60) % 60), 0);

			sb.Append(minutes.ToString().PadLeft(2, '0'));
			sb.Append(":");
			sb.Append(seconds.ToString().PadLeft(2, '0'));

			return sb.ToString();
		}
	}
}