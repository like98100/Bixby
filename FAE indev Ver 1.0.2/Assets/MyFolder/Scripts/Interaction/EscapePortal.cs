using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapePortal : MonoBehaviour
{
    private GameObject potalObj;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            QuestObject.manager.DungeonRunaway();
        }
    }
}
