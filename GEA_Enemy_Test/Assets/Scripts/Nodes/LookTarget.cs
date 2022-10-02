using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/Look Target")]
    public class LookTarget : Leaf
    {
        public TransformReference targetPosition;
        public TransformReference selfPosition;

        public float updateInterval = 3.0f;
        public float time = 0.0f;

        public override NodeResult Execute()
        {
            Transform target = targetPosition.Value;
            Transform self = selfPosition.Value;
            Vector3 dir = target.position - self.position;
            
            time += Time.deltaTime * 1.5f;

            if(time > updateInterval)
            {
                // Reset time and update destination
                time = 0;
                return NodeResult.success;
            }
            
            self.rotation = Quaternion.Lerp(self.rotation, Quaternion.LookRotation(dir), 
                                          Time.deltaTime * 5.0f);

            return NodeResult.running;
        }
    }
}
