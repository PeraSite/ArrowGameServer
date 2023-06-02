using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ArrowGame.Common.Packets.Server {
	public class ServerRoomStatusPacket : IPacket {
		public PacketType Type => PacketType.ServerRoomStatus;

		public int Id;
		public RoomState State;
		public int PlayerCount;
		public Dictionary<int, int> PlayerHp;

		public ServerRoomStatusPacket(int id, RoomState state, int playerCount, Dictionary<int, int> playerHp) {
			Id = id;
			State = state;
			PlayerCount = playerCount;
			PlayerHp = playerHp;
		}

		public ServerRoomStatusPacket(BinaryReader reader) {
			Id = reader.ReadInt32();
			State = (RoomState)reader.ReadByte();
			PlayerCount = reader.ReadInt32();

			PlayerHp = new Dictionary<int, int>();
			for (int i = 0; i < PlayerCount; i++) {
				var playerId = reader.ReadInt32();
				var playerHp = reader.ReadInt32();
				PlayerHp.Add(playerId, playerHp);
			}
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(Id);
			writer.Write((byte)State);
			writer.Write(PlayerCount);
			foreach (var (playerId, playerHp) in PlayerHp) {
				writer.Write(playerId);
				writer.Write(playerHp);
			}
		}

		public override string ToString() {
			var hpStr = string.Join(", ", PlayerHp.Select(x => {
				var (id, hp) = x;
				return $"{id}:{hp}";
			}));

			return $"{nameof(ServerRoomStatusPacket)} {{ {nameof(Id)}: {Id}, {nameof(State)}: {State}, {nameof(PlayerCount)}: {PlayerCount}, {nameof(PlayerHp)}: {hpStr}}}";
		}
	}
}
