using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")        // 플레이어와 충돌 시
        {
            GameObject.Find("GameManager").GetComponent<QuestObject>().SetIsClear(true);
        }
    }
}
