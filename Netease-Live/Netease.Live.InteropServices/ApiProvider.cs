using Netease.Live.InteropServices.Enums;
using Netease.Live.InteropServices.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Netease.Live.InteropServices
{
    public sealed class ApiProvider : IDisposable
    {
        private static ApiProvider _default;

        private static readonly object Token = new object();

        private bool _disposed;

        private MergedVideoSamplerCallback _mergedVideoSamplerCallback;
        private StatusNotifyCallback _statusNotifyCallback;

        public static ApiProvider Default
        {
            get
            {
                if (_default == null)
                {
                    lock (Token)
                    {
                        if (_default == null)
                        {
                            _default = new ApiProvider();
                        }
                    }
                }

                return _default;
            }
        }

        /// <summary>
        /// SDK版本号
        /// </summary>
        public string SDKVersion
        {
            get
            {
                var version = string.Empty;

                var versionPtr = Utilities.UTF8StringToIntPtr(new string('\0', 20));

                try
                {
                    Api.GetSDKVersion(out versionPtr);

                    version = Utilities.IntPtrToUTF8String(versionPtr);
                }
                finally
                {
                    //Marshal.FreeCoTaskMem(versionPtr);
                }

                return version;
            }
        }

        /// <summary>
        /// 获取直播推流实例
        /// </summary>
        internal IntPtr Service { get; private set; }

        /// <summary>
        /// 视频预览时发生
        /// </summary>
        public event EventHandler<VideoPreviewingEventArgs> VideoPreviewing;

        /// <summary>
        /// 直播推流状态发生更改时发生
        /// </summary>
        public event EventHandler<LiveStreamStatusChangedEventArgs> LiveStreamStatusChanged;

        private ApiProvider()
        {
            _mergedVideoSamplerCallback = new MergedVideoSamplerCallback(OnVideoSamplerMerged);
            _statusNotifyCallback = new StatusNotifyCallback(OnStatusChanged);
        }

        ~ApiProvider()
        {
            Dispose(false);
        }

        private void OnStatusChanged(IntPtr service, Status status, ErrorCode errorCode)
        {
            if (service != Service)
            {
                return;
            }

            LiveStreamStatusChanged?.Invoke(this, new LiveStreamStatusChangedEventArgs(status, errorCode));
        }

        private void OnVideoSamplerMerged(IntPtr service, VideoSampler sampler)
        {
            if (service != Service ||
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

        /// <summary>
        /// 获取可采集图像的App列表信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AppWindowInfo> GetAvailableAppWindows()
        {
            var results = new List<AppWindowInfo>();

            var maxNum = 0;

            if (Api.GetAvailableAppWindowNum(out maxNum) == FuncResult.Error)
            {
                return results;
            }

            if (maxNum == 0)
            {
                throw new ArgumentException();
            }

            var infos = new _AppWindowInfo[maxNum];

            try
            {
                var infosPtr = Marshal.UnsafeAddrOfPinnedArrayElement(infos, 0);

                for (var i = 0; i < infos.Length; i++)
                {
                    infos[i].Initialize();
                }

                if (Api.GetAvailableAppWindow(infosPtr, maxNum) == FuncResult.Ok)
                {
                    for (var i = 0; i < infos.Length; i++)
                    {
                        results.Add((AppWindowInfo)infos[i]);
                    }
                }
            }
            finally
            {
                for (int i = 0; i < infos.Length; i++)
                {
                    infos[i].Dispose();
                }
            }

            return results;
        }

        /// <summary>
        /// 获取可用多媒体设备列表名称，暂时只支持DShow采集音视频
        /// </summary>
        /// <param name="videoDeviceInfos">视频采集设备信息</param>
        /// <param name="audioDeviceInfos">音频采集设备信息</param>
        public void GetFreeDeviceInfos(out IEnumerable<InDeviceInfo> videoDeviceInfos, out IEnumerable<InDeviceInfo> audioDeviceInfos)
        {
            var videoDevices = new List<InDeviceInfo>();
            var audioDevices = new List<InDeviceInfo>();

            videoDeviceInfos = videoDevices;
            audioDeviceInfos = audioDevices;

            var videoDeviceNum = 0;
            var audioDeviceNum = 0;

            if (Api.GetFreeDevicesNum(out videoDeviceNum, out audioDeviceNum) == FuncResult.Error)
            {
                return;
            }

            if (videoDeviceNum == 0 &&
                audioDeviceNum == 0)
            {
                return;
            }

            var videoInfos = new _InDeviceInfo[videoDeviceNum];
            var audioInfos = new _InDeviceInfo[audioDeviceNum];

            try
            {
                var videoInfosPtr = Marshal.UnsafeAddrOfPinnedArrayElement(videoInfos, 0);
                var audioInfosPtr = Marshal.UnsafeAddrOfPinnedArrayElement(audioInfos, 0);

                for (var i = 0; i < videoInfos.Length; i++)
                {
                    videoInfos[i].Initialize();
                }
                for (var i = 0; i < audioInfos.Length; i++)
                {
                    audioInfos[i].Initialize();
                }

                if (Api.GetFreeDeviceInfos(videoInfosPtr, videoDeviceNum, audioInfosPtr, audioDeviceNum) == FuncResult.Ok)
                {
                    for (var i = 0; i < videoInfos.Length; i++)
                    {
                        videoDevices.Add((InDeviceInfo)videoInfos[i]);
                    }
                    for (var i = 0; i < audioInfos.Length; i++)
                    {
                        audioDevices.Add((InDeviceInfo)audioInfos[i]);
                    }
                }
            }
            finally
            {
                for (var i = 0; i < videoInfos.Length; i++)
                {
                    videoInfos[i].Dispose();
                }
                for (var i = 0; i < audioInfos.Length; i++)
                {
                    audioInfos[i].Dispose();
                }
            }
        }

        /// <summary>
        /// 获取摄像头设备支持的采集参数
        /// </summary>
        /// <param name="camere">摄像头设备信息</param>
        /// <returns></returns>
        public IEnumerable<CameraCaptureParam> GetCamereCaptureInfos(InDeviceInfo camere)
        {
            var results = new List<CameraCaptureParam>();

            var @params = new CameraCaptureParam[20];

            var camereInfo = (_InDeviceInfo)camere;

            var paramsPtr = Marshal.UnsafeAddrOfPinnedArrayElement(@params, 0);

            var num = @params.Length;

            if (Api.GetCamereCaptureInfo(ref camereInfo, paramsPtr, ref num) == FuncResult.Ok &&
                num > 0)
            {
                for (int i = 0; i < num; i++)
                {
                    results.Add(@params[i]);
                }
            }

            return results;
        }

        /// <summary>
        /// 获取所有DeckLink设备信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<InDeviceInfo> GetDeckLinkDevices()
        {
            var results = new List<InDeviceInfo>();

            var deviceInfos = new _InDeviceInfo[5];

            try
            {
                var deviceInfosPtr = Marshal.UnsafeAddrOfPinnedArrayElement(deviceInfos, 0);

                for (int i = 0; i < deviceInfos.Length; i++)
                {
                    deviceInfos[i].Initialize();
                }

                var num = deviceInfos.Length;

                if (Api.GetDeckLinkDeviceList(deviceInfosPtr, ref num) == FuncResult.Ok)
                {
                    for (int i = 0; i < num; i++)
                    {
                        results.Add((InDeviceInfo)deviceInfos[i]);
                    }
                }
            }
            finally
            {
                for (var i = 0; i < deviceInfos.Length; i++)
                {
                    deviceInfos[i].Dispose();
                }
            }

            return results;
        }

        /// <summary>
        /// 获取指定DeckLink设备的所有模式信息
        /// </summary>
        /// <param name="deviceId">DeckLink设备ID</param>
        /// <returns></returns>
        public IEnumerable<InDeviceModeInfo> GetDeckLinkDeviceModesById(string deviceId)
        {
            var results = new List<InDeviceModeInfo>();

            var deviceModes = new _InDeviceModeInfo[10];

            try
            {
                var deviceModesPtr = Marshal.UnsafeAddrOfPinnedArrayElement(deviceModes, 0);

                for (int i = 0; i < deviceModes.Length; i++)
                {
                    deviceModes[i].Initialize();
                }

                var num = deviceModes.Length;

                if (Api.GetDeckLinkDeviceModeListById(deviceId, deviceModesPtr, ref num) == FuncResult.Ok)
                {
                    for (int i = 0; i < num; i++)
                    {
                        results.Add((InDeviceModeInfo)deviceModes[i]);
                    }
                }
            }
            finally
            {
                for (int i = 0; i < deviceModes.Length; i++)
                {
                    deviceModes[i].Dispose();
                }
            }

            return results;
        }

        /// <summary>
        /// 创建直播推流实例，推流对象只允许存在一个，多次直播可以只调用一次
        /// </summary>
        /// <param name="cachePath">会在该目录下面生成日志,需要有文件创建和写入权限</param>
        /// <returns></returns>
        public bool Create(string cachePath)
        {
            var service = IntPtr.Zero;

            if (Api.Create(DllPathConfig.WorkPath, cachePath, out service) == FuncResult.Ok)
            {
                Service = service;

                SetStatusCallback(_statusNotifyCallback);

                return true;
            }

            return false;
        }

        internal void ThrowIfServiceException()
        {
            if (Service == IntPtr.Zero)
            {
                throw new InvalidOperationException("service");
            }
        }

        /// <summary>
        /// 销毁直播推流实例，可以程序退出再销毁
        /// </summary>
        public void Destroy()
        {
            ThrowIfServiceException();

            SetStatusCallback(null);

            Api.Destroy(Service);

            Service = IntPtr.Zero;
        }

        /// <summary>
        /// 获取直播默认参数
        /// </summary>
        /// <param name="defaultParam">直播参数</param>
        /// <returns></returns>
        public bool GetDefaultParam(out Param defaultParam)
        {
            ThrowIfServiceException();

            defaultParam = null;

            _Param param;

            if (Api.GetDefaultParam(Service, out param) == FuncResult.Ok)
            {
                defaultParam = (Param)param;

                var audioOutParam = new AudioOutParam();

                // 測試發現這兩個數據顛倒了
                audioOutParam.Codec = (AudioOutCodec)defaultParam.AudioParam.BitRate;
                audioOutParam.BitRate = (int)defaultParam.AudioParam.Codec;

                defaultParam.AudioParam = audioOutParam;

                return true;
            }

            return false;
        }

        /// <summary>
        /// 初始化直播参数
        /// </summary>
        /// <param name="param">直播参数</param>
        /// <returns></returns>
        public bool InitParam(Param param)
        {
            ThrowIfServiceException();

            if (param == null)
            {
                throw new ArgumentNullException("param");
            }

            var p = (_Param)param;

            try
            {
                if (Api.InitParam(Service, ref p) == FuncResult.Ok)
                {
                    return true;
                }
            }
            finally
            {
                p.Dispose();
            }

            return false;
        }

        /// <summary>
        /// 清除直播参数
        /// </summary>
        public void UninitParam()
        {
            ThrowIfServiceException();

            Api.UninitParam(Service);
        }

        /// <summary>
        /// 设置视频水印，默认是无水印
        /// </summary>
        /// <param name="param">水印参数</param>
        public void SetVideoWaterMark(VideoWaterParam param)
        {
            ThrowIfServiceException();

            var p = (_VideoWaterParam)param;

            try
            {
                Api.SetVideoWaterMark(Service, ref p);
            }
            finally
            {
                p.Dispose();
            }
        }

        /// <summary>
        /// 设置视频截图的的回调
        /// </summary>
        /// <param name="callback">视频截图图像回调</param>
        private void SetVideoSamplerCallback(MergedVideoSamplerCallback callback)
        {
            ThrowIfServiceException();

            Api.SetVideoSamplerCallback(Service, callback);
        }

        /// <summary>
        /// 设置直播过程中状态回调
        /// </summary>
        /// <param name="callback">直播状态通知回调</param>
        private void SetStatusCallback(StatusNotifyCallback callback)
        {
            ThrowIfServiceException();

            Api.SetStatusCallback(Service, callback);
        }

        /// <summary>
        /// 修改视频输出参数，必须在未推流状态下执行
        /// </summary>
        /// <param name="param">新的视频输出参数</param>
        /// <returns>修改成功返回true，失败返回false</returns>
        public bool UpdateVideoOutParam(VideoOutParam param)
        {
            ThrowIfServiceException();

            return Api.UpdateVideoOutParam(Service, ref param);
        }

        /// <summary>
        /// 修改音频输出参数，必须在未推流状态下执行
        /// </summary>
        /// <param name="param">新的音频输出参数</param>
        /// <returns>修改成功返回true，失败返回false</returns>
        public bool UpdateAudioOutParam(AudioOutParam param)
        {
            ThrowIfServiceException();

            return Api.UpdateAudioOutParam(Service, ref param);
        }

        /// <summary>
        /// 修改推流地址，必须在未推流状态下执行
        /// </summary>
        /// <param name="pushUrl">新的推流地址</param>
        /// <returns>修改成功返回true，失败返回false</returns>
        public bool UpdatePushUrl(string url)
        {
            ThrowIfServiceException();

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException("url");
            }

            return Api.UpdatePushUrl(Service, url);
        }

        /// <summary>
        /// 修改透传同步时间戳类型
        /// </summary>
        /// <param name="type">新的同步时间戳类型</param>
        /// <returns>修改成功返回true，失败返回false</returns>
        public bool UpdateSyncTimestampType(SyncTimestampType type)
        {
            ThrowIfServiceException();

            return Api.UpdateSyncTimestampType(Service, type);
        }

        /// <summary>
        /// 启动处理，在各类初始化之后，在预览和直播启动之前
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            ThrowIfServiceException();

            return Api.Start(Service) == FuncResult.Ok;
        }

        /// <summary>
        /// 停止处理，在预览和直播停止之后，在UninitParam之前
        /// </summary>
        public void Stop()
        {
            ThrowIfServiceException();

            Api.Stop(Service);
        }

        /// <summary>
        /// 打开视频预览
        /// </summary>
        /// <returns></returns>
        public bool StartVideoPreview()
        {
            ThrowIfServiceException();

            SetVideoSamplerCallback(_mergedVideoSamplerCallback);

            return Api.StartVideoPreview(Service) == FuncResult.Ok;
        }

        /// <summary>
        /// 暂停视频预览
        /// </summary>
        public void PauseVideoPreview()
        {
            ThrowIfServiceException();

            Api.StopVideoPreview(Service);
        }

        /// <summary>
        /// 恢复视频预览
        /// </summary>
        public void ResumeVideoPreview()
        {
            ThrowIfServiceException();

            Api.ResumeVideoPreview(Service);
        }

        /// <summary>
        /// 停止视频预览
        /// </summary>
        public void StopVideoPreview()
        {
            ThrowIfServiceException();

            Api.StopVideoPreview(Service);

            SetVideoSamplerCallback(null);
        }

        /// <summary>
        /// 启动直播推流
        /// </summary>
        /// <returns></returns>
        public bool StartLiveStream()
        {
            ThrowIfServiceException();

            return Api.StartLiveStream(Service) == FuncResult.Ok;
        }

        /// <summary>
        /// 停止直播推流
        /// </summary>
        public void StopLiveStream()
        {
            ThrowIfServiceException();

            Api.StopLiveStream(Service);
        }

        /// <summary>
        /// 暂停视频直播推流
        /// </summary>
        public void PauseVideoLiveStream()
        {
            ThrowIfServiceException();

            Api.PauseVideoLiveStream(Service);
        }

        /// <summary>
        /// 恢复视频直播推流
        /// </summary>
        public void ResumeVideoLiveStream()
        {
            ThrowIfServiceException();

            Api.ResumeVideoLiveStream(Service);
        }

        /// <summary>
        /// 暂停音频直播推流
        /// </summary>
        public void PauseAudioLiveStream()
        {
            ThrowIfServiceException();

            Api.PauseAudioLiveStream(Service);
        }

        /// <summary>
        /// 恢复音频直播推流
        /// </summary>
        public void ResumeAudioLiveStream()
        {
            ThrowIfServiceException();

            Api.ResumeAudioLiveStream(Service);
        }

        /// <summary>
        /// 开始直播录制
        /// </summary>
        /// <param name="param">录制参数（录制文件存放路径、录制格式等）</param>
        /// <returns></returns>
        public bool StartRecord(RecordParam param)
        {
            ThrowIfServiceException();

            if (param == null)
            {
                throw new ArgumentNullException("param");
            }

            var directory = Path.GetDirectoryName(param.Path);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var p = (_RecordParam)param;

            try
            {
                return Api.StartRecord(Service, ref p) == FuncResult.Ok;
            }
            finally
            {
                p.Dispose();
            }
        }

        /// <summary>
        /// 停止直播录制
        /// </summary>
        public void StopRecord()
        {
            ThrowIfServiceException();

            Api.StopRecord(Service);
        }

        /// <summary>
        /// 获取当前直播流的pts（实际上是音频流的dts），单位毫秒
        /// </summary>
        /// <returns>前直播流的pts，单位毫秒</returns>
        public long GetStreamPts()
        {
            ThrowIfServiceException();

            return Api.GetStreamPts(Service);
        }

        /// <summary>
        /// 获取当前透传的同步时间戳（通过视频流透传，Param.SyncTimestampType不为None时，此接口有效），单位毫秒
        /// </summary>
        /// <returns>当前透传的同步时间戳，单位毫秒</returns>
        public long GetSyncTimestamp()
        {
            ThrowIfServiceException();

            return Api.GetSyncTimestamp(Service);
        }

        /// <summary>
        /// 发送透传的自定义数据
        /// </summary>
        /// <param name="param">要发送的自定义数据参数</param>
        /// <returns></returns>
        public bool SendCustomData(CustomDataParam param)
        {
            ThrowIfServiceException();

            if (param == null)
            {
                throw new ArgumentNullException("param");
            }

            var p = (_CustomDataParam)param;

            try
            {
                return Api.SendCustomData(Service, ref p) == FuncResult.Ok;
            }
            finally
            {
                p.Dispose();
            }
        }

        /// <summary>
        /// 获取直播推流状态信息
        /// </summary>
        /// <param name="status">直播推流统计信息</param>
        /// <returns></returns>
        public bool GetStaticInfo(out StatusInfo status)
        {
            ThrowIfServiceException();

            return Api.GetStaticInfo(Service, out status) == FuncResult.Ok;
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

                LiveStreamStatusChanged = null;

                _default = null;
            }

            if (Service != IntPtr.Zero)
            {
                Destroy();
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
