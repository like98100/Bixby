using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementControl : ElementRule, IElementReaction
{
    public ElementType MyElement;
    public ElementType EnemyElement;
    public Stack<ElementType> ElementStack = new Stack<ElementType>(); // �Ӽ� �ռ� �ó��� ����. 2���� ���̸� �ٷ� ���� ��!

    public GameObject ExplosionObj;
    public GameObject TransmissionObj;

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

    public bool IsFusion = false;
    public bool IsEvaporation = false;
    public bool IsElectronicShock = false;
    public bool IsExplosion = false;
    public bool IsFreezing = false;
    public bool IsTransmission = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //ElementStack ;
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
        if (this.ElementStack.Count >= 2)
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

        if ((firstElement == ElementType.FIRE && secondElement == ElementType.ICE)
            || (secondElement == ElementType.FIRE && firstElement == ElementType.ICE)) //����
        {
            if (!IsFusion)
            {
                Debug.Log("����!");
                Fusion();
                IsFusion = true;
                UI_Control.Inst.damageSet("����", this.gameObject);
            }
        }
        if ((firstElement == ElementType.WATER && secondElement == ElementType.ICE)
            || (secondElement == ElementType.WATER && firstElement == ElementType.ICE)) //����
        {
            if (!IsFreezing)
            {
                Debug.Log("����!");
                Freezing();
                IsFreezing = true;
                UI_Control.Inst.damageSet("����", this.gameObject);
            }
        }
        if ((firstElement == ElementType.ELECTRICITY && secondElement == ElementType.ICE)
            || (secondElement == ElementType.ELECTRICITY && firstElement == ElementType.ICE)) //����
        {
            if (!IsTransmission)
            {
                Debug.Log("����!");
                Transmission();
                //IsTransmission = true;
                UI_Control.Inst.damageSet("����", this.gameObject);
            }
        }
        if ((firstElement == ElementType.ELECTRICITY && secondElement == ElementType.FIRE)
            || (secondElement == ElementType.ELECTRICITY && firstElement == ElementType.FIRE)) //����
        {
            if (!IsExplosion)
            {
                Debug.Log("����!");
                Explosion();
                //IsExplosion = true;
                UI_Control.Inst.damageSet("����", this.gameObject);
            }
        }
        if ((firstElement == ElementType.ELECTRICITY && secondElement == ElementType.WATER)
            || (secondElement == ElementType.ELECTRICITY && firstElement == ElementType.WATER)) //����
        {
            if (!IsElectronicShock)
            {
                Debug.Log("����!");
                ElectricShock();
                IsElectronicShock = true;
                UI_Control.Inst.damageSet("����", this.gameObject);
            }
        }
        if ((firstElement == ElementType.WATER && secondElement == ElementType.FIRE)
            || (secondElement == ElementType.WATER && firstElement == ElementType.FIRE)) //����
        {
            if (!IsEvaporation)
            {
                Debug.Log("����!");
                Evaporation();
                IsEvaporation = true;
                UI_Control.Inst.damageSet("����", this.gameObject);
            }
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
        //50�� ������.
        StartCoroutine(fusion(10.0f));
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
        Instantiate(ExplosionObj, this.transform.position, this.transform.rotation);
        IsExplosion = false;
    }
    public virtual void Evaporation()
    {
        //���ݵ����̰� 2��� ����.
        StartCoroutine(evaporation(10.0f));
    }
    public virtual void Transmission()
    {
        Instantiate(TransmissionObj, this.transform.position, this.transform.rotation);
        IsTransmission = false;
    }

    IEnumerator fusion(float time)
    {
        this.gameObject.GetComponent<CombatStatus>().AdditionalDamage = 1.5f; //50�� ������
        while (time > 0)
        {
            time--;
            yield return new WaitForSeconds(1.0f);
        }
        IsEvaporation = false;
        this.gameObject.GetComponent<CombatStatus>().AdditionalDamage = 1.0f;

        float temp = this.gameObject.GetComponent<CombatStatus>().FireRate;

        this.gameObject.GetComponent<CombatStatus>().FireRate =
        this.gameObject.GetComponent<CombatStatus>().FireRate * 2.0f;
        while (time > 0)
        {
            time--;
            yield return new WaitForSeconds(1.0f);
        }
        IsFusion = false;
        this.gameObject.GetComponent<CombatStatus>().FireRate = temp;
    }

    IEnumerator freezing(float time)
    {
        this.gameObject.GetComponent<CombatStatus>().SpeedMultiply = 0.5f;
        while (time > 0)
        {
            time--;
            yield return new WaitForSeconds(1.0f);
        }
        IsFreezing = false;
        this.gameObject.GetComponent<CombatStatus>().SpeedMultiply = 1.0f;
    }

    IEnumerator eletricShock(float time, float damage)
    {
        while(time > 0)
        {
            if (this.gameObject.tag == "Player")
            {
                this.gameObject.GetComponent<PlayerStatusControl>().TakeHit(damage);
            }
            else if(this.gameObject.tag == "Enemy")
            {
                this.gameObject.GetComponent<Enemy>().TakeHit(damage);
            }
            time--;
            yield return new WaitForSeconds(1.0f);
        }
        IsElectronicShock = false;
    }

    IEnumerator evaporation(float time)
    {
        float temp = this.gameObject.GetComponent<CombatStatus>().FireRate;

        this.gameObject.GetComponent<CombatStatus>().FireRate =
        this.gameObject.GetComponent<CombatStatus>().FireRate * 2.0f;
        while (time > 0)
        {
            time--;
            yield return new WaitForSeconds(1.0f);
        }
        IsFusion = false;
        this.gameObject.GetComponent<CombatStatus>().FireRate = temp;
    }

    // �ӽ� �߰��κ�. ���� �����ϱ� ���� �ڵ�, �ۼ���: ��â��
    protected void setEnemyElement(ElementType element)
    {
        EnemyElement = element;
    }
}