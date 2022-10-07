using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Services/Enemy To Player Distance Service")]
    public class EnemyToPlayerDistance : Service
    {
        [Space]
        public GameObjectReference transform1;
        public TransformReference transform2;
        public FloatReference variable = new FloatReference(VarRefMode.DisableConstant);

        public override void Task()
        {
            GameObject t1 = transform1.Value;
            Transform t2 = transform2.Value;
            if (t1 == null || t2 == null)
            {
                return;
            }
            variable.Value = Vector3.Distance(t1.transform.position, t2.position);
        }
    }
}
