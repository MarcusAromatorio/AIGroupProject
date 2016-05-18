using UnityEngine;
using System.Collections;

using RAIN.Core;

public class LichKing : MonoBehaviour {

    private AI lichAi;
    public int generation;
    public bool[] genotype;
    public float phenotype;
    public int timeTaken;


	// Use this for initialization
	void Start () {
        AIRig rig = GetComponentInChildren<AIRig>();
        lichAi = rig.AI;
        timeTaken = 0;
	}

    // Update is called once per frame
    void Update()
    {
        // If the lich hasn't captured enough followers, keep counting updates. Lower time is higher fitness
        if (lichAi.WorkingMemory.GetItem<int>("numberOfFollowers") < lichAi.WorkingMemory.GetItem<int>("requiredFollowerCount"))
        {
            timeTaken++;
        }

        lichAi.WorkingMemory.SetItem("seekSpeed", phenotype);
	}

    public void ExpressGenotype(bool[] _genotype)
    {
        genotype = _genotype;
        phenotype = MapGeneToPhen(_genotype);
    }

    // Mapping function to express a genotype as a float which will then be sent to the lichKing's Ai.Motor.Speed
    float MapGeneToPhen(bool[] gene)
    {
        float pheno = 1.0f; // Lowest possible value (all booleans false in genotype) results in speed of 1

        foreach (bool b in gene)
        {
            if (b)
                pheno += 1.0f;  // Speed increases by 1 for each true bool, doesn't increase for each false
        }

        return pheno;
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
