using Netease.Live.InteropServices.Enums;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices.Models
{
    /// <summary>
    /// 直播推流音频参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct AudioParam
    {
        /// <summary>
        /// 音频输入参数
        /// </summary>
        public _AudioInParam In;

        /// <summary>
        /// 音频输出参数
        /// </summary>
        public AudioOutParam Out;
    }
}
