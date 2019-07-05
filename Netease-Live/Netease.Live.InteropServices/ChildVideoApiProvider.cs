using Netease.Live.InteropServices.Enums;
using Netease.Live.InteropServices.Models;
using System;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices
{
    public sealed class ChildVideoApiProvider : IDisposable
    {
        private bool _disposed;

        private ChildVideoSamplerCallback _videoSamplerCallback;

        /// <summary>
        /// 音频实例
        /// </summary>
        internal IntPtr ChildService { get; private set; }

        /// <summary>
        /// 是否为其他采集设备（如视频采集卡）
        /// </summary>
        public bool IsOtherDevice
        {
            get
            {
                ThrowIfChildServiceException();

                return ChildVideoApi.IsOtherDevice(ChildService);
            }
        }

        /// <summary>
        /// 视频预览时发生
        /// </summary>
        public event EventHandler<VideoPreviewingEventArgs> VideoPreviewing;

        public ChildVideoApiProvider()
        {
            _videoSamplerCallback = new ChildVideoSamplerCallback(OnVideoSamplerChanged);
        }

        ~ChildVideoApiProvider()
        {
            Dispose(false);
        }

        private void OnVideoSamplerChanged(IntPtr service, IntPtr childService, VideoSampler sampler)
        {
            if (service != ApiProvider.Default.Service ||
                childService != ChildService ||
                sampler == null ||
                sampler.DataSize == 0 ||
                sampler.Data == IntPtr.Zero)
            {
                return;
            }

            var data = new byte[sampler.DataSize];

            Marshal.Copy(sampler.Data, data, 0, data.Length);

            // TODO : 图像格式与像素大小值对应
            var image = Utilities.CreateBitmap(
                data,
                sampler.Width,
                sampler.Height,
                PixelFormat.Format32bppArgb,
                sampler.Width * sampler.Height * 4);

            VideoPreviewing?.Invoke(this, new VideoPreviewingEventArgs(image));
        }

        internal void ThrowIfChildServiceException()
        {
            if (ChildService == IntPtr.Zero)
            {
                throw new InvalidOperationException("ChildService");
            }
        }

        /// <summary>
        /// 打开采集设备的高级设置
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 该接口可能会弹出采集系统配置窗口，所以建议在IsOtherDevice检测到之后，让用户手动选择是否开启
        /// </remarks>
        public bool OpenOtherDeviceConfig()
        {
            ThrowIfChildServiceException();

            return ChildVideoApi.OpenOtherDeviceConfig(ChildService) == FuncResult.Ok;
        }

        /// <summary>
        /// 打开子视频实例，用于推流直播
        /// </summary>
        /// <param name="param">子视频采集参数</param>
        /// <returns></returns>
        public bool Open(VideoInParam param)
        {
            ApiProvider.Default.ThrowIfServiceException();

            if (param == null)
            {
                throw new ArgumentNullException("param");
            }

            var p = (_VideoInParam)param;

            try
            {
                ChildService = ChildVideoApi.Open(ApiProvider.Default.Service, ref p);

                return ChildService != IntPtr.Zero;
            }
            finally
            {
                p.Dispose();
            }
        }

        /// <summary>
        /// 关闭子视频实例
        /// </summary>
        public void Close()
        {
            ThrowIfChildServiceException();

            ChildVideoApi.Close(ChildService);

            ChildService = IntPtr.Zero;
        }

        /// <summary>
        /// 将该子视频设为背景层，同时将子视频显示全部铺开
        /// </summary>
        public void SetBackLayer()
        {
            ThrowIfChildServiceException();

            ChildVideoApi.SetBackLayer(ChildService);
        }

        /// <summary>
        /// 将该子视频层级调降或者调升一层来显示
        /// </summary>
        /// <param name="adjustUp">True上升一层，False降一层</param>
        public void AdjustLayer(bool adjustUp)
        {
            ThrowIfChildServiceException();

            ChildVideoApi.AdjustLayer(ChildService, adjustUp);
        }

        /// <summary>
        /// 设置该子视频窗口在主窗口中的显示位置和大小
        /// </summary>
        /// <param name="param">显示位置参数</param>
        public void SetDisplayRect(RectScreenParam param)
        {
            ThrowIfChildServiceException();

            ChildVideoApi.SetDisplayRect(ChildService, ref param);
        }

        /// <summary>
        /// 临时开|关该子视频窗口是否显示
        /// </summary>
        /// <param name="hide">子视频窗口单独是否隐藏</param>
        public void SwitchDisplay(bool hide)
        {
            ThrowIfChildServiceException();

            ChildVideoApi.SwitchDisplay(ChildService, hide);
        }

        /// <summary>
        /// 打开子视频采集，需要在视频预览前调用，当需要改变采集设备的时候需要先停止再重新打开
        /// </summary>
        /// <returns></returns>
        public bool StartCapture()
        {
            ThrowIfChildServiceException();

            return ChildVideoApi.StartCapture(ChildService) == FuncResult.Ok;
        }

        /// <summary>
        /// 关闭视频采集
        /// </summary>
        public void StopCapture()
        {
            ThrowIfChildServiceException();

            ChildVideoApi.StopCapture(ChildService);
        }

        /// <summary>
        /// 设置单独预览的视频流buffer回调函数，可用于预览进行显示
        /// </summary>
        /// <param name="callback">单独推流的buffer回调</param>
        private void SetSoloPreviewCallback(ChildVideoSamplerCallback callback)
        {
            ThrowIfChildServiceException();

            ChildVideoApi.SetSoloPreviewCallback(ChildService, callback);
        }

        /// <summary>
        /// 开|关子视频单独预览
        /// </summary>
        /// <param name="on">子视频窗口单独预览开关</param>
        public void SwitchSoloPreview(bool on)
        {
            ThrowIfChildServiceException();

            ChildVideoApi.SwitchSoloPreview(ChildService, on);

            if (on)
            {
                SetSoloPreviewCallback(_videoSamplerCallback);
            }
            else
            {
                SetSoloPreviewCallback(null);
            }
        }

        /// <summary>
        /// 暂停视频直播推流
        /// </summary>
        public void PauseLiveStream()
        {
            ThrowIfChildServiceException();

            ChildVideoApi.PauseLiveStream(ChildService);
        }

        /// <summary>
        /// 恢复视频直播推流
        /// </summary>
        public void ResumeLiveStream()
        {
            ThrowIfChildServiceException();

            ChildVideoApi.ResumeLiveStream(ChildService);
        }

        /// <summary>
        /// 用户指定视频流推流发送接口
        /// </summary>
        /// <param name="videoData">用户指定视频流数据</param>
        /// <returns></returns>
        public bool SendCustomData(byte[] videoData)
        {
            ThrowIfChildServiceException();

            if (videoData == null)
            {
                throw new ArgumentNullException("videoData");
            }

            return ChildVideoApi.SendCustomData(ChildService, videoData, videoData.Length) == FuncResult.Ok;
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                VideoPreviewing = null;
            }

            if (ChildService != IntPtr.Zero)
            {
                Close();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
}
