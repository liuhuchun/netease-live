using System;
using System.Drawing;

namespace Netease.Live.InteropServices
{
    /// <summary>
    /// 视频时预览参数
    /// </summary>
    public class VideoPreviewingEventArgs : EventArgs
    {
        /// <summary>
        /// 预览图片
        /// </summary>
        public Image Image { get; private set; }

        public VideoPreviewingEventArgs(Image image)
        {
            Image = image;
        }
    }
}
