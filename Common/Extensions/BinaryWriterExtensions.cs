﻿using System;
using System.IO;

namespace ArrowGame.Common.Extensions {
	public static class BinaryWriterExtensions {
		public static void Write(this BinaryWriter writer, Guid guid) {
			writer.Write(guid.ToByteArray());
		}

		public static void Write(this BinaryWriter writer, IPacket packet) {
			writer.Write((byte)packet.Type);
			packet.Serialize(writer);
			writer.Flush();
		}
	}
}
