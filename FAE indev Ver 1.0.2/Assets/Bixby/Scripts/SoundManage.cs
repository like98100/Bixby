using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManage : MonoBehaviour
{
    public static SoundManage instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);  // �� ������Ʈ�� ���� ����ǵ� ���ŵ��� �ʴ´�

        if (instance == null)    // �Ҵ�Ǿ� ���� �ʴٸ�
        {
            instance = this;
        }
        else if (instance != this)        // ���� �Ҵ�Ǿ� �ִ� ������Ʈ���ƴ϶��
        {
            Destroy(gameObject);        // NAGA
        }

        bgmPlayer = gameObject.AddComponent<AudioSource>();
        charSfxPlayer = gameObject.AddComponent<AudioSource>();
        charLoopSfxPlayer = gameObject.AddComponent<AudioSource>();
        systemSfxPlayer = gameObject.AddComponent<AudioSource>();
    }

    public AudioClip[] BGMClips = new AudioClip[7];   // Title, Field, FireD, IceD, WaterD, ElecD, BossD
    public AudioClip[] PlayerSFXClips;
    public AudioClip[] PlayerLoopSFXClips;
    public AudioClip[] SystemSFXClips;

    AudioSource bgmPlayer;
    AudioSource charSfxPlayer;
    AudioSource charLoopSfxPlayer;
    AudioSource systemSfxPlayer;

    int curLoopIdx;
    // Start is called before the first frame update
    void Start()
    {
        PlayBGMSound(SceneManager.GetActiveScene().name, 0.7f); // ������� ����
        curLoopIdx = -1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayBGMSound(string fieldName, float volume = 1f)
    {
        bgmPlayer.Pause();

        int idx = -1;
        switch(fieldName)
        {
            case "Title":
                idx = 0;
                break;
            case "FieldScene":
                idx = 1;
                break;
            case "FireDungeon":
                idx = 2;
                break;
            case "IceDungeon":
                idx = 3;
                break;
            case "WaterDungeon":
                idx = 4;
                break;
            case "ElectricDungeon":
                idx = 5;
                break;
            case "BossDungeon":
                idx = 6;
                break;
            default:
                break;
        }

        if (idx == -1) return;

        //BGMSources[idx].loop = true;
        //BGMSources[idx].volume = volume;

        //BGMSources[idx].Play();

        bgmPlayer.loop = true;
        bgmPlayer.volume = volume;
        bgmPlayer.clip = BGMClips[idx];

        bgmPlayer.Play();

        Debug.Log(fieldName + " Bgm ��� ��");
    }

    public void PlaySFXSound(int sfxIdx, string flag, float volume = 1f)
    {
        // flag�� Player�� ���� System�� �� ���� �ٸ� �÷��̾ ������ ��
        switch(flag)
        {
            case "Player":
                //if (charSfxPlayer.isPlaying) break;          // ���尡 ������̸� �н�(�ϴ��� �� ����ϰ� �����ϱ�)
                charSfxPlayer.volume = volume;
                charSfxPlayer.loop = false;
                //charSfxPlayer.PlayOneShot(PlayerSFXClips[sfxIdx]);

                charSfxPlayer.clip = PlayerSFXClips[sfxIdx];
                charSfxPlayer.Play();
                break;
            case "PlayerLoop":
                if (sfxIdx == curLoopIdx && charLoopSfxPlayer.isPlaying) break;            // �ݺ� ��� ����

                charLoopSfxPlayer.volume = volume;
                charLoopSfxPlayer.loop = true;
                charLoopSfxPlayer.clip = PlayerLoopSFXClips[sfxIdx];
                charLoopSfxPlayer.Play();

                curLoopIdx = sfxIdx;
                break;
            case "System":
                systemSfxPlayer.volume = volume;
                systemSfxPlayer.loop = false;
                systemSfxPlayer.PlayOneShot(SystemSFXClips[sfxIdx]);
                break;
            default:
                break;
        }

        Debug.Log(flag + "�� " + sfxIdx + " ���� ���");
    }

    public AudioSource GetPlayerLoopSFXPlayer()
    {
        return charLoopSfxPlayer;
    }
}
