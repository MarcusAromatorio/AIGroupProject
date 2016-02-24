using UnityEngine;
using System.Collections;

    /**
        IAgent is an interface for Agent subclasses
        AgentBase implements IAgent and implements the basic steering algorithms Seek and Flee
        Further steering algorithms that will be implemented across all agents will be added here
    */

public interface IAgent {

    void Seek(Vector3 target);
    void Flee(Vector3 target);
}
