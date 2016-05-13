using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

using RAIN.Core;

public class BayesID3 : MonoBehaviour {

    public GameManager gm;

    //3 Categories, the end result(boolean) will be to eat a zombie or not
    //This will affect the ghoul
    struct Observation
    {
        public int zombieCount;        //if #zombies goes over amount  (x>=15 = true)
        public int skeleCount;         //if #skeletons goes over amount (x>=5 = true)
        public bool hot;                //Hot or not
        public bool eat;                //result, to eat zombies or not
    }

    //Changeable Hot bool via hotkey
    private bool changeHot = false;
    private int zCount = 0;
    private int sCount = 0;

    private List<Observation> tempObs = new List<Observation>();

    //RAIN AI
    private AIRig ghoulAIRig;
    private AI ghoulAI;


    //Bayes
    double sqrt2PI = Math.Sqrt(2.0 * Math.PI);

    List<Observation> obsTab = new List<Observation>();

    int[] zombieSum = new int[2];
    int[] zombieSumSq = new int[2];
    double[] zombieMean = new double[2];
    double[] zombieStdDev = new double[2];

    int[] skeleSum = new int[2];
    int[] skeleSumSq = new int[2];
    double[] skeleMean = new double[2];
    double[] skeleStdDev = new double[2];

    int[,] hotCt = new int[2, 2];
    double[,] hotPrp = new double[2, 2];

    int[] eatCt = new int[2];
    double[] eatPrp = new double[2];	


	// Use this for initialization
	void Start () {
        InitStats();
        InitStartingTable();
        BuildStats();

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        ghoulAIRig = gameObject.GetComponentInChildren<AIRig>();
        ghoulAI = ghoulAIRig.AI;
	}
	
	// Update is called once per frame
	void Update () {
        //Hotkey to change current hotness
        if (Input.GetKeyDown("h"))
        {
            changeHot = !changeHot;
        }

        //Hotkey to update Bayes
        if (Input.GetKeyDown("b"))
        {
            Debug.Log("Added to observations");
            foreach (Observation o in tempObs)
            {
                AddToObs(o.zombieCount, o.skeleCount, o.hot, o.eat);
            }
        }
        BuildStats();
        zCount = gm.zombies.Count;
        sCount = gm.skeletons.Count;

        bool temp = Decide(zCount, sCount, changeHot);
        Debug.Log(zCount + " " + sCount + " " + changeHot);
        Debug.Log("Decision: " + temp);
        AddTempObs(zCount, sCount, changeHot, Decide(zCount, sCount, changeHot));
        if (temp)
        {
            Enable();
        }
        else
        {
            Disable();
        }
	}

    void InitStats()
    {
        for (int i = 0; i < zombieSum.Length; i++)
        {
            zombieSum [i] = 0;
            zombieSumSq [i] = 0;
        }

        for (int i = 0; i < skeleSum.Length; i++)
        {
            skeleSum[i] = 0;
            skeleSumSq[i] = 0;
        }

        for (int i = 0; i < 2; i++)
        {
            eatCt[i] = 0;
        }
    }

    //Unlike the demo which is read from file, this will be done manually
    void InitStartingTable()
    {
        //These will be the starting data values.  They were chosen via RNG.
        //9, 16, 21, 4, 11, 20, 24, 3, 9, 15
        //7, 10, 4, 3, 3, 10, 4, 5, 2, 3
        //1, 2, 1, 2, 1, 1, 1, 2, 2, 1

        /*
        AddToObs(9, 7, true, false);
        AddToObs(16, 10, false, true);
        AddToObs(21, 4, true, true);
        AddToObs(4, 3, false, false);
        AddToObs(1, 3, true, false);
        AddToObs(20, 10, true, true);
        AddToObs(24, 4, true, true);
        AddToObs(3, 5, false, false);
        AddToObs(9, 2, false, true);
        AddToObs(15, 3, true, true);
         */

        for (int i = 0; i < 100; i++)
        {
            bool h;
            bool e = true;
            if (UnityEngine.Random.Range(0, 1) == 0){
                h = true;
            }else{
                h = false;
            }
            int z = UnityEngine.Random.Range(0, 50);
            int s = UnityEngine.Random.Range(0, 20);

            if (h)
            {
                e = false;
                if (z >= 20)
                {
                    e = true;
                }
                if (s >= 10)
                {
                    e = false;
                } 
            }
            else
            {
                e = true;
                if (s >= 10)
                {
                    e = false;
                }
            }

           AddToObs(z, s, h, e);
        }
    }

    void AddToObs(int z, int s, bool h, bool e)
    {
        Observation obs;
        obs.zombieCount = z;
        obs.skeleCount = s;
        obs.hot = h;
        obs.eat = e;
        obsTab.Add(obs);
    }

