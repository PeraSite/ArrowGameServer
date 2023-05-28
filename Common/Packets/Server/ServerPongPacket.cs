using System.IO;

namespace ArrowGame.Common.Packets.Server {
	public readonly struct ServerPongPacket : IPacket {
		public PacketType Type => PacketType.ServerPong;
		public void Serialize(BinaryWriter writer) { }

		public override string ToString() {
			return $"{nameof(ServerPongPacket)}";
		}
	}
}
