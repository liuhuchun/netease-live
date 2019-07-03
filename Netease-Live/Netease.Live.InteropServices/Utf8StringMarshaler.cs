using System;
using System.Runtime.InteropServices;

namespace Netease.Live.InteropServices
{
    /// <summary>
    /// Marshal unicode string param to utf-8 string,usage:[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]
    /// </summary>
    public class Utf8StringMarshaler : ICustomMarshaler
    {
        private static Utf8StringMarshaler _instance = new Utf8StringMarshaler();

        public Utf8StringMarshaler()
        {

        }

        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            if (!(ManagedObj is string))
            {
                throw new InvalidOperationException("Utf8StringMarshaler:ManagedObj must be string");
            }

            return Utilities.UTF8StringToIntPtr(ManagedObj as string);
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            return Utilities.IntPtrToUTF8String(pNativeData);
        }

        public void CleanUpManagedData(object ManagedObj)
        {
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            Marshal.FreeCoTaskMem(pNativeData);

            pNativeData = IntPtr.Zero;
        }

        public int GetNativeDataSize()
        {
            return -1;
        }

        public static ICustomMarshaler GetInstance(string s)
        {
            return _instance;
        }
    }
}
