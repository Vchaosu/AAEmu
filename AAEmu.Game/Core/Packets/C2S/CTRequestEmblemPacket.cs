using AAEmu.Commons.Network;
using AAEmu.Game.Core.Managers.Stream;
using AAEmu.Game.Core.Network.Stream;

namespace AAEmu.Game.Core.Packets.C2S
{
    public class CTRequestEmblemPacket : StreamPacket
    {
        public CTRequestEmblemPacket() : base(0x03)
        {
        }

        public override void Read(PacketStream stream)
        {
            var type = stream.ReadUInt64();
            UccManager.Instance.RequestUcc(Connection, type);
        }
    }
}
