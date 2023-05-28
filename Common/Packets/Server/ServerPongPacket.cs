using System.IO;

namespace ArrowGame.Common.Packets.Server {
	public class ServerPongPacket : IPacket {
		public PacketType Type => PacketType.ServerPong;

		public int PlayerId { get; }

		public ServerPongPacket(int playerId) {
			PlayerId = playerId;
		}

		public ServerPongPacket(BinaryReader reader) {
			PlayerId = reader.ReadInt32();
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(PlayerId);
		}

		public override string ToString() {
			return $"{nameof(ServerPongPacket)} {{ {nameof(PlayerId)}: {PlayerId} }}";
		}
	}
}
