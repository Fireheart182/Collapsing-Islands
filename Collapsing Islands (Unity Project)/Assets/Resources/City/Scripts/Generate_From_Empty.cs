using UnityEngine;
using System.Collections;
using System.IO;

public class Generate_From_Empty : MonoBehaviour {

    public int scalar;
    public int n;
    public int m;

    private GameObject[][] buildings;

	// Use this for initialization
	void Start () 
    {
        string path = "City/City Prefabs/";
        Object[] prefabs = Resources.LoadAll(path, typeof(GameObject));

        // Default value of n,m = 5, scalar = 3
        n = n>0 ? n:5;
        m = m>0 ? m:5;
        scalar = scalar>0 ? scalar:3;
        buildings =  new GameObject[n][];

        // New Building location
        float x, y, z;
        

        for (int i = 0; i < n; i++)
        {
            buildings[i] = new GameObject[m];
            float offX = scalar * n/2;
            float offZ = scalar * m/2;
            for (int j = 0; j < m; j++)
            {
                int to_make = (int) (Random.Range(0, prefabs.Length -.001f));
                x = transform.position.x + i * scalar 
                    + Random.Range(-scalar/2, scalar/2) - offX;
                y = transform.position.y;
                z = transform.position.z + j * scalar 
                    + Random.Range(-scalar/2, scalar/2) - offZ;

				buildings[i][j] = (GameObject) Instantiate(
                    prefabs[to_make], 
                    new Vector3(x,y,z), 
                    Quaternion.LookRotation(Vector3.up, Random.onUnitSphere));
                float individual_scale = Random.Range(0.5f, 1.5f);
				buildings[i][j].transform.localScale = new Vector3(individual_scale, individual_scale, individual_scale);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
