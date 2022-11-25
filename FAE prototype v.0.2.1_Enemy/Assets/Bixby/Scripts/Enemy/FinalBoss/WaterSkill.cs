using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/Water Skill")]
    public class WaterSkill : Leaf
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public GameObjectReference targetObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        
        public float UpdateInterval = 5.5f;
        public float Time_ = 0.0f;
        public int count = 0;

        GameObject obj;
        Vector3 target;
        Vector3 p;
        Vector3 p1;
        Vector3 p2;
        Vector3 p3;
        Vector3 p4;

        public override void OnEnter()
        {
            //ObjRef.Value.GetComponent<FinalBoss>().Anim.SetTrigger("qwe");
            p  = new Vector3(1140.0f, -452.11f, 350.0f);
            p1 = new Vector3(1080.0f, 85.0f, 285.0f);
            p2 = new Vector3(1200.0f, 85.0f, 285);
            p3 = new Vector3(1100.0f, 85.0f, 420.0f);
            p4 = new Vector3(1200.0f, 85.0f, 425.0f);

            ObjRef.Value.transform.position = p;

            ObjRef.Value.GetComponent<FinalBoss>().Anim.SetTrigger("Water");
        }

        public override NodeResult Execute()
        {
            obj = ObjRef.Value;
            target = targetObjRef.Value.transform.position;
            Time_ += Time.deltaTime;

            if(Time_ > UpdateInterval)
            {
                // Reset time and update destination
                Time_ = 0.0f;
                return NodeResult.success;
            }

            Instantiate(obj.GetComponent<FinalBoss>().WaterBall, p1, transform.rotation);
            Instantiate(obj.GetComponent<FinalBoss>().WaterBall, p2, transform.rotation);
            Instantiate(obj.GetComponent<FinalBoss>().WaterBall, p3, transform.rotation);
            Instantiate(obj.GetComponent<FinalBoss>().WaterBall, p4, transform.rotation);
            
            return NodeResult.success;
            //return NodeResult.running;
        }

        public override void OnExit()
        {
            //ObjRef.Value.GetComponent<FinalBoss>().Count = 0;

            Instantiate(obj.GetComponent<FinalBoss>().WaterBallSpawner,
                        new Vector3(target.x, target.y+20.0f, target.z),
                        transform.rotation);
            ObjRef.Value.GetComponent<FinalBoss>().SkillCooldown = 10.0f;
        }
    }
}