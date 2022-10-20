using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MinimapCam : MonoBehaviour
{
    GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        Vector3 temp = player.transform.position;
        temp.y += 10f;
        this.transform.position = temp;
    }
}
