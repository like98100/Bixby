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
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public FloatReference Variable = new FloatReference(VarRefMode.DisableConstant);
        

        public float UpdateInterval = 3.0f;
        public float Time_ = 0.0f;
        
        public float AttackRange = 0.0f;


        public override NodeResult Execute()
        {
            Transform target = TargetPosition.Value;
            Transform self = ObjRef.Value.transform;
            Vector3 dir = target.position - self.position;

            Time_ += Time.deltaTime * 1.5f;

            if((Time_ > UpdateInterval) || ((int)ObjRef.Value.GetComponent<Enemy>().State == 3) ||
                (ObjRef.Value.GetComponent<Enemy>().Stat.hp <= 0) || (Variable.Value > AttackRange))
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
