using UnityEngine;
using System.Collections.Generic;

    /*
        The GameManager class handles the running of a standard game scene
        Start() within this class sets up the proper procedure for beginning ai for specific agents

        Zombies: 
            targetTree: any randomly chosen BrainTree from the "Forest" gameObject
            targetGrave: any iteratively chosen Grave from the "Graveyard" gameObject

    */

public class GameManager : MonoBehaviour {

    public GameObject forest;
    public GameObject graveyard;
    public GameObject congaLeader;
    public GameObject skeletonPrefab;
    public GameObject zombiePrefab;
    public GameObject ghoulPrefab;
    public List<GameObject> trees;
    public List<GameObject> graves;
    private List<Skeleton> skeletons;
    private List<Zombie> zombies;
    private List<Ghoul> ghouls;
    private int graveIterator;
    private int randomIndex;
    private int numGhouls;
    private int numSkellies;
    private int numZombies;

    // Use this for initialization
    void Start () {

        numGhouls = 10;
        numZombies = 10;
        numSkellies = 3;
        graveIterator = 0;
        randomIndex = 0;
        graves = new List<GameObject>(); 
        trees = new List<GameObject>();
        skeletons = new List<Skeleton>();
        zombies = new List<Zombie>();
        ghouls = new List<Ghoul>();

        // Assign the array values to the proper associated transform's gameObject member
        foreach (Transform child in forest.transform)
        {
            trees.Add(child.gameObject);
        }
        foreach (Transform child in graveyard.transform)
        {
            graves.Add(child.gameObject);
        }
        
        float x, z;

        for(int i = 0; i < numSkellies; i++)
        {
            x = Random.Range(-3.0f, 3.0f);
            z = Random.Range(-3.0f, 3.0f);
            GameObject s = (GameObject)Instantiate(skeletonPrefab, new Vector3(x, 1.0f, z), Quaternion.identity);
            skeletons.Add(s.GetComponent<Skeleton>());
            skeletons[i].isTail = false;
        }

        skeletons[0].following = congaLeader.GetComponent<Skeleton>();
        
        for(int i = 1; i < numSkellies; i++)
        {
            skeletons[i].following = skeletons[i - 1];
            skeletons[i - 1].isFollowedBy = skeletons[i];
        }

        skeletons[numSkellies - 1].isTail = true;

        for (int i = 0; i < numZombies; i++)
        {
            x = Random.Range(-3.0f, 3.0f);
            z = Random.Range(-3.0f, 3.0f);
            GameObject s = (GameObject)Instantiate(zombiePrefab, new Vector3(x, 1.0f, z), Quaternion.identity);
            zombies.Add(s.GetComponent<Zombie>());
        }

        for (int i = 0; i < numGhouls; i++)
        {
            x = Random.Range(-3.0f, 3.0f);
            z = Random.Range(-3.0f, 3.0f);
            GameObject s = (GameObject)Instantiate(ghoulPrefab, new Vector3(x, 1.0f, z), Quaternion.identity);
            ghouls.Add(s.GetComponent<Ghoul>());
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Encapsulate iterative process of assigning graves to zombies
    void AssignIterativeGrave(Zombie z)
    {
        // Check if graveIterator is within valid range
        if(graveIterator > graves.Count - 1 || graveIterator < 0)
        {
            graveIterator = 0;
        }
        z.SetGraveTarget(graves[graveIterator]);
        graveIterator++;
    }
	
	
}
