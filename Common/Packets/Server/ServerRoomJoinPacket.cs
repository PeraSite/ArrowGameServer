using System.IO;

namespace ArrowGame.Common.Packets.Server {
	public class ServerRoomJoinPacket : IPacket {
		public PacketType Type => PacketType.ServerRoomJoin;

		public int PlayerId { get; }

		public ServerRoomJoinPacket(int playerId) {
			PlayerId = playerId;
		}

		public ServerRoomJoinPacket(BinaryReader reader) {
			PlayerId = reader.ReadInt32();
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(PlayerId);
		}

		public override string ToString() {
			return $"{nameof(ServerRoomJoinPacket)} {{ {nameof(PlayerId)}: {PlayerId} }}";
		}
	}
}
