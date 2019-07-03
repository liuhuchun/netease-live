using Netease.Live.InteropServices.Enums;
using Netease.Live.InteropServices.Models;
using System;

namespace Netease.Live.InteropServices
{
    public sealed class ChildAudioApiProvider : IDisposable
    {
        private bool _disposed;

        /// <summary>
        /// 音频实例
        /// </summary>
        internal IntPtr ChildService { get; private set; }

        ~ChildAudioApiProvider()
        {
            Dispose(false);
        }

        /// <summary>
        /// 以默认值初始化子音频输入参数
        /// </summary>
        /// <param name="service">直播推流实例</param>
        /// <param name="param">子音频参数</param>
        public AudioInParam GetDefaultParam()
        {
            ApiProvider.Default.ThrowIfServiceException();

            _AudioInParam param;

            ChildAudioApi.GetDefaultParam(ApiProvider.Default.Service, out param);

            return (AudioInParam)param;
        }

        /// <summary>
        /// 打开子音频实例
        /// </summary>
        /// <param name="param">子音频采集参数</param>
        /// <returns></returns>
        public bool Open(AudioInParam param)
        {
            ApiProvider.Default.ThrowIfServiceException();

            if (param == null)
            {
                throw new ArgumentNullException("param");
            }

            if (ChildService != IntPtr.Zero)
            {
                Close();
            }

            var p = (_AudioInParam)param;

            try
            {
                ChildService = ChildAudioApi.Open(ApiProvider.Default.Service, ref p);

                return ChildService != IntPtr.Zero;
            }
            finally
            {
                p.Dispose();
            }
        }

        internal void ThrowIfChildServiceException()
        {
            if (ChildService == IntPtr.Zero)
            {
                throw new InvalidOperationException("ChildService");
            }
        }

        /// <summary>
        /// 关闭子音频实例
        /// </summary>
        public void Close()
        {
            ThrowIfChildServiceException();

            ChildAudioApi.Close(ChildService);
        }

        /// <summary>
        /// 打开子音频采集，当需要改变采集设备的时候需要先停止再重新打开
        /// </summary>
        /// <returns></returns>
        public bool StartCapture()
        {
            ThrowIfChildServiceException();

            return ChildAudioApi.StartCapture(ChildService) == FuncResult.Ok;
        }

        /// <summary>
        /// 关闭子音频采集
        /// </summary>
        public void StopCapture()
        {
            ThrowIfChildServiceException();

            ChildAudioApi.StopCapture(ChildService);
        }

        /// <summary>
        /// 暂停/继续子音频采集
        /// </summary>
        /// <param name="pause">是否暂停子音频采集</param>
        /// <remarks>
        /// 此方法必须在StartCapture和StopCapture之间执行
        /// </remarks>
        public void SwitchCapture(bool pause)
        {
            ThrowIfChildServiceException();

            ChildAudioApi.SwitchCapture(ChildService, pause);
        }

        /// <summary>
        /// 设置子音频音量
        /// </summary>
        /// <param name="ratio">暂定0-100；0表示静音，100表示原始音量</param>
        public void SetVolume(int ratio)
        {
            ThrowIfChildServiceException();

            ChildAudioApi.SetVolume(ChildService, ratio);
        }

        /// <summary>
        /// 暂停子音频推流（不会暂停采集）
        /// </summary>
        public void PauseLiveStream()
        {
            ThrowIfChildServiceException();

            ChildAudioApi.PauseLiveStream(ChildService);
        }

        /// <summary>
        /// 恢复子音频推流
        /// </summary>
        public void ResumeLiveStream()
        {
            ThrowIfChildServiceException();

            ChildAudioApi.ResumeLiveStream(ChildService);
        }

        /// <summary>
        /// 用户指定音频流推流发送接口
        /// </summary>
        /// <param name="audioData">用户指定音频流数据</param>
        /// <returns></returns>
        public bool SendCustomData(byte[] audioData)
        {
            ThrowIfChildServiceException();

            if (audioData == null)
            {
                throw new ArgumentNullException("audioData");
            }

            return ChildAudioApi.SendCustomData(ChildService, audioData, audioData.Length) == FuncResult.Ok;
        }

        /// <summary>
        /// 用户指定音频流推流发送接口，支持直播过程中音频数据重采样
        /// </summary>
        /// <param name="audioData">用户指定音频流数据</param>
        /// <param name="sampleRate">音频流采样率，支持中间变化入参</param>
        /// <returns></returns>
        public bool SendCustomData(byte[] audioData, int sampleRate)
        {
            ThrowIfChildServiceException();

            if (audioData == null)
            {
                throw new ArgumentNullException("audioData");
            }

            if (sampleRate <= 0)
            {
                throw new ArgumentException("sampleRate");
            }

            return ChildAudioApi.SendCustomData(ChildService, audioData, audioData.Length, sampleRate) == FuncResult.Ok;
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                //
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
