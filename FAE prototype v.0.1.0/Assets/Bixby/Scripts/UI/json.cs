using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
[System.Serializable]
public class itemData
{
    public int itemID;
    public string[] tag;
    public string itemName;
    public float Left;
    public float Up;
    public float xSize;
    public float ySize;
    public bool isEquip;
    public int price;
    public bool isSell;
}
public class itemJsonData
{
    public List<itemData> itemList = new List<itemData>();
    public int gold;

    public itemJsonData()
    {
        itemData nullItem = new itemData();
        itemList.Add(nullItem);
        gold = 0;
    }
}
public class speechData
{
    public List<string> data;
}
public class speechJsonData
{
    //public List<speechData> speechDatas = new List<speechData>();
    public List<string> speechDatas = new List<string>();
    public speechJsonData()
    {
        speechDatas.Add("");
    }
}

public enum QuestKind     // ����Ʈ ����
{
    management,             // ������ ����(0)
    kill,                   // óġ(1)
    cook,                   // �丮(2)
    hunt,                   // ����(3)
    interactive             // ��ȣ�ۿ�(4)
};

[System.Serializable]
public class questData
{
    public QuestKind questObject;
    public int objectVar;       // �� ������ ���� ����� ����(óġ �� ��, �丮 ������ �� ��ȣ�ۿ� ������Ʈ ID ��
    public float time;          // ���� �ð�
    public string npcName;      // ����Ʈ ���� ���� �� �ϷḦ ���� ��ȭ�ؾ� �ϴ� npc �̸�(������ -1 ��� ���� Ư�� ��ȣ�� ǥ���� ��)

    public questData()
    {
        questObject = QuestKind.management;
        objectVar = -1;
        time = -1f;
        npcName = "missingNo";
    }

    public void Log()
    {
        Debug.Log(questObject);
        Debug.Log(objectVar);
        Debug.Log(time);
        Debug.Log(npcName);
    }
}

public class questJsonData
{
    public List<questData> questList = new List<questData>();
    public int questIndex;      // ���� ����Ʈ �÷� ���� ����
    public questJsonData()
    {
        questData nullQuest = new questData();
        questList.Add(nullQuest);
        questIndex = -1;
    }
}

public class json
{
    public static string ObjectToJson(object obj) { return JsonUtility.ToJson(obj); }
    public static T JsonToObject<T>(string jsonData) { return JsonUtility.FromJson<T>(jsonData); }
    public static void CreateJsonFile(string createPath, string fileName, string jsonData) { File.WriteAllText(string.Format("{0}/Resources/json/{1}.json", createPath, fileName), jsonData); }
    public static T LoadJsonFile<T>(string loadPath, string fileName)
    {
        string temp = File.ReadAllText(string.Format("{0}/Resources/json/{1}.json", loadPath, fileName));
        T jsonData = JsonToObject<T>(temp);
        return jsonData;
    }
    public static bool FileExist(string loadPath, string fileName)
    {
        FileInfo info = new FileInfo(string.Format("{0}/Resources/json/{1}.json", loadPath, fileName));
        return info.Exists;
    }
}
