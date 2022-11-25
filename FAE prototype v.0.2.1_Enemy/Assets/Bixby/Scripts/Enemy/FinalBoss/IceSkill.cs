using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/Ice Skill")]
    public class IceSkill : Leaf
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public GameObjectReference targetRef = new GameObjectReference(VarRefMode.DisableConstant);
        public FloatReference dist = new FloatReference(VarRefMode.DisableConstant);
        public Vector3 targetVec;

        public float UpdateInterval = 3.5f;
        public float Time_ = 0.0f;

        public override void OnEnter()
        {
            targetVec = targetRef.Value.transform.position;

            ObjRef.Value.GetComponent<FinalBoss>().Anim.SetTrigger("Ice");
        } 

        public override NodeResult Execute()
        {
            Time_ += Time.deltaTime;

            if(Time_ > UpdateInterval)
            {
                // Reset time and update destination
                Time_ = 0.0f;
                return NodeResult.success;
            }
            
            ObjRef.Value.transform.position = Vector3.MoveTowards(transform.position, 
                                                        targetVec, 
                                                        15.0f * Time.deltaTime);

            return NodeResult.running;
        }

        public override void OnExit()
        {
            //ObjRef.Value.GetComponent<FinalBoss>().Count = 0;
            // // agent.ResetPath();
            ObjRef.Value.GetComponent<FinalBoss>().SkillCooldown = 10.0f;
        }
    }

}