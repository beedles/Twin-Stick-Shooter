using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Player : Unit {
	private int experience_required_per_level = 20;
	public float camera_height = -40.0f;

	private Vector3 temp_vector;
	private Vector3 temp_vector2;
	public int movement_direction = 0;
	
	//Statistics
	private int experience_have = 0;
	private int experience_needed = 0;
	
	//HUD container
	private GameObject hud;
	
	//HUD canvas's
	private GameObject hud_character_canvas;
	private GameObject hud_skills_canvas;
	private GameObject hud_canvas;
	
	//HUD Panels
	private GameObject hud_character_panel;
	private GameObject hud_character_details;
	private GameObject hud_bottom;
	
	protected override void Awake() {
		int player_level = 0;	
		player_level = Get_Player_Level();
		
		Set_Level(player_level);
		experience_needed = player_level * experience_required_per_level;
		//Set up statistics based on the class we chose and level
		stamina = 5;
		
		//Set up stat helpers
		health_per_stamina = 10;
	}
	
	protected override void Start() {	
		
		base.Start();
		gameObject.tag="Player";
		is_a_player = true;		
		show_name = true;
		/*if(!PhotonNetwork.offlineMode) {
			unit_name = GetComponent<PhotonView>().owner.name;
		} else {
			unit_name = "Offline_Player";
		}	*/	
		
		if(isLocalPlayer) {
			hud = GameObject.Find("HUD");
			hud_character_canvas = hud.transform.FindChild("Character Canvas").gameObject;
			hud_character_panel = hud_character_canvas.transform.FindChild("Character Panel").gameObject;
			hud_character_details = hud_character_canvas.transform.FindChild("Character Details").gameObject;
			
			hud_skills_canvas = hud.transform.FindChild("Skills Canvas").gameObject;
			
			hud_canvas = hud.transform.FindChild("HUD Canvas").gameObject;
			hud_bottom = hud_canvas.transform.FindChild("Bottom Panel").gameObject;
			
			//Set the player name on HUD
			hud_character_details.transform.FindChild("Player Name").gameObject.GetComponent<Text>().text = unit_name;
			//Set the player class on HUD
			hud_character_details.transform.FindChild("Player Class").gameObject.GetComponent<Text>().text = "Onion Knight";
			//Default the player experience
			hud_character_details.transform.FindChild("Player Experience").gameObject.GetComponent<Text>().text = 
				"Experience " + experience_have + "/" + experience_needed;
		}
	}
	
	protected override void Update() {
		base.Update();
		if(isLocalPlayer) {
			Find_Input();
			Process_Movement();
			Handle_Camera();
			//Debug.Log(rigidbody.velocity);
			Update_Player_HUD();
		}
	}
	
	protected override void Find_Input() {
		temp_vector2 = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);		
		temp_vector = Input.mousePosition;		
		//temp_vector.y = temp_vector2.y;					
		
		input_rotation = temp_vector - temp_vector2;
		if(Input.GetMouseButton(0)) {			
			if(!isServer) {
				CmdSend_Fire(input_rotation);				
			} else {
				RpcSend_Fire(input_rotation);
			}
			Shoot (input_rotation);
		}
		if(Input.GetKeyUp(KeyCode.C)) {
			hud_character_canvas.GetComponent<Canvas>().enabled = !hud_character_canvas.GetComponent<Canvas>().enabled;
		}
		if(Input.GetKeyUp(KeyCode.B)) {
			hud_skills_canvas.GetComponent<Canvas>().enabled = !hud_skills_canvas.GetComponent<Canvas>().enabled;
		}
	}

	protected override void Process_Movement () {
		base.Process_Movement();
		Rigidbody rigid_body = GetComponent<Rigidbody>();
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
			rigid_body.MovePosition(rigid_body.position + Vector3.right * -movement_speed / 50);
			GetComponent<Rigidbody>().velocity += Vector3.right * -movement_speed * Time.deltaTime;
			movement_direction = 1;
		}
		if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
			rigid_body.MovePosition(rigid_body.position + Vector3.right * movement_speed / 50);
			GetComponent<Rigidbody>().velocity += Vector3.right * movement_speed * Time.deltaTime;
			movement_direction = 3;
		}	
		if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
			rigid_body.MovePosition(rigid_body.position + Vector3.up * movement_speed / 50);
			GetComponent<Rigidbody>().velocity += Vector3.up * movement_speed * Time.deltaTime;
			movement_direction = 2;
		}	
		if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
			rigid_body.MovePosition(rigid_body.position + Vector3.up * -movement_speed / 50);
			GetComponent<Rigidbody>().velocity += Vector3.up * -movement_speed * Time.deltaTime;
			movement_direction = 0;
		}			
		//Fix diagonal movement being faster than up/down/left/right
		if(Mathf.Abs(GetComponent<Rigidbody>().velocity.x) > 0 && Mathf.Abs(GetComponent<Rigidbody>().velocity.y) > 0) {
			float vel_x = Mathf.Sqrt(Mathf.Abs(GetComponent<Rigidbody>().velocity.x));
			float vel_y = vel_x;
			if(GetComponent<Rigidbody>().velocity.x < 0) {
				vel_x *= -1;
			}
			if(GetComponent<Rigidbody>().velocity.y < 0) {
				vel_y *= -1;
			}
			GetComponent<Rigidbody>().velocity += new Vector3(vel_x, vel_y, 0) * Time.deltaTime;
		}	
	}	
	
	void Handle_Camera() {
		Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, camera_height);
		Camera.main.transform.eulerAngles = new Vector3(0, 0, 0);
	}
	
	private void Update_Player_HUD() {
	}	
	
	private void Update_Player_Experience() {
		//Update Experience
		hud_character_details.transform.FindChild("Player Experience").gameObject.GetComponent<Text>().text = 
			"Experience " + experience_have + "/" + experience_needed;
	}
	
	//Get the players level from server
	private int Get_Player_Level() {
		return 1;
	}	
	
	public void Collect_Experience(int amount) {
		//photonView.RPC ("Add_Experience", PhotonTargets.All, amount);
	}
	
	private void Level_Up() {
		this.Set_Level(Get_Level + 1);
		//Set new experience needed
		this.experience_needed += Get_Level * experience_required_per_level;
		
		//increase damage
		this.damage_min += 2;
		this.damage_max += 2;
		this.damage_modifier++;
		
		//Increase stats based on class stat gain
		stamina += 1;
		
		//Update health and max health
		health = stamina * health_per_stamina;
		max_health = health;
		Debug.Log(health + "/" + max_health);
	}
	
	[Command]
	void CmdSend_Fire(Vector3 input_rotation) {
		Shoot (input_rotation);
	}
	
	[ClientRpc]
	void RpcSend_Fire(Vector3 input_rotation) {
		Shoot (input_rotation);
	}
	
	//[RPC]
	void Do_Hit(float damage) {
		On_Hit(damage);
	}
	
	//[RPC]
	void Add_Experience(int amount){
		this.experience_have += amount;
		//Check if we have leveled up
		if(this.experience_have >= this.experience_needed) {
			Level_Up();			
		}
		/*if(photonView.isMine) {
			Update_Player_Experience();
		}*/
	}
}
