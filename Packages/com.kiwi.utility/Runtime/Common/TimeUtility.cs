using System;
using System.Globalization;
using System.Text;

using UnityEngine;

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

		/// <summary>
		/// 获取时间为一年中的第几周
		/// </summary>
		/// <param name="dateTime">目标时间</param>
		/// <param name="firstDayOfWeek">一周的第一天为星期几</param>
		/// <returns>时间为一年中的第几周</returns>
		public static int GetWeekOfYear(DateTime dateTime , DayOfWeek firstDayOfWeek = DayOfWeek.Monday)
		{
			// 计算该年第一天是星期几
			var startOfYear         = new DateTime(dateTime.Year , 1 , 1);
			var daysFromStartOfYear = (dateTime - startOfYear).Days;

			// 计算该日期在一年中的第几周
			var weekOfYear = (daysFromStartOfYear + (int) startOfYear.DayOfWeek - (int) firstDayOfWeek) / 7 + 1;

			return weekOfYear;
		}

		/// <summary>
		/// 将日期加上指定天数
		/// </summary>
		/// <param name="dateTime">起始日期</param>
		/// <param name="days">附加天数</param>
		/// <returns>加上指定天数的日期</returns>
		public static DateTime AddDays(DateTime dateTime , int days)
		{
			return dateTime.AddDays(days);
		}

		/// <summary>
		/// 将日期加上指定小时
		/// </summary>
		/// <param name="dateTime">起始日期</param>
		/// <param name="hours">附加小时数</param>
		/// <returns>加上指定小时数的日期</returns>
		public static DateTime AddHours(DateTime dateTime , int hours)
		{
			return dateTime.AddHours(hours);
		}

		/// <summary>
		/// 将日期加上指定分钟
		/// </summary>
		/// <param name="dateTime">起始日期</param>
		/// <param name="minutes">附加分钟数</param>
		/// <returns>加上指定分钟数的日期</returns>
		public static DateTime AddMinutes(DateTime dateTime , int minutes)
		{
			return dateTime.AddMinutes(minutes);
		}

		/// <summary>
		/// 将日期加上指定秒数
		/// </summary>
		/// <param name="dateTime">起始日期</param>
		/// <param name="seconds">附加秒数</param>
		/// <returns>加上指定秒数的日期</returns>
		public static DateTime AddSeconds(DateTime dateTime , int seconds)
		{
			return dateTime.AddSeconds(seconds);
		}

		/// <summary>
		/// 将日期加上指定毫秒数
		/// </summary>
		/// <param name="dateTime">起始日期</param>
		/// <param name="milliseconds">附加毫秒数</param>
		/// <returns>加上指定毫秒数的日期</returns>
		public static DateTime AddMilliseconds(DateTime dateTime , int milliseconds)
		{
			return dateTime.AddMilliseconds(milliseconds);
		}

		#endregion

		#region 时间格式化相关函数

		///  <summary>
		///  格式化日期为字符串
		///  </summary>
		///  <param name="dateTime">时间日期</param>
		///  <param name="format">格式,参考 example</param>
		///  <returns>格式化后的时间字符串</returns>
		///	 <example>
		/// 	格式示例 :
		///  <para><b>"s" = 2008-04-18T06:30:00</b> (sortable)</para>
		///  <para><b>"d" = 04/18/2008</b> (short date)</para>
		///  <para><b>"g" = 04/18/2008 06:30</b> (general short)</para>
		///  <para><b>"f" = Friday, 18 April 2008 06:30</b> (full date short)</para>
		///  <para><b>"t" = 2008-04-18T06:30:00</b> (short time)</para>
		///  <para><b>"u" = 2008-4-18 06:30:00Z</b> (universal sortable)</para>
		///  <para><b>"D" = Friday, 18 April 2008</b> (long date)</para>
		///  <para><b>"F" = Friday, 18 April 2008 06:30:00</b> (full date long)</para>
		///  <para><b>"G" = 04/18/2008 06:30:00</b> (general long)</para>
		///  <para><b>"M" = April 18</b> (month)</para>
		///  <para><b>"O" = 2008-04-18T06:30:00.0000000</b> (ISO 8601)</para>
		///  <para><b>"R" = Fri, 18 Apr 2008 06:30:00 GMT</b> (RFC 1123)</para>
		///  <para><b>"T" = 06:30:00</b>(long time)</para>
		///  <para><b>"U" = Thursday, 17 April 2008 22:30:00</b>(universal full)</para>
		///  <para><b>"Y" = 2008 April</b> (year month)</para>
		///	 <para><b>"dddd" = Friday</b> (custom)</para>
		///  <para><b>"M/d/yy" = 4/18/08</b> (custom)</para>
		///  <para><b>"h:mm:ss tt zz" = 4:03:05 PM -07</b> (custom)</para>
		///  <para><b>"hh:mm:ss t z" = 04:03:05 P -7</b> (custom)</para>
		///  <para><b>"yyyy-M-d dddd" = 2008-4-18 Friday</b> (custom)</para>
		///  <para><b>"HH:m:s zzz" = 16:3:5 -07:00</b> (custom)</para>
		///  <para><b>"HH:mm:ss zz" = 16:03:05 -07</b> (custom)</para>
		///  <para><b>"MM/dd/yyyy" = 04/18/2008</b> (custom)</para>
		///  <para><b>"yy-MM-dd" = 08-04-18</b> (custom)</para>
		///  <para><b>"yy-MMM-dd ddd" = 08-Apr-18 Fri</b> (custom)</para>
		///  <para><b>"yyyy MMMM dd" = 2008 April 18</b> (custom)</para>
		///  </example>
		public static string FormatDateTime(DateTime dateTime , string format = "yyyy-MM-dd HH:mm:ss")
		{
			return dateTime.ToString(format);
		}

		/// <summary>
		/// 格式化秒数为字符串
		/// </summary>
		/// <param name="seconds">秒数</param>
		/// <param name="format">格式,参考<see cref="FormatDateTime(System.DateTime,string)"/></param>
		/// <returns>格式化后的时间字符串</returns>
		public static string FormatSeconds(int seconds , string format = "HH:mm:ss")
		{
			var dateTime = new DateTime().AddSeconds(seconds);

			return FormatDateTime(dateTime , format);
		}

		/// <summary>
		/// 格式化毫秒为字符串
		/// </summary>
		/// <param name="milliseconds">毫秒</param>
		/// <param name="format">格式,参考<see cref="FormatDateTime(System.DateTime,string)"/></param>
		/// <returns>格式化后的时间字符串</returns>
		public static string FormatMilliseconds(int milliseconds , string format = "HH:mm:ss")
		{
			var dateTime = new DateTime().AddMilliseconds(milliseconds);

			return FormatDateTime(dateTime , format);
		}

		/// <summary>
		/// 格式化日期为字符串
		/// </summary>
		/// <param name="dateTime">时间日期</param>
		/// <param name="cultureInfo">地区,参考<see cref="CulturePreset"/></param>
		/// <param name="format">格式,参考<see cref="FormatDateTime(System.DateTime,string)"/></param>
		/// <returns>格式化后的时间字符串</returns>
		public static string FormatDateTime(DateTime dateTime , CultureInfo cultureInfo , string format = "yyyy-MM-dd HH:mm:ss")
		{
			return dateTime.ToString(format , cultureInfo);
		}

		/// <summary>
		/// 从字符串解析日期
		/// </summary>
		/// <param name="dateTimeStr">时间日期字符串</param>
		/// <param name="format">格式,参考<see cref="FormatDateTime(System.DateTime,string)"/></param>
		/// <returns>根据字符解析到的时间日期</returns>
		public static DateTime ParseDateTime(string dateTimeStr , string format = "yyyy-MM-dd HH:mm:ss")
		{
			return DateTime.ParseExact(dateTimeStr , format , CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// 地区预设
		/// </summary>
		public struct CulturePreset
		{
			/// <summary>
			/// 美式英语
			/// </summary>
			public static CultureInfo en_US { get; } = new("en-US");

			/// <summary>
			/// 汉语（中国）
			/// </summary>
			public static CultureInfo zh_CN { get; } = new("zh-CN");

			/// <summary>
			/// 法语（法国）
			/// </summary>
			public static CultureInfo fr_FR { get; } = new("fr-FR");

			/// <summary>
			/// 德语（德国）
			/// </summary>
			public static CultureInfo de_DE { get; } = new("de-DE");

			/// <summary>
			/// 日语（日本）
			/// </summary>
			public static CultureInfo ja_JP { get; } = new("ja-JP");

			/// <summary>
			/// 韩语（韩国）
			/// </summary>
			public static CultureInfo ko_KR { get; } = new("ko-KR");
		}

		#endregion

		#region 时区相关函数

		/// <summary>
		/// 获取当前系统时区
		/// </summary>
		/// <returns>当前系统所在时区</returns>
		public static TimeZoneInfo GetLocalTimeZone()
		{
			return TimeZoneInfo.Local;
		}

		/// <summary>
		/// 将时间转换为特定时区时间
		/// </summary>
		/// <param name="dateTime">要转换的时间</param>
		/// <param name="timeZoneId">时区 ID</param>
		/// <returns>转换为特定时区后的时间</returns>
		/// <remarks>
		/// 可以使用一下代码查看 timeZoneId 的值
		///	<code>
		///	foreach (var timezone in TimeZoneInfo.GetSystemTimeZones())
		///	{
		///		Debug.Log(timezone.Id + " | " + timezone.DisplayName);
		///	}
		/// </code>
		/// </remarks>
		public static DateTime ConvertToTimeZone(DateTime dateTime , string timeZoneId)
		{
			try
			{
				var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

				return TimeZoneInfo.ConvertTime(dateTime , timeZone);
			}
			catch (TimeZoneNotFoundException)
			{
				// 处理时区未找到的情况
				Debug.LogError($"指定的时区不存在 : {timeZoneId}");

				return dateTime; // 或返回其他默认值
			}
		}

		/// <summary>
		/// 获取时区偏移量
		/// </summary>
		/// <param name="timeZone">时区</param>
		/// <param name="dateTime">目标时间</param>
		/// <returns>目标时间相对指定时区的偏移量</returns>
		public static TimeSpan GetTimeZoneOffset(TimeZoneInfo timeZone , DateTime dateTime)
		{
			return timeZone.GetUtcOffset(dateTime);
		}

		#endregion

		#region 时间判断相关函数

		/// <summary>
		/// 判断指定年份是否是闰年
		/// </summary>
		/// <param name="year">年份</param>
		/// <returns>指定年份是否是闰年</returns>
		public static bool IsLeapYear(int year)
		{
			return DateTime.IsLeapYear(year);
		}

		/// <summary>
		/// 判断指定时间是否是周末
		/// </summary>
		/// <param name="dateTime">时间</param>
		/// <returns>指定时间是否是周末</returns>
		public static bool IsWeekend(DateTime dateTime)
		{
			var dayOfWeek = dateTime.DayOfWeek;

			return dayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
		}

		/// <summary>
		/// 判断当前时间是否在两个时间范围内
		/// </summary>
		/// <param name="time">当前时间</param>
		/// <param name="startTime">开始时间</param>
		/// <param name="endTime">结束时间</param>
		/// <returns>当前时间是否在两个时间范围内</returns>
		public static bool IsTimeInRange(DateTime time , DateTime startTime , DateTime endTime)
		{
			return time >= startTime && time <= endTime;
		}

		/// <summary>
		/// 判断两个时间是否跨年
		/// </summary>
		/// <param name="dateTime1">时间 1</param>
		/// <param name="dateTime2">时间 2</param>
		/// <returns>两个时间是否跨年</returns>
		public static bool IsAcrossYear(DateTime dateTime1 , DateTime dateTime2)
		{
			return Math.Abs((dateTime2 - dateTime1).TotalDays) >= 365;
		}

		/// <summary>
		/// 判断两个时间是否跨月
		/// </summary>
		/// <param name="dateTime1">时间 1</param>
		/// <param name="dateTime2">时间 2</param>
		/// <returns>两个时间是否跨月</returns>
		public static bool IsAcrossMonth(DateTime dateTime1 , DateTime dateTime2)
		{
			return dateTime1.Year != dateTime2.Year || dateTime1.Month != dateTime2.Month;
		}

		/// <summary>
		/// 判断两个时间是否跨星期
		/// </summary>
		/// <param name="dateTime1">时间 1</param>
		/// <param name="dateTime2">时间 2</param>
		/// <returns>两个时间是否跨星期</returns>
		/// <remarks>
		///	原理就是判断两个日期在同一周内的第一天（星期一）是否是同一天
		/// </remarks>
		public static bool IsAcrossWeek(DateTime dateTime1 , DateTime dateTime2)
		{
			var startOfWeek1 = dateTime1.AddDays(-(int) dateTime1.DayOfWeek + (int) DayOfWeek.Monday);
			var startOfWeek2 = dateTime2.AddDays(-(int) dateTime2.DayOfWeek + (int) DayOfWeek.Monday);

			return startOfWeek1 != startOfWeek2;
		}

		/// <summary>
		/// 判断两个时间是否跨天
		/// </summary>
		/// <param name="dateTime1">时间 1</param>
		/// <param name="dateTime2">时间 2</param>
		/// <returns>两个时间是否跨天</returns>
		public static bool IsAcrossDay(DateTime dateTime1 , DateTime dateTime2)
		{
			return dateTime1.Date != dateTime2.Date;
		}

		/// <summary>
		/// 判断两个时间是否为同一天
		/// </summary>
		/// <param name="dateTime1">时间 1</param>
		/// <param name="dateTime2">时间 2</param>
		/// <returns>两个时间是否为同一天</returns>
		public static bool IsSameDay(DateTime dateTime1 , DateTime dateTime2)
		{
			return dateTime1.Year == dateTime2.Year
			 && dateTime1.Month == dateTime2.Month
			 && dateTime1.Day == dateTime2.Day;
		}

		#endregion

		#region 定时任务相关函数

		/// <summary>
		/// 获取指定时间 N 月之后的时间
		/// </summary>
		/// <param name="currentTime">当前时间</param>
		/// <param name="months">月</param>
		/// <returns>指定时间 N 月之后的时间</returns>
		public static DateTime GetNextMonths(DateTime currentTime , int months = 1)
		{
			return new DateTime(currentTime.Year ,
			                    currentTime.Month ,
			                    currentTime.Day ,
			                    currentTime.Hour ,
			                    currentTime.Minute ,
			                    currentTime.Second ,
			                    currentTime.Millisecond)
				.AddMonths(months);
		}

		/// <summary>
		/// 获取指定时间 N 天之后的时间
		/// </summary>
		/// <param name="currentTime">当前时间</param>
		/// <param name="days">天数</param>
		/// <returns>指定时间 N 天之后的时间</returns>
		public static DateTime GetNextDays(DateTime currentTime , int days = 1)
		{
			return new DateTime(currentTime.Year ,
			                    currentTime.Month ,
			                    currentTime.Day ,
			                    currentTime.Hour ,
			                    currentTime.Minute ,
			                    currentTime.Second ,
			                    currentTime.Millisecond)
				.AddDays(days);
		}

		/// <summary>
		/// 获取指定时间 N 小时之后的时间
		/// </summary>
		/// <param name="currentTime">当前时间</param>
		/// <param name="hours">小时</param>
		/// <returns>指定时间 N 小时之后的时间</returns>
		public static DateTime GetNextHours(DateTime currentTime , int hours)
		{
			return new DateTime(currentTime.Year ,
			                    currentTime.Month ,
			                    currentTime.Day ,
			                    currentTime.Hour ,
			                    currentTime.Minute ,
			                    currentTime.Second ,
			                    currentTime.Millisecond)
				.AddHours(hours);
		}

		/// <summary>
		/// 获取指定时间 N 分钟之后的时间
		/// </summary>
		/// <param name="currentTime">当前时间</param>
		/// <param name="minutes">分钟</param>
		/// <returns>指定时间 N 分钟之后的时间</returns>
		public static DateTime GetNextMinutes(DateTime currentTime , int minutes)
		{
			return new DateTime(currentTime.Year ,
			                    currentTime.Month ,
			                    currentTime.Day ,
			                    currentTime.Hour ,
			                    currentTime.Minute ,
			                    currentTime.Second ,
			                    currentTime.Millisecond)
				.AddMonths(minutes);
		}

		/// <summary>
		/// 获取指定时间 N 秒数之后的时间
		/// </summary>
		/// <param name="currentTime">当前时间</param>
		/// <param name="seconds">秒数</param>
		/// <returns>指定时间 N 秒数之后的时间</returns>
		public static DateTime GetNextSeconds(DateTime currentTime , int seconds)
		{
			return new DateTime(currentTime.Year ,
			                    currentTime.Month ,
			                    currentTime.Day ,
			                    currentTime.Hour ,
			                    currentTime.Minute ,
			                    currentTime.Second ,
			                    currentTime.Millisecond)
				.AddMonths(seconds);
		}

		/// <summary>
		/// 获取指定时间 N 毫秒之后的时间
		/// </summary>
		/// <param name="currentTime">当前时间</param>
		/// <param name="milliseconds">毫秒</param>
		/// <returns>指定时间 N 毫秒之后的时间</returns>
		public static DateTime GetNextMilliseconds(DateTime currentTime , int milliseconds)
		{
			return new DateTime(currentTime.Year ,
			                    currentTime.Month ,
			                    currentTime.Day ,
			                    currentTime.Hour ,
			                    currentTime.Minute ,
			                    currentTime.Second ,
			                    currentTime.Millisecond)
				.AddMonths(milliseconds);
		}

		#endregion
	}
}
