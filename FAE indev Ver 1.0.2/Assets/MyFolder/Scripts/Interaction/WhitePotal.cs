using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WhitePotal : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (QuestObject.manager.GetIndex() > 6)
            {
                LoadingSceneController.Instance.LoadScene("IceDungeon");
            }
            else
            {
                UI_Control.Inst.Speech.Tutorial.ElementGetText(4); //�غ� ���� �ʾҴٴ� �ý��� �޽��� ���.
            }
        }
    }
}
