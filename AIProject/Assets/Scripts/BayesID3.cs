using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class BayesID3 : MonoBehaviour {

    //3 Categories, the end result(boolean) will be to eat a zombie or not
    //This will affect the ghoul
    struct Observation
    {
        public bool zombieCount;        //if #zombies goes over amount  (x>=15 = true)
        public bool skeleCount;         //if #skeletons goes over amount (x>=5 = true)
        public bool hot;                //Hot or not
        public bool eat;                //result, to eat zombies or not
    }

    double sqrt2PI = Math.Sqrt(2.0 * Math.PI);

    List<Observation> obsTab = new List<Observation>();

    int[,] zombieCt = new int[2, 2];
    double[,] zombiePrp = new double[2, 2];

    int[,] skeleCt = new int[2, 2];
    double[,] skelePrp = new double[2, 2];

    int[,] hotCt = new int[2, 2];
    double[,] hotPrp = new double[2, 2];

    int[] eatCt = new int[2];
    double[] eatPrp = new double[2];	


	// Use this for initialization
	void Start () {
        InitStats();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void InitStats()
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                zombieCt[i, j] = 0;
                skeleCt[i, j] = 0;
            }
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
        //if #zombies goes over amount  (x>=15 = true)
        //if #skeletons goes over amount (x>=5 = true)
        //9, 16, 21, 4, 11, 20, 24, 3, 9, 15
        //7, 10, 4, 3, 3, 10, 4, 5, 2, 3
        //1, 2, 1, 2, 1, 1, 1, 2, 2, 1
        //2, 1, 2, 1, 2, 1, 1, 2, 1, 1
        AddToObs(false, true, true, false);
        AddToObs(true, true, false, true);
        AddToObs(true, false, true, false);
        AddToObs(false, false, false, true);
        AddToObs(false, false, true, false);
        AddToObs(true, true, true, true);
        AddToObs(true, false, true, true);
        AddToObs(false, true, false, false);
        AddToObs(false, false, false, true);
        AddToObs(true, false, true, true);
    }

    void AddToObs(bool z, bool s, bool h, bool e)
    {
        Observation obs;
        obs.zombieCount = z;
        obs.skeleCount = s;
        obs.hot = h;
        obs.eat = e;
        obsTab.Add(obs);
    }

    void BuildStats()
    {
        InitStats();

        // Accumulate all the counts and sums
        foreach (Observation obs in obsTab)
        {
            // Do this once
            int eatOff = obs.eat ? 0 : 1;

            zombieCt[obs.zombieCount ? 0 : 1, eatOff]++;
            skeleCt[obs.skeleCount ? 0 : 1, eatOff]++;
            hotCt[obs.hot ? 0 : 1, eatOff]++;

            eatCt[eatOff]++;
        }

        CalcProps(zombieCt, eatCt, zombiePrp);
        CalcProps(skeleCt, eatCt, skelePrp);
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
    double CalcBayes(bool z, bool s, bool h, bool e)
    {
        int eatOff = e ? 0 : 1;
        double like = zombiePrp[z ? 0 : 1, eatOff] *
                      skelePrp[s ? 0 : 1, eatOff] *
                      hotPrp[h ? 0 : 1, eatOff] *
                      eatPrp[eatOff];
        return like;
    }
    // Decide whether to play or not.
    // Returns true if decision is to play, false o/w
    // Can turn on/off diagnostic output to Console by playing with "*/"
    public bool Decide(bool z, bool s, bool h)
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
}
