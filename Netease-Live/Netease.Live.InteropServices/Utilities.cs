﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Netease.Live.InteropServices
{
    internal class Utilities
    {
        public static IntPtr UTF8StringToIntPtr(string data)
        {
            if (data == null)
            {
                return IntPtr.Zero;
            }

            var utf8Bytes = Encoding.UTF8.GetBytes(data);
            var ptr = Marshal.AllocCoTaskMem(utf8Bytes.Length + 1);
            Marshal.Copy(utf8Bytes, 0, ptr, utf8Bytes.Length);
            Marshal.WriteByte(ptr, utf8Bytes.Length, 0);

            return ptr;
        }

        public static string IntPtrToUTF8String(IntPtr pNativeData)
        {
            if (pNativeData == IntPtr.Zero)
            {
                return null;
            }

            var bytes = new List<byte>();
            for (var offset = 0; ; offset++)
            {
                var b = Marshal.ReadByte(pNativeData, offset);

                if (b == 0)
                {
                    break;
                }

                bytes.Add(b);
            }

            return Encoding.UTF8.GetString(bytes.ToArray(), 0, bytes.Count);
        }
    }
}