using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/Sleep")]
    public class Sleep : Leaf
    {
        public GameObjectReference obj1;

        public float updateInterval = 3.0f;
        public float time = 0.0f;

        public override NodeResult Execute()
        {
            GameObject obj = obj1.Value;
            
            time += Time.deltaTime * 1.5f;

            if(time > updateInterval)
            {
                // Reset time and update destination
                time = 0;
                return NodeResult.success;
            }
            
            //obj.GetComponent<Enemy>().hp = 100.0f;

            return NodeResult.running;
        }
    }
}
