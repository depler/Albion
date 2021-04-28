using Albion.Engine;

namespace Albion.Code
{
    class Program
    {
        static void Main(string[] args)
        {
            using var engine = new FilteringEngine();

            //init and clear existing filters
            engine.Initialize();
            engine.ClearFilters();

            //prevent port scanning
            engine.SetSilentBlockInV4();
            engine.SetSilentBlockInV6();

            //block all incoming connections
            engine.AddFilterInV4(false);
            engine.AddFilterInV6(false);

            //allow all outgoing connections
            engine.AddFilterOutV4(true);
            engine.AddFilterOutV6(true);

            //allow incoming ping requests
            engine.AddFilterInV6(true, new[] { Native.IPPROTO.ICMP });

            //allow incoming requests for DNS, HTTP, HTTPS
            engine.AddFilterInV4(true, new[] { Native.IPPROTO.TCP }, new [] { "53", "80", "443" });

            //allow SMB and RDP incoming connections for specified network
            engine.AddFilterInV4(true, new[] { Native.IPPROTO.TCP }, new[] { "445", "3389" }, new[] { "192.168.1.0/24" });
        }
    }
}
