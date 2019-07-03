using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices.Models
{
    /// <summary>
    /// 直播视频源为区域截屏时，输入参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RectScreenParam
    {
        /// <summary>
        /// 截屏区域左边与屏幕左边距离
        /// </summary>
        public int Left;

        /// <summary>
        /// 截屏区域上边与屏幕上边距离
        /// </summary>
        public int Top;

        /// <summary>
        /// 截屏区域右边与屏幕左边距离
        /// </summary>
        public int Right;

        /// <summary>
        /// 截屏区域下边与屏幕上边距离
        /// </summary>
        public int Bottom;
    }
}
