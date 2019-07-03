
namespace Netease.Live.InteropServices.Enums
{
    /// <summary>
    /// 当视频流为用户采集时（即VideoInType.RawData时），输入的视频流格式
    /// </summary>
    public enum VideoInFormat
    {
        NV12 = 0,
        NV21,
        I420,
        YUY2,
        UYVY,
        BGRA32,
        ARGB32,
        BGR24,
        BGR24Flip,
        MJPG,
        Invalid
    }
}
