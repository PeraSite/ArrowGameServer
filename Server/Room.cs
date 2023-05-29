using System;
using System.Collections.Generic;
using ArrowGame.Common;
using ArrowGame.Common.Packets.Server;

namespace ArrowGame.Server;

public class Room {
	private const int MAX_PLAYER = 2;
	private GameServer _server;
	private RoomState _state;

	public int Id { get; }

	public RoomState State {
		get => _state;
		private set {
			_state = value;
			BroadcastState();
		}
	}
	public List<PlayerConnection> PlayerConnections { get; private set; }

	public Room(GameServer server, int id) {
		_server = server;
		Id = id;
		PlayerConnections = new List<PlayerConnection>();
		_state = RoomState.Waiting;
	}

	public bool IsAvailable() => PlayerConnections.Count < MAX_PLAYER && State == RoomState.Waiting;
	public bool IsFull() => PlayerConnections.Count == MAX_PLAYER;

	public override string ToString() {
		return $"Room {Id} ({PlayerConnections.Count}/{MAX_PLAYER})";
	}

	public void AddPlayer(PlayerConnection playerConnection) {
		if (IsFull()) {
			throw new Exception("Can't add player to full room!");
		}
		PlayerConnections.Add(playerConnection);
		BroadcastState();
	}

	public void RemovePlayer(PlayerConnection playerConnection) {
		PlayerConnections.Remove(playerConnection);
		BroadcastState();
	}

	public void StartGame() {
		State = RoomState.Playing;
	}

	public void BroadcastState() => BroadcastPacket(new ServerRoomStatusPacket(Id, State, PlayerConnections.Count));

	public void BroadcastPacket(IPacket packet) {
		PlayerConnections.ForEach(x => x.SendPacket(packet));
	}
}
