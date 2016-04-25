using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class EnthrallZombie : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        Zombie z = ai.WorkingMemory.GetItem<GameObject>("nearestZombie").GetComponent<Zombie>();
        z.Enthrall(ai.Body);
        // Count up one for numberOfFollowers
        //int numFollowers = ai.WorkingMemory.GetItem<int>("numberOfFollowers");
        //numFollowers++;
        //ai.WorkingMemory.SetItem<int>("numberOfFollowers", numFollowers);

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}