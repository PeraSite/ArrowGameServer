using System.IO;

namespace ArrowGame.Common.Packets.Client {
	public class PlayerInputPacket : IPacket {
		public PacketType Type => PacketType.PlayerInput;

		public int PlayerId { get; }
		public InputState State { get; }

		public PlayerInputPacket(int playerId, InputState state) {
			PlayerId = playerId;
			State = state;
		}

		public PlayerInputPacket(BinaryReader reader) {
			PlayerId = reader.ReadInt32();
			State = new InputState {
				HorizontalMovement = reader.ReadSingle()
			};
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(PlayerId);
			writer.Write(State.HorizontalMovement);
		}

		public override string ToString() {
			return $"{nameof(PlayerInputPacket)} {{ {nameof(PlayerId)}: {PlayerId}, {nameof(State)}: {State} }}";
		}
	}
}
