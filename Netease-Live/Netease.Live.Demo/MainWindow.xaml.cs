using Netease.Live.InteropServices;
using Netease.Live.InteropServices.Enums;
using Netease.Live.InteropServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Drawing = System.Drawing;

namespace Netease.Live.Demo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _serviceCreated;
        private bool _paramInited;
        private bool _serviceStarted;
        private bool _videoPreviewStarted;
        private bool _liveStreamStarted;
        private bool _recordStarted;

        private ChildVideoApiProvider _desktop;
        private bool _desktopOpened;
        private bool _desktopStarted;

        private ChildVideoApiProvider _camera;
        private bool _cameraOpened;
        private bool _cameraStarted;

        private ChildAudioApiProvider _system;
        private bool _systemOpened;
        private bool _systemStarted;

        public MainWindow()
        {
            InitializeComponent();

            ApiProvider.Default.LiveStreamStatusChanged -= Default_LiveStreamStatusChanged;
            ApiProvider.Default.LiveStreamStatusChanged += Default_LiveStreamStatusChanged;
            ApiProvider.Default.VideoPreviewing -= Default_VideoPreviewing;
            ApiProvider.Default.VideoPreviewing += Default_VideoPreviewing;

            _desktop = new ChildVideoApiProvider();
            _desktop.VideoPreviewing += _desktop_VideoPreviewing;

            _camera = new ChildVideoApiProvider();
            _camera.VideoPreviewing += _camera_VideoPreviewing;

            _system = new ChildAudioApiProvider();
        }

        private void DoAction(Action action)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    action?.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }));
        }

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        private BitmapSource ToBitmapSource(Drawing.Image image)
        {
            BitmapSource toReturn = null;

            using (var bmp = new Drawing.Bitmap(image))
            {
                var hBitmap = bmp.GetHbitmap();

                try
                {
                    toReturn = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                finally
                {
                    DeleteObject(hBitmap);
                }
            }

            return toReturn;
        }

        private void _camera_VideoPreviewing(object sender, VideoPreviewingEventArgs e)
        {
            DoAction(() =>
            {
                using (var img = e.Image)
                {
                    imgPreviewCamera.Source = ToBitmapSource(img);
                }
            });
        }

        private void _desktop_VideoPreviewing(object sender, VideoPreviewingEventArgs e)
        {

            DoAction(() =>
            {
                using (var img = e.Image)
                {
                    imgPreviewDesktop.Source = ToBitmapSource(img);
                }
            });
        }

        private void Default_VideoPreviewing(object sender, VideoPreviewingEventArgs e)
        {
            DoAction(() =>
            {
                using (var img = e.Image)
                {
                    imgPreviewMerged.Source = ToBitmapSource(img);
                }
            });
        }

        private void Default_LiveStreamStatusChanged(object sender, LiveStreamStatusChangedEventArgs e)
        {
            DoAction(() =>
            {
                tbxStatus.AppendText($"{DateTime.Now.ToLongTimeString()}-status:{e.Status} errorCode:{e.Code}{Environment.NewLine}");
            });
        }

        private void btnVersion_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                MessageBox.Show($"{ApiProvider.Default.SDKVersion}", "SDKVersion");
            });
        }

        private void btnApps_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                var text = string.Empty;

                var apps = ApiProvider.Default.GetAvailableAppWindows();

                if (apps.Count() > 0)
                {
                    foreach (var info in apps)
                    {
                        text += $"{info.Title}{Environment.NewLine}";
                    }
                }

                MessageBox.Show(text, "AvailableApps");
            });
        }

        private void btnDevice_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                var text = string.Empty;

                IEnumerable<InDeviceInfo> videoDeviceInfos;
                IEnumerable<InDeviceInfo> audioDeviceInfos;

                ApiProvider.Default.GetFreeDeviceInfos(out videoDeviceInfos, out audioDeviceInfos);

                if (videoDeviceInfos.Count() > 0)
                {
                    text += $"Video:{Environment.NewLine}";

                    foreach (var info in videoDeviceInfos)
                    {
                        text += $"name:{info.FriendlyName} path:{info.Path}{Environment.NewLine}";
                    }

                    text += $"{Environment.NewLine}";
                }

                if (audioDeviceInfos.Count() > 0)
                {
                    text += $"Audio:{Environment.NewLine}";

                    foreach (var info in audioDeviceInfos)
                    {
                        text += $"name:{info.FriendlyName} path:{info.Path}{Environment.NewLine}";
                    }

                    text += $"{Environment.NewLine}";
                }

                var devices = ApiProvider.Default.GetDeckLinkDevices();

                if (devices.Count() > 0)
                {
                    text += $"DeckLink:{Environment.NewLine}";

                    foreach (var device in devices)
                    {
                        text += $"name:{device.FriendlyName} path:{device.Path}{Environment.NewLine}";

                        var modes = ApiProvider.Default.GetDeckLinkDeviceModesById(device.Path);

                        foreach (var mode in modes)
                        {
                            text += $"    {mode.Mode}-{mode.ModeName}";
                        }
                    }

                    text += $"{Environment.NewLine}";
                }

                MessageBox.Show(text, "Devices");
            });
        }

        private void btnCamere_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                IEnumerable<InDeviceInfo> videoDeviceInfos;
                IEnumerable<InDeviceInfo> audioDeviceInfos;

                ApiProvider.Default.GetFreeDeviceInfos(out videoDeviceInfos, out audioDeviceInfos);

                if (videoDeviceInfos.Count() > 0)
                {
                    var infos = ApiProvider.Default.GetCamereCaptureInfos(videoDeviceInfos.First());

                    if (infos.Count() > 0)
                    {
                        var text = string.Format("{0,10}{1,10}{2,10}{3,10}{4}", "width", "height", "fps", "format", Environment.NewLine);

                        for (int i = 0; i < infos.Count(); i++)
                        {
                            var info = infos.ElementAt(i);

                            text += string.Format("{0,10}{1,10}{2,10}{3,10}{4}", info.Width, info.Height, info.Fps, info.Format, Environment.NewLine);
                        }

                        MessageBox.Show(text, "Camere");
                    }
                }
            });
        }

        private void btnCreateService_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                var cachePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "缓存");

                if (_serviceCreated = ApiProvider.Default.Create(cachePath))
                {
                    MessageBox.Show(cachePath, "Created");
                }
            });
        }

        private void btnDestroyService_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_serviceCreated)
                {
                    ApiProvider.Default.Destroy();

                    _serviceCreated = false;
                }
            });
        }

        private void btnInitParam_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_serviceCreated)
                {
                    Param param;

                    if (ApiProvider.Default.GetDefaultParam(out param) &&
                        param != null)
                    {
                        var text = $"content:{param.Content}{Environment.NewLine}";
                        text += $"format:{param.Format}{Environment.NewLine}";
                        text += $"syncTimestampType:{param.SyncTimestampType}{Environment.NewLine}";
                        text += $"video.width:{param.VideoParam.Width}{Environment.NewLine}";
                        text += $"video.height:{param.VideoParam.Height}{Environment.NewLine}";
                        text += $"video.fps:{param.VideoParam.Fps}{Environment.NewLine}";
                        text += $"video.bitRate:{param.VideoParam.BitRate}{Environment.NewLine}";
                        text += $"video.codec:{param.VideoParam.Codec}{Environment.NewLine}";
                        text += $"video.hardEncode:{param.VideoParam.HardEncode}{Environment.NewLine}";
                        text += $"video.autoRatio:{param.VideoParam.QosAutoChangeRatio}{Environment.NewLine}";
                        text += $"audio.codec:{param.AudioParam.Codec}{Environment.NewLine}";
                        text += $"audio.bitRate:{param.AudioParam.BitRate}{Environment.NewLine}";
                        text += $"audio.hardEncode:{param.AudioParam.HardEncode}{Environment.NewLine}";

                        MessageBox.Show(text, "DefaultParam");

                        // 需要初始化，否则调用UpdateUrl报错
                        param.Url = new string('\0', 1024);

                        _paramInited = ApiProvider.Default.InitParam(param);
                    }
                }
            });
        }

        private void btnUninitParam_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_paramInited)
                {
                    ApiProvider.Default.UninitParam();

                    _paramInited = false;
                }
            });
        }

        private void btnUpdateVideoParam_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_paramInited)
                {
                    var param = new VideoOutParam();

                    param.Width = ((int)SystemParameters.PrimaryScreenWidth / 4) * 4;
                    param.Height = ((int)SystemParameters.PrimaryScreenHeight / 4) * 4;
                    param.Fps = 6;
                    param.BitRate = 800000;
                    param.Codec = VideoOutCodec.OpenH264;
                    param.HardEncode = true;
                    param.QosAutoChangeRatio = true;

                    if (ApiProvider.Default.UpdateVideoOutParam(param))
                    {
                        var text = $"video.width:{param.Width}{Environment.NewLine}";
                        text += $"video.height:{param.Height}{Environment.NewLine}";
                        text += $"video.fps:{param.Fps}{Environment.NewLine}";
                        text += $"video.bitRate:{param.BitRate}{Environment.NewLine}";
                        text += $"video.codec:{param.Codec}{Environment.NewLine}";
                        text += $"video.hardEncode:{param.HardEncode}{Environment.NewLine}";
                        text += $"video.autoRatio:{param.QosAutoChangeRatio}{Environment.NewLine}";

                        MessageBox.Show(text, "NewParam");
                    }
                }
            });
        }

        private void btnUpdateAudioParam_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_paramInited)
                {
                    var param = new AudioOutParam();

                    param.BitRate = 64000;
                    param.Codec = AudioOutCodec.AAC;
                    //param.HardEncode = true;

                    if (ApiProvider.Default.UpdateAudioOutParam(param))
                    {
                        var text = $"audio.codec:{param.Codec}{Environment.NewLine}";
                        text += $"audio.bitRate:{param.BitRate}{Environment.NewLine}";
                        text += $"audio.hardEncode:{param.HardEncode}{Environment.NewLine}";

                        MessageBox.Show(text, "NewParam");
                    }
                }
            });
        }

        private void btnUpdatePushUrl_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_paramInited)
                {
                    var url = "这里是网易rtmp推流地址";

                    if (ApiProvider.Default.UpdatePushUrl(url))
                    {
                        MessageBox.Show(url, "PushUrl");
                    }
                }
            });
        }

        private void btnUpdateSyncTimestampType_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_paramInited &&
                    ApiProvider.Default.UpdateSyncTimestampType(SyncTimestampType.BaseOnStreamStart))
                {
                    var text = ApiProvider.Default.GetSyncTimestamp() + " ms";

                    MessageBox.Show(text, "SyncTimestamp");
                }
            });
        }

        private void btnWater_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                var param = new VideoWaterParam();

                param.FilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Water.png");
                param.StartX = 10;
                param.StartY = 10;

                ApiProvider.Default.SetVideoWaterMark(param);

                var text = $"filePath:{param.FilePath}{Environment.NewLine}";
                text += $"startX:{param.StartX}{Environment.NewLine}";
                text += $"startY:{param.StartY}{Environment.NewLine}";

                MessageBox.Show(text, "WaterParam");
            });
        }

        private void btnOpenDesktop_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                var param = new VideoInParam();

                param.Type = VideoInType.FullScreen;
                param.CaptureFps = 6;

                if (_desktopOpened = _desktop.Open(param))
                {
                    MessageBox.Show("Opened!", "Open Success");

                    _desktop.SetBackLayer();
                }
            });
        }

        private void btnCloseDesktop_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_desktopOpened)
                {
                    _desktop.Close();

                    _desktopOpened = false;
                }
            });
        }

        private void btnStartDesktop_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_desktopOpened &&
                    (_desktopStarted = _desktop.StartCapture()))
                {
                    _desktop.SwitchSoloPreview(true);

                    MessageBox.Show("Started!", "Start Success");
                }
            });
        }

        private void btnStopDesktop_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_desktopStarted)
                {
                    _desktop.SwitchSoloPreview(false);

                    imgPreviewDesktop.Source = null;

                    _desktop.StopCapture();

                    _desktopStarted = false;
                }
            });
        }

        private void btnPauseDesktop_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_desktopStarted)
                {
                    _desktop.PauseLiveStream();
                }
            });
        }

        private void btnResumeDesktop_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_desktopStarted)
                {
                    _desktop.ResumeLiveStream();
                }
            });
        }

        private void btnOpenCamere_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                IEnumerable<InDeviceInfo> videoDeviceInfos;
                IEnumerable<InDeviceInfo> audioDeviceInfos;

                ApiProvider.Default.GetFreeDeviceInfos(out videoDeviceInfos, out audioDeviceInfos);

                if (videoDeviceInfos.Count() > 0)
                {
                    var param = new VideoInParam();

                    param.Type = VideoInType.Camera;
                    param.CaptureFps = 20;
                    param.Camera = new CameraParam();
                    param.Camera.DevicePath = videoDeviceInfos.First().Path;
                    param.Camera.QualityLevel = VideoQualityLevel.Middle;

                    if (_cameraOpened = _camera.Open(param))
                    {
                        var text = $"fps:{param.CaptureFps}{Environment.NewLine}";
                        text += $"devicePath:{param.Camera.DevicePath}{Environment.NewLine}";
                        text += $"qualityLevel:{param.Camera.QualityLevel}{Environment.NewLine}";

                        MessageBox.Show(text, "Open Success");

                        var rect = new RectScreenParam
                        {
                            Left = ((int)SystemParameters.PrimaryScreenWidth / 4) * 4 - 210,
                            Top = ((int)SystemParameters.PrimaryScreenHeight / 4) * 4 - 110,
                            Right = 10,
                            Bottom = 10
                        };

                        // TODO 此方法无效
                        _camera.SetDisplayRect(rect);
                        _camera.AdjustLayer(false);

                        if (_cameraStarted = _camera.StartCapture())
                        {
                            _camera.SwitchSoloPreview(true);
                        }
                    }
                }
            });
        }

        private void btnCloseCamere_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_cameraStarted)
                {
                    _camera.SwitchSoloPreview(false);

                    imgPreviewCamera.Source = null;

                    _camera.StopCapture();

                    _cameraStarted = false;
                }

                if (_cameraOpened)
                {
                    _camera.Close();

                    _cameraOpened = false;
                }
            });
        }

        private void btnOpenSystem_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                var param = _system.GetDefaultParam();

                param.Type = AudioInType.System;
                //param.SampleRate = 44100;
                //param.NumOfChannels = 1;
                //param.FrameSize = 2048;
                //param.Format = AudioInFormat.S16;

                if (_systemOpened = _system.Open(param))
                {
                    var text = $"sampleRate:{param.SampleRate}{Environment.NewLine}";
                    text += $"numOfChannels:{param.NumOfChannels}{Environment.NewLine}";
                    text += $"frameSize:{param.FrameSize}{Environment.NewLine}";
                    text += $"format:{param.Format}{Environment.NewLine}";

                    MessageBox.Show(text, "Open Success");
                }
            });
        }

        private void btnCloseSystem_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_systemOpened)
                {
                    _system.Close();

                    _systemOpened = false;
                }
            });
        }

        private void btnStartSystem_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_systemOpened)
                {
                    if (_systemStarted = _system.StartCapture())
                    {
                        MessageBox.Show("Started!", "Start Success");
                    }
                }
            });
        }

        private void btnStopSystem_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_systemStarted)
                {
                    _system.StopCapture();

                    _systemStarted = false;
                }
            });
        }

        private void btnPauseSystem_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_systemStarted)
                {
                    _system.PauseLiveStream();
                }
            });
        }

        private void btnResumeSystem_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_systemStarted)
                {
                    _system.ResumeLiveStream();
                }
            });
        }

        private void btnStartService_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_serviceStarted = ApiProvider.Default.Start())
                {
                    MessageBox.Show("Started!", "Start Success");
                }
            });
        }

        private void btnStopService_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_serviceStarted)
                {
                    ApiProvider.Default.Stop();

                    _serviceStarted = false;
                }
            });
        }

        private void btnStartVideoPreview_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_serviceStarted &&
                    (_videoPreviewStarted = ApiProvider.Default.StartVideoPreview()))
                {
                    MessageBox.Show("Started!", "Start Success");
                }
            });
        }

        private void btnStopVideoPreview_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_videoPreviewStarted)
                {
                    ApiProvider.Default.StopVideoPreview();

                    imgPreviewMerged.Source = null;

                    _videoPreviewStarted = false;
                }
            });
        }

        private void btnPauseVideoPreview_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_videoPreviewStarted)
                {
                    ApiProvider.Default.PauseVideoPreview();
                }
            });
        }

        private void btnResumeVideoPreview_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_videoPreviewStarted)
                {
                    ApiProvider.Default.ResumeVideoPreview();
                }
            });
        }

        private void btnStartLiveStream_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_serviceStarted &&
                    (_liveStreamStarted = ApiProvider.Default.StartLiveStream()))
                {
                    MessageBox.Show("Started!", "Start Success");
                }
            });
        }

        private void btnStopLiveStream_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_liveStreamStarted)
                {
                    ApiProvider.Default.StopLiveStream();

                    _liveStreamStarted = false;
                }
            });
        }

        private void btnPauseVideoLiveStream_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_liveStreamStarted)
                {
                    ApiProvider.Default.PauseVideoLiveStream();
                }
            });
        }

        private void btnResumeVideoLiveStream_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_liveStreamStarted)
                {
                    ApiProvider.Default.ResumeVideoLiveStream();
                }
            });
        }

        private void btnPauseAudioLiveStream_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_liveStreamStarted)
                {
                    ApiProvider.Default.PauseAudioLiveStream();
                }
            });
        }

        private void btnResumeAudioLiveStream_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_liveStreamStarted)
                {
                    ApiProvider.Default.ResumeAudioLiveStream();
                }
            });
        }

        private void btnStartRecord_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_liveStreamStarted)
                {
                    var param = new RecordParam();

                    param.Format = RecordFormat.FLV;
                    param.Path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"录制\{new Random().Next()}.flv");

                    if (_recordStarted = ApiProvider.Default.StartRecord(param))
                    {
                        var text = $"format:{param.Format}{Environment.NewLine}";
                        text += $"path:{param.Path}{Environment.NewLine}";

                        MessageBox.Show(text, "Start Success");
                    }
                }
            });
        }

        private void btnStopRecord_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_recordStarted)
                {
                    ApiProvider.Default.StopRecord();

                    _recordStarted = false;
                }
            });
        }

        private void btnStatus_Click(object sender, RoutedEventArgs e)
        {
            DoAction(() =>
            {
                if (_liveStreamStarted)
                {
                    StatusInfo status;

                    if (ApiProvider.Default.GetStaticInfo(out status))
                    {
                        var text = $"{DateTime.Now.ToLongTimeString()}-{Environment.NewLine}";
                        text += $"VideoSendFrameRate:{status.VideoSendFrameRate}{Environment.NewLine}";
                        text += $"VideoSendBitRate:{status.VideoSendBitRate}{Environment.NewLine}";
                        text += $"VideoSendWidth:{status.VideoSendWidth}{Environment.NewLine}";
                        text += $"VideoSendHeight:{status.VideoSendHeight}{Environment.NewLine}";
                        text += $"VideoSetFrameRate:{status.VideoSetFrameRate}{Environment.NewLine}";
                        text += $"VideoSetBitRate:{status.VideoSetBitRate}{Environment.NewLine}";
                        text += $"VideoSetWidth:{status.VideoSetWidth}{Environment.NewLine}";
                        text += $"VideoSetHeight:{status.VideoSetHeight}{Environment.NewLine}";
                        text += $"AudioSendBitRate:{status.AudioSendBitRate}{Environment.NewLine}";
                        text += $"NetworkLevel:{status.NetworkLevel}{Environment.NewLine}";

                        tbxStatus.AppendText(text);
                    }
                }
            });
        }
    }
}
