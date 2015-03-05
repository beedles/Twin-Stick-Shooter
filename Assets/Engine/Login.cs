using UnityEngine;
using System.Collections;

public class Login : MonoBehaviour {
	private string player_name = string.Empty;	
	private Rect login_window = new Rect(0,0,Screen.width,Screen.height);
	
	public bool show_login = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		if(show_login) {
			GUI.Window(0, login_window, Login_Window, "Enter Player Name");
		}
	}
	
	void Login_Window(int window_id) {
		player_name = GUI.TextField(new Rect(Screen.width/3, Screen.height/2, Screen.width/3, Screen.height/10), player_name, 10);
		
		if(GUI.Button(new Rect(Screen.width/2, 4 * Screen.height/5, Screen.width/8, Screen.height/8), "Login")) {
			if(player_name == "") {
				Debug.Log ("Please Enter a Name");
			} else {
				PhotonNetwork.playerName = player_name;
				PhotonNetwork.JoinRandomRoom();
				show_login = false;
			}
		}
		GUI.Label(new Rect(Screen.width/3, 35 * Screen.height/100, Screen.width/5, Screen.height/8), "Player Name");
	}
	
	public void Show_Login_Screen() {
		show_login = true;
	}
}