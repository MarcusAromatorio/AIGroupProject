using UnityEngine;
using System.Collections;
    /*
     *   The AIController is the "brain" of the calculations done in the AI of the actors in-game.
     *
     *   Flee is calculated using 2D vectors, the arguments of which are stored in two float arrays.
     *       These two arrays are populated from all of the controlled GameObjects' Transform member variables.
     *       Changes done to these two arrays directly alter the location of actors in game.
    **/
public class AIController : MonoBehaviour {

    // Reference to each actor's respective x and y locations held in these arrays
    private float[] actorX;
    private float[] actorY;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
