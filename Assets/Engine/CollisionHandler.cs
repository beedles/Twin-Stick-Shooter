using UnityEngine;
using System.Collections;
using TSS;

public class CollisionHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject p in projectiles) {
			if(p.transform.parent.tag == "Player") {
				foreach(GameObject e in enemies) {
					Rect a = WorkManager.CalculateSelectionBox(p.GetComponent<SpriteRenderer>().bounds);
					Rect b = WorkManager.CalculateSelectionBox(e.GetComponent<SpriteRenderer>().bounds);
					if(a.x < (b.x + b.width) &&
					   (a.x + a.width) > b.x &&
					   a.y < (b.y + b.height) &&
					   (a.y + a.height) > b.y) {
						//Hit Detected
						//Remove Projectile
						
						Destroy(p);
						//Deal damage to enemy
						if(PhotonNetwork.isMasterClient) {
							e.GetPhotonView().RPC ("Do_Hit", PhotonTargets.All, p.GetComponent<Projectile>().owner.GetComponent<Unit>().damage);
						}
					}
				}
			} else if (p.transform.parent.tag == "Enemy") {
				foreach(GameObject e in players) {
					Rect a = WorkManager.CalculateSelectionBox(p.GetComponent<SpriteRenderer>().bounds);
					Rect b = WorkManager.CalculateSelectionBox(e.GetComponent<SpriteRenderer>().bounds);
					if(a.x < (b.x + b.width) &&
					   (a.x + a.width) > b.x &&
					   a.y < (b.y + b.height) &&
					   (a.y + a.height) > b.y) {
						//Hit Detected
						//Remove Projectile
						
						Destroy(p);
						//Deal damage to enemy
						if(PhotonNetwork.isMasterClient) {
							e.GetPhotonView().RPC ("Do_Hit", PhotonTargets.All, p.GetComponent<Projectile>().owner.GetComponent<Unit>().damage);
						}
					}
				}
			}			
		}
	}
}
