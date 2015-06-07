using UnityEngine;
using System.Collections;
using TSS;

public class CollisionHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
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
					//Hit Detected
					//Remove Projectile
					
					//Deal damage to enemy
					if(PhotonNetwork.isMasterClient) {
						e.GetPhotonView().RPC ("Do_Hit", PhotonTargets.All, p.GetComponent<Projectile>().projectile_damage);
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
					//Hit Detected
					//Remove Projectile
					
					//Deal damage to enemy
					if(PhotonNetwork.isMasterClient) {
						e.GetPhotonView().RPC ("Do_Hit", PhotonTargets.All, p.GetComponent<Projectile>().projectile_damage);
					}
					Destroy(p);
				}
			}						
		}
	}
}
