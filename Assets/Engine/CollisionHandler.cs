using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TSS;

public class CollisionHandler : NetworkBehaviour {
	// Update is called once per frame
	void Update () {
		GameObject[] player_projectiles = GameObject.FindGameObjectsWithTag("Player_Projectile");
		GameObject[] enemy_projectiles = GameObject.FindGameObjectsWithTag("Enemy_Projectile");
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject p in player_projectiles) {
			foreach(GameObject e in enemies) {
				Rect a = WorkManager.CalculateSelectionBox(p.GetComponent<SpriteRenderer>().bounds);
				Rect b = WorkManager.CalculateSelectionBox(e.GetComponent<SpriteRenderer>().bounds);
				if(a.x < (b.x + b.width) &&
				   (a.x + a.width) > b.x &&
				   a.y < (b.y + b.height) &&
				   (a.y + a.height) > b.y) {

					if(isServer) {
						e.GetComponent<Enemy>().Take_Damage(p.GetComponent<Projectile>().projectile_damage);
					}
					Destroy(p);
				}
			}
		}
		foreach(GameObject p in enemy_projectiles) {
			foreach(GameObject e in players) {
				Rect a = WorkManager.CalculateSelectionBox(p.GetComponent<SpriteRenderer>().bounds);
				Rect b = WorkManager.CalculateSelectionBox(e.GetComponent<SpriteRenderer>().bounds);
				if(a.x < (b.x + b.width) &&
				   (a.x + a.width) > b.x &&
				   a.y < (b.y + b.height) &&
				   (a.y + a.height) > b.y) {

					if(isServer) {
						e.GetComponent<Player>().Take_Damage(p.GetComponent<Projectile>().projectile_damage);
					}					
					Destroy(p);
				}
			}						
		}
	}
}
