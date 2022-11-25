using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Services/Check Enemy Skill Cooldown Service")]
    public class CheckEnemySkillCooldown : Service
    {
        [Space]
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public FloatReference Variable = new FloatReference(VarRefMode.DisableConstant);
        
        public override void Task()
        {
            GameObject obj = ObjRef.Value;

            if (obj == null)
            {
                return;
            }

            Variable.Value = obj.GetComponent<FinalBoss>().SkillCooldown;
        }
    }
}
