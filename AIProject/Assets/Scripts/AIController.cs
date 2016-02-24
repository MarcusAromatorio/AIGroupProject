using UnityEngine;
using System.Collections;
    /*
         The AIController is the "brain" of the calculations done in the AI of the actors in-game.
      
         Flee is calculated using 2D vectors, the arguments of which are stored in two float arrays.
             These two arrays are populated from all of the controlled GameObjects' Transform member variables.
             Changes done to these two arrays directly alter the location of actors in game.
     */
public enum Targets
{
    player
};

public enum Directives
{
    seek,
    flee
};
public class AIController : MonoBehaviour {

    // Constants used at start-up
    private const int NUM_ACTORS = 32;
    private const float HALF_WIDTH = 10.0f;
    private const float HALF_HEIGHT = 4.5f;

    // Reference to each actor's respective fields
    private AgentBase[] actors;
    private Targets[] actorTargets;
    private Directives[] actorDirectives;

    // Prefab used to instantiate controlled actors
    public AgentBase controlledEntity;

    // Target that represents the first "player" tagged object found
    public Vector3 playerLocation;
    public GameObject playerTarget;

    // Counter to move through the arrays
    public int actorIndex;

	// Use this for initialization
	void Start () {
        actorIndex = 0;
        actors = new AgentBase[NUM_ACTORS];
        actorTargets = new Targets[NUM_ACTORS];
        actorDirectives = new Directives[NUM_ACTORS];

        // Save a reference to the player object
        playerTarget = GameObject.FindGameObjectWithTag("Player"); // Possible error: Make sure a player exists in-game at start up
        playerLocation = playerTarget.transform.position;
        
        // Instantiating each actor with a random location within view
        float x;
        float y;
        for(int i = 0; i < NUM_ACTORS; i++)
        {
            x = Random.Range(-HALF_WIDTH, HALF_WIDTH);
            y = Random.Range(-HALF_HEIGHT, HALF_HEIGHT);
            actors[i] = (AgentBase)Instantiate(controlledEntity, new Vector3(x, y), Quaternion.identity);
            actorTargets[i] = Targets.player; // Target the indexed actor at the player
        }

	}
	
	// Update is called once per frame
    // The behavior tree will probably grow here when we learn its implementation
	void Update () {
        // Each update call only calculates target/directive for one Actor, iterating a counter through the various arrays saved
        // See which target and directive the currently indexed actor has
        switch (actorTargets[actorIndex])
        {
            case Targets.player:
                // The player is the target, so send the actorListener the player position
                actors[actorIndex].Seek(playerLocation);
                break;
        }

        // Determine how to modify the actor's movement vector depending on directive
        switch (actorDirectives[actorIndex])
        {
            case Directives.seek:
                // Seeking the target (currently) needs no additional change to its vector
                break;

            case Directives.flee:
                // Fleeing requires the vector be inverted
                // actorMovementVectors[actorIndex] *= -1;
                break;
        }

        // Generic actors should automatically move towards the player tagged object
        // Make sure the target is still accurate by updating it
        playerLocation = playerTarget.transform.position;

        // At the end of update, increment the actorIndex and make sure it doesn't exceed NUM_ACTORS
        actorIndex++;
        if(actorIndex >= NUM_ACTORS)
        {
            actorIndex = 0;
        }
	}
}
