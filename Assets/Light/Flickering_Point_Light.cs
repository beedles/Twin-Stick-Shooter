using UnityEngine;
using System.Collections;

public class Flickering_Point_Light : MonoBehaviour {
	private GameObject point_light;
	private Light light_source;
	public string wave_function = "sin";
	public float start = 0.0f;
	public float amplitude = 1.0f;
	public float phase = 0.0f;
	public float frequency = 0.5f;
	
	private Color original_colour;
	private float original_intensity;

	// Use this for initialization
	void Start () {
		light_source = GetComponent<Light>();;
		original_colour = light_source.color;	
		original_intensity = light_source.intensity;
	}	
	
	void Update() {
		//light_source.color = original_colour * (Eval_Wave());
		light_source.intensity = original_intensity * (Eval_Wave());
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
