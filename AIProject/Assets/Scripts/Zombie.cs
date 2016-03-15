using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using RAIN.Core;

public class Zombie : MonoBehaviour {

    public GameObject graveObject;
    public GameObject treeObject;
    public List<GameObject> trees;
    public List<GameObject> graves;
    private AIRig zombieAiRig;
    private AI zombieAi;
    private int hunger;


	// Use this for initialization
	void Start () {
        zombieAiRig = gameObject.GetComponentInChildren<AIRig>();
        zombieAi = zombieAiRig.AI;

        trees = GameObject.Find("GameManager").GetComponent<GameManager>().trees;
        graves = GameObject.Find("GameManager").GetComponent<GameManager>().graves;
        SetTreeTarget(RandomTree());
        SetGraveTarget(RandomGrave());
	}
	
	// Update is called once per frame
	void Update () {
        hunger = zombieAi.WorkingMemory.GetItem<int>("hunger");
        if (hunger == -98 || hunger == -48)
        {
            SetTreeTarget(RandomTree());
        }
        if (hunger == 1 || hunger == 51)
        {
            SetGraveTarget(RandomGrave());
        }
	}

    public GameObject RandomTree()
    {
        int randomIndex = Random.Range(0, trees.Count);
        //Debug.Log("tree:" + randomIndex);
        return trees[randomIndex];
    }

    public GameObject RandomGrave()
    {
        int randomIndex = Random.Range(0, graves.Count);
        //Debug.Log("grave: " + randomIndex);
        return graves[randomIndex];
    }

    // Methods to set behavior targets reference values
    public void SetGraveTarget(GameObject grave)
    {
        graveObject = grave;
        zombieAi.WorkingMemory.SetItem<GameObject>("targetGrave", graveObject);
    }

    public void SetTreeTarget(GameObject tree)
    {
        treeObject = tree;
        zombieAi.WorkingMemory.SetItem<GameObject>("targetTree", treeObject);
    }

    // Method to stop the AI and child components from updating
    public void Disable()
    {
        if(zombieAi.Started)
        {
            zombieAi.IsActive = false;
            gameObject.SetActive(false);
        }
    }

    // Method to start the AI and child components after being disabled
    public void Enable()
    {
        if(zombieAi.Started &!zombieAi.IsActive)
        {
            zombieAi.IsActive = true;
            gameObject.SetActive(true);
        }
    }
}