    void AddTempObs(int z, int s, bool h, bool e)
    {
        Observation obs;
        obs.zombieCount = z;
        obs.skeleCount = s;
        obs.hot = h;
        obs.eat = e;
        tempObs.Add(obs);
    }

    void BuildStats()
    {
        InitStats();

        // Accumulate all the counts and sums
        foreach (Observation obs in obsTab)
        {
            // Do this once
            int eatOff = obs.eat ? 0 : 1;

            zombieSum[eatOff] += obs.zombieCount;
            zombieSumSq[eatOff] += obs.zombieCount * obs.zombieCount;

            skeleSum[eatOff] += obs.skeleCount;
            skeleSumSq[eatOff] += obs.skeleCount * obs.skeleCount;

            hotCt[obs.hot ? 0 : 1, eatOff]++;

            eatCt[eatOff]++;
        }

        //Calculate
        zombieMean[0] = Mean(zombieSum[0], eatCt[0]);
        zombieMean[1] = Mean(zombieSum[1], eatCt[1]);
        zombieStdDev[0] = StdDev(zombieSumSq[0], zombieSum[0], eatCt[0]);
        zombieStdDev[1] = StdDev(zombieSumSq[1], zombieSum[1], eatCt[1]);

        skeleMean[0] = Mean(skeleSum[0], eatCt[0]);
        skeleMean[1] = Mean(skeleSum[1], eatCt[1]);
        skeleStdDev[0] = StdDev(skeleSumSq[0], skeleSum[0], eatCt[0]);
        skeleStdDev[1] = StdDev(skeleSumSq[1], skeleSum[1], eatCt[1]);

        CalcProps(hotCt, eatCt, hotPrp);

        eatPrp[0] = (double)eatCt[0] / obsTab.Count;
        eatPrp[1] = (double)eatCt[1] / obsTab.Count;
    }

    // Calculates the proportions for a discrete table of counts
    // Handles the 0-frequency problem by assigning an artificially
    // low value that is still greater than 0.
    void CalcProps(int[,] counts, int[] n, double[,] props)
    {
        for (int i = 0; i < counts.GetLength(0); i++)
            for (int j = 0; j < counts.GetLength(1); j++)
                // Detects and corrects a 0 count by assigning a proportion
                // that is 1/10 the size of a proportion for a count of 1
                if (counts[i, j] == 0)
                    props[i, j] = 0.1d / eatCt[j];	// Can't have 0
                else
                    props[i, j] = (double)counts[i, j] / n[j];
    }
    double Mean(int sum, int n)
    {
        return (double)sum / n;
    }

    double StdDev(int sumSq, int sum, int n)
    {
        return Math.Sqrt((sumSq - (sum * sum) / (double)n) / (n - 1));
    }

    // Calculates probability of x in a normal distribution of
    // mean and stdDev.  This corrects a mistake in the pseudo-code,
    // which used a power function instead of an exponential.
    double GauProb(double mean, double stdDev, int x)
    {
        double xMinusMean = x - mean;
        return (1.0d / (stdDev * sqrt2PI)) *
        Math.Exp(-1.0d * xMinusMean * xMinusMean / (2.0d * stdDev * stdDev));
    }
    // Bayes likelihood for four condition values and one action value
    // For each possible action value, call this with a specific set of four
    // condition values, and pick the action that returns the highest
    // likelihood as the most likely action to take, given the conditions.
    double CalcBayes(int z, int s, bool h, bool e)
    {
        int eatOff = e ? 0 : 1;
        double like = GauProb(zombieMean[eatOff], zombieStdDev[eatOff], z) *
                      GauProb(skeleMean[eatOff], zombieStdDev[eatOff], s) *
                      hotPrp[h ? 0 : 1, eatOff] *
                      eatPrp[eatOff];
        return like;
    }
    // Decide whether to play or not.
    // Returns true if decision is to play, false o/w
    // Can turn on/off diagnostic output to Console by playing with "*/"
    public bool Decide(int z, int s, bool h)
    {
        double playYes = CalcBayes(z, s, h, true);
        double playNo = CalcBayes(z, s, h, false);

        /* To turn off output, remove this end comment -> 
        double yesNno = playYes + playNo;
        Console.WriteLine("playYes: {0}", playYes);	// Use scientifice notation
        Console.WriteLine("playNo:  {0}", playNo);		// for very small numbers
        Console.WriteLine("playYes Normalized: {0,6:F4}", playYes / yesNno);
        Console.WriteLine("playNo  Normalized: {0,6:F4}", playNo / yesNno);
        /* */

        return playYes > playNo;
    }


    // Method to stop the AI and child components from updating
    public void Disable()
    {
        if (ghoulAI.Started)
        {
            ghoulAI.IsActive = false;
        }
    }

    // Method to start the AI and child components after being disabled
    public void Enable()
    {
        if (ghoulAI.Started & !ghoulAI.IsActive)
        {
            ghoulAI.IsActive = true;
        }
    }
}
