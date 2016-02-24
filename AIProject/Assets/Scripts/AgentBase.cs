using UnityEngine;
using System.Collections;

    /**
        The AgentBase is the default implementation of the basic steering algorithms described in IAgent
        Early implementation keeps track of a target, rotation, acceleration, and velocity of the Agent
    */

public class AgentBase : MonoBehaviour, IAgent {

    // Members of the class are Vector3 references that describe motion
    private Vector3 acceleration;
    private Vector3 velocity;
    private Quaternion rotation;

    // Constants of the class include properties of movement like maxSpeed and maxTurnSpeed
    public const float MAX_SPEED = 2.5f;
    public const float MAX_TURN_SPEED = 20.0f;

    // Use this for initialization
    void Start () {
        acceleration = new Vector3();
        velocity = new Vector3(); 
        rotation = Quaternion.identity;
	}
	
	// Update is called once per frame
	void Update () {
        // Smooth linear interpolation allows for a natural turning to a target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * MAX_TURN_SPEED);

        // Taking the acceleration vector (which points directly to a calculated target through steering alg's)
        // and projecting it onto the velocity vector (via dot product) will allow the agent to move at appropriate speeds
        velocity += acceleration;
        velocity = Vector3.ClampMagnitude(velocity, MAX_SPEED);
        transform.position += velocity * Time.deltaTime;

        // Zero out acceleration, as any steering should be recalculated frequently
        acceleration = Vector3.zero;
	}

    public void Seek(Vector3 target)
    {
        // Set the target acceleration to point at the argument
        acceleration = target - transform.position;

        // Start calculating how far to rotate so that the Agent looks at the target
        float polarRotation = -Mathf.Atan2(target.x - transform.position.x, target.y - transform.position.y);
        float degreeRotation = polarRotation * (180 / Mathf.PI); // Converted to degrees

        // Account for half of the conversion resulting in negative
        degreeRotation = 360.0f + degreeRotation;

        // Save the Quaternion rotation from the calculated scalar value
        rotation = Quaternion.Euler(0, 0, degreeRotation);
    }

    public void Flee(Vector3 target)
    {
        // The target will be manipulated to point in the opposite direction, then control will defer to Seek
        target = transform.position - target;
        target *= 2.0f;
        Seek(target);
    }
}
