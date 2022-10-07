using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/Look Target")]
    public class LookTarget : Leaf
    {
        public TransformReference TargetPosition;
        public TransformReference SelfPosition;

        public float UpdateInterval = 3.0f;
        public float Time_ = 0.0f;

        public override NodeResult Execute()
        {
            Transform target = TargetPosition.Value;
            Transform self = SelfPosition.Value;
            Vector3 dir = target.position - self.position;

            Time_ += Time.deltaTime * 1.5f;

            if(Time_ > UpdateInterval)
            {
                // Reset time and update destination
                Time_ = 0;
                return NodeResult.success;
            }
            
            self.rotation = Quaternion.Lerp(self.rotation, Quaternion.LookRotation(dir), 
                                          Time.deltaTime * 5.0f);

            return NodeResult.running;
        }
    }
}
