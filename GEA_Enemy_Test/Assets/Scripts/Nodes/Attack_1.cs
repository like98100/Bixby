using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Task/Attack_1")]
    public class Attack_1 : Leaf
    {
        public TransformReference selfPosition;
        public GameObject Bullet;

        public override NodeResult Execute()
        {
            Transform self = selfPosition.Value;

            Instantiate(Bullet, self.position, self.rotation);

            return NodeResult.success;
        }
    }
}
