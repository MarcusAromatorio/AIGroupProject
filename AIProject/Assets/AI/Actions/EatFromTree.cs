using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class EatFromTree : RAINAction
{
    // Potential to change hunger reduction extesions
    private int hungerDrain;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        hungerDrain = 100;
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {

        // Three steps: Read hunger from memory, reduce by hungerDrain, save hunger to memory
        int hungerLevel = ai.WorkingMemory.GetItem<int>("hunger");
        hungerLevel -= hungerDrain;
        ai.WorkingMemory.SetItem<int>("hunger", hungerLevel);

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}