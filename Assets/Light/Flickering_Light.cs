using UnityEngine;
using System.Collections;

public class Flickering_Light : MonoBehaviour {
	public string wave_function = "sin";
	public float start = 0.0f;
	public float amplitude = 1.0f;
	public float phase = 0.0f;
	public float frequency = 0.5f;
	
	private Color original_colour;
	private SpriteRenderer light_source;
	
	void Start() {
		light_source = GetComponent<SpriteRenderer>();
		original_colour = light_source.color;		
	}
	
	void Update() {
		light_source.color = original_colour * (Eval_Wave());
	}
	
	float Eval_Wave() {
		float x = (Time.time + phase) * frequency;
		float y;
		
		//Normalize x
		x = x - Mathf.Floor(x);
		
		if(wave_function == "sin") {
			y = Mathf.Sin(x * 2 * Mathf.PI);
		} else {
			y = 1.0f;
		}
		
		return (y * amplitude) + start;
	}
}
