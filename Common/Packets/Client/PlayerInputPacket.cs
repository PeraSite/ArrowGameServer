using System.IO;

namespace ArrowGame.Common.Packets.Client {
	public struct PlayerInputPacket : IPacket {
		public PacketType Type => PacketType.PlayerInput;

		public InputState State;

		public PlayerInputPacket(InputState state) {
			State = state;
		}

		public PlayerInputPacket(BinaryReader reader) {
			State = new InputState {
				HorizontalMovement = reader.ReadSingle()
			};
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(State.HorizontalMovement);
		}
	}
}
