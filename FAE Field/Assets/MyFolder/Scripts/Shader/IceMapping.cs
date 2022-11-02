using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMapping : MonoBehaviour
{
    private ShaderManager shaderManager;

    [SerializeField] private Shader iceShader;
    
    // Start is called before the first frame update
    void Start()
    {
        shaderManager = this.gameObject.GetComponent<ShaderManager>();

        GameObject[] iceObjects = GameObject.FindGameObjectsWithTag("iceObject");

        shaderManager.ChangeShader(iceObjects, iceShader);
    }
}
