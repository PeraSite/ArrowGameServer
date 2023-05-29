using System.IO;

namespace ArrowGame.Common.Packets.Server {
	public class ServerRoomQuitPacket : IPacket {
		public PacketType Type => PacketType.ServerRoomQuit;

		public int PlayerId { get; }

		public ServerRoomQuitPacket(int playerId) {
			PlayerId = playerId;
		}

		public ServerRoomQuitPacket(BinaryReader reader) {
			PlayerId = reader.ReadInt32();
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(PlayerId);
		}

		public override string ToString() {
			return $"{nameof(ServerRoomQuitPacket)} {{ {nameof(PlayerId)}: {PlayerId} }}";
		}
	}
}
