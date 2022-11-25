using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/Fire Skill")]
    public class FireSkill : Leaf
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        
        public Vector3 myTransform;

        public float UpdateInterval = 5.5f;
        public float Time_ = 0.0f;
        public int count = 0;

        public override void OnEnter()
        {
            myTransform = ObjRef.Value.transform.position;

            ObjRef.Value.GetComponent<FinalBoss>().Anim.SetTrigger("Fire");
        }

        public override NodeResult Execute()
        {
            Time_ += Time.deltaTime;

            if(Time_ > UpdateInterval)
            {
                // Reset time and update destination
                Time_ = 0.0f;
                count = 0;
                return NodeResult.success;
            }
            else if(Time_ >= 1.0 && count == 0)
            {
                Instantiate(ObjRef.Value.GetComponent<FinalBoss>().FireBall, 
                            new Vector3(myTransform.x+4.0f, myTransform.y+8.5f, myTransform.z-1.0f), 
                            this.transform.rotation);
                count ++;
            }
            else if(Time_ >= 2.0f && count == 1)
            {
                Instantiate(ObjRef.Value.GetComponent<FinalBoss>().FireBall, 
                            new Vector3(myTransform.x+2.5f, myTransform.y+10.0f, myTransform.z-1.0f), 
                            this.transform.rotation);
                count ++;
            }
            else if(Time_ >= 3.0f && count == 2)
            {
                Instantiate(ObjRef.Value.GetComponent<FinalBoss>().FireBall, 
                            new Vector3(myTransform.x, myTransform.y+11.0f, myTransform.z-1.0f), 
                            this.transform.rotation);
                count ++;
            }
            else if(Time_ >= 4.0f && count == 3)
            {
                Instantiate(ObjRef.Value.GetComponent<FinalBoss>().FireBall, 
                            new Vector3(myTransform.x-2.5f, myTransform.y+10.0f, myTransform.z-1.0f), 
                            this.transform.rotation);
                count ++;
            }
            else if(Time_ >= 5.0f && count == 4)
            {
                Instantiate(ObjRef.Value.GetComponent<FinalBoss>().FireBall, 
                            new Vector3(myTransform.x-4.0f, myTransform.y+8.5f, myTransform.z-1.0f), 
                            this.transform.rotation);
                count ++;
            }

            
            return NodeResult.running;
        }

        public override void OnExit()
        {
            ObjRef.Value.GetComponent<FinalBoss>().SkillCooldown = 10.0f;
        }
    }
}