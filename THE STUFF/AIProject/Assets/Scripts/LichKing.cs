using UnityEngine;
using System.Collections;

using RAIN.Core;

public class LichKing : MonoBehaviour {

    private AI lichAi;

	// Use this for initialization
	void Start () {
        AIRig rig = GetComponentInChildren<AIRig>();
        lichAi = rig.AI;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // This method allows the lichking to learn why they lost a zombie
    public void ReportForgetting(GameObject cause)
    {
        // The AI must decrease its known number of followers by one each time it loses a thrall
        int numFollowers = lichAi.WorkingMemory.GetItem<int>("numberOfFollowers");
        numFollowers--;
        lichAi.WorkingMemory.SetItem<int>("numberOfFollowers", numFollowers);
    }
}
