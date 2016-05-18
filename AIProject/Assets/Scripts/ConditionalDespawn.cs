using UnityEngine;
using System.Collections;

using RAIN.Core;

public class ConditionalDespawn : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        // Ignore if the other collider isn't part of a lichKing
        GameObject parent = other.gameObject;
        if(parent.GetComponent<LichKing>())
        {
            // Only lichKings will execute this code
            // Check if the lichKing AI has enough followers (in case it collides with the trigger before actively seeking it)
            AI kingAi = parent.GetComponent<AIRig>().AI;
            int numFollowers = kingAi.WorkingMemory.GetItem<int>("numberOfFollowers");
            int neededFollowers = kingAi.WorkingMemory.GetItem<int>("requiredFollowerCount");
            if(numFollowers >= neededFollowers)
            {
                GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
                gm.ReportFitness(parent.GetComponent<LichKing>().timeTaken);
                // Now the king should "exit" by being destroyed
                Destroy(parent);
            }
        }
    }
}
