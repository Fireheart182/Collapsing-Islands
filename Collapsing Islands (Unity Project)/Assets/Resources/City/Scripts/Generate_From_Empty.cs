using UnityEngine;
using System.Collections;
using System.IO;


public class Generate_From_Empty : MonoBehaviour {

    public float offset;
    public int n;
    public int m;
    public float scalar;

    private GameObject[][] buildings;
    private bool falling;

    //Feather the edge of the city
    bool create_building(int i, int j, int n, int m)
    {
        int magic =  5;
        if (i < magic)
        {
           return (Random.Range(0, i + 1) > 0);
        }
        else if (j < magic)
        {
           return (Random.Range(0, j + 1) > 0);
        }
        else if (i > n - magic)
        {
           return (Random.Range(0, n - i) > 0);
        }
        else if (j > m - magic)
        {
           return (Random.Range(0, n - j) > 0);
        }
        else return true;
    }
	// Use this for initialization
	void Start () 
    {
        falling = true;
        string path = "City/City Prefabs/";
        Object[] prefabs = Resources.LoadAll(path, typeof(GameObject));

        // Default value of n,m = 5, offset = 3
        n = n > 0 ? n : 5;
        m = m > 0 ? m : 5;
        offset = offset > 0 ? offset : 3;
        offset = (offset * scalar);
        buildings =  new GameObject[n][];

        // New Building location
        float x, y, z;
        

        for (int i = 0; i < n; i++)
        {
            buildings[i] = new GameObject[m];
            float offX = offset * n/2;
            float offZ = offset * m/2;
            for (int j = 0; j < m; j++)
            {
                int to_make = (int) (Random.Range(0, prefabs.Length -.001f));
                x = transform.position.x + i * offset 
                    + Random.Range(-offset/2, offset/2) - offX;
                y = transform.position.y;
                z = transform.position.z + j * offset 
                    + Random.Range(-offset/2, offset/2) - offZ;
                Debug.Log("X:" + x + " Y:" + y + " Z:" + z);

                if (create_building(i,j,n,m))
                {
    				buildings[i][j] = (GameObject) Instantiate(
                        prefabs[to_make], 
                        new Vector3(x,y,z), 
                        Quaternion.identity);
                    float individual_scale = Random.Range(0.5f * scalar, 1.5f * scalar);
    				buildings[i][j].transform.localScale = new Vector3(individual_scale, individual_scale, individual_scale);
                    buildings[i][j].transform.Rotate(0f,Random.Range(0.0f,360.0f),0f);
                }
            }
        } 
	}
	
	// Update is called once per frame
	void Update () 
    {
        if( Input.GetKeyDown( KeyCode.Space ) )
        {
            for (int choose = Random.Range(n*m,n*m); choose > 0 && falling; choose --)
            {
                int i = Random.Range(0, n-1);
                int j = Random.Range(0, m-1);
                int count = 0;
                while (buildings[i][j] == null && count < n * m)
                {
                    i +=1;
                    if (i == n)
                    {
                        i = 0;
                        j += 1;
                        j = j % m;
                    }
                    count ++;
                }

                GameObject b = buildings[i][j];
                if (b == null)
                {
                    falling = false;
                }
                else
                {
                    Animation anim = b.GetComponent<Animation>();
                    anim.Play();
                    buildings[i][j] = null;
                }
            }
    	}
    }
}
