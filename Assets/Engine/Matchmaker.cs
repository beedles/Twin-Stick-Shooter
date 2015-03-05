using UnityEngine;
using System.Collections;

public class Matchmaker : MonoBehaviour {
	public int max_enemies = 5;
	public float spawn_rate = 2f;
	public bool spawn_enemies = false;
	
	private float last_spawn_time = 0.0f;
	public float total_enemies = 0.0f;
	
	public GameObject[] enemies;

	// Use this for initialization
	void Start () {
		PhotonNetwork.ConnectUsingSettings("0.1");
		//PhotonNetwork.logLevel = PhotonLogLevel.Full;
	}
	
	// Update is called once per frame
	void Update () {
		if(PhotonNetwork.isMasterClient && spawn_enemies) {
			Enemy_Spawner();			
		}
	}
	
	void OnGUI() {
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}
	
	void OnJoinedLobby() {
		//Show Login Screen
		gameObject.GetComponent<Login>().Show_Login_Screen();	
		
	}
	
	void OnPhotonRandomJoinFailed() {
		Debug.Log("Can't join random room, making a new one");
		PhotonNetwork.CreateRoom(null);
	}
	
	void OnJoinedRoom() {
		HUDDebug player_hud = gameObject.GetComponent<HUDDebug>();
		GameObject.Find("Plane").GetComponent<Renderer>().enabled = true;
		PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
		spawn_enemies = true;
		player_hud.number_of_players++;
	}
	
	private void Enemy_Spawner() {
		//GameObject spawnedEnemy;
		if(total_enemies < max_enemies) {
			last_spawn_time += Time.deltaTime;
			if(last_spawn_time > spawn_rate) {
				total_enemies++;
				last_spawn_time = 0.0f;
				PhotonNetwork.Instantiate(enemies[Random.Range(0, enemies.GetLength(0))].name, Calculate_Spawn_Point(), Quaternion.identity, 0);
				//Debug.Log(PhotonNetwork.playerName + " spawned a mob");
			}
		}
	}
	
	private Vector3 Calculate_Spawn_Point() {
		Vector3 spawn_point;
		spawn_point = new Vector3 (1,1,0);
		return spawn_point;
	}
}
