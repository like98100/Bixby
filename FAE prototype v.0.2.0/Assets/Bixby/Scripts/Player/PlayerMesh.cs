using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMesh : MonoBehaviour
{
    public Material[] CharacterMaterial = new Material[4];
    public Mesh[] CharacterMesh = new Mesh[4];

    SkinnedMeshRenderer skinRenderer;

    // Start is called before the first frame update
    void Start()
    {
        skinRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeClass(int idx)
    {
        // None Class�� �����ϰ� �����ϰ� Ȱ�� �����ϴ� switch�� �ƴ� if ���
        if(idx == -1)   // None Class
        {
            skinRenderer.material = CharacterMaterial[idx + 1]; // Change Material
            skinRenderer.sharedMesh = CharacterMesh[idx + 1];   // Changer Mesh
        }
        else
        {
            skinRenderer.material = CharacterMaterial[idx]; // Change Material
            skinRenderer.sharedMesh = CharacterMesh[idx];   // Changer Mesh
        }
    }
}
