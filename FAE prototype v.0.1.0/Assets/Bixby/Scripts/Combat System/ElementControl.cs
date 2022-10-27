using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementControl : ElementRule, IElementReaction
{
    public ElementType MyElement;
    public ElementType EnemyElement;
    public Stack<ElementType> ElementStack = new Stack<ElementType>(); // �Ӽ� �ռ� �ó��� ����. 2���� ���̸� �ٷ� ���� ��!

    protected Color mySkillStartColor = Color.yellow;
    protected Color mySkillEndColor = Color.white;

    public static Color FireSkillStartColor = new Color(201 / 255f, 29 / 255f, 6 / 255f);
    public static Color FireSkillEndColor = new Color(255 / 255f, 185 / 255f, 33 / 255f);

    public static Color IceSkillStartColor = new Color(33 / 255f, 255 / 255f, 255 / 255f);
    public static Color IceSkillEndColor = new Color(237 / 255f, 253 / 255f, 255 / 255f);

    public static Color WaterSkillStartColor = new Color(16 / 255f, 46 / 255f, 222 / 255f);
    public static Color WaterSkillEndColor = new Color(33 / 255f, 255 / 255f, 255 / 255f);

    public static Color ElectroSkillStartColor = new Color(112 / 255f, 69 / 255f, 255 / 255f);
    public static Color ElectroSkillEndColor = new Color(112 / 255f, 69 / 255f, 255 / 255f);

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (Elements.Count < 4)
        {
            Elements.AddLast(ElementType.WATER);
            Elements.AddLast(ElementType.FIRE);
            Elements.AddLast(ElementType.ICE);
            Elements.AddLast(ElementType.ELECTRICITY);
        }

        MyElement = ElementType.NONE;
        EnemyElement = ElementType.NONE;
    }


    protected void checkIsPopTime()
    {
        if (ElementStack.Count >= 2)
        {
            elementReaction();
        }
        else
            return;
    }

    protected void elementReaction()
    {
        ElementType firstElement = ElementStack.Pop();
        ElementType secondElement = ElementStack.Pop();

        if (firstElement == ElementType.FIRE && secondElement == ElementType.ICE) //����
        {
            Fusion();
        }
        else if (firstElement == ElementType.WATER && secondElement == ElementType.ICE) //����
        {
            Freezing();
        }
        else if (firstElement == ElementType.ELECTRICITY && secondElement == ElementType.ICE) //����
        {
            Transmission();
        }
        else if (firstElement == ElementType.ELECTRICITY && secondElement == ElementType.FIRE) //����
        {
            Explosion();
        }
        else if (firstElement == ElementType.ELECTRICITY && secondElement == ElementType.WATER) //����
        {
            ElectricShock();
        }
        else if (firstElement == ElementType.WATER && secondElement == ElementType.FIRE) //����
        {
            Evaporation();
        }
        else
        {
            return;
        }
    }

    protected float attackedOnNormal(float damage)
    {
        int adventage = CheckAdventage(MyElement, EnemyElement);
        switch (adventage)
        {
            case 1:
                return damage * 2;
            case 0:
                return damage * 1;
            case -1:
                return damage / 2;
        }
        return 0;
    }

    protected void attackedOnSheild() // �ݵ��� ���� ���ɼ��� �ֱ� ������, ��ȯ���� �ϴ� void�� �Ѵ�. 
    { // �ڱ� �ڽſ��� ������� ��� �� ��Ȳ(�ڱ� �ڽſ� ���� ����)�� ���� ���ɼ��� �ִ�.
        int adventage = CheckAdventage(MyElement, EnemyElement);
        switch (adventage)
        {
            case 1:

                break;
            case 0:

                break;
            case -1:

                break;
        }
    }

    public virtual void Fusion()
    {
        //�ϴ�, ���ݼӵ��� 50�ۼ�Ʈ �����.
        StartCoroutine(fusion(100.0f));
    }
    public virtual void Freezing()
    {
        StartCoroutine(freezing(10.0f));
        //�̵��ӵ��� 50�ۼ�Ʈ �����.
    }
    public virtual void ElectricShock()
    {
        StartCoroutine(eletricShock(5.0f, 10.0f));
    }
    public virtual void Explosion()
    {
        //instaciate();
    }
    public virtual void Evaporation()
    {
        StartCoroutine(evaporation(10.0f));
    }
    public virtual void Transmission()
    {
        //instanciate();
    }

    IEnumerator fusion(float time)
    {
        float temp = this.gameObject.GetComponent<PlayerStatusControl>().FireRate;

        this.gameObject.GetComponent<PlayerStatusControl>().FireRate =
        this.gameObject.GetComponent<PlayerStatusControl>().FireRate * 1.5f;
        while (time > 0)
        {
            time--;
            yield return new WaitForSeconds(1.0f);
        }
        this.gameObject.GetComponent<PlayerStatusControl>().FireRate = temp;
    }

    IEnumerator freezing(float time)
    {
        this.gameObject.GetComponent<PlayerStatusControl>().SpeedMultiply = 0.5f;
        while (time > 0)
        {
            time--;
            yield return new WaitForSeconds(1.0f);
        }
        this.gameObject.GetComponent<PlayerStatusControl>().SpeedMultiply = 1.0f;
    }

    IEnumerator eletricShock(float time, float damage)
    {
        while(time > 0)
        {
            this.gameObject.GetComponent<PlayerStatusControl>().TakeHit(damage);
            time--;
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator evaporation(float time)
    {
        this.gameObject.GetComponent<PlayerStatusControl>().AdditionalDamage = 1.5f; //50�� ������
        while (time > 0)
        {
            time--;
            yield return new WaitForSeconds(1.0f);
        }
        this.gameObject.GetComponent<PlayerStatusControl>().AdditionalDamage = 1.0f;
    }

    // �ӽ� �߰��κ�. ���� �����ϱ� ���� �ڵ�, �ۼ���: ��â��
    protected void setEnemyElement(ElementType element)
    {
        EnemyElement = element;
    }
}