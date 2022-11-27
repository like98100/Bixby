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
            if (ObjRef.Value.tag == "DungeonBoss")
            {
                targetVec += TargetObjRef.Value.transform.forward*3.0f;
                ObjRef.Value.GetComponent<DungeonBoss>().Anim.SetTrigger("isDashAttacked");
                ObjRef.Value.GetComponent<DungeonBoss>().isAttacked = true;
            }
            else if (ObjRef.Value.tag == "FinalBoss")
            {
                targetVec += TargetObjRef.Value.transform.forward*5.0f;
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

            // if (ObjRef.Value.tag == "Enemy")
            // {
            //     if(!ObjRef.Value.GetComponent<Enemy>().isAttacked)
            //         return NodeResult.success;
            // }
            if (ObjRef.Value.tag == "DungeonBoss")
            {
                if(!ObjRef.Value.GetComponent<DungeonBoss>().isAttacked)
                    return NodeResult.success;
            }
            else if (ObjRef.Value.tag == "FinalBoss")
            {
                if(!ObjRef.Value.GetComponent<FinalBoss>().isAttacked)
                    return NodeResult.success;
            }
            
            if (Time_ <= 1.0f)
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
            Time_ = 0.0f;
        }
            
    }
}
