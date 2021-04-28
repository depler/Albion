using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Albion.Native
{
    public struct FWPM_DISPLAY_DATA0
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string name;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string description;
    }

    public struct FWPM_SESSION0
    {
        public Guid sessionKey;
        public FWPM_DISPLAY_DATA0 displayData;
        public FWPM_SESSION_FLAG flags;
        public uint txnWaitTimeoutInMSec;
        public uint processId;
        public IntPtr sid;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string username;
        [MarshalAs(UnmanagedType.Bool)]
        public bool kernelMode;
    }

    public struct FWP_VALUE0
    {
        public FWP_DATA_TYPE type;
        public Union value;

        [StructLayout(LayoutKind.Explicit)]
        public struct Union
        {
            [FieldOffset(0)]
            public byte uint8;
            [FieldOffset(0)]
            public ushort uint16;
            [FieldOffset(0)]
            public uint uint32;
            [FieldOffset(0)]
            public IntPtr uint64;
            [FieldOffset(0)]
            public sbyte int8;
            [FieldOffset(0)]
            public short int16;
            [FieldOffset(0)]
            public int int32;
            [FieldOffset(0)]
            public IntPtr int64;
            [FieldOffset(0)]
            public float float32;
            [FieldOffset(0)]
            public IntPtr double64;
            [FieldOffset(0)]
            public IntPtr byteArray16;
            [FieldOffset(0)]
            public IntPtr byteBlob;
            [FieldOffset(0)]
            public IntPtr sid;
            [FieldOffset(0)]
            public IntPtr sd;
            [FieldOffset(0)]
            public IntPtr tokenInformation;
            [FieldOffset(0)]
            public IntPtr tokenAccessInformation;
            [FieldOffset(0)]
            public IntPtr unicodeString;
            [FieldOffset(0)]
            public IntPtr byteArray6;
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct FWPM_ACTION0
    {
        [FieldOffset(0)]
        public FWP_ACTION_TYPE type;
        [FieldOffset(4)]
        public Guid filterType;
        [FieldOffset(4)]
        public Guid calloutKey;
    }

    public struct FWPM_FILTER0
    {
        public Guid filterKey;
        public FWPM_DISPLAY_DATA0 displayData;
        public FWPM_FILTER_FLAG flags;
        public IntPtr providerKey;
        public FWP_BYTE_BLOB providerData;
        public Guid layerKey;
        public Guid subLayerKey;
        public FWP_VALUE0 weight;
        public uint numFilterConditions;
        public IntPtr filterConditions;
        public FWPM_ACTION0 action;
        public Union context;
        public IntPtr reserved;
        public ulong filterId;
        public FWP_VALUE0 effectiveWeight;

        [StructLayout(LayoutKind.Explicit)]
        public struct Union
        {
            [FieldOffset(0)]
            public ulong rawContext;
            [FieldOffset(0)]
            public Guid providerContextKey;
        }
    }

    public struct FWP_BYTE_BLOB
    {
        public uint size;
        public IntPtr data;
    }

    public struct FWP_CONDITION_VALUE0
    {
        public FWP_DATA_TYPE type;
        public Union value;

        [StructLayout(LayoutKind.Explicit)]
        public struct Union
        {
            [FieldOffset(0)]
            public byte uint8;
            [FieldOffset(0)]
            public ushort uint16;
            [FieldOffset(0)]
            public uint uint32;
            [FieldOffset(0)]
            public IntPtr uint64;
            [FieldOffset(0)]
            public sbyte int8;
            [FieldOffset(0)]
            public short int16;
            [FieldOffset(0)]
            public int int32;
            [FieldOffset(0)]
            public IntPtr int64;
            [FieldOffset(0)]
            public float float32;
            [FieldOffset(0)]
            public IntPtr double64;
            [FieldOffset(0)]
            public IntPtr byteArray16;
            [FieldOffset(0)]
            public IntPtr byteBlob;
            [FieldOffset(0)]
            public IntPtr sid;
            [FieldOffset(0)]
            public IntPtr sd;
            [FieldOffset(0)]
            public IntPtr tokenInformation;
            [FieldOffset(0)]
            public IntPtr tokenAccessInformation;
            [FieldOffset(0)]
            public IntPtr unicodeString;
            [FieldOffset(0)]
            public IntPtr byteArray6;
            [FieldOffset(0)]
            public IntPtr v4AddrMask;
            [FieldOffset(0)]
            public IntPtr v6AddrMask;
            [FieldOffset(0)]
            public IntPtr rangeValue;
        }
    }

    public struct FWPM_FILTER_CONDITION0
    {
        public Guid fieldKey;
        public FWP_MATCH matchType;
        public FWP_CONDITION_VALUE0 conditionValue;
    }

    public struct FWPM_PROVIDER0
    {
        public Guid providerKey;
        public FWPM_DISPLAY_DATA0 displayData;
        public FWPM_PROVIDER_FLAG flags;
        public FWP_BYTE_BLOB providerData;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string serviceName;
    }

    public struct FWPM_FILTER_ENUM_TEMPLATE0
    {
        public IntPtr providerKey;
        public Guid layerKey;
        public FWP_FILTER_ENUM_TYPE enumType;
        public FWP_FILTER_ENUM_FLAG flags;
        public IntPtr providerContextTemplate;
        public uint numFilterConditions;
        public IntPtr filterCondition;
        public uint actionMask;
        public IntPtr calloutKey;
    }

    public struct FWPM_SUBLAYER0
    {
        public Guid subLayerKey;
        public FWPM_DISPLAY_DATA0 displayData;
        public FWPM_SUBLAYER_FLAG flags;
        public IntPtr providerKey;
        public FWP_BYTE_BLOB providerData;
        public ushort weight;
    }

    public struct FWP_V4_ADDR_AND_MASK
    {
        public uint addr;
        public uint mask;
    }

    public struct FWP_V6_ADDR_AND_MASK
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.U1)]
        public byte[] addr;
        public byte prefixLength;
    }
}
