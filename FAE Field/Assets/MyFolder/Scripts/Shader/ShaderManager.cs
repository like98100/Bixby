using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderManager : MonoBehaviour
{
    public void ChangeShader(GameObject[] objects, Shader shader)
    {
        foreach (GameObject obj in objects)
        {
            MeshRenderer mr = obj.GetComponent<MeshRenderer>();
            mr.material.shader = shader;
        }
    }
    public void ChangeMaterial(GameObject obj, Material material)
    { 
        MeshRenderer mr = obj.GetComponent<MeshRenderer>();   // 일단 MeshRenderer 컴포넌트를 얻고
        mr.material = material;  
    }
    
}
