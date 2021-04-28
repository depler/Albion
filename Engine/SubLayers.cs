using Albion.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albion.Engine
{
    public static class SubLayers
    {
        public static readonly Guid ALBION_SUBLAYER_ALE_AUTH_CONNECT_V4 = new Guid("b2e6b9ee-67b8-4ced-8061-598837ebfd8e");
        public static readonly Guid ALBION_SUBLAYER_ALE_AUTH_CONNECT_V6 = new Guid("e5408987-73f3-40f7-b63b-80acd0f3d57a");
        public static readonly Guid ALBION_SUBLAYER_ALE_AUTH_RECV_ACCEPT_V4 = new Guid("a4ab9504-217a-45e6-8652-812ad0f3e3a6");        
        public static readonly Guid ALBION_SUBLAYER_ALE_AUTH_RECV_ACCEPT_V6 = new Guid("8e1e0ef2-035a-4f42-9284-5b72251a5e25");

        public static readonly Guid ALBION_SUBLAYER_INBOUND_TRANSPORT_V4 = new Guid("2cb14db2-46fa-4c22-8c12-e89f7e13a95c");
        public static readonly Guid ALBION_SUBLAYER_INBOUND_TRANSPORT_V6 = new Guid("299e7cf4-0596-4f71-9e96-3aa329247bc4");
        public static readonly Guid ALBION_SUBLAYER_OUTBOUND_TRANSPORT_V4 = new Guid("01f8d768-1156-4472-b236-8f78902c6d8a");
        public static readonly Guid ALBION_SUBLAYER_OUTBOUND_TRANSPORT_V6 = new Guid("d5d43dc2-8112-4dea-97c1-6d4171390953");

        public static readonly Guid ALBION_SUBLAYER_INBOUND_TRANSPORT_V4_DISCARD = new Guid("b6ab91a1-3c5c-4524-8a54-194ca4739c1f");
        public static readonly Guid ALBION_SUBLAYER_INBOUND_TRANSPORT_V6_DISCARD = new Guid("59cc007a-d1cb-46e4-abc3-641e4e944c49");
        public static readonly Guid ALBION_SUBLAYER_OUTBOUND_TRANSPORT_V4_DISCARD = new Guid("e1c808cd-b9fb-47a8-8366-f15dad53a704");
        public static readonly Guid ALBION_SUBLAYER_OUTBOUND_TRANSPORT_V6_DISCARD = new Guid("2a1a6963-f2e9-49c5-a1ca-41666c38e529");

        public static KeyValuePair<Guid, string>[] All()
        {
            return typeof(SubLayers).GetFields().Select(x => new KeyValuePair<Guid, string>((Guid)x.GetValue(null), x.Name)).ToArray();
        }

        public static Guid Get(Guid layerKey)
        {
            if (layerKey == Layers.FWPM_LAYER_ALE_AUTH_CONNECT_V4)
                return ALBION_SUBLAYER_ALE_AUTH_CONNECT_V4;
            if (layerKey == Layers.FWPM_LAYER_ALE_AUTH_CONNECT_V6)
                return ALBION_SUBLAYER_ALE_AUTH_CONNECT_V6;
            if (layerKey == Layers.FWPM_LAYER_ALE_AUTH_RECV_ACCEPT_V4)
                return ALBION_SUBLAYER_ALE_AUTH_RECV_ACCEPT_V4;
            if (layerKey == Layers.FWPM_LAYER_ALE_AUTH_RECV_ACCEPT_V6)
                return ALBION_SUBLAYER_ALE_AUTH_RECV_ACCEPT_V6;

            if (layerKey == Layers.FWPM_LAYER_INBOUND_TRANSPORT_V4)
                return ALBION_SUBLAYER_INBOUND_TRANSPORT_V4;
            if (layerKey == Layers.FWPM_LAYER_INBOUND_TRANSPORT_V6)
                return ALBION_SUBLAYER_INBOUND_TRANSPORT_V6;
            if (layerKey == Layers.FWPM_LAYER_OUTBOUND_TRANSPORT_V4)
                return ALBION_SUBLAYER_OUTBOUND_TRANSPORT_V4;
            if (layerKey == Layers.FWPM_LAYER_OUTBOUND_TRANSPORT_V6)
                return ALBION_SUBLAYER_OUTBOUND_TRANSPORT_V6;

            if (layerKey == Layers.FWPM_LAYER_INBOUND_TRANSPORT_V4_DISCARD)
                return ALBION_SUBLAYER_INBOUND_TRANSPORT_V4_DISCARD;
            if (layerKey == Layers.FWPM_LAYER_INBOUND_TRANSPORT_V6_DISCARD)
                return ALBION_SUBLAYER_INBOUND_TRANSPORT_V6_DISCARD;
            if (layerKey == Layers.FWPM_LAYER_OUTBOUND_TRANSPORT_V4_DISCARD)
                return ALBION_SUBLAYER_OUTBOUND_TRANSPORT_V4_DISCARD;
            if (layerKey == Layers.FWPM_LAYER_OUTBOUND_TRANSPORT_V6_DISCARD)
                return ALBION_SUBLAYER_OUTBOUND_TRANSPORT_V6_DISCARD;

            throw new Exception($"Sub-layer not found for {layerKey}");
        }
    }
}
