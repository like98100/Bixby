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
        public TransformReference destination;
        public float Speed = 3.0f;

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

            obj.transform.position = Vector3.MoveTowards(transform.position, 
                                                        destination.Value.position, 
                                                        Speed * Time.deltaTime);

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
                        colliders[i].GetComponent<Player>().TakeDamage(10.0f);
                        Debug.Log("Hit_10.0f");
                    }
                }
            }
        }
    }
}
