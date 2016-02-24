﻿using UnityEngine;
using System.Collections;

    /*
        The Actor class represents a movable character on-screen, meant to be a superclass of custom characters.
        The purpose of Actor specifically is to supply a standard representation of control for actor subclasses.
        Currently, the actor will be a demonstration class, but should be virtualized in later versions when actor-specific AI is implemented
    */

public class Actor : MonoBehaviour {

    // Constants of the class include properties of movement like maxSpeed and maxTurnSpeed
    public const float MAX_SPEED = 2.5f;
    public const float MAX_TURN_DEGREES = 20.0f;

    // Members of each actor include vectors describing acceleration and velocity
    // When the actor is active, acceleration is set to zero each frame
    public Vector3 acceleration;
    public Vector3 velocity;
    public Vector3 target;
    public float rotation;

    // Used in referencing to specific objects in the array in AIController
    public int aiIndex;

	// Use this for initialization
	void Start () {
        acceleration = new Vector3();
        velocity = transform.forward;
        rotation = 0;
	}
	
	// Update is called once per frame
	void Update () {

        // Rotate by desired degrees
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        rotation = 0; // Reset rotation 

        velocity += acceleration;

        // Constrain velocity to MAX_SPEED
        velocity = Vector3.ClampMagnitude(velocity, MAX_SPEED);

        // Add velocity to transform.position
        transform.position += velocity * Time.deltaTime;

        // Reset acceleration
        acceleration = Vector3.zero;

        RecalculateTarget();

        // Debug.DrawLine(transform.position, transform.position + velocity);
	}

    // Recalculate where the Actor will move depending on the target given
    // Does nothing if target is null
    // Generic behavior is to seek the target by setting velocity & rotation
    private void RecalculateTarget()
    {
        rotation = -Mathf.Atan2(target.x - transform.position.x, target.y - transform.position.y);
        rotation = rotation * (180 / Mathf.PI);

        //turning rotatio into 0-360 (if not it'd be -180 to 180)
        if (rotation < 0)
        {
            rotation = 360 - (-rotation);
        }

        // The following two comparisons clamp rotation to be at or under max
        //if (rotation > MAX_TURN_DEGREES)
        //    rotation = MAX_TURN_DEGREES;
        //else if (rotation < -MAX_TURN_DEGREES)
        //    rotation = -MAX_TURN_DEGREES;

        acceleration = target - transform.position;
        
    }

    #region Accessors_And_Mutators
    public  void SetVelocity(Vector3 _velocity)
    {
        velocity = _velocity;
    }
    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public void SetAcceleration(Vector3 _acceleration)
    {
        acceleration = _acceleration;
    }
    public Vector3 getAcceleration()
    {
        return acceleration;
    }

    public void SetRotation(float _rotation)
    {
        rotation = _rotation;
    }
    public float GetRotation()
    {
        return rotation;
    }

    public void SetAiIndex(int _index)
    {
        aiIndex = _index;
    }
    public int GetAiIndex()
    {
        return aiIndex;
    }

    public void setTarget(Vector3 _target)
    {
        target = _target;
    }
    public Vector3 getTarget()
    {
        return target;
    }
    #endregion

}
