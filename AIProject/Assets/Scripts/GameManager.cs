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

    public List<bool[]> geneList; // List of genotypes used in lich kings
    public GameObject forest;
    public GameObject graveyard;
    public GameObject congaLeader;
    public GameObject skeletonPrefab;
    public GameObject zombiePrefab;
    public GameObject ghoulPrefab;
    public GameObject entryPortal;
    public GameObject exitPortal;
    public GameObject lichKingPrefab;
    public List<GameObject> trees;
    public List<GameObject> graves;
    public List<Skeleton> skeletons;
    public List<Zombie> zombies;
    private List<Ghoul> ghouls;
    private int graveIterator;
    private int randomIndex;
    private int numGhouls;
    private int numSkellies;
    private int numZombies;
    public List<int> generationList; // List of generations, matched to genotypes of geneList
    public int[] fitnessList; // Array of fitness values as it is determined after a genotype is expressed and completed
    public int currentGen;
    public int currentKingCount;
    private int currentKingFitness;
    private LichKing kingRef;

    // Use this for initialization
    void Start () {

        numGhouls = 1;
        numZombies = 10;
        numSkellies = 3;
        graveIterator = 0;
        randomIndex = 0;
        currentGen = 0;
        currentKingCount = 0;
        currentKingFitness = 0;
        graves = new List<GameObject>(); 
        trees = new List<GameObject>();
        skeletons = new List<Skeleton>();
        zombies = new List<Zombie>();
        ghouls = new List<Ghoul>();
        geneList = new List<bool[]>();
        generationList = new List<int>();
        fitnessList = new int[6];

        // Make six Generation Zero genotypes to use when lich kings need them
        for(int i = 0; i < 6; i++)
        {
            fitnessList[i] = -1;
            bool[] genotype = new bool[15];
            // Inner loop to populate generation zero genotypes randomly
            for(int j = 0; j < 15; j++)
            {
                genotype[j] = Random.value > 0.50f; // Sets as true only 50% of the time
            }
            // Save each new random genotype as generation zero in the lists
            geneList.Add(genotype);
            generationList.Add(0);
        }


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

    public void ReportFitness(int timeTaken)
    {
        int count = 0;
        int temp = fitnessList[count];
        while(temp > 0)
        {
            count++;
            temp = fitnessList[count];
        }
        fitnessList[count] = timeTaken;
    }
    
    // Method that crosses two genotypes to produce offspring
    bool[] Cross(bool[] parentA, bool[] parentB)
    {
        bool[] offspring = new bool[15];
        bool inheritA = Random.value > 0.5f;
        for(int i = 0; i < 15; i++)
        {
            if(inheritA)
            {
                offspring[i] = parentA[i];
            }
            else
            {
                offspring[i] = parentB[i];
            }
            // 50% chance to inherit from one or the other, each allele
            inheritA = Random.value > 0.5f;
        }

        return offspring;
    }

    // Update is called once per frame
    void Update()
    {
        
        // Spawn a lichking of one isn't present in the scene
        if(!FindObjectOfType<LichKing>())
        {

            // Generation complete if these two conditions met
            if(currentKingCount % 6 == 0 && currentKingCount > 0)
            {
                currentGen = currentKingCount / 6; // Generations of six kings each
                int tempA = 99999999;
                int tempB = 99999999;
                for(int i = 0; i < 6; i++)
                {
                    if(tempA > fitnessList[i])
                    {
                        tempA = fitnessList[i];
                    }
                    if(tempB > fitnessList[i] && tempA < fitnessList[i])
                    {
                        tempB = fitnessList[i];
                    }
                }


                for(int i = 0; i < 6; i++)
                {
                    bool[] child = Cross(geneList[tempA], geneList[tempB]); // The fittest two produce six new kings for the next generation
                    geneList.Add(child);
                }
            }

            // Spawn a new king at the entry portal
            GameObject newKing = (GameObject)Instantiate(lichKingPrefab, entryPortal.transform.position, Quaternion.identity);
            LichKing king = newKing.GetComponent<LichKing>();
            king.ExpressGenotype(geneList[currentKingCount]); // Give the king the predefined genotype for its indexed value
            king.generation = currentGen;
            currentKingCount++;
            kingRef = king;
        }

        try
        {
            currentKingFitness = kingRef.timeTaken;
        }
        catch
        {
            ReportFitness(currentKingFitness);
            currentKingFitness = 0;
        }


        if (Input.GetKeyDown("space"))
        {
            float x = Random.Range(-3.0f, 3.0f);
            float z = Random.Range(-3.0f, 3.0f);
            GameObject s = (GameObject)Instantiate(zombiePrefab, new Vector3(x, 1.0f, z), Quaternion.identity);
            zombies.Add(s.GetComponent<Zombie>());
        }
        if (Input.GetKeyDown("r"))
        {
            DestroyImmediate(skeletons[0].gameObject);
            skeletons.RemoveAt(0);
            skeletons[0].following = congaLeader.GetComponent<Skeleton>();
            for (int i = 1; i < numSkellies; i++)
            {
                skeletons[i].following = skeletons[i - 1];
                skeletons[i - 1].isFollowedBy = skeletons[i];
            }
            skeletons[numSkellies - 1].isTail = true;
        }
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

    //Removes the zombie object and spawns a skeleton in its place
    public void killZombie(Zombie z)
    {
        if (z != null)
        {
            float xCoord = z.gameObject.transform.position.x;
            float zCoord = z.gameObject.transform.position.z;
            zombies.RemoveAt(zombies.IndexOf(z));
            DestroyImmediate(z.gameObject);
            spawnSkeleton(xCoord, zCoord);
        }
    }

    //Spawns a skeleton and fixes following order
    public void spawnSkeleton(float x, float z)
    {
        skeletons[skeletons.Count - 1].isTail = false;
        GameObject s = (GameObject)Instantiate(skeletonPrefab, new Vector3(x, 1.0f, z), Quaternion.identity);
        skeletons.Add(s.GetComponent<Skeleton>());
        for (int i = 1; i < skeletons.Count; i++)
        {
            skeletons[i].following = skeletons[i - 1];
            skeletons[i - 1].isFollowedBy = skeletons[i];
        }
        skeletons[skeletons.Count - 1].isTail = true;
    }
}
