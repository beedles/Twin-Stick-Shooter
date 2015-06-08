using UnityEngine;
using System.Collections;

public class Matchmaker : MonoBehaviour {
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

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(PhotonNetwork.isMasterClient && spawn_enemies) {
			Enemy_Spawner();			
		}
		if(!is_alive && in_game && Input.GetKeyDown(KeyCode.R)) {
			Spawn_Player();
		}
	}
	
	void OnGUI() {
		if(!in_game && !mode_set) {
			Do_Overlay();
			
			if(GUI.Button(new Rect(6*Screen.width/16, Screen.height/2, Screen.width/8, Screen.height/8), "Online")) {
				PhotonNetwork.ConnectUsingSettings("0.1");
				mode_set = true;
			}
			if(GUI.Button(new Rect(8*Screen.width/16, Screen.height/2, Screen.width/8, Screen.height/8), "Offline")) {
				PhotonNetwork.offlineMode = true;
				PhotonNetwork.CreateRoom("Offline Room");
				mode_set = true;
			}
		}
		//GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
		if(!is_alive && in_game) {
			GUI.Label(new Rect(Screen.width * 0.50f - 150f, Screen.height * 0.50f, 300f, 20f), System.String.Format("You Are Dead!"));	
			GUI.Label(new Rect(Screen.width * 0.50f - 150f, Screen.height * 0.54f, 300f, 20f), System.String.Format("Press 'R' to Respawn"));	
		}
	}
	
	void Do_Overlay() {
		GUI.skin = overlay_skin;
		GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
		GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
		GUI.EndGroup();
	}
	
	void OnJoinedLobby() {
		//Show Login Screen
		gameObject.GetComponent<Login>().Show_Login_Screen();	
		
	}
	
	void OnPhotonRandomJoinFailed() {
		//Debug.Log("Can't join random room, making a new one");
		PhotonNetwork.CreateRoom(null);
	}
	
	void Spawn_Player() {
		PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
		is_alive = true;
	}
	
	void OnJoinedRoom() {
		in_game = true;
		HUDDebug debug_hud = gameObject.GetComponent<HUDDebug>();
		LoadWorld load_chunk = Camera.main.GetComponent<LoadWorld>();
		load_chunk.do_map = true;
		//GameObject.Find("Plane").GetComponent<Renderer>().enabled = true;
		Spawn_Player();
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
				PhotonNetwork.Instantiate(enemies[Random.Range(0, enemies.GetLength(0))].name, Calculate_Spawn_Point(SPAWN_MODE.PLAYER, 0, 0), Quaternion.identity, 0);
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
