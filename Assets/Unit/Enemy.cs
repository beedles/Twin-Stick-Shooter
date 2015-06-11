using UnityEngine;
using System.Collections;

public class Enemy : Unit {
	private Vector3 target_direction = Vector3.zero;	
	
	//AI Modes
	private enum AI_MODE{BASIC};
	AI_MODE AI_STATE = AI_MODE.BASIC;
	
	//Movement Modes
	private enum MOVEMENT_MODE{	MOVE_TOWARDS,
						MOVE_AWAY	};
	MOVEMENT_MODE MOVEMENT_STATE = MOVEMENT_MODE.MOVE_TOWARDS;

	protected override void Awake() {
		//Set up stats from enemy details
		stamina = 2;
		
		//Set up stat helpers
		health_per_stamina = 10;		
	}
	// Use this for initialization
	protected override void Start () {
		base.Start();
		gameObject.tag="Enemy";
		//Set the level of the enemy
		Determine_Level();
		show_name = true;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		Find_Input();
		Process_Movement();		
	}
	
	private void Determine_Level() {
		Set_Level(1);
	}
	
	protected override void Find_Input() {
		Enemy_AI();	
	}
	
	protected override void Process_Movement() {
		switch(MOVEMENT_STATE) {
			case(MOVEMENT_MODE.MOVE_TOWARDS):
				transform.position += target_direction * movement_speed * Time.deltaTime;
				break;
			case(MOVEMENT_MODE.MOVE_AWAY):
				transform.position -= target_direction * movement_speed * Time.deltaTime;
				break;
			default:
				break;
		}		
	}
	
	private void Enemy_AI() {
		switch(AI_STATE) {
			case(AI_MODE.BASIC):
				GameObject target = Get_Closest_Player(transform.position);
				float target_distance;
				if(target == null) {
					input_movement = Vector3.zero;
					input_rotation = transform.forward * -1;
					return;
				}
				target_distance =  Vector3.Distance(transform.position, target.transform.position);
				target_direction = target.transform.position - transform.position;
				input_rotation = target_direction;
				if(target_distance < 2) {
					MOVEMENT_STATE = MOVEMENT_MODE.MOVE_AWAY;
				} else if (target_distance > 3){
					MOVEMENT_STATE = MOVEMENT_MODE.MOVE_TOWARDS;
				}
				//photonView.RPC("Send_Fire", PhotonTargets.All, input_rotation);
				break;
			default:
				break;
		}
	}
	
	private GameObject Get_Closest_Player(Vector3 enemy_location) {
		GameObject closest_player = null;
		float closest_player_distance = Mathf.Infinity;
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject player in players) {
			float temp_distance = Vector3.Distance(enemy_location, player.transform.position);
			if(temp_distance < closest_player_distance) {
				closest_player = player;
				closest_player_distance = temp_distance;
			}
		}
		return closest_player;
	}
	
	//[RPC]
	void Do_Hit(float damage) {
		On_Hit(damage);		
	}
	
	//[RPC]
	void Send_Fire(Vector3 input_rotation) {
		Shoot (input_rotation);
	}
	
	//[RPC]
	void Give_Experience() {
		float experience_distance = 50f;
		//Work out experience to be given and radius
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject player in players) {
			float temp_distance = Vector3.Distance(transform.position, player.transform.position);
			if(temp_distance < experience_distance) {
				player.GetComponent<Player>().Collect_Experience(5 * Get_Level);
			} else {
				Debug.Log("Too far " + temp_distance);
			}
		}
	}
}
