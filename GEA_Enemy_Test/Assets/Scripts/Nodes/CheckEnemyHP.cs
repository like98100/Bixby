using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Services/Check Enemy HP Service")]
    public class CheckEnemyHP : Service
    {
        [Space]
        public GameObjectReference obj1;
        public FloatReference variable = new FloatReference(VarRefMode.DisableConstant);
        
        public override void Task()
        {
            GameObject obj = obj1.Value;

            if (obj == null)
            {
                return;
            }
            //variable.Value = obj.GetComponent<Enemy>().hp;
            //variable.Value = obj.GetComponent<Enemy>().stat.hp;
        }
    }
}
