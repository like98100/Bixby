using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MBT;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/Move To Transform")]
    public class MoveToTransform : Leaf
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public GameObjectReference TargetRef =  new GameObjectReference(VarRefMode.DisableConstant);
        public FloatReference Variable = new FloatReference(VarRefMode.DisableConstant);
        public IntReference myState = new IntReference(VarRefMode.DisableConstant);

        public NavMeshAgent agent;
        public float stopDistance = 2f;
        [Tooltip("How often target position should be updated")]
        public float updateInterval = 1f;
        private float time = 0;

        public float ExitDist = 14.5f;

        public override void OnEnter()
        {
            time = 0;
            agent.isStopped = false;
            agent.SetDestination(TargetRef.Value.transform.position);
            if (ObjRef.Value.tag == "Enemy")
                ObjRef.Value.GetComponent<Enemy>().Anim.SetBool("IsMove", true);
            else if (ObjRef.Value.tag == "DungeonBoss")
                ObjRef.Value.GetComponent<DungeonBoss>().Anim.SetBool("isMoved", true);
            else if (ObjRef.Value.tag == "FinalBoss")
                ObjRef.Value.GetComponent<FinalBoss>().Anim.SetBool("isMove", true);
        }
        
        public override NodeResult Execute()
        {
            time += Time.deltaTime;
            // Update destination every given interval
            if (time > updateInterval)
            {
                // Reset time and update destination
                time = 0;
                agent.SetDestination(TargetRef.Value.transform.position);
            }
            
            if (ObjRef.Value.tag == "Enemy")
            {
                if ((myState.Value == 1) || (myState.Value == 3))
                {
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
                    return NodeResult.failure;
                }
            }
            else if (ObjRef.Value.tag == "DungeonBoss")
            {
                
            }
            else if (ObjRef.Value.tag == "FinalBoss")
            {
                
            }
            
            // Check if path is ready
            if (agent.pathPending)
            {
                return NodeResult.running;
            }
            // Check if agent is very close to destination
            if (agent.remainingDistance < stopDistance)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                return NodeResult.success;
            }
            // Check if there is any path (if not pending, it should be set)
            if (agent.hasPath)
            {
                return NodeResult.running;
            }

            // By default return failure
            agent.velocity = Vector3.zero;
            return NodeResult.failure;
        }

        public override void OnExit()
        {
            if (ObjRef.Value.tag == "Enemy")
                ObjRef.Value.GetComponent<Enemy>().Anim.SetBool("IsMove", false);
            else if (ObjRef.Value.tag == "DungeonBoss")
                ObjRef.Value.GetComponent<DungeonBoss>().Anim.SetBool("isMoved", false);
            else if (ObjRef.Value.tag == "FinalBoss")
                ObjRef.Value.GetComponent<FinalBoss>().Anim.SetBool("isMove", false);
            
            agent.isStopped = true;
            // agent.ResetPath();
        }

        public override bool IsValid()
        {
            return !(TargetRef.isInvalid || agent == null);
        }
    }
}
