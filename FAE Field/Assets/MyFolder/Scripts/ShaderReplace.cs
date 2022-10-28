using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderReplace : MonoBehaviour
{
    [SerializeField] private Shader shader;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[]  sampleObjects = GameObject.FindGameObjectsWithTag("iceObject");
        foreach (GameObject obj in sampleObjects)
        {
            MeshRenderer mr = obj.GetComponent<MeshRenderer>();
            mr.material.shader = shader;
        }
    }
}
