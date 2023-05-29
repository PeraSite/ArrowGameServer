using System.IO;

namespace ArrowGame.Common.Packets.Server {
	public class ServerAssignPlayerIdPacket : IPacket {
		public PacketType Type => PacketType.ServerAssignPlayerId;

		public int PlayerId { get; }

		public ServerAssignPlayerIdPacket(int playerId) {
			PlayerId = playerId;
		}

		public ServerAssignPlayerIdPacket(BinaryReader reader) {
			PlayerId = reader.ReadInt32();
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(PlayerId);
		}
	}
}
