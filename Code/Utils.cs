using Albion.Native;
using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Albion.Code
{
    public static class Utils
    {
        public static uint GetIpAddrV4(byte[] data)
        {
            var bytesTotal = sizeof(uint);
            if (data.Length > bytesTotal)
                throw new ArgumentOutOfRangeException(nameof(data));

            var addr = new BitArray(data.Reverse().ToArray());
            var array = new uint[1];
            addr.CopyTo(array, 0);
            return array[0];
        }

        public static uint GetIpMaskV4(int bits)
        {
            var bitsTotal = sizeof(uint) * 8;
            if (bits > bitsTotal)
                throw new ArgumentOutOfRangeException(nameof(bits));

            var mask = new BitArray(bitsTotal, false);
            for (int i = 1; i <= bits; i++)
                mask.Set(bitsTotal - i, true);

            var array = new uint[1];
            mask.CopyTo(array, 0);
            return array[0];
        }

        public static void ParseSubnetV4(string subnet, out uint addr, out uint mask)
        {
            var parts = subnet.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
                throw new ArgumentOutOfRangeException(nameof(subnet));

            var ip = IPAddress.Parse(parts[0]);
            if (ip.AddressFamily != AddressFamily.InterNetwork)
                throw new ArgumentOutOfRangeException(nameof(subnet));
            
            addr = GetIpAddrV4(ip.GetAddressBytes());
            mask = GetIpMaskV4(int.Parse(parts[1]));
        }

        public static bool TryParseSubnetV4(string subnet, out uint addr, out uint mask)
        {
            try
            {
                ParseSubnetV4(subnet, out addr, out mask);
                return true;
            }
            catch
            {
                addr = 0;
                mask = 0;
                return false;
            }
        }

        public static void ParseSubnetV6(string subnet, out byte[] addr, out byte prefix)
        {
            var parts = subnet.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
                throw new ArgumentOutOfRangeException(nameof(subnet));

            var ip = IPAddress.Parse(parts[0]);
            if (ip.AddressFamily != AddressFamily.InterNetworkV6)
                throw new ArgumentOutOfRangeException(nameof(subnet));

            addr = ip.GetAddressBytes();
            prefix = byte.Parse(parts[1]);
        }

        public static bool TryParseSubnetV6(string subnet, out byte[] addr, out byte prefix)
        {
            try
            {
                ParseSubnetV6(subnet, out addr, out prefix);
                return true;
            }
            catch
            {
                addr = null;
                prefix = 0;
                return false;
            }
        }

        public static string GetStaticGuids(Type type)
        {
            return string.Join('\n', type.GetFields().Where(x => x.FieldType == typeof(Guid))
                .Select(x => $"public static readonly Guid {x.Name} = new Guid(\"{x.GetValue(null)}\");"));
        }
    }
}
