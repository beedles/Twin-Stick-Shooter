using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Matchmaker : NetworkBehaviour {
	public int max_enemies = 5;
	public float spawn_rate = 2f;
	public bool spawn_enemies = true;
	
	private float last_spawn_time = 0.0f;
	public float total_enemies = 0.0f;
	
	public GameObject[] enemies;
	
	private enum SPAWN_MODE{PLAYER,
							SET_X_Y};

	void Start () {
		
	}
	
	public override void OnStartServer() {
		spawn_enemies = true;
	}

	void Update () {
		if(isServer && spawn_enemies) {
			Enemy_Spawner();
		}
	}
	
	private void Enemy_Spawner() {
		if(total_enemies < max_enemies) {
			last_spawn_time += Time.deltaTime;
			if(last_spawn_time > spawn_rate) {
				total_enemies++;
				last_spawn_time = 0.0f;
				GameObject enemy = (GameObject)Instantiate(enemies[0], Calculate_Spawn_Point(SPAWN_MODE.PLAYER, 0, 0), Quaternion.identity);
				NetworkServer.Spawn(enemy);
			}
		}
	}
	
	private Vector3 Calculate_Spawn_Point(SPAWN_MODE spawn, int x, int y) {
		Vector3 spawn_point = new Vector3(0,0,0);
		switch(spawn) {
			case SPAWN_MODE.PLAYER:
				spawn_point = Spawn_Ahead_Player();
				break;
			case SPAWN_MODE.SET_X_Y:
				spawn_point = new Vector3 (x,y,0);
				break;
			default:
				break;
		}		
		return spawn_point;
	}
	
	private GameObject Find_Random_Player() {
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		if(players.Length > 0) {
			return players[Random.Range(0, players.Length)];
		} else {
			return null;
		}
	}
	
	//Spawn Functions
	private Vector3 Spawn_Ahead_Player() {
		//Find Random Player
		GameObject random_player = Find_Random_Player();
		if(random_player == null) {
			return new Vector3(0,0,0);
		}
		Vector3 player_position = random_player.transform.position;
		int player_direction = random_player.gameObject.GetComponent<Player>().movement_direction;
		if(player_direction == 0) {
			return player_position + new Vector3(0, -12.5f, 0);
		}
		if(player_direction == 1) {
			return player_position + new Vector3(-27.5f, 0, 0);
		}
		if(player_direction == 2) {
			return player_position + new Vector3(0, 12.5f, 0);
		}
		if(player_direction == 3) {
			return player_position + new Vector3(27.5f, 0, 0);
		}
		return new Vector3(0,0,0);
	}
}
