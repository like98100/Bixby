using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Services/Find Player Service")]
    public class FindPlayerService : Service
    {
        public LayerMask mask = -1;
        [Tooltip("Sphere radius")]
        public float range = 5;
        public TransformReference variableToSet = new TransformReference(VarRefMode.DisableConstant);

        public float updateInterval = 5.0f;
        public float time = 0.0f;

        public override void Task()
        {
            // Find target in radius and feed blackboard variable with results
            Collider[] colliders = Physics.OverlapSphere(transform.position, range, mask, QueryTriggerInteraction.Ignore);
            
            time += Time.deltaTime * 1.5f;

            if(time > updateInterval)
            {
                // Reset time and update destination
                time = 0;
            }

            if (colliders.Length > 0)
            {
                for(int i = 0; i < colliders.Length; i++)
                {
                    if(colliders[i].tag == "Player")
                    {
                        variableToSet.Value = colliders[i].transform;
                    }
                }//variableToSet.Value = colliders[0].transform;
            }
            else
            {
                variableToSet.Value = null;
            }
        }
    }
}
