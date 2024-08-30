using System;

using NUnit.Framework;

using UnityEngine;

namespace Kiwi.Utility.Test.Editor
{
	public class TimeConvertTest
	{
		// [ Test ]
		// public void PrintUnixTime()
		// {
		// 	Debug.Log($"Unix Time : {TimeUtility.UnixTime}");
		// }
		//
		// [ Test ]
		// public void PrintLocalTimestampSec()
		// {
		// 	Debug.Log($"本地时间戳 : {TimeUtility.GetLocalTimestampSec()} 秒");
		// }
		//
		// [ Test ]
		// public void PrintLocalTimestampMS()
		// {
		// 	Debug.Log($"本地时间戳 : {TimeUtility.GetLocalTimestampMS()} 毫秒");
		// }
		//
		// [ Test ]
		// public void PrintConvertTimestampSec()
		// {
		// 	Debug.Log($"时间戳转换 单位秒   UTC : {TimeUtility.ConvertTimestamp(TimeUtility.GetLocalTimestampSec())}");
		// }
		//
		// [ Test ]
		// public void PrintConvertTimestampMS()
		// {
		// 	Debug.Log($"时间戳转换 单位毫秒 UTC : {TimeUtility.ConvertTimestamp(TimeUtility.GetLocalTimestampMS())}");
		// }
		//
		// [ Test ]
		// public void PrintConvertTimestampToString()
		// {
		// 	Debug.Log($"时间戳转换字符串 单位秒 UTC : {TimeUtility.ConvertTimestampToString(TimeUtility.GetLocalTimestampMS())}");
		// }
		//
		// [ Test ]
		// public void PrintTimeSpanByTimestampSec()
		// {
		// 	Debug.Log($"根据时间戳(单位:秒)获取时间跨度信息 : {TimeUtility.GetTimeSpanByTimestampSec(TimeUtility.GetLocalTimestampSec() , 1724143398)}");
		// }
		//
		// [ Test ]
		// public void PrintTimeSpanByTimestampMS()
		// {
		// 	Debug.Log($"根据时间戳(单位:毫秒)获取时间跨度信息 : {TimeUtility.GetTimeSpanByTimestampMS(TimeUtility.GetLocalTimestampMS() , 1724143398348)}");
		// }
		//
		[ Test ]
		public void Test()
		{
			Debug.Log($"Unix 纪元时间 : {TimeUtility.UnixTime}");
			
			var utcTime = TimeUtility.GetCurrentUtcTime();
			var localTime = TimeUtility.GetCurrentLocalTime();
			var unixTimestampSecond = TimeUtility.GetUnixTimestamp();
			var unixTimestampMillisecond = TimeUtility.GetUnixTimestampMilliseconds();
			
			Debug.Log($"UTC 时间 : {utcTime}");
			Debug.Log($"本地时间 : {localTime}");
			
			Debug.Log($"Unix 时间戳 : {unixTimestampSecond} 秒");
			Debug.Log($"Unix 时间戳 : {unixTimestampMillisecond} 毫秒");
			
			Debug.Log($"UTC 时间转换为本地时间 : {TimeUtility.ConvertUtcToLocal(utcTime)}");
			Debug.Log($"本地时间转换为 UTC 时间 : {TimeUtility.ConvertLocalToUtc(localTime)}");
			
			Debug.Log($"将秒级 Unix 时间戳转换为 DateTime : {TimeUtility.ConvertUnixTimestampToDateTime(unixTimestampSecond)}");
			Debug.Log($"将毫秒级 Unix 时间戳转换为 DateTime : {TimeUtility.ConvertUnixTimestampMillisecondsToDateTime(unixTimestampMillisecond)}");
			
			Debug.Log($"将 DateTime 转换为秒级 Unix 时间戳 : {TimeUtility.ConvertDateTimeToUnixTimestamp(utcTime)}");
			Debug.Log($"将 DateTime 转换为毫秒级 Unix 时间戳 : {TimeUtility.ConvertDateTimeToUnixTimestampMilliseconds(utcTime)}");
			
			
		}
	}
}
