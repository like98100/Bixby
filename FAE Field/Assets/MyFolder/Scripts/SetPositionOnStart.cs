using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPositionOnStart : MonoBehaviour
{
    static Vector3 nextCamPos;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start" + nextCamPos);
        if (!(nextCamPos.x == 0 && nextCamPos.y == 0 && nextCamPos.z == 0))
        {
            Debug.Log("in" + nextCamPos);
            transform.position = new Vector3(nextCamPos.x, nextCamPos.y, nextCamPos.z);
            nextCamPos = new Vector3(0, 0, 0);
        }
    }

}
