using System.IO;

namespace ArrowGame.Common.Packets.Server {
	public class ServerRoomStatusPacket : IPacket {
		public PacketType Type => PacketType.ServerRoomStatus;

		public int Id;
		public RoomState State;
		public int Players;

		public ServerRoomStatusPacket(int id, RoomState state, int players) {
			Id = id;
			State = state;
			Players = players;
		}

		public ServerRoomStatusPacket(BinaryReader reader) {
			Id = reader.ReadInt32();
			State = (RoomState)reader.ReadByte();
			Players = reader.ReadInt32();
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(Id);
			writer.Write((byte)State);
			writer.Write(Players);
		}

		public override string ToString() {
			return $"{nameof(ServerRoomStatusPacket)} {{ {nameof(Id)}: {Id}, {nameof(State)}: {State}, {nameof(Players)}: {Players} }}";
		}
	}
}
