using UnityEngine;
using RAIN.Core;
using System.Collections;

public class Skeleton : MonoBehaviour {

    public bool isTail;
    public bool isHead;
    public Skeleton following;
    public Skeleton isFollowedBy;
    public Vector3 rear;
    private AI aiClass;

	// Use this for initialization
	void Start () {
        isTail = true;
        isHead = false;
        rear = transform.forward * -1;

        AIRig aiComponent = gameObject.GetComponentInChildren<AIRig>();
        aiClass = aiComponent.AI;
	}
	
	// Update is called once per frame
	void Update () {
        // Rear is updated consistently
        rear = transform.forward * -1;
        if(!isHead && following != null)
        {
            aiClass.WorkingMemory.SetItem<GameObject>("following", following.gameObject);
        }
	}


}
