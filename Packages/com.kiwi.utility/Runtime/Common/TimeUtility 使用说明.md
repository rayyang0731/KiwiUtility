一个时间工具集在项目开发中非常有用，可以帮助简化各种与时间相关的操作。以下是一些常见的时间操作函数，它们通常会出现在时间工具类中：

### 1. 获取当前时间相关的函数
- **获取当前 UTC 时间**
  ```csharp
  public static DateTime GetCurrentUtcTime()
  {
      return DateTime.UtcNow;
  }
  ```

- **获取当前本地时间**
  ```csharp
  public static DateTime GetCurrentLocalTime()
  {
      return DateTime.Now;
  }
  ```

- **获取时间戳**
    - **Unix 时间戳（秒）**
      ```csharp
      public static long GetUnixTimestamp()
      {
          return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
      }
      ```

    - **Unix 时间戳（毫秒）**
      ```csharp
      public static long GetUnixTimestampMilliseconds()
      {
          return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
      }
      ```

### 2. 时间转换相关的函数
- **将 UTC 时间转换为本地时间**
  ```csharp
  public static DateTime ConvertUtcToLocal(DateTime utcTime)
  {
      return utcTime.ToLocalTime();
  }
  ```

- **将本地时间转换为 UTC 时间**
  ```csharp
  public static DateTime ConvertLocalToUtc(DateTime localTime)
  {
      return localTime.ToUniversalTime();
  }
  ```

- **将 Unix 时间戳转换为 DateTime**
    - **秒级别的时间戳**
      ```csharp
      public static DateTime ConvertUnixTimestampToDateTime(long timestamp)
      {
          return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
      }
      ```

    - **毫秒级别的时间戳**
      ```csharp
      public static DateTime ConvertUnixTimestampMillisecondsToDateTime(long timestamp)
      {
          return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
      }
      ```

- **将 DateTime 转换为 Unix 时间戳**
  ```csharp
  public static long ConvertDateTimeToUnixTimestamp(DateTime dateTime)
  {
      return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
  }
  ```

### 3. 时间计算相关的函数
- **计算两个日期之间的天数差**
  ```csharp
  public static int GetDaysDifference(DateTime startDate, DateTime endDate)
  {
      return (endDate.Date - startDate.Date).Days;
  }
  ```

- **计算两个时间之间的时间差（TimeSpan）**
  ```csharp
  public static TimeSpan GetTimeDifference(DateTime startTime, DateTime endTime)
  {
      return endTime - startTime;
  }
  ```

- **计算时间加减**
    - **加上指定天数**
      ```csharp
      public static DateTime AddDays(DateTime dateTime, int days)
      {
          return dateTime.AddDays(days);
      }
      ```

    - **加上指定小时**
      ```csharp
      public static DateTime AddHours(DateTime dateTime, int hours)
      {
          return dateTime.AddHours(hours);
      }
      ```

    - **加上指定分钟**
      ```csharp
      public static DateTime AddMinutes(DateTime dateTime, int minutes)
      {
          return dateTime.AddMinutes(minutes);
      }
      ```

### 4. 时间格式化相关的函数
- **格式化日期为字符串**
  ```csharp
  public static string FormatDateTime(DateTime dateTime, string format = "yyyy-MM-dd HH:mm:ss")
  {
      return dateTime.ToString(format);
  }
  ```

- **从字符串解析日期**
  ```csharp
  public static DateTime ParseDateTime(string dateTimeStr, string format = "yyyy-MM-dd HH:mm:ss")
  {
      return DateTime.ParseExact(dateTimeStr, format, System.Globalization.CultureInfo.InvariantCulture);
  }
  ```

### 5. 时区相关的函数
- **获取当前系统时区**
  ```csharp
  public static TimeZoneInfo GetLocalTimeZone()
  {
      return TimeZoneInfo.Local;
  }
  ```

- **将时间转换为特定时区**
  ```csharp
  public static DateTime ConvertToTimeZone(DateTime dateTime, string timeZoneId)
  {
      TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
      return TimeZoneInfo.ConvertTime(dateTime, timeZone);
  }
  ```

- **获取时区偏移量**
  ```csharp
  public static TimeSpan GetTimeZoneOffset(TimeZoneInfo timeZone, DateTime dateTime)
  {
      return timeZone.GetUtcOffset(dateTime);
  }
  ```

### 6. 时间判断相关的函数
- **判断某个时间是否是闰年**
  ```csharp
  public static bool IsLeapYear(int year)
  {
      return DateTime.IsLeapYear(year);
  }
  ```

- **判断某个时间是否是周末**
  ```c++
  public static bool IsWeekend(DateTime dateTime)
  {
      DayOfWeek dayOfWeek = dateTime.DayOfWeek;
      return dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;
  }
  ```

- **判断当前时间是否在两个时间范围内**
  ```csharp
  public static bool IsTimeInRange(DateTime time, DateTime startTime, DateTime endTime)
  {
      return time >= startTime && time <= endTime;
  }
  ```

### 7. 定时任务相关的函数
- **获取下一个指定时间点**
    - 例如获取下一个指定时间的整点
      ```csharp
      public static DateTime GetNextHour(DateTime currentTime)
      {
          return new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, 0, 0).AddHours(1);
      }
      ```
