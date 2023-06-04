using System;
using System.IO;
using ArrowGame.Common.Extensions;

namespace ArrowGame.Common.Packets.Client {
	public class ClientPingPacket : IPacket {
		public PacketType Type => PacketType.ClientPing;

		public Guid ClientId { get; }

		public ClientPingPacket(Guid clientId) {
			ClientId = clientId;
		}

		public ClientPingPacket(BinaryReader reader) {
			ClientId = reader.ReadGuid();
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(ClientId);
		}

		public override string ToString() {
			return $"{nameof(ClientPingPacket)} {{ {nameof(ClientId)}: {ClientId} }}";
		}
	}
}
