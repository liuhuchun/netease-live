using Netease.Live.InteropServices.Enums;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices.Models
{
    /// <summary>
    /// 直播视频源为视频裸数据模式时，输入参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CustomVideoParam
    {
        /// <summary>
        /// 输入源的宽
        /// </summary>
        public int Width;

        /// <summary>
        /// 输入源的高
        /// </summary>
        public int Height;

        /// <summary>
        /// 当输入源为yuv数据时，视频格式
        /// </summary>
        public VideoInFormat Format;
    }
}
