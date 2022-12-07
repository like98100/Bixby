using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RedPotal : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (QuestObject.manager.GetIndex() > 4)
            {
                LoadingSceneController.Instance.LoadScene("FireDungeon");
            }
            else
            {
                UI_Control.Inst.Speech.Tutorial.ElementGetText(4); //�غ� ���� �ʾҴٴ� �ý��� �޽��� ���.
            }
        }
    }
}
