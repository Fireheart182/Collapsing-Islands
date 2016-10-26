using UnityEngine;
using System.Collections;
using System.IO;


public class Generate_From_Empty : MonoBehaviour {

    public float offset; // Offset between two buildings \(0, intmax)
    public int n; // Grid width in x direction \(0, intmax)
    public int m; // Grid width in z direction \(0, intmax)
    public float scalar; // Scalar applied to the city \(0 - floatmax)
    public float scale_range; // Range of random scale applied to each building \[0, 1]
    public float c_min; // Minimum percentage of buildings chosen \[0, 1]
    public float c_max; // Maximum percentage of buildings chosen \[c_min, 1]

    private GameObject[][] buildings;
    private Object[] prefabs;
    private bool falling;
	private float counter;
	private float old_counter;
	private float step;

    // Don't let buildings get created outside the grid
    bool create_building(float x, float y, float z)
    {
        float xdist = (n/2f) * offset + transform.position.x;
        float zdist = (m/2f) * offset + transform.position.z;
        return (x > xdist || x < -xdist || 
            z > zdist || z < -zdist); 
    }

	// Initialize a city of size N X M with the buildings offset apart. Scalar 
    // scales the entire system.
	void Start () 
    {
        // Variable intitializations
        var_init();

        // New Building location
        float x, y, z;

        for (int i = 0; i < n; i++)
        {
            buildings[i] = new GameObject[m];
            float offX = offset * n/2;
            float offZ = offset * m/2;

            for (int j = 0; j < m; j++)
            {
                // Jitter the init locations of each building. Note that y is up,
                // for some godforsaken reason.
                int to_make = (int) (Random.Range(0, prefabs.Length -.001f));
                x = transform.position.x + i * offset 
                    + Random.Range(-offset/2, offset/2) - offX;
                y = transform.position.y;
                z = transform.position.z + j * offset 
                    + Random.Range(-offset/2, offset/2) - offZ;

                // Check if a building meets my heuristic for whether or not to 
                // create it, then create it with a random rotation.
                if (create_building(x,y,z))
                {
    				buildings[i][j] = (GameObject) Instantiate(
                        prefabs[to_make], 
                        new Vector3(x,y,z), 
                        Quaternion.identity);

                    float individual_scale = Random.Range((1 - scale_range) * scalar, 
                                                            (1 + scale_range) * scalar);
                    Vector3 rescale = new Vector3(individual_scale, 
                                                individual_scale, 
                                                individual_scale);
    				buildings[i][j].transform.localScale = rescale;
                    buildings[i][j].transform.Rotate(0f, 
                                                    Random.Range(0.0f,360.0f),
                                                    0f);
                }
                else
                {
                    // Technically redundant, but explicitly declares my flags
                    // for whether or not a building has been destroyed.
                    buildings[i][j] = null;
                }
            }
        } 
	}

    void var_init()
    {
        falling = true;
        string path = "City/City Prefabs/";
        prefabs = Resources.LoadAll(path, typeof(GameObject));

        // Default value of n,m = 5, offset = 3
        n = n > 0 ? n : 5;
        m = m > 0 ? m : 5;
        c_min = c_min >= 0 && c_min <= 1  ? c_min : .25f;
        c_max = c_max >= 0 && c_max <= 1 && c_max > c_min ? c_max : c_min;
        scale_range = scale_range > 0 ? scale_range : 0;
        offset = offset > 0 ? offset : 3;
        offset = (offset * scalar);
        buildings =  new GameObject[n][];

		counter = 0;
		step = .125f;
    }

    // Destroy function runs a building's collapse animation.
    IEnumerator destroy(GameObject b)
    {
        Animation anim = b.GetComponent<Animation>();
        // Make sure all the buildings don't start collapsing at exactly the same time
        yield return new WaitForSeconds(Random.Range(0f, 5f));
        anim.Play();
    }
	
	// Update is called once per frame
	void Update () 
    {
        // To simulate blinking
        if(Input.GetKeyDown( KeyCode.Space ))
        {
            // Choose some random number of buildings to destroy on each blink
            int choose;
            for (choose = (int) Random.Range(n*m*c_min, n*m*c_max); choose>0 && falling; choose--)
            {
                //Select a random slot
                int i = Random.Range(0, n-1);
                int j = Random.Range(0, m-1);
                int count = 0;
                //If it's already been collapsed, walk through until you find one
                while (buildings[i][j] == null && count < n * m)
                {
                    i += 1;
                    if (i == n)
                    {
                        i = 0;
                        j += 1;
                        j = j % m;
                    }
                    count ++; //Don't loop forever!
                }

                GameObject b = buildings[i][j];
                if (b == null)
                {
                    falling = false; // We've collapsed all our buildings, 
                                     // don't go looking anymore.
                }
                else
                {
                    StartCoroutine("destroy", b); // Destroy building in parallel
                    buildings[i][j] = null; // Mark a building as chosen
                }
            }
			counter += step;
			RenderSettings.skybox.SetColor("_Tint", new Color(.5f - counter, .5f- counter, .5f - counter));
    	}
    }
}
