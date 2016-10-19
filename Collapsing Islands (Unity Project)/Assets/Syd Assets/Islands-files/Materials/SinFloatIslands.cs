using UnityEngine;
using System.Collections;

public class SinFloatIslands : MonoBehaviour {
	public GameObject cube6;
	public GameObject cube4;
	public GameObject cube2;
	public GameObject cube3;
	public float speedX = 1.0f;
	public float amplitude = -0.3f;//-0.001f

	void Awake () {

		cube6 = GameObject.Find("pCube6");
		cube4 = GameObject.Find("pCube4");
		cube2 = GameObject.Find("pCube2");
		cube3 = GameObject.Find("pCube3");


	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		cube6.transform.Translate(0,amplitude*Mathf.Sin(Time.time*speedX), 0);
		cube4.transform.Translate(0,amplitude*Mathf.Sin(Time.time*speedX)*1.2f, 0);
		cube2.transform.Translate(0,amplitude*Mathf.Sin(Time.time*speedX)*0.94f, 0);
		cube3.transform.Translate(0,amplitude*Mathf.Sin(Time.time*speedX)*0.8f, 0);

	
	}
}
