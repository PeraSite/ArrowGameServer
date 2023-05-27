using System.IO;

namespace ArrowGame.Common.Packets.Server {
	public struct ServerPongPacket : IPacket {
		public PacketType Type => PacketType.ServerPong;
		public void Serialize(BinaryWriter writer) { }
	}
}
