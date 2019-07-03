
namespace Netease.Live.InteropServices.Enums
{
    /// <summary>
    /// 直播推流视频源模式：即视频推流内容
    /// </summary>
    public enum VideoInType
    {
        /// <summary>
        /// 不采集视频
        /// </summary>
        None,

        /// <summary>
        /// 摄像头模式
        /// </summary>
        Camera,

        /// <summary>
        /// 全屏模式
        /// </summary>
        FullScreen,

        /// <summary>
        /// 任意区域截屏
        /// </summary>
        RectScreen,

        /// <summary>
        /// 应用程序窗口截取
        /// </summary>
        AppWindow,

        /// <summary>
        /// PNG图片源，可以作为背景
        /// </summary>
        Png,

        /// <summary>
        /// 视频裸数据模式
        /// </summary>
        RawData,

        /// <summary>
        /// DeckLink视频采集卡设备
        /// </summary>
        DeckLink
    }
}
