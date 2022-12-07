using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadingSceneController : MonoBehaviour
{
    private static LoadingSceneController instance;

    [SerializeField] private CanvasGroup canvasGroup;

    private string loadSceneName;
    private string previousSceneName;

    private SpawnPlayer spawnPlayer;
    private LoadingUIManager loadingUIManager;
    public static LoadingSceneController Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(LoadingSceneController)) as LoadingSceneController;

                if (instance == null)
                {
                    instance = Create();
                }
            }
            return instance;
        }
    }
    
    private void Awake()
    {
        loadSceneName = "FieldScene";
        if (instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    private void OnEnable()
    {
        loadingUIManager = this.gameObject.GetComponent<LoadingUIManager>();
    }
    private static LoadingSceneController Create()
    {
        return Instantiate(Resources.Load<LoadingSceneController>("LoadingUI"));
    }

    public void LoadScene(string sceneName)
    {
        this.gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;

        previousSceneName = loadSceneName;
        loadSceneName = sceneName;

        //if(json.FileExist(Application.dataPath, "quests"))                                      // ����Ʈ json ������ ������ ��
        //{
        //    questJsonData currentQuestData =
        //        GameObject.Find("GameManager").GetComponent<QuestObject>().GetQuestData();      // ���� ����Ʈ �����Ȳ ���� ����

        //    string questStr = json.ObjectToJson(currentQuestData);                              // To String

        //    json.OverWriteJsonFile(Application.dataPath, "quests", questStr);                   // OverWrite
        //}

        StartCoroutine(LoadSceneProceess());
    }
    public void ReloadScene()
    {
        this.gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;

        //if(json.FileExist(Application.dataPath, "quests"))                                      // ����Ʈ json ������ ������ ��
        //{
        //    questJsonData currentQuestData =
        //        GameObject.Find("GameManager").GetComponent<QuestObject>().GetQuestData();      // ���� ����Ʈ �����Ȳ ���� ����

        //    string questStr = json.ObjectToJson(currentQuestData);                              // To String

        //    json.OverWriteJsonFile(Application.dataPath, "quests", questStr);                   // OverWrite
        //}
        previousSceneName = loadSceneName;
        StartCoroutine(LoadSceneProceess());
    }

    private IEnumerator LoadSceneProceess()
    {
        loadingUIManager.SetDungeonInfo(loadSceneName);

        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return null;

            if (loadingUIManager.SetProgressBar(op.progress))
            {
                op.allowSceneActivation = true;
                yield break;
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == loadSceneName)
        {
            spawnPlayer = GameObject.FindWithTag("Player").GetComponent<SpawnPlayer>();
            spawnPlayer.SetPosition(previousSceneName);

            StartCoroutine(Fade(false));

            QuestObject.manager.MissionSet();           // �� �ε��� �Ϸ�Ǹ� �̼� �ؽ�Ʈ �缳��

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0.0f;
        while (timer <= 1.0f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 3.0f;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0.0f, 1.0f, timer) : Mathf.Lerp(1.0f, 0.0f, timer);
        }
        if (!isFadeIn)
        {
            this.gameObject.SetActive(false);
        }
    }
}