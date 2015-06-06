using UnityEngine;
using System.Collections;

public class Torch : Dynamic_Object {
	public GameObject light_prefab;
	private GameObject torch_light;
	private SpriteRenderer light_renderer;
	
	protected override void Start() {
		base.Start();
		/*torch_light = Instantiate(light_prefab, transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
		torch_light.transform.parent = gameObject.transform;
		light_renderer = torch_light.GetComponent<SpriteRenderer>();*/
		
	}
	
	protected override void Update() {
		base.Update();
		//Check if we are "lit" and enable light_prefab
		/*if(anim_state == 1 && light_renderer != null) {
			light_renderer.enabled = true;
		} else {
			light_renderer.enabled = false;
		}*/
	} 	
}
