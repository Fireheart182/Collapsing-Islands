using UnityEngine;
using System.Collections;

public class SinFloat : MonoBehaviour {
	public GameObject cube;
	public float speedX = 1.0f;
	public float amplitude = 0.002f;

	void Awake () {

		cube = GameObject.Find("MyCube");

	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		cube.transform.Translate(0,amplitude*Mathf.Sin(Time.time*speedX), 0);
	
	}
}
