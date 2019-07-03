
namespace Netease.Live.InteropServices.Enums
{
    /// <summary>
    /// 错误码
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// 正确
        /// </summary>
        No,

        /// <summary>
        /// 音频初始化
        /// </summary>
        AudioInit = 2001,

        /// <summary>
        /// 音频开始传输失败
        /// </summary>
        AudioStart,

        /// <summary>
        /// 视频初始化
        /// </summary>
        VideoInit = 3001,

        /// <summary>
        /// 视频开始传输失败
        /// </summary>
        VideoStart,

        /// <summary>
        /// 网络超时
        /// </summary>
        NetTimeOut = 4001,

        /// <summary>
        /// url地址无效
        /// </summary>
        UrlInvalid
    }
}
