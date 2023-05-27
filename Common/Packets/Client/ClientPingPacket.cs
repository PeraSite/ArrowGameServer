using System.IO;

namespace ArrowGame.Common.Packets.Client {
	public struct ClientPingPacket : IPacket {
		public PacketType Type => PacketType.ClientPing;
		public void Serialize(BinaryWriter writer) { }
	}
}
