using System.IO;

namespace ArrowGame.Common {
	public interface IPacket {
		public PacketType Type { get; }
		public void Serialize(BinaryWriter writer);
	}
}
