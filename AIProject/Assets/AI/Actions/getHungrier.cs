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

        // Get the current value saved as local variable
        int hunger = ai.WorkingMemory.GetItem<int>("hunger");

        // Increase the hunger level of the character
        hunger++;
        
        // Save the new value to the AI memory
        ai.WorkingMemory.SetItem<float>("hunger", hunger);
        

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}