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
    private List<GameObject> trees;
    private List<GameObject> graves;
    private int graveIterator;
    private int randomIndex;

    // Use this for initialization
    void Start () {

        graveIterator = 0;
        randomIndex = 0;
        graves = new List<GameObject>(); 
        trees = new List<GameObject>();

        // Assign the array values to the proper associated transform's gameObject member
        foreach (Transform child in forest.transform)
        {
            trees.Add(child.gameObject);
        }
        foreach (Transform child in graveyard.transform)
        {
            graves.Add(child.gameObject);
        }

    }
    
    // Update is called once per frame
	void Update () {
        
	}

    // Encapsulate process of assigning each zombie a random new tree to target
    void AssignRandomTree(Zombie z)
    {
        randomIndex = Random.Range(0, trees.Count - 1);
        z.SetTreeTarget(trees[randomIndex]);
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
