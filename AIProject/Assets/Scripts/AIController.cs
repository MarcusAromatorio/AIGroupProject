using UnityEngine;
using System.Collections;
using System.Collections.Generic;
    /*
         The AIController is the "brain" of the calculations done in the AI of the agents in-game.
      
         Flee is calculated using 2D vectors, the arguments of which are stored in two float arrays.
             These two arrays are populated from all of the controlled GameObjects' Transform member variables.
             Changes done to these two arrays directly alter the location of agents in game.
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
    private const int NUM_AGENTS = 32;
    private const float HALF_WIDTH = 10.0f;
    private const float HALF_HEIGHT = 4.5f;

    // Reference to each actor's respective fields

    private AgentBase[] agents;
    private Targets[] agentTargets;
    private Directives[] agentDirectives;


    // Prefab used to instantiate controlled agents
    public AgentBase controlledEntity;

    // Target that represents the first "player" tagged object found
    public Vector3 playerLocation;
    public GameObject playerTarget;

    // Counter to move through the arrays
    public int agentIndex;

	// Use this for initialization
	void Start () {

        agentIndex = 0;
        agents = new AgentBase[NUM_AGENTS];
        agentTargets = new Targets[NUM_AGENTS];
        agentDirectives = new Directives[NUM_AGENTS];

        // Save a reference to the player object
        playerTarget = GameObject.FindGameObjectWithTag("Player"); // Possible error: Make sure a player exists in-game at start up
        playerLocation = playerTarget.transform.position;
        
        // Instantiating each actor with a random location within view
        float x;
        float z;
        for(int i = 0; i < NUM_AGENTS; i++)
        {
            x = Random.Range(-HALF_WIDTH, HALF_WIDTH);
            z = Random.Range(-HALF_HEIGHT, HALF_HEIGHT);
            agents[i] = (AgentBase)Instantiate(controlledEntity, new Vector3(x, 0, z), Quaternion.identity);
            agentTargets[i] = Targets.player; // Target the indexed agent at the player
        }

	}
	
	// Update is called once per frame
    // The behavior tree will probably grow here when we learn its implementation
	void Update () {
        // Each update call only calculates target/directive for one Agent, iterating a counter through the various arrays saved
        // See which target and directive the currently indexed actor has
        switch (agentTargets[agentIndex])
        {
            case Targets.player:
                // The player is the target, so tell the agent to seek the player
                agents[agentIndex].Seek(playerLocation);
                break;
        }

        // Determine how to modify the agent's movement vector depending on directive
        switch (agentDirectives[agentIndex])
        {
            case Directives.seek:
                // Seeking the target (currently) needs no additional change to its vector
                break;

            case Directives.flee:
                // Fleeing requires the vector be inverted
                // actorMovementVectors[agentIndex] *= -1;
                break;
        }

        // Generic agents should automatically move towards the player tagged object
        // Make sure the target is still accurate by updating it
        playerLocation = playerTarget.transform.position;

        // At the end of update, increment the agentIndex and make sure it doesn't exceed NUM_AGENTS
        agentIndex++;
        if(agentIndex >= NUM_AGENTS)
        {
            agentIndex = 0;
        }
	}
}
