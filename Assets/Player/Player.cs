using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : Unit {
	private int experience_required_per_level = 20;
	public float camera_height = -40.0f;

	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;
	private Vector3 temp_vector;
	private Vector3 temp_vector2;
	private bool spawned = false;
	public int movement_direction = 0;
	
	//Statistics
	private int experience_have = 0;
	private int experience_needed = 0;
	
	private GameObject hud;
	private GameObject hud_side;
	private GameObject hud_bottom;
	
	protected override void Start() {	
		int player_level = 0;	
		base.Start();
		gameObject.tag="Player";
		is_a_player = true;		
		show_name = true;
		if(!PhotonNetwork.offlineMode) {
			unit_name = GetComponent<PhotonView>().owner.name;
		} else {
			unit_name = "Offline_Player";
		}		
		player_level = Get_Player_Level();
		
		Set_Level(player_level);
		experience_needed = player_level * experience_required_per_level;
		if(photonView.isMine) {
			hud = GameObject.Find("HUD");
			hud_side = hud.transform.FindChild("Side Panel").gameObject;
			hud_bottom = hud.transform.FindChild("Bottom Panel").gameObject;
			//Set the player name on HUD
			hud_side.transform.FindChild("Player Name").gameObject.GetComponent<Text>().text = unit_name;
			//Set the player class on HUD
			hud_side.transform.FindChild("Player Class").gameObject.GetComponent<Text>().text = "Onion Knight";
			//Default the player experience
			hud_side.transform.FindChild("Player Experience").gameObject.GetComponent<Text>().text = 
				"Experience " + experience_have + "/" + experience_needed;
		}
	}
	
	protected override void Update() {
		base.Update();
		if(photonView.isMine) {
			Find_Input();
			Process_Movement();
			Handle_Camera();
			//Debug.Log(rigidbody.velocity);
			Update_Player_HUD();
		} else {
			SyncedMovement();
		}
	}
	
	protected override void Find_Input() {
		if(photonView.isMine) {
			
			temp_vector2 = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);		
			temp_vector = Input.mousePosition;		
			//temp_vector.y = temp_vector2.y;					
			
			input_rotation = temp_vector - temp_vector2;
			if(Input.GetMouseButton(0)) {
				photonView.RPC("Send_Fire", PhotonTargets.All, input_rotation);
			}
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
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if(stream.isWriting) {
			stream.SendNext(GetComponent<Rigidbody>().position);
			stream.SendNext(GetComponent<Rigidbody>().velocity);
		} else {
			Vector3 syncPosition = (Vector3)stream.ReceiveNext();
			Vector3 syncVelocity = (Vector3)stream.ReceiveNext();
			syncEndPosition = syncPosition + syncVelocity * syncDelay;
			if(!spawned) {
				syncStartPosition = syncEndPosition;
				spawned = true;
			} else {
				syncStartPosition = GetComponent<Rigidbody>().position;
			}
			
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
			//Debug.Log("velocity" + syncVelocity);
		}
	}
	
	private void Update_Player_HUD() {
	}	
	
	private void Update_Player_Experience() {
		//Update Experience
		hud_side.transform.FindChild("Player Experience").gameObject.GetComponent<Text>().text = 
			"Experience " + experience_have + "/" + experience_needed;
	}
	
	private void SyncedMovement() {
		syncTime += Time.deltaTime;
		GetComponent<Rigidbody>().position = Vector3.MoveTowards(syncStartPosition, syncEndPosition, movement_speed * syncTime);
		/*Debug.Log(rigidbody.position);
		Debug.Log(syncTime);
		Debug.Log(syncDelay);
		Debug.Log(syncTime/syncDelay);*/
	}
	
	//Get the players level from server
	private int Get_Player_Level() {
		return 1;
	}	
	
	public void Collect_Experience(int amount) {
		photonView.RPC ("Add_Experience", PhotonTargets.All, amount);
	}
	
	private void Level_Up() {
		this.Set_Level(Get_Level + 1);
		//Set new experience needed
		this.experience_needed += Get_Level * experience_required_per_level;
		
		//increase damage
		this.damage_min += 2;
		this.damage_max += 2;
		this.damage_modifier++;
	}
	
	[RPC]
	void Send_Fire(Vector3 input_rotation) {
		Shoot (input_rotation);
	}
	
	[RPC]
	void Do_Hit(float damage) {
		On_Hit(damage);
	}
	
	[RPC]
	void Add_Experience(int amount){
		this.experience_have += amount;
		//Check if we have leveled up
		if(this.experience_have >= this.experience_needed) {
			Level_Up();			
		}
		if(photonView.isMine) {
			Update_Player_Experience();
		}
	}
}
