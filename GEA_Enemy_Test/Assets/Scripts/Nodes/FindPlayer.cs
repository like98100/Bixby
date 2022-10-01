using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Service/Find Player")]
    public class FindPlayer : Service
    {
        public LayerMask mask = -1;
        [Tooltip("Sphere radius")]
        public float range = 5;
        public TransformReference variableToSet = new TransformReference(VarRefMode.DisableConstant);

        public override void Task()
        {
            // Find target in radius and feed blackboard variable with results
            Collider[] colliders = Physics.OverlapSphere(transform.position, range, mask, QueryTriggerInteraction.Ignore);
            if (colliders.Length > 0)
            {
                variableToSet.Value = colliders[0].transform;
            }
            else
            {
                variableToSet.Value = null;
            }
        }
    }
}
