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

    // This method sets the zombie's "enthralled" boolean to true and assigns the target to follow
    public void Enthrall(GameObject master)
    {
        zombieAi.WorkingMemory.SetItem<bool>("enthralled", true);
        zombieAi.WorkingMemory.SetItem<GameObject>("master", master);
    }

    // This method makes the zombie forget the master and go back to normal behavior
    public GameObject ForgetMaster(GameObject sourceOfForgetting)
    {
        GameObject empty = new GameObject();

        if(zombieAi.WorkingMemory.GetItem<GameObject>("master") != null)
        {
            GameObject masterToForget = zombieAi.WorkingMemory.GetItem<GameObject>("master"); // Grab the reference of the master object
            masterToForget.GetComponent<LichKing>().ReportForgetting(sourceOfForgetting); // This method call tells the lichking why it lost the zombie thrall
            zombieAi.WorkingMemory.SetItem("master", empty); // This sets the master as an empty object
            return masterToForget;
        }
        else
        {
            return empty; // Return the empty object signifying no master to forget
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
