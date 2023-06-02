using System.IO;

namespace ArrowGame.Common.Packets.Server {
	public class ServerArrowSpawnPacket : IPacket {
		public PacketType Type => PacketType.ServerArrowSpawn;

		public float X { get; }
		public float Speed { get; }

		public ServerArrowSpawnPacket(float x, float speed) {
			X = x;
			Speed = speed;
		}

		public ServerArrowSpawnPacket(BinaryReader reader) {
			X = reader.ReadSingle();
			Speed = reader.ReadSingle();
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(X);
			writer.Write(Speed);
		}

		public override string ToString() {
			return $"{nameof(ServerArrowSpawnPacket)} {{ {nameof(X)}: {X}, {nameof(Speed)}: {Speed}}}";
		}
	}
}
