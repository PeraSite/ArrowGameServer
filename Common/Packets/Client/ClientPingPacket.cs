using System.IO;

namespace ArrowGame.Common.Packets.Client {
	public class ClientPingPacket : IPacket {
		public PacketType Type => PacketType.ClientPing;
		public void Serialize(BinaryWriter writer) { }

		public override string ToString() {
			return $"{nameof(ClientPingPacket)}";
		}
	}
}
