using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;
using RAIN.Navigation;
using RAIN.Navigation.Graph;
using RAIN.Entities.Aspects;

[RAINAction]
public class SetChaseTarget : RAINAction
{
    public Expression avoidRange;

    private Vector3 _target;
    private IList<RAINAspect> _targetsToChase;
    private float range;
    private Vector3 between;
    private Vector3 avoidVector;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        _targetsToChase = ai.Senses.Match("eyes", "Zombie");

        if (!float.TryParse(avoidRange.ExpressionAsEntered, out range))
            range = 2f;
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        int rng = Random.Range(0, _targetsToChase.Count);
       
        Chase(ai, _targetsToChase[rng]);
        

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }

    private void Chase(AI ai, RAINAspect aspect)
    {
        ai.Motor.MoveTo(aspect.Position);
    }



    private bool CheckPositionOnNavMesh(Vector3 loc, AI ai)
    {
        RAIN.Navigation.Pathfinding.RAINPath myPath = null;
        if (ai.Navigator.GetPathTo(loc, 10, true, out myPath))
            return true;

        return false;
    }
}