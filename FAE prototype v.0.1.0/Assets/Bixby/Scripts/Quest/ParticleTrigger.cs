using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")        // �÷��̾�� �浹 ��
        {
            GameObject.Find("GameManager").GetComponent<QuestObject>().SetIsClear(true);
        }
    }
}
