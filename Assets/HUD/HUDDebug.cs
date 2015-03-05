using UnityEngine;
using System.Collections;

public class HUDDebug : MonoBehaviour {
	public float update_interval = 0.5f;
	public int number_of_players = 0;
	
	private float fps_accum = 0;
	private int frames = 0;
	private float fps_time_left;
	private string fps_string = "";
	private float fps = 0;
	// Use this for initialization
	void Start () {
		fps_time_left = update_interval;
	}
	
	// Update is called once per frame
	void Update () {
		fps_time_left -= Time.deltaTime;
		
		fps_accum += Time.timeScale/Time.deltaTime;
		
		++frames;
		
		if(fps_time_left <= 0.0) {
			fps = fps_accum/frames;
			fps_string = System.String.Format("{0:F2} FPS", fps);
			
			fps_time_left = update_interval;
			fps_accum = 0.0f;
			frames = 0;
		}
	}
	
	void OnGUI() {
		GUI.Label(new Rect(Screen.width * 0.90f, Screen.height * 0.05f, 500f, 20f), fps_string);
		if(PhotonNetwork.room != null) {
			GUI.Label(new Rect(Screen.width * 0.90f, Screen.height * 0.09f, 500f, 20f), System.String.Format("{0:0} Players", PhotonNetwork.room.playerCount));
		} else {
			GUI.Label(new Rect(Screen.width * 0.90f, Screen.height * 0.09f, 500f, 20f), System.String.Format("Waiting to join room"));
		}
		GUI.Label(new Rect(Screen.width * 0.90f, Screen.height * 0.13f, 500f, 20f), System.String.Format("{0:F2}x : {0:F2} y", Input.mousePosition.x, Input.mousePosition.y));
		GUI.Label(new Rect(Screen.width * 0.90f, Screen.height * 0.17f, 500f, 20f), System.String.Format("{0:F2}x : {0:F2} y", 
			Camera.main.transform.position.x, Camera.main.transform.position.y));
		GUI.Label(new Rect(Screen.width * 0.90f, Screen.height * 0.21f, 500f, 20f), System.String.Format("{0:0} Ping", PhotonNetwork.GetPing()));
	}
}
