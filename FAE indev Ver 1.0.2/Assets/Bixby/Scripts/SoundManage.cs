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
        systemSfxPlayer = gameObject.AddComponent<AudioSource>();
    }

    public AudioClip[] BGMClips = new AudioClip[7];   // Title, Field, FireD, IceD, WaterD, ElecD, BossD
    public AudioClip[] PlayerSFXClips;
    public AudioClip[] SystemSFXClips;

    AudioSource bgmPlayer;
    AudioSource charSfxPlayer;
    AudioSource systemSfxPlayer;

    // Start is called before the first frame update
    void Start()
    {
        PlayBGMSound(SceneManager.GetActiveScene().name, 0.7f); // ������� ����
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

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

                if (sfxIdx == 0 || sfxIdx == 2 || sfxIdx == 11)      // �Ȱų� �ٰų� �������� ��
                {
                    if (charSfxPlayer.clip == PlayerSFXClips[1]   // ��� ���̰�
                            && charSfxPlayer.isPlaying) return;   // ���尡 ������̸� �н�
                    charSfxPlayer.volume = volume;
                    charSfxPlayer.loop = true;
                    charSfxPlayer.clip = PlayerSFXClips[sfxIdx];
                    charSfxPlayer.Play();
                }
                else
                {
                    if ((charSfxPlayer.clip != PlayerSFXClips[0] || charSfxPlayer.clip != PlayerSFXClips[2])    // �Ȱų� �޸��� ���°� �ƴϰ�
                        && charSfxPlayer.isPlaying) return;                                                     // ���尡 ������̸� �н�
                    charSfxPlayer.volume = volume;
                    charSfxPlayer.loop = false;
                    charSfxPlayer.PlayOneShot(PlayerSFXClips[sfxIdx]);
                }
                break;
            case "System":
                systemSfxPlayer.volume = volume;

                systemSfxPlayer.PlayOneShot(SystemSFXClips[sfxIdx]);
                break;
            default:
                break;
        }

        Debug.Log(flag + "�� " + sfxIdx + " ���� ���");
    }

    public AudioSource GetPlayerSFXPlayer()
    {
        return charSfxPlayer;
    }
}
