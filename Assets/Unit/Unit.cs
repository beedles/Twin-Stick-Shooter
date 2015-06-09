using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TSS;

public class Unit : Photon.MonoBehaviour {
	public float movement_speed = 5.0f;
	public float fire_rate = 2.0f;
	public GameObject bullet;	
	public float projectile_speed = 0.4f;
	public float projectile_life = 1.0f;
	public float health = 50f;
	public float max_health;
	public float damage = 5f;
	public float damage_min = 0f;
	public float damage_max = 0f;
	public float damage_modifier = 0f;
	public bool show_name = false;
	public string unit_name = "";
	public GUISkin name_skin;
	public GameObject health_bar;
	public GameObject death_effect;
	public GameObject sct_prefab;
	public Transform sct_transform;
	//public Texture select_skin;
	/*SOUNDS*/
	public AudioClip shoot_sound;
	public AudioClip hit_sound;
	public AudioClip death_sound;
	
	public bool invulnerable = false;
	
	protected Vector3 input_rotation;
	protected Vector3 input_movement;
	protected bool is_a_player;	
	//protected Rect bounding_box;
	
	private float next_shot_time = 0.0f;
	private GameObject unit_health_bar;
	private float top_of_unit = 0.0f;
	private float center_of_unit = 0.0f;
	
	private Animator animator;
	
	private AudioSource source;
	/*private float vol_low_range = 0.5f;
	private float vol_high_range = 1.0f;*/
	private float pitch_low_range = 0.9f;
	private float pitch_high_range = 1.1f;
	
	//Stats
	private int level = 0;
	public int strength = 0;
	public int dexterity = 0;
	public int intelligence = 0;
	public int wisdom = 0;
	public int stamina = 0;
	
	//Stat helpers
	public float health_per_stamina = 0f;
	
	protected virtual void Awake() {
		source = GetComponent<AudioSource>();
	}

	// Use this for initialization
	protected virtual void Start () {
		top_of_unit = gameObject.GetComponent<SpriteRenderer>().bounds.center.y + gameObject.GetComponent<SpriteRenderer>().bounds.extents.y;
		center_of_unit = gameObject.GetComponent<SpriteRenderer>().bounds.center.x;
		animator = gameObject.GetComponent<Animator>();
		
		health = stamina * health_per_stamina;
		max_health = health;		
		unit_health_bar = (GameObject)Instantiate(health_bar, new Vector3(center_of_unit, top_of_unit + 0.5f, 0), Quaternion.identity);
		unit_health_bar.transform.SetParent(gameObject.transform);
		//make width of health bar same as sprite
		unit_health_bar.transform.localScale = new Vector3(gameObject.GetComponent<SpriteRenderer>().bounds.extents.x * 3, 
			unit_health_bar.transform.localScale.y, 
			unit_health_bar.transform.localScale.z);
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		//bounding_box = WorkManager.CalculateSelectionBox(gameObject.GetComponent<SpriteRenderer>().bounds);
		
		Handle_Animation();
	}
	
	protected virtual void Find_Input() {
	
	}	
	
	protected virtual void Process_Movement() {
	
	}
	
	protected virtual void Handle_Animation() {		
		float angle = Mathf.Atan2(input_rotation.y, input_rotation.x) * Mathf.Rad2Deg;
		int direction = 0;
		if(angle > -135.0f && angle <= -45.0f) {
			direction = 0;
		} else if (angle > 135.0f || angle <= -135.0f) {
			direction = 1;
		} else if (angle > 45.0f && angle <= 135.0f) {
			direction = 2;
		} else if (angle > -45.0f && angle <= 45.0f) {
			direction = 3;
		}
		if(animator) {
			animator.SetInteger("Direction", direction);
		}
	}
	
	protected virtual void OnGUI() {
		//GUI.DrawTexture(bounding_box, select_skin);
		if(show_name) {
			Draw_Name();
		}
		if(health < max_health) {
			unit_health_bar.GetComponent<Canvas>().enabled = true;
			Update_Health_Bar(health/max_health);
		} else {
			unit_health_bar.GetComponent<Canvas>().enabled = false;
		}
	}	
	
	protected virtual void Shoot(Vector3 owner_rotation) {
		//Debug.Log("Shooting");
		if(Time.time >= next_shot_time) {
			Handle_Bullets(owner_rotation);
			next_shot_time = Time.time+fire_rate;
		}
	}
	
