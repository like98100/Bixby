using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/Electrocity Skill")]
    public class ElectrocitySkill : Leaf
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public GameObjectReference targetRef = new GameObjectReference(VarRefMode.DisableConstant);
        public FloatReference dist = new FloatReference(VarRefMode.DisableConstant);
        public Vector3 targetVec;

        public float UpdateInterval = 3.5f;
        public float Time_;

        public override void OnEnter()
        {
            targetVec = targetRef.Value.transform.position;
            targetVec += ObjRef.Value.transform.forward * 10.0f;
            targetVec += ObjRef.Value.transform.right * 10.0f;

            //ObjRef.Value.GetComponent<FinalBoss>().Anim.SetTrigger("asd");

            Time_ = 0.0f;
        } 

        public override NodeResult Execute()
        {
            Time_ += Time.deltaTime;

            if(Time_ > UpdateInterval)
            {
                // Reset time and update destination
                Time_ = 0.0f;
                //ObjRef.Value.GetComponent<FinalBoss>().elecCount++;
                return NodeResult.success;
            }
            
            ObjRef.Value.transform.position = Vector3.MoveTowards(transform.position, 
                                                        targetVec, 
                                                        35.0f * Time.deltaTime);

            return NodeResult.running;
        }

        public override void OnExit()
        {
            //if (ObjRef.Value.GetComponent<FinalBoss>().elecCount == 5)
                //ObjRef.Value.GetComponent<FinalBoss>().Count = 0;
            // // agent.ResetPath();
            ObjRef.Value.GetComponent<FinalBoss>().SkillCooldown = 10.0f;
        }
    }

}