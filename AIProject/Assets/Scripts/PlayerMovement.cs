using UnityEngine;
using System.Collections;

    /*
        The PlayerMovement class allows the player to move a gameobject with arrow keys or WASD
        The script finds all objects with the tag of "player" and controls their movements
     */
public class PlayerMovement : MonoBehaviour {

    // Array to contain pointers to each object tagged "player"
    public GameObject[] playerObjects;

    // Variable to determine player movement speed
    public float playerSpeed = 10.0f;

    // Internal variables for proper execution
    private bool isEmpty;
    private float horizontalAxis;
    private float verticalAxis;

	// Use this for initialization
	void Start () {
        // Find all player objects
        playerObjects = GameObject.FindGameObjectsWithTag("Player");

        // If objects were found, isEmpty is false
        if (playerObjects.Length > 0)
        {
            isEmpty = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    // Only bother catching input if the array is populated
        if(!isEmpty)
        {
            // Store the movement across 2 Axes from input (catches WASD & arrows)
            horizontalAxis = playerSpeed * Input.GetAxis("Horizontal");
            verticalAxis = playerSpeed * Input.GetAxis("Vertical");

            // Scale each axis by Time.deltaTime
            horizontalAxis *= Time.deltaTime;
            verticalAxis *= Time.deltaTime;

            // Translate each player accordingly
            foreach(GameObject player in playerObjects)
            {
                player.transform.Translate(horizontalAxis, verticalAxis, 0.0f);
            }
        }
	}
}
