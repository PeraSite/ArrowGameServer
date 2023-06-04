using System;
using System.Text;

namespace ArrowGame.Server;

internal static class Program {
	private static void Main() {
		// 콘솔 입출력 한글 깨짐 수정
		Console.InputEncoding = Encoding.Unicode;
		Console.OutputEncoding = Encoding.Unicode;

		// 환경변수 가져오기
		var tcpPort = int.Parse(GetEnvironmentVariable("TCP_PORT"));
		var udpPort = int.Parse(GetEnvironmentVariable("UDP_PORT"));

		// 서버 시작
		using GameServer server = new GameServer(tcpPort, udpPort);
		server.Start();
	}

	private static string GetEnvironmentVariable(string key)
		=> Environment.GetEnvironmentVariable(key) ?? throw new Exception($"{key} is not set");
}
