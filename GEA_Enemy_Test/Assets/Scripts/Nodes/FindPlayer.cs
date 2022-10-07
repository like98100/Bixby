using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Services/Find Player")]
    public class FindPlayer : Service
    {
        public LayerMask Mask = -1;
        [Tooltip("Sphere radius")]
        public float Range = 5;
        public TransformReference VariableToSet = new TransformReference(VarRefMode.DisableConstant);

        public override void Task()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, Range, Mask, QueryTriggerInteraction.Ignore);
            if (colliders.Length > 0)
            {
                VariableToSet.Value = colliders[0].transform;
            }
            else
            {
                VariableToSet.Value = null;
            }
        }
    }
}
