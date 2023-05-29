using System.IO;

namespace ArrowGame.Common.Packets.Server {
	public class ServerArrowSpawnPacket : IPacket {
		public PacketType Type => PacketType.ServerArrowSpawn;

		public float X { get; }

		public ServerArrowSpawnPacket(float x) {
			X = x;
		}

		public ServerArrowSpawnPacket(BinaryReader reader) {
			X = reader.ReadSingle();
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(X);
		}

		public override string ToString() {
			return $"{nameof(ServerArrowSpawnPacket)} {{ {nameof(X)}: {X} }}";
		}
	}
}
