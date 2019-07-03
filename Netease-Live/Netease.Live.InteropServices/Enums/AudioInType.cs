
namespace Netease.Live.InteropServices.Enums
{
    /// <summary>
    /// 直播推流音频源模式：即音频推流采集源
    /// </summary>
    public enum AudioInType
    {
        /// <summary>
        /// 不采集声音
        /// </summary>
        None,

        /// <summary>
        /// 麦克风模式
        /// </summary>
        Micphone,

        /// <summary>
        /// Windows系统声音
        /// </summary>
        System,

        /// <summary>
        /// 音频流裸数据模式
        /// </summary>
        RawData,

        /// <summary>
        /// DeckLink设备的音频
        /// </summary>
        DeckLink
    }
}
