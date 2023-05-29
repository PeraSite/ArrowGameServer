using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ArrowGame.Common;
using ArrowGame.Common.Packets.Client;
using ArrowGame.Common.Packets.Server;

namespace ArrowGame.Server;

public class GameServer : IDisposable {
	public List<PlayerConnection> PlayerConnections;

	private readonly TcpListener _tcpServer;
	private readonly ConcurrentQueue<(PlayerConnection playerConnection, IPacket packet)> _receivedPacketQueue;
	private readonly List<Room> _rooms;

	public GameServer(int port) {
		_tcpServer = new TcpListener(IPAddress.Any, port);
		PlayerConnections = new List<PlayerConnection>();
		_receivedPacketQueue = new ConcurrentQueue<(PlayerConnection playerConnection, IPacket packet)>();
		_rooms = new List<Room>();
	}

#region Basic client handling
	public void Start() {
		Console.WriteLine("[TCP 서버] 서버 시작");
		_tcpServer.Start();


		// 패킷 Dequeue Thread
		var dequeueThread = new Thread(() => {
			try {
				while (true) {
					if (_receivedPacketQueue.TryDequeue(out var tuple)) {
						var (playerConnection, packet) = tuple;

						// handle packet
						HandlePacket(packet, playerConnection);
					}
				}
			}
			catch (Exception e) {
				Console.WriteLine(e);
				throw;
			}
		});
		dequeueThread.Start();

		try {
			// 서버가 켜진 동안 클라이언트 접속 받기
			while (true) {
				// 새로운 TCP 클라이언트 접속 받기
				var client = _tcpServer.AcceptTcpClient();

				// 새 스레드에서 클라이언트 처리
				var thread = new Thread(() => HandleNewClient(client));
				thread.Start();
			}
		}
		catch (Exception e) {
			Console.WriteLine(e);
			throw;
		}
	}

	public void Dispose() {
		Console.WriteLine("[TCP 서버] 서버 종료");
		_tcpServer.Stop();
		GC.SuppressFinalize(this);
	}


	private void HandleNewClient(TcpClient client) {
		// PlayerConnection 생성
		var playerConnection = new PlayerConnection(client);
		PlayerConnections.Add(playerConnection);
		Console.WriteLine($"[TCP 서버] 클라이언트 접속: {playerConnection}");

		// 패킷 읽기
		try {
			while (client.Connected) {
				// 패킷 읽기
				var packet = playerConnection.ReadPacket();

				// 패킷 큐에 추가
				_receivedPacketQueue.Enqueue((playerConnection, packet));
			}
		}
		catch (IOException) {
			// 클라이언트가 강제 종료했을 때 처리
		}
		catch (Exception e) {
			Console.WriteLine(e);
			throw;
		}
		finally {
			// 클라이언트 접속 종료 처리
			HandleClientQuit(playerConnection);
		}
	}

	private void HandleClientQuit(PlayerConnection playerConnection) {
		var address = playerConnection.IP;

		// 접속하고 있던 방 나가기
		QuitRoom(playerConnection);

		// PlayerConnection Dictionary 에서 삭제
		PlayerConnections.Remove(playerConnection);

		// 클라이언트 닫기
		playerConnection.Dispose();

		Console.WriteLine("[TCP 서버] 클라이언트 종료: IP 주소={0}, 포트 번호={1}", address.Address, address.Port);
	}
  #endregion

#region Util Functions
	private void Broadcast(IPacket packet) {
		foreach (var playerConnection in PlayerConnections)
			playerConnection.SendPacket(packet);
	}

	private void Broadcast(IEnumerable<PlayerConnection> targets, IPacket packet) {
		foreach (var playerConnection in targets)
			playerConnection.SendPacket(packet);
	}
#endregion

#region Matchmaking
	private void QuitRoom(PlayerConnection playerConnection) {
		var room = _rooms.FirstOrDefault(x => x.PlayerIds.ContainsKey(playerConnection));

		if (room == null) return;

		room.RemovePlayer(playerConnection);
		if (room.IsEmpty()) {
			Console.WriteLine($"[TCP 서버] 방 {room.Id}이 비어서 삭제됨");
			DeleteRoom(room);
		}
	}

	private Room GetAvailableOrCreateRoom() {
		// 빈 방 찾기
		var availableRoom = _rooms.FirstOrDefault(x => x.IsAvailable());

		// 빈 방이 있으면 반환
		if (availableRoom != null) return availableRoom;

		// 없으면 새로 생성 후 추가
		var id = _rooms.Count;
		var newRoom = new Room(id);
		_rooms.Add(newRoom);

		Console.WriteLine($"[TCP 서버] 방 생성 - {newRoom}");
		return newRoom;
	}

	private void DeleteRoom(Room room) {
		Console.WriteLine($"[TCP 서버] 방 삭제: {room}");
		_rooms.Remove(room);
	}
#endregion

	private void HandlePacket(IPacket basePacket, PlayerConnection playerConnection) {
		switch (basePacket) {
			case ClientPingPacket packet: {
				HandleClientPingPacket(playerConnection, packet);
				break;
			}
			case PlayerInputPacket packet: {
				HandlePlayerInputPacket(playerConnection, packet);
				break;
			}
			default:
				throw new ArgumentOutOfRangeException(nameof(basePacket));
		}
	}

	private void HandleClientPingPacket(PlayerConnection playerConnection, ClientPingPacket packet) {
		var room = GetAvailableOrCreateRoom();

		var playerId = room.GetNewPlayerId();
		room.AddPlayer(playerConnection, playerId);
		playerConnection.SendPacket(new ServerAssignPlayerIdPacket(playerId));
	}

	private void HandlePlayerInputPacket(PlayerConnection playerConnection, PlayerInputPacket packet) {
		Broadcast(PlayerConnections.Where(x => x != playerConnection), packet);
	}
}
