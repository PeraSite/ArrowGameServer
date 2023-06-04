using System.Net;
using System.Net.Sockets;
using ArrowGame.Common;
using ArrowGame.Common.Extensions;
using ArrowGame.Common.Packets.Client;
var client = new TcpClient();
client.Connect(IPAddress.Loopback, 9000);

var stream = client.GetStream();
var reader = new BinaryReader(stream);
var writer = new BinaryWriter(stream);

var listenThread = new Thread(ListenThread);
listenThread.Start();

while (true) {
	Console.ReadLine();
	var sendPacket = new ClientPingPacket(Guid.NewGuid());
	writer.Write(sendPacket);
}

void ListenThread() {
	while (client.Connected) {
		var packetId = stream.ReadByte();
		if (packetId == -1) break;

		var packetType = (PacketType)packetId;
		var packet = packetType.CreatePacket(reader);

		Console.WriteLine($"[C -> S] {packet}");
	}
}
