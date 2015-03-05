using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeColourByScale : MonoBehaviour {
	public Image image;
	public float min_value = 0.0f;
	public float max_value = 1.0f;
	public Color min_color = Color.red;
	public Color max_color = Color.green;
	
	// Use this for initialization
	void Start () {
		if(image==null) {
			image = GetComponent<Image>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(transform.localScale.x);
		image.color = Color.Lerp(min_color, max_color, Mathf.Lerp(min_value, max_value, transform.localScale.x));
	}
}
