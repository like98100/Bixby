using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
[System.Serializable]
public class ItemData
{
    public int ItemID;
    public string ItemName;
    public float Left;
    public float Up;
    public float XSize;
    public float YSize;
    public bool IsEquip;
}
public class ItemJsonData
{
    public List<ItemData> ItemList = new List<ItemData>();
    public int Gold;

    public ItemJsonData()
    {
        ItemData nullItem = new ItemData();
        ItemList.Add(nullItem);
        Gold = 0;
    }
}
public class SpeechData
{
    public List<string> Data;
}
public class SpeechJsonData
{
    //public List<speechData> speechDatas = new List<speechData>();
    public List<string> SpeechDatas = new List<string>();
    public SpeechJsonData()
    {
        SpeechDatas.Add("");
    }
}
public class Json
{
    public static string ObjectToJson(object obj) { return JsonUtility.ToJson(obj); }
    public static T JsonToObject<T>(string jsonData) { return JsonUtility.FromJson<T>(jsonData); }
    public static void CreateJsonFile(string createPath, string fileName, string jsonData)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/Resources/json/{1}.json", createPath, fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }
    public static T LoadJsonFile<T>(string loadPath, string fileName)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/Resources/json/{1}.json", loadPath, fileName), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonUtility.FromJson<T>(jsonData);
    }
    public static bool FileExist(string loadPath, string fileName)
    {
        FileInfo info = new FileInfo(string.Format("{0}/Resources/json/{1}.json", loadPath, fileName));
        return info.Exists;
    }
}
