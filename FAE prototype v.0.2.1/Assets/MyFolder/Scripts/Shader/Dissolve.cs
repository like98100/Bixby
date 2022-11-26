using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    private ShaderManager shaderManager;

    [SerializeField] private bool isDissolve;
    [SerializeField] private Material dissolve;
    private Material[] dissolves;
    [ColorUsage(true, true)] [SerializeField] private Color color;
    [SerializeField] private float[] scales;
    void Start()
    {
        shaderManager = this.gameObject.GetComponent<ShaderManager>();

        CreateMaterial();

        isDissolve = false;
    }

    void Update()
    {
        if (isDissolve == true )
        {
            StartCoroutine(Act(this.gameObject));
        }
    }

    public IEnumerator Act(GameObject go)
    {
        yield return new WaitUntil(() => Mathf.Sin(Time.time) <= -0.99);

        isDissolve = false;
        shaderManager.ChangeMaterials(this.gameObject, dissolves);

        yield return new WaitForSeconds(3.1f);

        this.gameObject.SetActive(false);
        Destroy(go);
    }
    private void CreateMaterial()
    {
        Material[] mats = this.gameObject.GetComponent<Renderer>().materials;
        //Set Color
        Enemy enemy = transform.parent.GetComponent<Enemy>();
        if (enemy)
            dissolve.SetColor("_Color", enemy.GetMyElementColor());
        else
            dissolve.SetColor("_Color", color);

        dissolves = new Material[mats.Length];
        for (int i = 0; i < mats.Length; i++)
        {
            dissolves[i] = Instantiate(dissolve);

            //Set Texture
            dissolves[i].SetTexture("_MainTexture", mats[i].mainTexture);
            //dissolves[i].color = mats[i].color;

            //Set Scale
            dissolves[i].SetFloat("_Scale", scales[i]);
        }
    }
}