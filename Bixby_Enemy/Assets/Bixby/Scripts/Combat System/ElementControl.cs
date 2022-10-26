using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementControl : ElementRule, IElementReaction
{
    public ElementType MyElement;
    public ElementType EnemyElement;
    public Stack<ElementType> ElementStack = new Stack<ElementType>(); // ¼?¼º ??¼º ½?³??? ½º??. 2½º?? ½×??¸? ¹?·? ??º? Æ?!

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

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.mySkillStartColor = FireSkillStartColor;
            this.mySkillEndColor = FireSkillEndColor;
            this.MyElement = ElementType.FIRE;
            Debug.Log("current element : FIRE");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.mySkillStartColor = IceSkillStartColor;
            this.mySkillEndColor = IceSkillEndColor;
            this.MyElement = ElementType.ICE;
            Debug.Log("current element : ICE");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            this.mySkillStartColor = WaterSkillStartColor;
            this.mySkillEndColor = WaterSkillEndColor;
            this.MyElement = ElementType.WATER;
            Debug.Log("current element : WATER");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            this.mySkillStartColor = ElectroSkillStartColor;
            this.mySkillEndColor = ElectroSkillEndColor;
            this.MyElement = ElementType.ELECTRICITY;
            Debug.Log("current element : ELECTRICITY");
        }
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

        if (firstElement == ElementType.FIRE && secondElement == ElementType.ICE) //?¶?Ø
        {
            Fusion();
        }
        else if (firstElement == ElementType.WATER && secondElement == ElementType.ICE) //º?°?
        {
            Freezing();
        }
        else if (firstElement == ElementType.ELECTRICITY && secondElement == ElementType.ICE) //????
        {
            Transmission();
        }
        else if (firstElement == ElementType.ELECTRICITY && secondElement == ElementType.FIRE) //Æø¹ß
        {
            Explosion();
        }
        else if (firstElement == ElementType.ELECTRICITY && secondElement == ElementType.WATER) //°¨??
        {
            ElectricShock();
        }
        else if (firstElement == ElementType.WATER && secondElement == ElementType.FIRE) //??¹ß
        {
            Evaporation();
        }
        else
        {
            return;
        }
    }

    // 건드린 부분
    protected float attackedOnNormal(float damage)
    {
        int adventage = CheckAdventage(MyElement, EnemyElement);
        switch (adventage)
        {
            case 1:
                return damage * 2;
                //break;
            case 0:
                return damage * 1;
                //break;
            case -1:
                return damage / 2;
                //break;
        }
        return 0;
    }

    protected void attackedOnSheild()
    {
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

    }
    public virtual void Freezing()
    {

    }
    public virtual void ElectricShock()
    {

    }
    public virtual void Explosion()
    {

    }
    public virtual void Evaporation()
    {

    }
    public virtual void Transmission()
    {

    }


    // 건드린 부분
    protected void setEnemyElement(ElementType element)
    {
        EnemyElement = element;
    }


}

