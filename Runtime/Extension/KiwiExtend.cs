namespace Kiwi.Utility
{
	public static partial class KiwiExtend
	{
		/// <summary>
		/// 判断是否为 null
		/// </summary>
		/// <param name="obj">被扩展的对象</param>
		/// <returns></returns>
		public static bool IsNull(this UnityEngine.Object obj) => obj == null || obj.Equals(null);
	}
}
