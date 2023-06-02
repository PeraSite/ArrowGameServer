using System;
using System.Collections.Generic;
using System.Threading;
using ArrowGame.Common;
using ArrowGame.Common.Packets.Server;

namespace ArrowGame.Server;

public class Room {
	private const int MAX_PLAYER = 1;
	private const int ARROW_SPAWN_DELAY = 1000;
	private const float ARROW_SPAWN_MIN_X = -8f;
	private const float ARROW_SPAWN_MAX_X = 8f;
	private const float ARROW_SPAWN_MIN_SPEED = 2.5f;
	private const float ARROW_SPAWN_MAX_SPEED = 6f;
	private const int ARROW_DAMAGE = 10;
	private const int DEFAULT_HEALTH = 100;

	public int Id { get; }
	public RoomState State {
		get => _state;
		private set {
			_state = value;
			BroadcastState();
		}
	}
	public Dictionary<PlayerConnection, int> PlayerIds { get; private set; }
	public Dictionary<int, int> PlayerHP { get; private set; }

	private RoomState _state;
	private int _lastId;
	private Thread? _roomThread;

	public Room(int id) {
		Id = id;
		PlayerIds = new Dictionary<PlayerConnection, int>();
		PlayerHP = new Dictionary<int, int>();
		_state = RoomState.Waiting;
	}

	public bool IsEmpty() => PlayerIds.Count == 0;
	public bool IsAvailable() => PlayerIds.Count < MAX_PLAYER && State == RoomState.Waiting;
	public bool IsFull() => PlayerIds.Count == MAX_PLAYER;
	public int GetNewPlayerId() => _lastId++;

	public override string ToString() {
		return $"Room {Id} ({PlayerIds.Count}/{MAX_PLAYER})";
	}

	public void AddPlayer(PlayerConnection playerConnection, int playerId) {
		if (IsFull()) {
			throw new Exception("Can't add player to full room!");
		}
		BroadcastPacket(new ServerRoomJoinPacket(playerId));

		// 기존에 사람이 있었다면
		if (PlayerIds.Count > 0) {
			foreach (var existId in PlayerIds.Values) {
				playerConnection.SendPacket(new ServerRoomJoinPacket(existId));
			}
		}

		PlayerIds[playerConnection] = playerId;
		PlayerHP[playerId] = DEFAULT_HEALTH;
		BroadcastState();

		if (IsFull()) {
			StartGame();
		}
	}

	public void RemovePlayer(PlayerConnection playerConnection) {
		var id = PlayerIds[playerConnection];
		PlayerIds.Remove(playerConnection);
		BroadcastPacket(new ServerRoomQuitPacket(id));
		BroadcastState();

		if (!IsFull()) {
			// 만약 누가 나갔는데 2명이 되지 않는다면(미래 대응) 게임 종료
			StopGame();
		}
	}

	private void StartGame() {
		State = RoomState.Playing;

		_roomThread = new Thread(() => {
			while (State == RoomState.Playing) {
				SpawnArrow();
				Thread.Sleep(ARROW_SPAWN_DELAY);
			}
		});
		_roomThread.Start();
	}

	private void SpawnArrow() {
		var x = Random(ARROW_SPAWN_MIN_X, ARROW_SPAWN_MAX_X);
		var speed = Random(ARROW_SPAWN_MIN_SPEED, ARROW_SPAWN_MAX_SPEED);

		BroadcastPacket(new ServerArrowSpawnPacket(x, speed));
		Console.WriteLine($"[TCP 서버] Room {Id}: Spawn arrow at {x} with speed {speed}");
	}

	private static float Random(float min, float max) {
		return new Random().NextSingle() * (max - min) + min;
	}

	private void StopGame() {
		State = RoomState.Waiting;
		_roomThread = null;
	}

	private void BroadcastState() => BroadcastPacket(new ServerRoomStatusPacket(Id, State, PlayerIds.Count, PlayerHP));

	private void BroadcastPacket(IPacket packet) {
		foreach (PlayerConnection connection in PlayerIds.Keys) {
			connection.SendPacket(packet);
		}
	}
	public void HitByArrow(int playerId) {
		PlayerHP[playerId] -= ARROW_DAMAGE;
		BroadcastState();

		if (PlayerHP[playerId] <= 0) {
			// BroadcastPacket(new ServerPlayerDiePacket(playerId));
			StopGame();
		}
	}
}
