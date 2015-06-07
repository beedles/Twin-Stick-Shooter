using UnityEngine;
using System.Collections;
using TSS;

public class Projectile : MonoBehaviour {
	public float projectile_life = 1.0f;
	public float projectile_speed = 1f;
	public float projectile_damage = 0f;
	public GameObject owner;
	//public Texture select_skin;
	
	private float time_spent_alive;
	
	//protected Rect bounding_box;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		time_spent_alive += Time.deltaTime;
		if(time_spent_alive > projectile_life) {
			Remove_Me();		
		}
		
		//bounding_box = WorkManager.CalculateSelectionBox(gameObject.GetComponent<SpriteRenderer>().bounds);
	}
	
	void FixedUpdate() {
		transform.Translate(Vector3.up * projectile_speed);
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
	}
	
	void OnGUI() {
		//GUI.DrawTexture(bounding_box, select_skin);
	}
	
	void Remove_Me() {
		//graphic for hit
		//Instantiate(ptr_script_variable.par_bullet_hit, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
	
	[RPC]
	void Projectile_Hit(GameObject projectile) {
		if(gameObject == projectile) {
			Remove_Me();
		}
	}
}
