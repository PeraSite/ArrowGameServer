using System;
using System.IO;
using ArrowGame.Common.Packets.Client;
using ArrowGame.Common.Packets.Server;

namespace ArrowGame.Common {
	public enum PacketType : byte {
		ClientPing,
		ServerPong,

		PlayerInput,
	}

	public static class PacketTypes {
		public static IPacket CreatePacket(this PacketType type, BinaryReader reader) {
			return type switch {
				PacketType.ClientPing => new ClientPingPacket(),
				PacketType.ServerPong => new ServerPongPacket(reader),

				PacketType.PlayerInput => new PlayerInputPacket(reader),

				_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
			};
		}
	}
}
