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
        public TransformReference destination;

        public float UpdateInterval = 2.0f;
        public float Time_ = 0.0f;

        public float AttakRange = 0.0f;

        public override NodeResult Execute()
        {
            GameObject obj = ObjRef.Value;
            int Element;
            // CheckElements
            Element = (int)obj.GetComponent<Enemy>().Element;

            Time_ += Time.deltaTime * 1.5f;

            if(Time_ > UpdateInterval)
            {
                // Reset time and update destination
                Time_ = 0.0f;
                return NodeResult.success;
            }

            // 무언가 해야한다면 여기

            return NodeResult.running;
        }

        public override void OnExit()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, AttakRange, 
                                                    1, QueryTriggerInteraction.Ignore);
            
            if(colliders.Length > 0)
            {
                for(int i = 0; i < colliders.Length; i++)
                {
                    if(colliders[i].tag == "Player")
                    {
                        colliders[i].GetComponent<Player>().TakeDamage(5.0f);
                        Debug.Log("Hit_5.0f");
                    }
                }
            }
        }
    }
}
