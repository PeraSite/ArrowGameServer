using System;
using System.IO;
using ArrowGame.Common.Packets.Client;
using ArrowGame.Common.Packets.Server;

namespace ArrowGame.Common {
	public enum PacketType : byte {
		ClientPing,

		PlayerInput,

		ServerRoomStatus,
		ServerAssignPlayerId,
		ServerRoomJoin,
		ServerRoomQuit,

		ServerArrowSpawn
	}

	public static class PacketTypes {
		public static IPacket CreatePacket(this PacketType type, BinaryReader reader) {
			return type switch {
				PacketType.ClientPing => new ClientPingPacket(),

				PacketType.PlayerInput => new PlayerInputPacket(reader),

				PacketType.ServerRoomStatus => new ServerRoomStatusPacket(reader),
				PacketType.ServerAssignPlayerId => new ServerAssignPlayerIdPacket(reader),
				PacketType.ServerRoomJoin => new ServerRoomJoinPacket(reader),
				PacketType.ServerRoomQuit => new ServerRoomQuitPacket(reader),
				PacketType.ServerArrowSpawn => new ServerArrowSpawnPacket(reader),
				_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
			};
		}
	}
}
