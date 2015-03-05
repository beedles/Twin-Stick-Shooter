using UnityEngine;
using System.Collections;

public class TimedDestroy : MonoBehaviour {
	public Object to_be_destroyed;
	public float timer;
	// Use this for initialization
	void Start () {
		if(to_be_destroyed == null) {
			to_be_destroyed = gameObject;
		}
		
		Destroy(to_be_destroyed, timer);
	}
}
