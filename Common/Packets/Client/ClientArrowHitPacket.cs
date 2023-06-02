using System.IO;

namespace ArrowGame.Common.Packets.Client {
	public class ClientArrowHitPacket : IPacket {
		public PacketType Type => PacketType.ClientArrowHit;

		public int PlayerId { get; }

		public ClientArrowHitPacket(int playerId) {
			PlayerId = playerId;
		}

		public ClientArrowHitPacket(BinaryReader reader) {
			PlayerId = reader.ReadInt32();
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(PlayerId);
		}

		public override string ToString() {
			return $"{nameof(ClientArrowHitPacket)} {{ {nameof(PlayerId)}: {PlayerId} }}";
		}
	}
}
