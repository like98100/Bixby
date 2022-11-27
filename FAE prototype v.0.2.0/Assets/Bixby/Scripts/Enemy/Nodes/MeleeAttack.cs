using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/MeleeAttack")]
    public class MeleeAttack : Leaf
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public FloatReference Variable = new FloatReference(VarRefMode.DisableConstant);

        public float UpdateInterval = 2.0f;
        public float Time_ = 0.0f;

        public override void OnEnter()
        {
            if (ObjRef.Value.tag == "Enemy")
                ObjRef.Value.GetComponent<Enemy>().Anim.SetBool("IsAttack", true);
            else if (ObjRef.Value.tag == "DungeonBoss")
            {
                ObjRef.Value.GetComponent<DungeonBoss>().Anim.SetTrigger("isAttacked");
                ObjRef.Value.GetComponent<DungeonBoss>().isAttacked = true;
            }
            else if (ObjRef.Value.tag == "FinalBoss")
            {
                ObjRef.Value.GetComponent<FinalBoss>().Anim.SetTrigger("isAttack");
                ObjRef.Value.GetComponent<FinalBoss>().isAttacked = true;
            }
        }

        public override NodeResult Execute()
        {
            // int Element;
            // CheckElements
            // Element = (int)ObjRef.Value.GetComponent<Enemy>().Element;

            Time_ += Time.deltaTime * 1.5f;

            if (ObjRef.Value.tag == "Enemy")
            {
                if((Time_ > UpdateInterval) || ((int)ObjRef.Value.GetComponent<Enemy>().State == 3) ||
                    (ObjRef.Value.GetComponent<Enemy>().Stat.hp <= 0) || (Variable.Value > ObjRef.Value.GetComponent<Enemy>().Stat.attackRange))
                {
                    // Reset time and update destination
                    Time_ = 0.0f;
                    return NodeResult.success;
                }
            }
            else if (ObjRef.Value.tag == "DungeonBoss")
            {
                if((!ObjRef.Value.GetComponent<DungeonBoss>().isAttacked) || (ObjRef.Value.GetComponent<DungeonBoss>().Stat.hp <= 0))
                {
                    // Reset time and update destination
                    Time_ = 0.0f;
                    return NodeResult.success;
                }
            }
            else if (ObjRef.Value.tag == "FinalBoss")
            {
                if((!ObjRef.Value.GetComponent<FinalBoss>().isAttacked) || (ObjRef.Value.GetComponent<FinalBoss>().Stat.hp <= 0))
                {
                    // Reset time and update destination
                    Time_ = 0.0f;
                    return NodeResult.success;
                }
            }

            // 무언가 해야한다면 여기

            return NodeResult.running;
        }

        public override void OnExit()
        {
            if (ObjRef.Value.tag == "Enemy")
            {
                if (Variable.Value <= ObjRef.Value.GetComponent<Enemy>().Stat.attackRange)
                    //ObjRef.Value.GetComponent<Enemy>().MeleeAttack();
                

                ObjRef.Value.GetComponent<Enemy>().Anim.SetBool("IsAttack", false);
            }
        }
    }
}