	private void Handle_Bullets(Vector3 owner_rotation) {
		GameObject obj_created_bullet;
		Vector3 temp_vector;
		//code to handle offsetting (for gun muzzle on rotating character)
		/*temp_vector = Quaternion.AngleAxis(8f, Vector3.forward) * input_rotation;
		Debug.Log(temp_vector);*/
		temp_vector = (transform.position + (owner_rotation.normalized * .5f));
		obj_created_bullet = (GameObject) Instantiate(bullet, temp_vector, Quaternion.FromToRotation(Vector3.up, owner_rotation.normalized));
		//Debug.Log(gameObject.tag);
		if(gameObject.tag == "Player") {
			obj_created_bullet.tag = "Player_Projectile";
		} else if (gameObject.tag == "Enemy") {
			obj_created_bullet.GetComponent<Projectile>().tag = "Enemy_Projectile";
		}
		
		Calculate_Damage();
		
		obj_created_bullet.GetComponent<Projectile>().owner = gameObject;
		//obj_created_bullet.transform.SetParent(gameObject.transform);
		obj_created_bullet.GetComponent<Projectile>().projectile_speed = projectile_speed;
		obj_created_bullet.GetComponent<Projectile>().projectile_life = projectile_life;
		obj_created_bullet.GetComponent<Projectile>().projectile_damage = damage;
		//Physics.IgnoreCollision(obj_created_bullet.collider, collider);
		Play_Sound(shoot_sound);
	}
	
	private void Calculate_Damage() {
		damage = Random.Range(damage_min, damage_max) + damage_modifier;
	}
	
	private void Play_Sound(AudioClip sound) {
		if(sound != null && source != null) {
			//float volume = Random.Range(vol_low_range, vol_high_range);
			source.pitch = Random.Range(pitch_low_range, pitch_high_range);
			//volume *= gain;
			source.PlayOneShot(sound);
		}
	}
	
	//This is a bad way to do sound if we want to use the 3d sound system
	private void Play_Sound_At_Point(AudioClip sound, Vector3 point) {
		if(sound != null && source != null) {
			//float volume = Random.Range(vol_low_range, vol_high_range);
			source.pitch = Random.Range(pitch_low_range, pitch_high_range);
			//volume *= gain;
			AudioSource.PlayClipAtPoint(sound, point);
		}
	}
	
	private void Draw_Name() {
		//Find bottom of unit
		Rect bounding_box = WorkManager.CalculateSelectionBox(gameObject.GetComponent<SpriteRenderer>().bounds);
		Vector3 unit_base = new Vector3(bounding_box.x + bounding_box.width/2, bounding_box.y + bounding_box.height, 0);
		GUI.skin = name_skin;
		GUI.Label(new Rect(unit_base.x - 50f, unit_base.y, 100f, 25f), System.String.Format("{0:0} - {1}", Get_Level, unit_name));
	}
	
	protected virtual void Check_Health() {
		if(health <= 0) {
			Matchmaker matchmaker = (Matchmaker)GameObject.FindObjectOfType<Matchmaker>();
			if(death_effect != null) {
				Instantiate(death_effect, transform.position, Quaternion.identity);
			}
			Play_Sound_At_Point(death_sound, transform.position);
			Destroy(gameObject);
			//Update enemy list
			if(!is_a_player && PhotonNetwork.isMasterClient) {		
				photonView.RPC("Give_Experience", PhotonTargets.All);		
				if(matchmaker != null) {
					matchmaker.total_enemies--;
				}
			}	
			if(is_a_player && gameObject.GetComponent<PhotonView>().isMine) {
				matchmaker.is_alive = false;
			}
		}
	}
	
	private void Update_Health_Bar(float current_health_percentage) {
		unit_health_bar.transform.FindChild("HealthBar").transform.localScale = new Vector3(current_health_percentage, 1, unit_health_bar.transform.localScale.z);
	}
	
	private void Create_SCT_Popup(float damage) {
		if(sct_prefab == null || sct_transform == null) {
			return;
		}
		top_of_unit = gameObject.GetComponent<SpriteRenderer>().bounds.center.y + gameObject.GetComponent<SpriteRenderer>().bounds.extents.y;
		center_of_unit = gameObject.GetComponent<SpriteRenderer>().bounds.center.x;
		GameObject unit_sct = (GameObject) Instantiate(sct_prefab, new Vector3(center_of_unit, top_of_unit + 0.5f, 0), Quaternion.identity);
		unit_sct.transform.SetParent(gameObject.transform);
		unit_sct.gameObject.GetComponentInChildren<Text>().text = damage.ToString();
	}
	
	public bool Is_Player() {
		return is_a_player;
	}	
	
	protected virtual void On_Hit(float damage) {
		Create_SCT_Popup(damage);
		if(!this.invulnerable) {
			this.health -= damage;
		}
		Play_Sound(hit_sound);
		Check_Health();
	}
	
	//Setters
	public void Set_Level(int level) {
		this.level = level;
	}
	
	//Getters
	public int Get_Level { get { return this.level;} }
}
