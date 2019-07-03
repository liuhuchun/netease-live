using Netease.Live.InteropServices.Enums;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices.Models
{
    /// <summary>
    /// 直播推流视频源输入参数
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct _VideoInParam
    {
        /// <summary>
        /// 视频源类型
        /// </summary>
        [FieldOffset(0)]
        public VideoInType Type;

        /// <summary>
        /// 采集帧率
        /// </summary>
        [FieldOffset(4)]
        public int CaptureFps;

        /// <summary>
        /// 视频源类型为摄像头模式时，摄像头参数
        /// </summary>
        [FieldOffset(8)]
        public _CameraParam Camera;

        /// <summary>
        /// 视频源类型为截屏区域模式时，截屏区域参数
        /// </summary>
        [FieldOffset(8)]
        public RectScreenParam RectScreen;

        /// <summary>
        /// 视频源类型为应用程序模式时，应用程序参数
        /// </summary>
        [FieldOffset(8)]
        public AppVideoParam App;

        /// <summary>
        /// 视频源类型为自定义视频源模式时，自定义视频源参数
        /// </summary>
        [FieldOffset(8)]
        public CustomVideoParam CustomVideo;

        /// <summary>
        /// 视频源类型为图片模式时，图片参数
        /// </summary>
        [FieldOffset(8)]
        public _PngParam Png;

        /// <summary>
        /// 视频源类型为DeckLink设备模式时，DeckLink设备参数
        /// </summary>
        [FieldOffset(8)]
        public _DeckLinkParam DeckLink;

        public void Dispose()
        {
            Camera.Dispose();

            Png.Dispose();

            DeckLink.Dispose();
        }

        public static explicit operator _VideoInParam(VideoInParam param)
        {
            var result = new _VideoInParam
            {
                Type = param.Type,
                CaptureFps = param.CaptureFps
            };

            switch (param.Type)
            {
                case VideoInType.Camera:
                    result.Camera = (_CameraParam)param.Camera;
                    break;
                case VideoInType.RectScreen:
                    result.RectScreen = param.RectScreen;
                    break;
                case VideoInType.AppWindow:
                    result.App = param.App;
                    break;
                case VideoInType.RawData:
                    result.CustomVideo = param.CustomVideo;
                    break;
                case VideoInType.Png:
                    result.Png = (_PngParam)param.Png;
                    break;
                case VideoInType.DeckLink:
                    result.DeckLink = (_DeckLinkParam)param.DeckLink;
                    break;
            }

            return result;
        }
    }

    /// <summary>
    /// 直播推流视频源输入参数
    /// </summary>
    public class VideoInParam
    {
        /// <summary>
        /// 视频源类型
        /// </summary>
        public VideoInType Type { get; set; }

        /// <summary>
        /// 采集帧率
        /// </summary>
        public int CaptureFps { get; set; }

        /// <summary>
        /// 视频源类型为摄像头模式时，摄像头参数
        /// </summary>
        public CameraParam Camera { get; set; }

        /// <summary>
        /// 视频源类型为截屏区域模式时，截屏区域参数
        /// </summary>
        public RectScreenParam RectScreen { get; set; }

        /// <summary>
        /// 视频源类型为应用程序模式时，应用程序参数
        /// </summary>
        public AppVideoParam App { get; set; }

        /// <summary>
        /// 视频源类型为自定义视频源模式时，自定义视频源参数
        /// </summary>
        public CustomVideoParam CustomVideo { get; set; }

        /// <summary>
        /// 视频源类型为图片模式时，图片参数
        /// </summary>
        public PngParam Png { get; set; }

        /// <summary>
        /// 视频源类型为DeckLink设备模式时，DeckLink设备参数
        /// </summary>
        public DeckLinkParam DeckLink { get; set; }
    }
}
