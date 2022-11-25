using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")        // �÷��̾�� �浹 ��
        {
            //if(GameObject.Find("GameManager").GetComponent<QuestObject>().GetQuestKind() == QuestKind.spot) // ������ ������ ����Ʈ ������ ��
            //{
            //    GameObject.Find("GameManager").GetComponent<QuestObject>().SetIsClear(true);                // Ŭ����
            //    GameObject.Find("GameManager").GetComponent<SetPositionParticle>().InitializeVariable();    // ��ġ ����
            //}
            if (QuestObject.manager.GetQuestKind() == QuestKind.spot) // ������ ������ ����Ʈ ������ ��
            {
                QuestObject.manager.SetIsClear(true);                // Ŭ����
                GameObject.Find("GameManager").GetComponent<SetPositionParticle>().InitializeVariable();    // ��ġ ����
            }
        }
    }
}
