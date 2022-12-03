using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementRule : MonoBehaviour
{
    public static LinkedList<ElementType> Elements = new LinkedList<ElementType>();
    public enum ElementType
    {
        NONE = -1,
        FIRE = 0,
        ICE = 1,
        WATER = 2,
        ELECTRICITY = 3,

        NUM = 4,
    };

    public int CheckAdventage(ElementType myElementType, ElementType enemyElement) // 1�϶� �̱�, -1�϶� ��, 0�϶� ���.
    {
        if (myElementType == ElementType.NONE) return 0; //�Ӽ��� �������� �����غ��� �ǹ̰� ����.

        if(Elements.Find(myElementType).Next == Elements.Find(enemyElement)
            || (Elements.Find(myElementType).Next == null && enemyElement == ElementType.WATER))
        {
            return 1;
        }
        else if(Elements.Find(myElementType).Previous == Elements.Find(enemyElement)
            || (Elements.Find(myElementType).Previous == null && enemyElement == ElementType.ELECTRICITY))
        {
            return -1; // �Ϲ� ���¿��� ���� ���ذ� ���� ������ ������, ���忡 �� ���� �ݻ����ظ� �Դ´�.
        }
        else
        {
            return 0;
        }
    }
}
