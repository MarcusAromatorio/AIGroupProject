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
public class AvoidCollision : RAINAction
{
    public Expression avoidRange;

    private Vector3 _target;
    private IList<RAINAspect> _targetsToAvoid;
    private float range;
    private Vector3 between;
    private Vector3 avoidVector;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        _targetsToAvoid = ai.Senses.Match("eyes", "Zombie");

        if (!float.TryParse(avoidRange.ExpressionAsEntered, out range))
            range = 2f;
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        if (_targetsToAvoid.Count == 0)
            return ActionResult.SUCCESS;

        foreach (RAINAspect aspect in _targetsToAvoid)
        {
            if (IsTooClose(ai, aspect))
                DoAvoidance(ai, aspect);
        }

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }

    private bool IsTooClose(AI ai, RAINAspect aspect)
    {
        float dist = Vector3.Distance(ai.Kinematic.Position, aspect.Position);

        if (dist <= range)
            return true;

        return false;
    }

    private void DoAvoidance(AI ai, RAINAspect aspect)
    {
        between = ai.Kinematic.Position - aspect.Position;
        avoidVector = Vector3.Cross(Vector3.up, between);

        int direction = Random.Range(0, 100);

        avoidVector.Normalize();

        if (direction < 50)
            avoidVector *= -1;

        if (!CheckPositionOnNavMesh(avoidVector, ai))
            avoidVector *= -1;

        if (!CheckPositionOnNavMesh(avoidVector, ai))
        {
            //Debug.Log("Avoid not possible!");
            return;
        }

        Vector3 destination = ai.Kinematic.Position + avoidVector;
        //ai.Motor.MoveTo(destination);
        //ai.Kinematic.Position += avoidVector;
        ai.Kinematic.Position = Vector3.Lerp(ai.Kinematic.Position, destination, 0.3f);
        ai.Motor.FaceAt(avoidVector);
    }

    private bool CheckPositionOnNavMesh(Vector3 loc, AI ai)
    {
        RAIN.Navigation.Pathfinding.RAINPath myPath = null;
        if (ai.Navigator.GetPathTo(loc, 10, true, out myPath))
            return true;

        return false;
    }
}