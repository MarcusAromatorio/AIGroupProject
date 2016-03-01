using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class getHungrier : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        // Get the current values saved as local variables
        float hunger = (float)ai.WorkingMemory.GetItem("hunger");
        bool hungry = (bool)ai.WorkingMemory.GetItem("isHungry");

        // See if hunger is under half, making the zombie "hungry"
        if(hunger < 50.0f)
        {
            hungry = true;// Set the new isHungry value here
            ai.WorkingMemory.SetItem<bool>("isHungry", hungry);
        }

        // Decrement float and save new value for hunger
        hunger -= 1.0f;
        ai.WorkingMemory.SetItem<float>("hunger", hunger);

        Debug.Log("Hunger: " + hunger);

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}