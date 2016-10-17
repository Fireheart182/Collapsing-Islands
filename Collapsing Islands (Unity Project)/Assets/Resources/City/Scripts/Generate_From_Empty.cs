using UnityEngine;
using System.Collections;
using System.IO;

public class Node
{
    public GameObject building;
    public Node next;
    public Node prev;

    public Node(GameObject b)
    {
        building = b;
        next = null;
        prev = null;
    }
}
public class Stack
{
    public Node head;
    public int length;

    public Stack()
    {
        head = new Node(null);
        length = 0;
    }   

    public void push(GameObject b)
    {
        Debug.Log("Pushing");
        Node n = new Node(b);
        head.prev = n;
        n.next = head;
        head = n;
        length += 1;
    }

    public GameObject pop()
    {
        if (length == 0) return null;
        GameObject res = head.building;
        head = head.next;
        length --;
        return res;
    }
}

public class Generate_From_Empty : MonoBehaviour {

    public int offset;
    public int n;
    public int m;
    public float scalar;

    private GameObject[][] buildings;
    private Stack S;

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
        S = new Stack();
        string path = "City/City Prefabs/";
        Object[] prefabs = Resources.LoadAll(path, typeof(GameObject));

        // Default value of n,m = 5, offset = 3
        n = n > 0 ? n : 5;
        m = m > 0 ? m : 5;
        offset = offset > 0 ? offset : 3;
        offset = (int)(offset * scalar);
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
                    S.push(buildings[i][j]);
                }
            }
        } 
	}
	
	// Update is called once per frame
	void Update () 
    {
        if( Input.GetKeyDown( KeyCode.Space ) )
        {
    	    GameObject b = S.pop();
            if (b != null)
            {
                Animation anim = b.GetComponent<Animation>();
                anim.Play();
            }
    	}
    }
}
