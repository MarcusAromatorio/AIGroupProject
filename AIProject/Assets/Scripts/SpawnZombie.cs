using UnityEngine;
using System.Collections;
using System.Collections.Generic;

    /*
        The SpawnZombie class handles spawning zombies and keeping track of a stack of pre made objects
        Internally creates a Stack data structure of Zombie class instances
        Meant to be attached to the GameController object to encapsulate the process of making a zombie
    */

public class SpawnZombie : MonoBehaviour {

    public Zombie spawnPrefab;
    private int numPremadeZombies;
    private Stack<Zombie> premadeZombies;

	// Use this for initialization
	void Start () {
        // Populate the Stack with premade objects
        numPremadeZombies = 10;
        for(int i = 0; i < numPremadeZombies; i++)
        {
            Zombie z = (Zombie)Instantiate(spawnPrefab, Vector3.zero, Quaternion.identity);
            z.enabled = false;
            premadeZombies.Push(z);
        }
	}

    public Zombie Spawn(Vector3 position, Quaternion rotation)
    {
        if(premadeZombies.Peek() != null)
        {
            Zombie z = premadeZombies.Pop();
            z.enabled = true;
            z.transform.position = position;
            z.transform.rotation = rotation;
            return z;
        }
        else
        {
            return (Zombie)Instantiate(spawnPrefab, position, rotation);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
