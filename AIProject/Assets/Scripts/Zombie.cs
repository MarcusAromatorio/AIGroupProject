using UnityEngine;
using System.Collections;
using RAIN.Core;

public class Zombie : MonoBehaviour {

    public GameObject graveObject;
    public GameObject treeObject;
    private AIRig zombieAiRig;
    private AI zombieAi;


	// Use this for initialization
	void Start () {
        zombieAiRig = gameObject.GetComponentInChildren<AIRig>();
        zombieAi = zombieAiRig.AI;
	}
	
	// Update is called once per frame
	void Update () {
	
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
