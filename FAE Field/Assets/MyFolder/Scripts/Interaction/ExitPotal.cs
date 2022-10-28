using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPotal : MonoBehaviour
{
    private GameObject go;
    private SetPositionOnStart a;

    void Start()
    {
        go = GameObject.FindWithTag("MainCamera");
        a = go.GetComponent<SetPositionOnStart>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {            
            SceneManager.LoadScene("FieldScene");
            //a.nextCamPos = new Vector3(1, 2, 3);
        }
    }
}
