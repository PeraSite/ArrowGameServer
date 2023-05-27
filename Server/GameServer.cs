using System.Net;
using System.Net.Sockets;

namespace ArrowGame.Server;

public class GameServer : IDisposable {
	private readonly TcpListener _server;

	public GameServer(int listenPort) {
		_server = new TcpListener(IPAddress.Any, listenPort);
	}

	public void Start() {
		Console.WriteLine($"[TCP 서버] 서버 시작@{_server.LocalEndpoint}");
		_server.Start();
	}

	public void Dispose() {
		Console.WriteLine("[TCP 서버] 서버 종료");
		_server.Stop();
		GC.SuppressFinalize(this);
	}
}
