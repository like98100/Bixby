using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Services/Check Enemy State Service")]
    public class CheckEnemyState : Service
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public IntReference variable = new IntReference(VarRefMode.DisableConstant);

        public override void Task()
        {
            GameObject obj = ObjRef.Value;

            if(obj == null)
                return;

            variable.Value = (int)obj.GetComponent<Enemy>().State;                
        }
    }
}
