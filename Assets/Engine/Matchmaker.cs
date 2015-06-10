using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Matchmaker : NetworkManager {
	public int max_enemies = 5;
	public float spawn_rate = 2f;
	public bool spawn_enemies = false;
	
	private float last_spawn_time = 0.0f;
	public float total_enemies = 0.0f;
	
	public GameObject[] enemies;
	public GameObject player_hud;
	
	public bool is_alive = false;
	public bool in_game = false;
	public bool mode_set = false;
	
	public GUISkin overlay_skin;
	
	private enum SPAWN_MODE{PLAYER,
							SET_X_Y};

	void Start () {
		
	}

	void Update () {

	}
	
	void OnGUI() {
		if(!is_alive && in_game) {
			GUI.Label(new Rect(Screen.width * 0.50f - 150f, Screen.height * 0.50f, 300f, 20f), System.String.Format("You Are Dead!"));	
			GUI.Label(new Rect(Screen.width * 0.50f - 150f, Screen.height * 0.54f, 300f, 20f), System.String.Format("Press 'R' to Respawn"));	
		}
	}
	
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
		GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
		is_alive = true;
	}
	
	void OnJoinedRoom() {
		in_game = true;
		HUDDebug debug_hud = gameObject.GetComponent<HUDDebug>();
		LoadWorld load_chunk = Camera.main.GetComponent<LoadWorld>();
		load_chunk.do_map = true;
		//GameObject.Find("Plane").GetComponent<Renderer>().enabled = true;
		//Spawn_Player();
		spawn_enemies = true;
		debug_hud.number_of_players++;
		//Show the player HUD
		if(player_hud != null) {
			player_hud.transform.FindChild("HUD Canvas").GetComponent<Canvas>().enabled = true;
		}
	}
	
	void OnLeaveRoom() {
		HUDDebug debug_hud = gameObject.GetComponent<HUDDebug>();
		debug_hud.number_of_players--;
	}
	
	private void Enemy_Spawner() {
		//GameObject spawnedEnemy;
		if(total_enemies < max_enemies) {
			last_spawn_time += Time.deltaTime;
			if(last_spawn_time > spawn_rate) {
				total_enemies++;
				last_spawn_time = 0.0f;
				//PhotonNetwork.Instantiate(enemies[Random.Range(0, enemies.GetLength(0))].name, Calculate_Spawn_Point(SPAWN_MODE.PLAYER, 0, 0), Quaternion.identity, 0);
				//Debug.Log(PhotonNetwork.playerName + " spawned a mob");
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
