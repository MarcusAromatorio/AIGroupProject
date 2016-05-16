using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class eatZombie : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        if (ai.WorkingMemory.GetItem<GameObject>("chaseTarget").GetComponent<Zombie>() != null)
        {
            GameManager g = GameObject.Find("GameManager").GetComponent<GameManager>();
            g.killZombie(ai.WorkingMemory.GetItem<GameObject>("chaseTarget").GetComponent<Zombie>());
        }
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}