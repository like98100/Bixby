using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    private ShaderManager shaderManager;

    [SerializeField] private bool isDissolve;
    [SerializeField] private Material dissolve;
    [ColorUsage(true, true)] [SerializeField] private Color color;
    [SerializeField] private float scale;
    void Start()
    {
        shaderManager = this.gameObject.GetComponent<ShaderManager>();

        dissolve = Instantiate(dissolve);
        SetColor();
        SetTexture();
        SetScale();

        isDissolve = false;
    }

    void Update()
    {
        if (isDissolve == true && Mathf.Sin(Time.time) <= -0.99)
        {
            StartCoroutine(Act());
        }
    }

    IEnumerator Act()
    {
        isDissolve = false;
        shaderManager.ChangeMaterial(this.gameObject, dissolve);

        yield return new WaitForSeconds(3.1f);

        this.gameObject.SetActive(false);
    }

    private void SetColor()
    {
        dissolve.SetColor("_Color", color);
    }

    private void SetTexture()
    {
        Texture texture = this.gameObject.GetComponent<MeshRenderer>().material.mainTexture;
        dissolve.SetTexture("_MainTexture", texture);
    }

    private void SetScale()
    {
        dissolve.SetFloat("_Scale", scale);
    }
}
