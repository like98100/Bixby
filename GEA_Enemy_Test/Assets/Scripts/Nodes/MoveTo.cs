using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBTExample
{
    [MBTNode("Tasks/Move To")]
    [AddComponentMenu("")]
    public class MoveTo : Leaf
    {
        public Vector3Reference targetPosition;
        public TransformReference transformToMove;
        //public float speed = 0.1f;
        public float minDistance = 0f;

        public TransformReference destination;
        public UnityEngine.AI.NavMeshAgent agent;
        public float stopDistance = 2f;
        [Tooltip("How often target position should be updated")]
        public float updateInterval = 1f;
        private float time = 0;

        public override void OnEnter()
        {
            //time = 0;
            agent.isStopped = false;
            agent.SetDestination(destination.Value.position);
        }

        public override NodeResult Execute()
        {
            Vector3 target = targetPosition.Value;
            Transform obj = transformToMove.Value;
            // Move as long as distance is greater than min. distance
            float dist = Vector3.Distance(target, obj.position);
            if (dist > minDistance)
            {
                // Move towards target
                obj.position = Vector3.MoveTowards(
                    obj.position, 
                    target, 
                    //(speed >= dist)? dist : speed 
                    1
                );
                return NodeResult.running;
            }
            // else
            // {
            //     return NodeResult.success;
            // }

            time += Time.deltaTime;
            // Update destination every given interval
            if (time > updateInterval)
            {
                // Reset time and update destination
                time = 0;
                agent.SetDestination(destination.Value.position);
            }
            // Check if path is ready
            if (agent.pathPending)
            {
                return NodeResult.running;
            }
            // Check if agent is very close to destination
            if (agent.remainingDistance <= stopDistance)
            {
                return NodeResult.success;
            }
            // Check if there is any path (if not pending, it should be set)
            if (agent.hasPath)
            {
                return NodeResult.running;
            }
            // By default return failure
            return NodeResult.failure;
        }

         public override void OnExit()
        {
            agent.isStopped = true;
            // agent.ResetPath();
        }

        public override bool IsValid()
        {
            return !(destination.isInvalid || agent == null);
        }
    }
}
