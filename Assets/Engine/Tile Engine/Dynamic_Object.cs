using UnityEngine;
using System.Collections;

public class Dynamic_Object : MonoBehaviour {
	public bool is_interactable = false;
	
	//State for animations, should be 0 unless it has multiple states (e.g. torch unlit/lit)
	public int anim_state = 0;
	
	protected virtual void Start () {
	
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		//bounding_box = WorkManager.CalculateSelectionBox(gameObject.GetComponent<SpriteRenderer>().bounds);
		
		Handle_Animation();
	}
	
	protected virtual void Handle_Animation() {
		Animator animator = gameObject.GetComponent<Animator>();
		
		if(animator) {
			animator.SetInteger("State", anim_state);
		}
	}
}

