using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/MeleeDashAttack")]
    public class MeleeDashAttack : Leaf
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public GameObjectReference TargetObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public FloatReference Variable = new FloatReference(VarRefMode.DisableConstant);
        public float Speed = 3.0f;

        public float UpdateInterval = 3.0f;
        public float Time_ = 0.0f;

        public Vector3 targetVec;

        public override void OnEnter()
        {
            targetVec = TargetObjRef.Value.transform.position;
            if (ObjRef.Value.tag == "FinalBoss")
            {
                ObjRef.Value.GetComponent<FinalBoss>().Anim.SetTrigger("isDashAttack");
                ObjRef.Value.GetComponent<FinalBoss>().isAttacked = true;
            }
        }

        public override NodeResult Execute()
        {
            // int Element;
            // CheckElements
            // Element = (int)ObjRef.Value.GetComponent<Enemy>().Element;

            Transform self = ObjRef.Value.transform;
            Transform target;
            if (TargetObjRef.Value == null)
                return NodeResult.failure;
            else    
                target = TargetObjRef.Value.transform;
            Vector3 dir = target.position - self.position;
            
            Time_ += Time.deltaTime * 1.5f;

            if(!ObjRef.Value.GetComponent<FinalBoss>().isAttacked)
            {
                // Reset time and update destination
                Time_ = 0.0f;
                return NodeResult.success;
            }
            else if (Time_ <= 1.0f)
                self.rotation = Quaternion.Lerp(self.rotation, Quaternion.LookRotation(dir), 
                                          Time.deltaTime * 20.0f);
            else
                ObjRef.Value.transform.position = Vector3.MoveTowards(transform.position, 
                                                                    targetVec, 
                                                                    Speed * Time.deltaTime);

            return NodeResult.running;
        }

        public override void OnExit()
        {

        }
            
    }
}
