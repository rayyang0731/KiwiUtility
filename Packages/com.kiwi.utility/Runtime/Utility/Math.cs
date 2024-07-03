namespace Kiwi.Utility
{
	/// <summary>
	/// 数学公式类
	/// </summary>
	public static class Math
	{
		/// <summary>
		/// 是否为近似值(主要比较两个 float 值是否一样)
		/// <para>
		/// 主要是因为 Unity 自己的 Mathf.Approximation 无法自己定义容差,
		/// 而且最后的判断没明白为啥需要乘以 8.0f,所以创建了这个函数.
		/// </para>
		/// </summary>
		/// <param name="a">要比较的第一个数值</param>
		/// <param name="b">要比较的第二个数值</param>
		/// <param name="tolerance">容差,默认为 1e-6 (0.000001)</param>
		/// <returns>返回 true 可以理解为两个float值相等,否则返回 false</returns>
		public static bool Approximately(float a, float b, double tolerance = 1e-6) { return UnityEngine.Mathf.Abs(a - b) < tolerance; }

		/// <summary>
		/// 是否为奇数
		/// </summary>
		/// <param name="num">要检查的数字</param>
		/// <returns></returns>
		public static bool IsOdd(int num) => (num & 1) == 1;
	}
}