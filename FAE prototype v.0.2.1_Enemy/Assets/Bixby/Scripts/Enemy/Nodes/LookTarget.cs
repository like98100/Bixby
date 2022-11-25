using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/Look Target")]
    public class LookTarget : Leaf
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public GameObjectReference TargetObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public FloatReference Variable = new FloatReference(VarRefMode.DisableConstant);

        float delay;

        public override void OnEnter()
        {
            delay = 0.0f;
        }

        public override NodeResult Execute()
        {
            Transform self = ObjRef.Value.transform;
            Transform target = TargetObjRef.Value.transform;
            Vector3 dir = target.position - self.position;
            Vector3 rayVector = new Vector3(self.position.x, self.position.y + 1.0f, self.position.z);
            RaycastHit hitInfo;

            if (ObjRef.Value.tag == "Enemy")
            {
                delay += Time.deltaTime / ObjRef.Value.GetComponent<Enemy>().FireRate;

                //(Time_ > UpdateInterval) ||
                if(((delay > ObjRef.Value.GetComponent<Enemy>().Stat.attackSpeed) && Physics.Raycast(rayVector, self.forward, out hitInfo, 10.0f, ObjRef.Value.GetComponent<Enemy>().Mask)) ||
                    ((int)ObjRef.Value.GetComponent<Enemy>().State == 3) || (ObjRef.Value.GetComponent<Enemy>().Stat.hp <= 0) || 
                    (Variable.Value > ObjRef.Value.GetComponent<Enemy>().Stat.attackRange))
                {
                    // Reset time and update destination
                    return NodeResult.success;
                }
            }
            else if (ObjRef.Value.tag == "FinalBoss")
            {
                delay += Time.deltaTime / ObjRef.Value.GetComponent<FinalBoss>().FireRate;

                if(((delay > ObjRef.Value.GetComponent<FinalBoss>().Stat.attackSpeed) && Physics.Raycast(rayVector, self.forward, out hitInfo, ObjRef.Value.GetComponent<FinalBoss>().Stat.sight, ObjRef.Value.GetComponent<FinalBoss>().Mask)) ||
                    (ObjRef.Value.GetComponent<FinalBoss>().Stat.hp <= 0))
                {
                    // Reset time and update destination
                    return NodeResult.success;
                }
            }
            
            self.rotation = Quaternion.Lerp(self.rotation, Quaternion.LookRotation(dir), 
                                          Time.deltaTime * 20.0f);

            Debug.DrawRay(rayVector, self.forward * 10.0f, Color.red);

            return NodeResult.running;
        }
    }
}
